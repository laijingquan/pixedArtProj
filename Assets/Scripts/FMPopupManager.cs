// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class FMPopupManager : MonoBehaviour
{
	protected FMPopup ActivePopup {  get; private set; }

	protected void Open(FMPopup popup, FMPopupManager.FMPopupPriority priority = FMPopupManager.FMPopupPriority.Normal)
	{
		if (this.ActivePopup == null)
		{
			this.ActivePopup = popup;
			this.ActivePopup.Open();
		}
		else
		{
			this.AddPopupToQueue(popup, priority);
			if (priority == FMPopupManager.FMPopupPriority.ForceOpen)
			{
				this.CloseActivePopup(true);
			}
		}
	}

	protected void CloseActivePopup(bool openQueued = true)
	{
		if (this.ActivePopup != null)
		{
			this.ActivePopup.Close();
			this.ActivePopup = null;
		}
		if (openQueued && this.queuedList.Count > 0)
		{
			this.ActivePopup = this.GetFirstQueuedPopup();
			this.ActivePopup.Open();
		}
	}

	protected virtual void Update()
	{
		if (this.ActivePopup != null && this.ActivePopup.IsAutoclosingPopup)
		{
			this.ActivePopup.currentShowTime -= Time.deltaTime;
			if (this.ActivePopup.currentShowTime <= 0f)
			{
				this.CloseActivePopup(true);
			}
		}
	}

	protected void ClearQueue()
	{
		this.queuedList.Clear();
	}

	protected void ClearQueueByPriority(FMPopupManager.FMPopupPriority priority)
	{
		if (this.queuedList.Count > 0)
		{
			for (int i = this.queuedList.Count - 1; i >= 0; i--)
			{
				if (this.queuedList[i].priority == priority)
				{
					this.queuedList.RemoveAt(i);
				}
			}
		}
	}

	private void AddPopupToQueue(FMPopup popup, FMPopupManager.FMPopupPriority priority)
	{
		FMPopupManager.FMPPopupQueueItem item = new FMPopupManager.FMPPopupQueueItem(popup, priority);
		if (this.queuedList.Count == 0)
		{
			this.queuedList.Add(item);
		}
		else
		{
			int num = this.queuedList.Count;
			for (int i = this.queuedList.Count - 1; i >= 0; i--)
			{
				if (item.priority < this.queuedList[i].priority)
				{
					num = i;
				}
				else if (this.queuedList[i].priority == item.priority)
				{
					break;
				}
			}
			if (num >= this.queuedList.Count)
			{
				this.queuedList.Add(item);
			}
			else
			{
				this.queuedList.Insert(num, item);
			}
		}
	}

	private FMPopup GetFirstQueuedPopup()
	{
		FMPopup popup = this.queuedList[0].popup;
		this.queuedList.RemoveAt(0);
		return popup;
	}

	private List<FMPopupManager.FMPPopupQueueItem> queuedList = new List<FMPopupManager.FMPPopupQueueItem>();

	public enum FMPopupPriority
	{
		ForceOpen,
		High,
		Normal,
		Low
	}

	private struct FMPPopupQueueItem
	{
		public FMPPopupQueueItem(FMPopup popup, FMPopupManager.FMPopupPriority priority)
		{
			this.popup = popup;
			this.priority = priority;
		}

		public FMPopup popup;

		public FMPopupManager.FMPopupPriority priority;
	}
}

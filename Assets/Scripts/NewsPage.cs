// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class NewsPage : Page
{
	private ConnectionResumeHelper ConnectionResumeHelper
	{
		get
		{
			if (this.connectionResumeHelper == null)
			{
				this.connectionResumeHelper = base.GetComponent<ConnectionResumeHelper>();
			}
			return this.connectionResumeHelper;
		}
	}

	public void LoadContent(NewsInfo newsInfo)
	{
		this.PrepareForContnentLoad();
		this.scroll.LoadContent(newsInfo, !base.IsOpened);
		this.UpdateNewsIndicator();
		base.StartCoroutine(base.DelayAction(3, 0f, new Action(this.ContentLoadComplete)));
	}

	private void UpdateNewsIndicator()
	{
		List<int> newsId = this.scroll.GetNewsId();
		if (newsId != null && newsId.Count > 0)
		{
			this.unreadNewsIds = SharedData.Instance.HasUnreadNews(newsId);
			if (this.unreadNewsIds != null && this.unreadNewsIds.Count > 0)
			{
				this.newItemsIndicator.SetActive(true);
			}
		}
	}

	private void MarkNewsAsRead()
	{
		if (this.newItemsIndicator.activeSelf)
		{
			this.newItemsIndicator.SetActive(false);
			if (this.unreadNewsIds != null && this.unreadNewsIds.Count > 0)
			{
				List<OrderedItemInfo> itemsWithIds = this.scroll.GetItemsWithIds(this.unreadNewsIds);
				SharedData.Instance.MarkNewsAsRead(itemsWithIds);
			}
		}
	}

	private void OnEnable()
	{
		WebLoader.Instance.ConnectionResume += this.OnConnectionResume;
		WebLoader.Instance.ConnectionError += this.OnConnectionError;
	}

	private void OnDisable()
	{
		WebLoader.Instance.ConnectionResume -= this.OnConnectionResume;
		WebLoader.Instance.ConnectionError -= this.OnConnectionError;
	}

	private void OnConnectionError()
	{
	}

	private void OnConnectionResume()
	{
		this.ConnectionResumeHelper.SafeResume(delegate
		{
			this.scroll.ReloadFailedIcon();
		});
	}

	protected override void OnBeginOpenning()
	{
		this.scroll.ReloadTextures();
		this.MarkNewsAsRead();
	}

	protected override void OnOpened()
	{
	}

	protected override void OnBeginClosing()
	{
	}

	protected override void OnClosed()
	{
		this.scroll.UnloadTextures();
	}

	[SerializeField]
	private GameObject newItemsIndicator;

	[SerializeField]
	private NewsScroll scroll;

	private List<int> unreadNewsIds;

	private ConnectionResumeHelper connectionResumeHelper;
}

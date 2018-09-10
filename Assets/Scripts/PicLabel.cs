// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicLabel : MonoBehaviour
{
	public void AddComplete()
	{
		this.completeLabel.SetActive(true);
		if (this.active != null)
		{
			if (this.active.Contains(PictureLabel.New))
			{
				this.newLabel.SetActive(false);
			}
			if (this.active.Contains(PictureLabel.Facebook))
			{
				this.fbLabel.SetActive(false);
			}
		}
	}

	public void AddDailyTab(int date)
	{
		this.dailyTab.text = date.ToString();
		this.dailyTab.transform.parent.gameObject.SetActive(true);
	}

	public void RemoveComplete()
	{
		this.completeLabel.SetActive(false);
	}

	public void AddLabels(List<PictureLabel> labels)
	{
		this.active = labels;
		for (int i = 0; i < this.active.Count; i++)
		{
			PictureLabel pictureLabel = this.active[i];
			if (pictureLabel != PictureLabel.New)
			{
				if (pictureLabel != PictureLabel.Daily)
				{
					if (pictureLabel == PictureLabel.Facebook)
					{
						this.fbLabel.SetActive(true);
					}
				}
			}
			else
			{
				this.newLabel.SetActive(true);
			}
		}
	}

	public void Clean()
	{
		if (this.active != null)
		{
			for (int i = 0; i < this.active.Count; i++)
			{
				PictureLabel pictureLabel = this.active[i];
				if (pictureLabel != PictureLabel.New)
				{
					if (pictureLabel != PictureLabel.Daily)
					{
						if (pictureLabel == PictureLabel.Facebook)
						{
							this.fbLabel.SetActive(false);
						}
					}
				}
				else
				{
					this.newLabel.SetActive(false);
				}
			}
			this.active = null;
		}
		this.dailyTab.transform.parent.gameObject.SetActive(false);
	}

	[SerializeField]
	private GameObject completeLabel;

	[SerializeField]
	private GameObject newLabel;

	[SerializeField]
	private GameObject fbLabel;

	[SerializeField]
	private Text dailyTab;

	private List<PictureLabel> active;
}

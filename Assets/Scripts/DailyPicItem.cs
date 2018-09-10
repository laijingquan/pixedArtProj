// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyPicItem : FeaturedItem
{
	public PicItem PicItem
	{
		get
		{
			return this.picItem;
		}
	}

	public int Order { get; private set; }

	public void Init(DailyPicInfo picInfo)
	{
		base.Id = picInfo.id;
		this.Order = picInfo.order;
		base.Type = FeaturedItem.ItemType.Daily;
		if (GeneralSettings.IsOldDesign)
		{
			this.dayLabel.text = picInfo.day;
			this.monthLabel.text = picInfo.month;
		}
		else
		{
			this.dayLabel.text = picInfo.day + "\n" + picInfo.month;
			this.btnLabel.text = picInfo.btnLabel;
		}
		this.descLabel.text = picInfo.desc;
		this.picItem.Init(picInfo.picData, false, false, false);
		if (picInfo.picData.HasSave)
		{
			PictureSaveData save = SharedData.Instance.GetSave(picInfo.picData);
			if (save != null)
			{
				this.picItem.AddSave(save);
			}
		}
	}

	public override void Reset()
	{
		if (this.picItem != null)
		{
			this.picItem.Reset();
		}
		this.dayLabel.text = string.Empty;
		this.monthLabel.text = string.Empty;
		this.descLabel.text = string.Empty;
	}

	public override void ReloadFailedTextures()
	{
		if (this.picItem != null && this.picItem.IconLoadFailed())
		{
			this.picItem.RetryLoadIcon();
		}
	}

	public override void ReloadTextures()
	{
		if (this.picItem != null)
		{
			this.picItem.ReloadTextures();
		}
	}

	public override void UnloadTextures()
	{
		if (this.picItem != null)
		{
			this.picItem.UnloadTextures();
		}
	}

	[SerializeField]
	private Text dayLabel;

	[SerializeField]
	private Text monthLabel;

	[SerializeField]
	private Text descLabel;

	[SerializeField]
	private Text btnLabel;

	[SerializeField]
	private PicItem picItem;
}

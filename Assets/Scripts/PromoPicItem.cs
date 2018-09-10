// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class PromoPicItem : FeaturedItem
{
	public PicItem PicItem
	{
		get
		{
			return this.picItem;
		}
	}

	public int Order { get; private set; }

	public void Init(PromoPicInfo picInfo, bool lazyLoad = false)
	{
		base.Id = picInfo.id;
		base.Type = FeaturedItem.ItemType.PromoPic;
		this.Order = picInfo.order;
		this.picItem.Init(picInfo.picData, lazyLoad, false, false);
		if (picInfo.picData.HasSave)
		{
			PictureSaveData save = SharedData.Instance.GetSave(picInfo.picData);
			if (save != null)
			{
				this.picItem.AddSave(save);
			}
		}
		if (GeneralSettings.IsOldDesign)
		{
			this.titleLabel.text = picInfo.title;
			this.descLabel.text = picInfo.desc;
		}
		else
		{
			if (string.IsNullOrEmpty(picInfo.desc))
			{
				this.descLabel.gameObject.SetActive(false);
				this.titleLabel.rectTransform.anchoredPosition = Vector2.zero;
			}
			else
			{
				this.descLabel.text = picInfo.desc;
			}
			this.titleLabel.text = picInfo.title;
			this.btnLabel.text = picInfo.btnLabel;
		}
	}

	public override void Reset()
	{
		if (this.picItem != null)
		{
			this.picItem.Reset();
		}
		this.descLabel.gameObject.SetActive(true);
		this.titleLabel.text = string.Empty;
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
	private Text titleLabel;

	[SerializeField]
	private Text descLabel;

	[SerializeField]
	private Text btnLabel;

	[SerializeField]
	private PicItem picItem;
}

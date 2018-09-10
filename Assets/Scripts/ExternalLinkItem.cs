// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class ExternalLinkItem : FeaturedItem
{
	public BasicImageSlot ImageItem
	{
		get
		{
			return this.imageItem;
		}
	}

	public string TargetScheme { get; private set; }

	public string TargetUrl { get; private set; }

	public int Order { get; private set; }

	public void Init(ExternalLinkInfo picInfo, bool lazyLoad = false)
	{
		base.Type = FeaturedItem.ItemType.ExternalLink;
		base.Id = picInfo.id;
		this.Order = picInfo.order;
		this.imageItem.Init(picInfo.imageData, lazyLoad);
		this.TargetScheme = picInfo.targetScheme;
		this.TargetUrl = picInfo.targetUrl;
		if (string.IsNullOrEmpty(picInfo.title))
		{
			this.descCenteredLabel.gameObject.SetActive(true);
			this.descCenteredLabel.text = picInfo.desc;
		}
		else
		{
			this.titleLabel.gameObject.SetActive(true);
			this.descLabel.gameObject.SetActive(true);
			this.titleLabel.text = picInfo.title;
			this.descLabel.text = picInfo.desc;
		}
		if (!GeneralSettings.IsOldDesign)
		{
			this.btnLabel.text = picInfo.btnLabel;
		}
	}

	public override void Reset()
	{
		if (this.imageItem != null)
		{
			this.imageItem.Reset();
		}
		this.descLabel.gameObject.SetActive(true);
		this.titleLabel.text = string.Empty;
		this.descLabel.text = string.Empty;
	}

	public override void ReloadFailedTextures()
	{
		if (this.imageItem != null && this.imageItem.IconFailedToLoad())
		{
			this.imageItem.ReloadFailedIcon();
		}
	}

	public override void ReloadTextures()
	{
		if (this.imageItem != null)
		{
			this.imageItem.ReloadTextures();
		}
	}

	public override void UnloadTextures()
	{
		if (this.imageItem != null)
		{
			this.imageItem.UnloadTextures();
		}
	}

	[SerializeField]
	private Text titleLabel;

	[SerializeField]
	private Text descLabel;

	[SerializeField]
	private Text descCenteredLabel;

	[SerializeField]
	private Text btnLabel;

	[SerializeField]
	private BasicImageSlot imageItem;
}

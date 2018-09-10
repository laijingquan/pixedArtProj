// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class NewsScrollItem : InfiniteScrollItem
{
	public FeaturedItem Item
	{
		get
		{
			return this.item;
		}
	}

	public void OnFill(int row, OrderedItemInfo itemInfo, bool lazyLoad)
	{
		base.Row = row;
		if (itemInfo == null)
		{
			this.Clean();
			return;
		}
		OrderedItemType itemType = itemInfo.itemType;
		if (itemType != OrderedItemType.PromoPic)
		{
			if (itemType != OrderedItemType.ExternalLink)
			{
				FMLogger.vCore("news item " + row + " is empty");
			}
			else
			{
				ExternalLinkInfo externalLinkInfo = (ExternalLinkInfo)itemInfo;
				this.AddExternalLinkItem(externalLinkInfo, lazyLoad);
			}
		}
		else
		{
			PromoPicInfo promoPicInfo = (PromoPicInfo)itemInfo;
			this.AddEditorItem(promoPicInfo, lazyLoad);
		}
	}

	public override void ReloadFailedTextures()
	{
		if (this.item != null)
		{
			this.item.ReloadFailedTextures();
		}
	}

	public override void ReloadTextures()
	{
		if (this.item != null)
		{
			this.item.ReloadTextures();
		}
	}

	public override void UnloadTextures()
	{
		if (this.item != null)
		{
			this.item.UnloadTextures();
		}
	}

	protected override void Clean()
	{
		if (this.item != null)
		{
			this.item.Reset();
			this.item = null;
		}
	}

	private void AddExternalLinkItem(ExternalLinkInfo externalLinkInfo, bool lazyLoad)
	{
		ExternalLinkItem entity = this.externalLinkItemsPool.GetEntity<ExternalLinkItem>(base.transform);
		((RectTransform)entity.transform).anchoredPosition = Vector2.zero;
		entity.Init(externalLinkInfo, lazyLoad);
		this.item = entity;
	}

	private void AddEditorItem(PromoPicInfo promoPicInfo, bool lazyLoad)
	{
		PromoPicItem entity = this.promoItemsPool.GetEntity<PromoPicItem>(base.transform);
		((RectTransform)entity.transform).anchoredPosition = Vector2.zero;
		entity.Init(promoPicInfo, lazyLoad);
		this.item = entity;
	}

	private FeaturedItem item;

	public ObjectContainer promoItemsPool;

	public ObjectContainer externalLinkItemsPool;
}

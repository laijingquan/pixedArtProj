// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class NewsScroll : MonoBehaviour
{
	public void LoadContent(NewsInfo info, bool lazyLoad)
	{
		this.newsInfo = info;
		if (!GeneralSettings.IsOldDesign)
		{
			this.scroll.AddTopOffset((!SafeLayout.IsTablet) ? 40 : 30);
			this.scroll.height = ((!SafeLayout.IsTablet) ? 444 : 370);
		}
		this.scroll.FillItem += delegate(int row, InfiniteScrollItem item, bool isLazyLoad)
		{
			((NewsScrollItem)item).OnFill(row, this.RowToData(row), isLazyLoad);
		};
		this.scroll.InitData(this.newsInfo.orderedList.Count, 0, lazyLoad);
	}

	public void ReloadFailedIcon()
	{
		InfiniteScrollItem[] views = this.scroll.Views;
		if (views != null && views.Length > 0)
		{
			for (int i = 0; i < views.Length; i++)
			{
				views[i].ReloadFailedTextures();
			}
		}
	}

	public void ReloadTextures()
	{
		if (this.scroll.Views != null)
		{
			for (int i = 0; i < this.scroll.Views.Length; i++)
			{
				this.scroll.Views[i].ReloadTextures();
			}
		}
	}

	public void UnloadTextures()
	{
		if (this.scroll.Views != null)
		{
			for (int i = 0; i < this.scroll.Views.Length; i++)
			{
				this.scroll.Views[i].UnloadTextures();
			}
		}
	}

	private void OnEnable()
	{
		SharedData.Instance.PictureDataDeleted += this.OnPicDeleted;
	}

	private void OnDisable()
	{
		SharedData.Instance.PictureDataDeleted -= this.OnPicDeleted;
	}

	private void OnPicDeleted(PictureData delPicData, PictureData replacePicData)
	{
		if (replacePicData == null)
		{
			return;
		}
		if (this.scroll.Views != null)
		{
			for (int i = 0; i < this.scroll.Views.Length; i++)
			{
				NewsScrollItem newsScrollItem = (NewsScrollItem)this.scroll.Views[i];
				if (newsScrollItem.Item != null && newsScrollItem.Item.Type == FeaturedItem.ItemType.PromoPic)
				{
					PromoPicItem promoPicItem = (PromoPicItem)newsScrollItem.Item;
					if (promoPicItem.PicItem != null && promoPicItem.PicItem.Id == replacePicData.Id)
					{
						promoPicItem.PicItem.Reset();
						promoPicItem.PicItem.Init(replacePicData, false, false, false);
					}
				}
			}
		}
	}

	private OrderedItemInfo RowToData(int row)
	{
		if (row < this.newsInfo.orderedList.Count)
		{
			return this.newsInfo.orderedList[row];
		}
		return null;
	}

	public List<int> GetNewsId()
	{
		if (this.newsInfo == null || this.newsInfo.orderedList == null || this.newsInfo.orderedList.Count == 0)
		{
			return null;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < this.newsInfo.orderedList.Count; i++)
		{
			list.Add(this.newsInfo.orderedList[i].id);
		}
		return list;
	}

	public List<OrderedItemInfo> GetItemsWithIds(List<int> unreadNewsIds)
	{
		List<OrderedItemInfo> list = new List<OrderedItemInfo>();
		if (this.newsInfo != null && this.newsInfo.orderedList != null)
		{
			int i;
			for (i = 0; i < unreadNewsIds.Count; i++)
			{
				OrderedItemInfo orderedItemInfo = this.newsInfo.orderedList.Find((OrderedItemInfo x) => x.id == unreadNewsIds[i]);
				if (orderedItemInfo != null)
				{
					list.Add(orderedItemInfo);
				}
			}
		}
		return list;
	}

	[SerializeField]
	private InfiniteScrollGeneric scroll;

	private NewsInfo newsInfo;
}

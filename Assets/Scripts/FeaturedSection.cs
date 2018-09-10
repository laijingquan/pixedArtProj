// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class FeaturedSection : MonoBehaviour
{
	public int SectionHeight
	{
		get
		{
			return (int)((RectTransform)base.transform).sizeDelta.y;
		}
	}

	public void LoadContent(FeaturedInfo featuredInfo)
	{
		this.Init();
		for (int i = 0; i < featuredInfo.orderedList.Count; i++)
		{
			OrderedItemType itemType = featuredInfo.orderedList[i].itemType;
			if (itemType != OrderedItemType.PromoPic)
			{
				if (itemType != OrderedItemType.Daily)
				{
					if (itemType == OrderedItemType.ExternalLink)
					{
						ExternalLinkInfo externalLinkInfo = (ExternalLinkInfo)featuredInfo.orderedList[i];
						this.AddExternalLinkItem(externalLinkInfo);
					}
				}
				else
				{
					DailyPicInfo dailyPicInfo = (DailyPicInfo)featuredInfo.orderedList[i];
					this.AddDailyItem(dailyPicInfo);
				}
			}
			else
			{
				PromoPicInfo promoPicInfo = (PromoPicInfo)featuredInfo.orderedList[i];
				this.AddEditorItem(promoPicInfo);
			}
		}
		this.loopScroll.PrepareLayout();
	}

	private void AddExternalLinkItem(ExternalLinkInfo externalLinkInfo)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.externalLinkPrefab);
		ExternalLinkItem component = gameObject.GetComponent<ExternalLinkItem>();
		component.Init(externalLinkInfo, false);
		this.items.Add(component);
		this.AddItemToScroll(component);
	}

	private void AddEditorItem(PromoPicInfo promoPicInfo)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.promoPicPrefab);
		PromoPicItem component = gameObject.GetComponent<PromoPicItem>();
		component.Init(promoPicInfo, false);
		this.items.Add(component);
		this.AddItemToScroll(component);
	}

	private void AddDailyItem(DailyPicInfo dailyPicInfo)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dailyPrefab);
		DailyPicItem component = gameObject.GetComponent<DailyPicItem>();
		component.Init(dailyPicInfo);
		this.items.Add(component);
		this.AddItemToScroll(component);
	}

	private void AddItemToScroll(FeaturedItem item)
	{
		this.loopScroll.AddItem((RectTransform)item.transform);
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
		for (int i = 0; i < this.items.Count; i++)
		{
			PicItem picItem = null;
			FeaturedItem.ItemType type = this.items[i].Type;
			if (type != FeaturedItem.ItemType.Daily)
			{
				if (type == FeaturedItem.ItemType.PromoPic)
				{
					picItem = ((PromoPicItem)this.items[i]).PicItem;
				}
			}
			else
			{
				picItem = ((DailyPicItem)this.items[i]).PicItem;
			}
			if (picItem != null && picItem.PictureData.Id == replacePicData.Id)
			{
				FMLogger.Log("found and replaced in featured section " + replacePicData.Id);
				picItem.Reset();
				picItem.Init(replacePicData, false, false, false);
			}
		}
	}

	public void ReloadFailedIcon()
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			PicItem picItem = null;
			FeaturedItem.ItemType type = this.items[i].Type;
			if (type != FeaturedItem.ItemType.Daily)
			{
				if (type == FeaturedItem.ItemType.PromoPic)
				{
					picItem = ((PromoPicItem)this.items[i]).PicItem;
				}
			}
			else
			{
				picItem = ((DailyPicItem)this.items[i]).PicItem;
			}
			if (picItem != null && picItem.IconLoadFailed())
			{
				picItem.RetryLoadIcon();
			}
		}
	}

	private void Init()
	{
		if (SafeLayout.IsTablet)
		{
			this.dailyPrefab = this.dailyTablet;
			this.promoPicPrefab = this.promoTablet;
			this.externalLinkPrefab = this.externalTablet;
			this.config = this.tabletConfig;
		}
		else
		{
			this.dailyPrefab = this.dailyPhone;
			this.promoPicPrefab = this.promoPhone;
			this.externalLinkPrefab = this.externalPhone;
			this.config = this.phoneConfig;
		}
		this.loopScroll.Init(this.config);
		this.visabilityEventTracker.SetDelegate((int i) => this.items[i]);
	}

	public void WillBecomeVisable()
	{
		this.visabilityEventTracker.RootTabBecomeVisible();
	}

	[SerializeField]
	private GameObject dailyPhone;

	[SerializeField]
	private GameObject promoPhone;

	[SerializeField]
	private GameObject externalPhone;

	[SerializeField]
	private GameObject dailyTablet;

	[SerializeField]
	private GameObject promoTablet;

	[SerializeField]
	private GameObject externalTablet;

	private PageLoopScroll.Config phoneConfig = new PageLoopScroll.Config
	{
		itemSize = new Vector2(954f, 546f),
		spacing = 27
	};

	private PageLoopScroll.Config tabletConfig = new PageLoopScroll.Config
	{
		itemSize = new Vector2(1140f, 536f),
		spacing = 35
	};

	private GameObject dailyPrefab;

	private GameObject promoPicPrefab;

	private GameObject externalLinkPrefab;

	private PageLoopScroll.Config config;

	[SerializeField]
	private PageLoopScroll loopScroll;

	[SerializeField]
	private FeaturedVisabilityEventTracker visabilityEventTracker;

	private List<FeaturedItem> items = new List<FeaturedItem>();
}

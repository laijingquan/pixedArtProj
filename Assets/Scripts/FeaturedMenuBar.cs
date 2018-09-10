// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class FeaturedMenuBar : MonoBehaviour
{
	public int SectionHeight
	{
		get
		{
			return ((!SafeLayout.IsTablet) ? 530 : 510) + -1 * (int)((RectTransform)base.transform).anchoredPosition.y;
		}
	}

	public void LoadContent(FeaturedInfo featuredInfo)
	{
		this.loopScroll.Init((!SafeLayout.IsTablet) ? this.phoneConfig : this.tabletConfig);
		this.visabilityEventTracker.SetDelegate((int index) => this.items[index]);
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
		this.loopScroll.AddItem((RectTransform)component.transform);
	}

	private void AddEditorItem(PromoPicInfo promoPicInfo)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.promoPicPrefab);
		PromoPicItem component = gameObject.GetComponent<PromoPicItem>();
		component.Init(promoPicInfo, false);
		this.items.Add(component);
		this.loopScroll.AddItem((RectTransform)component.transform);
	}

	private void AddDailyItem(DailyPicInfo dailyPicInfo)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dailyPrefab);
		DailyPicItem component = gameObject.GetComponent<DailyPicItem>();
		component.Init(dailyPicInfo);
		this.items.Add(component);
		this.loopScroll.AddItem((RectTransform)component.transform);
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

	public void WillBecomeVisable()
	{
		this.visabilityEventTracker.RootTabBecomeVisible();
	}

	[SerializeField]
	private GameObject dailyPrefab;

	[SerializeField]
	private GameObject promoPicPrefab;

	[SerializeField]
	private GameObject externalLinkPrefab;

	[SerializeField]
	private FeaturedVisabilityEventTracker visabilityEventTracker;

	[SerializeField]
	private PageLoopScroll loopScroll;

	private List<FeaturedItem> items = new List<FeaturedItem>();

	private PageLoopScroll.Config phoneConfig = new PageLoopScroll.Config
	{
		itemSize = new Vector2(930f, 482f),
		spacing = 30
	};

	private PageLoopScroll.Config tabletConfig = new PageLoopScroll.Config
	{
		itemSize = new Vector2(1027f, 482f),
		spacing = 30
	};
}

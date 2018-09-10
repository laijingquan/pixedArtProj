// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.Events;

public class FeaturedVisabilityEventTracker : MonoBehaviour
{
	public void SetDelegate(Func<int, FeaturedItem> PageIndexToId)
	{
		this.pageIndexToId = PageIndexToId;
	}

	public void Init()
	{
		if (this.inited)
		{
			return;
		}
		this.inited = true;
		this.scrollSnap = base.GetComponent<UI_InfiniteScrollSnap>();
		this.scrollSnap.OnCompleteEvent.AddListener(new UnityAction<int>(this.OnPageChanged));
		if (this.FeaturedSectionIsVisible())
		{
			this.RootTabBecomeVisible();
		}
	}

	public void RootTabBecomeVisible()
	{
		if (this.FeaturedSectionIsVisible())
		{
			int currentPageClamped = this.scrollSnap.CurrentPageClamped;
			FeaturedItem featuredItem = this.pageIndexToId(currentPageClamped);
			this.ReportEvent(featuredItem.Id, featuredItem.Type);
		}
	}

	private void OnPageChanged(int page)
	{
		if (this.FeaturedSectionIsVisible())
		{
			FeaturedItem featuredItem = this.pageIndexToId(page);
			this.ReportEvent(featuredItem.Id, featuredItem.Type);
		}
	}

	private void ScrolledToFeatured()
	{
		if (this.FeaturedSectionIsVisible())
		{
			int currentPageClamped = this.scrollSnap.CurrentPageClamped;
			FeaturedItem featuredItem = this.pageIndexToId(currentPageClamped);
			this.ReportEvent(featuredItem.Id, featuredItem.Type);
		}
	}

	private bool FeaturedSectionIsVisible()
	{
		return this.inited && this.rootPage.IsOpened && this.isVisable;
	}

	private void ReportEvent(int id, FeaturedItem.ItemType pageType)
	{
		string text = string.Empty;
		if (pageType != FeaturedItem.ItemType.Daily)
		{
			if (pageType != FeaturedItem.ItemType.PromoPic)
			{
				if (pageType != FeaturedItem.ItemType.ExternalLink)
				{
				}
			}
			else
			{
				text = "editors_choise";
			}
		}
		else
		{
			text = "daily";
		}
		if (!string.IsNullOrEmpty(text))
		{
			AnalyticsManager.FeaturedItemVisable(id, text);
		}
	}

	private void Update()
	{
		if (!this.inited)
		{
			return;
		}
		if (this.scrollContent.anchoredPosition.y < (float)this.contentMaxOffset)
		{
			if (!this.isVisable)
			{
				this.isVisable = true;
				this.ScrolledToFeatured();
			}
		}
		else
		{
			this.isVisable = false;
		}
	}

	[SerializeField]
	private Page rootPage;

	[SerializeField]
	private RectTransform scrollContent;

	private bool inited;

	private Func<int, FeaturedItem> pageIndexToId;

	private UI_InfiniteScrollSnap scrollSnap;

	private bool isVisable = true;

	private int contentMaxOffset = 130;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PageLoopScroll : MonoBehaviour
{
	public void Init(PageLoopScroll.Config cfg)
	{
		this.config = cfg;
		int num = (int)((RectTransform)base.transform).rect.width;
		int left = (int)((float)num / 2f - this.config.itemSize.x / 2f);
		HorizontalLayoutGroup component = this.content.GetComponent<HorizontalLayoutGroup>();
		component.padding.left = left;
		component.spacing = (float)this.config.spacing;
	}

	public void AddItem(RectTransform rt)
	{
		this.itemsCount++;
		rt.SetParent(this.content);
		rt.localScale = Vector3.one;
	}

	public void PrepareLayout()
	{
		if (!GeneralSettings.IsOldDesign)
		{
			this.pagination.Init(this.itemsCount);
		}
		base.StartCoroutine(this.PrepareLayoEnumerator());
	}

	private IEnumerator PrepareLayoEnumerator()
	{
		HorizontalLayoutGroup layout = this.content.GetComponent<HorizontalLayoutGroup>();
		this.content.sizeDelta = new Vector2((float)layout.padding.left + (float)this.itemsCount * this.config.itemSize.x + (float)((this.itemsCount - 1) * this.config.spacing), this.content.sizeDelta.y);
		yield return 0;
		yield return 0;
		UI_InfiniteScrollSnap scroll = base.GetComponent<UI_InfiniteScrollSnap>();
		scroll.InitScroll();
		base.GetComponent<FeaturedAutoScroll>().Init();
		base.GetComponent<FeaturedVisabilityEventTracker>().Init();
		yield break;
	}

	[SerializeField]
	private PageIndicatorLayout pagination;

	[SerializeField]
	private RectTransform content;

	private PageLoopScroll.Config config;

	private Vector2 itemPos;

	private int itemsCount;

	public class Config
	{
		public Vector2 itemSize;

		public int spacing;
	}
}

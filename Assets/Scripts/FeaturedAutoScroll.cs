// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.Events;

public class FeaturedAutoScroll : MonoBehaviour
{
	public void Init()
	{
		if (this.inited)
		{
			return;
		}
		this.inited = true;
		this.scrollSnap = base.GetComponent<UI_InfiniteScrollSnap>();
		this.scrollSnap.OnCompleteEvent.AddListener(new UnityAction<int>(this.OnPageChanged));
		if (GeneralSettings.IsOldDesign)
		{
			this.contentMaxOffset = Mathf.RoundToInt((float)this.legacyFeaturedSection.SectionHeight * this.visabilityThresholdPercent);
		}
		else
		{
			this.contentMaxOffset = Mathf.RoundToInt((float)this.featuredSection.SectionHeight * this.visabilityThresholdPercent);
		}
	}

	private void OnPageChanged(int p)
	{
		this.currentTime = 0f;
	}

	private void Update()
	{
		if (!this.inited)
		{
			return;
		}
		if (!this.scrollSnap.IsDragging && this.page.IsOpened && this.scrollContent.anchoredPosition.y < (float)this.contentMaxOffset)
		{
			this.currentTime += Time.deltaTime;
			if (this.currentTime >= this.timePerPage)
			{
				this.currentTime = 0f;
				this.Next();
			}
		}
		else
		{
			this.currentTime = 0f;
		}
	}

	private void Next()
	{
		this.scrollSnap.MoveRight();
	}

	[SerializeField]
	private Page page;

	[SerializeField]
	[Range(0f, 1f)]
	private float visabilityThresholdPercent;

	[SerializeField]
	private FeaturedSection featuredSection;

	[SerializeField]
	private FeaturedMenuBar legacyFeaturedSection;

	[SerializeField]
	private RectTransform scrollContent;

	private UI_InfiniteScrollSnap scrollSnap;

	private int contentMaxOffset;

	private bool inited;

	private float timePerPage = 4f;

	private float currentTime;
}

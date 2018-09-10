// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuUILayout : MonoBehaviour
{
	private void Awake()
	{
		if (SafeLayout.IsTablet)
		{
			MenuScreen.RowItems = 3;
			if (GeneralSettings.IsOldDesign)
			{
				CanvasScaler component = base.GetComponent<CanvasScaler>();
				component.referenceResolution = new Vector2(1654f, 2927f);
				MenuScreen.RowItems = 3;
				this.ApplyTabletLayout();
			}
		}
		Canvas.GetDefaultCanvasMaterial().shader = this.uiShader;
	}

	private void Start()
	{
		base.StartCoroutine(this.FrameDelay(delegate
		{
			base.GetComponent<MenuSafeLayout>().ApplySafeArea();
		}));
	}

	private IEnumerator FrameDelay(Action a)
	{
		yield return new WaitForEndOfFrame();
		a();
		yield break;
	}

	private void ApplyTabletLayout()
	{
		int num = 30;
		this.tabDaily.offsetMax = new Vector2(this.tabDaily.offsetMax.x, this.tabDaily.offsetMax.y - (float)num);
		this.libFeaturedLayout.padding.left = 50;
		this.libFeaturedLayout.padding.right = 50;
		this.libFilterMenuBarRt.anchoredPosition = new Vector2(60f, 0f);
		this.libFBMenuBarRt.anchoredPosition = new Vector2(-60f, 0f);
		int num2 = 15;
		this.libDaily.sizeDelta = new Vector2(1027f, this.libDaily.sizeDelta.y);
		this.libDailyLabels.offsetMin = new Vector2(this.libDailyLabels.offsetMin.x + (float)num2, this.libDailyLabels.offsetMin.y);
		this.libDailyLabels.offsetMax = new Vector2(this.libDailyLabels.offsetMax.x - (float)num2, this.libDailyLabels.offsetMax.y);
		this.libPromoPoc.sizeDelta = new Vector2(1027f, this.libPromoPoc.sizeDelta.y);
		this.libPromoPocLabels.offsetMin = new Vector2(this.libPromoPocLabels.offsetMin.x + (float)num2, this.libPromoPocLabels.offsetMin.y);
		this.libPromoPocLabels.offsetMax = new Vector2(this.libPromoPocLabels.offsetMax.x - (float)num2, this.libPromoPocLabels.offsetMax.y);
		this.libExternalLink.sizeDelta = new Vector2(1027f, this.libExternalLink.sizeDelta.y);
		this.libExternalLinkLabels.offsetMin = new Vector2(this.libExternalLinkLabels.offsetMin.x + (float)num2, this.libExternalLinkLabels.offsetMin.y);
		this.libExternalLinkLabels.offsetMax = new Vector2(this.libExternalLinkLabels.offsetMax.x - (float)num2, this.libExternalLinkLabels.offsetMax.y);
		this.TabletTabbar();
		this.DailyTab();
		this.MenuTab();
		this.NewsTab();
	}

	private void TabletTabbar()
	{
		float num = 44f;
		int num2 = Mathf.CeilToInt(num * 0.35f);
		int num3 = 620;
		int num4 = 210;
		this.tabBarBtns[0].anchoredPosition = new Vector2((float)(-(float)num3), this.tabBarBtns[0].anchoredPosition.y - (float)num2);
		this.tabBarBtns[1].anchoredPosition = new Vector2((float)(-(float)num3), this.tabBarBtns[1].anchoredPosition.y - (float)num2);
		this.tabBarBtns[2].anchoredPosition = new Vector2((float)(-(float)num4), this.tabBarBtns[2].anchoredPosition.y - (float)num2);
		this.tabBarBtns[3].anchoredPosition = new Vector2((float)(-(float)num4), this.tabBarBtns[3].anchoredPosition.y - (float)num2);
		this.tabBarBtns[4].anchoredPosition = new Vector2((float)num4, this.tabBarBtns[4].anchoredPosition.y - (float)num2);
		this.tabBarBtns[5].anchoredPosition = new Vector2((float)num4, this.tabBarBtns[5].anchoredPosition.y - (float)num2);
		this.tabBarBtns[6].anchoredPosition = new Vector2((float)num3, this.tabBarBtns[6].anchoredPosition.y - (float)num2);
		this.tabBarBtns[7].anchoredPosition = new Vector2((float)num3, this.tabBarBtns[7].anchoredPosition.y - (float)num2);
		float num5 = 0.8f;
		for (int i = 0; i < this.tabBarBtns.Length; i++)
		{
			Text componentInChildren = this.tabBarBtns[i].GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				componentInChildren.fontSize = Mathf.CeilToInt((float)componentInChildren.fontSize * num5);
			}
			Image componentInChildren2 = this.tabBarBtns[i].GetComponentInChildren<Image>();
			if (componentInChildren2 != null)
			{
				RectTransform rectTransform = componentInChildren2.rectTransform;
				rectTransform.sizeDelta = new Vector2((float)Mathf.RoundToInt(rectTransform.sizeDelta.x * num5), (float)Mathf.RoundToInt(rectTransform.sizeDelta.y * num5));
				rectTransform.anchoredPosition -= new Vector2(0f, rectTransform.sizeDelta.y / num5 * (1f - num5) / 2f);
			}
		}
		this.tabbarBackground.sizeDelta = new Vector2(this.tabbarBackground.sizeDelta.x, this.tabbarBackground.sizeDelta.y - num);
		if (this.tabbarNewsIndicators != null)
		{
			for (int j = 0; j < this.tabbarNewsIndicators.Length; j++)
			{
				this.tabbarNewsIndicators[j].anchoredPosition -= new Vector2(0f, 15f);
			}
		}
	}

	private void DailyTab()
	{
		for (int i = 0; i < this.dailyFeaturedEars.Length; i++)
		{
			this.dailyFeaturedEars[i].SetActive(true);
		}
		this.dailyFeatured.sizeDelta = new Vector2(1050f, this.dailyFeatured.sizeDelta.y);
	}

	private void MenuTab()
	{
		int num = 2;
		this.menuTabBtnBtns[0].parent.GetComponent<VerticalLayoutGroup>().spacing = (float)num;
		int num2 = 108;
		int num3 = 80;
		int num4 = 58;
		int num5 = 432;
		int fontSize = 32;
		for (int i = 0; i < this.menuTabBtnBtns.Length; i++)
		{
			this.menuTabBtnBtns[i].sizeDelta = new Vector2(this.menuTabBtnBtns[i].sizeDelta.x, (float)num2);
			Text componentInChildren = this.menuTabBtnBtns[i].GetComponentInChildren<Text>();
			componentInChildren.fontSize = fontSize;
			RectTransform rectTransform = (RectTransform)componentInChildren.transform;
			rectTransform.anchoredPosition = new Vector2((float)num5, rectTransform.anchoredPosition.y);
			RectTransform rectTransform2 = (RectTransform)this.menuTabBtnBtns[i].GetChild(1).GetComponent<Image>().transform;
			rectTransform2.anchoredPosition = new Vector2((float)num3, rectTransform2.anchoredPosition.y);
			rectTransform2.sizeDelta = new Vector2((float)num4, (float)num4);
		}
	}

	private void NewsTab()
	{
		for (int i = 0; i < this.newsRts.Length; i++)
		{
			this.newsRts[i].sizeDelta = new Vector2(1150f, this.newsRts[i].sizeDelta.y);
		}
	}

	[SerializeField]
	private RectTransform libFeaturedSection;

	[SerializeField]
	private RectTransform tabDaily;

	[SerializeField]
	private HorizontalLayoutGroup libFeaturedLayout;

	[SerializeField]
	private RectTransform libFilterMenuBarRt;

	[SerializeField]
	private RectTransform libFBMenuBarRt;

	[SerializeField]
	private RectTransform libDaily;

	[SerializeField]
	private RectTransform libDailyLabels;

	[SerializeField]
	private RectTransform libPromoPoc;

	[SerializeField]
	private RectTransform libPromoPocLabels;

	[SerializeField]
	private RectTransform libExternalLink;

	[SerializeField]
	private RectTransform libExternalLinkLabels;

	[SerializeField]
	private RectTransform[] newsRts;

	[SerializeField]
	private RectTransform tabbarBackground;

	[SerializeField]
	private RectTransform[] tabBarBtns;

	[SerializeField]
	private RectTransform dailyFeatured;

	[SerializeField]
	private GameObject[] dailyFeaturedEars;

	[SerializeField]
	private RectTransform[] menuTabBtnBtns;

	[SerializeField]
	private RectTransform[] tabbarNewsIndicators;

	[SerializeField]
	private Shader uiShader;
}

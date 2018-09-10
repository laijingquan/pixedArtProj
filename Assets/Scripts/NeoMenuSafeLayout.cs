// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class NeoMenuSafeLayout : MonoBehaviour
{
	private void Start()
	{
		if (SafeLayout.IsTablet)
		{
			MenuScreen.RowItems = 3;
		}
		else
		{
			MenuScreen.RowItems = 2;
		}
		base.StartCoroutine(this.FrameDelay(new Action(this.ApplySafeArea)));
	}

	private IEnumerator FrameDelay(Action a)
	{
		yield return new WaitForEndOfFrame();
		a();
		yield break;
	}

	private void ApplySafeArea()
	{
		int num = (int)this.root.rect.height;
		int num2 = SafeLayout.GetMaxBottomCanvasOffset(num);
		int num3 = SafeLayout.GetMinTopCanvasOffset(num);
		bool flag = false;
		bool flag2 = false;
		if (num2 > 0 && num3 > 0)
		{
			for (int i = 0; i < this.notchCovers.Length; i++)
			{
				this.notchCovers[i].gameObject.SetActive(true);
			}
			flag = true;
		}
		bool flag3 = !GeneralSettings.AdsDisabled && AdsManager.Instance.GetBannerPosition() != BannerPosition.None && AdsManager.Instance.HasBannerPlacement(BannerPlacement.Menu);
		if (flag3)
		{
			num3 = ((AdsManager.Instance.GetBannerPosition() != BannerPosition.Top) ? SafeLayout.GetMinTopCanvasOffset(num) : SafeLayout.GetMaxTopCanvasOffset(num));
			BannerPosition bannerPosition = AdsManager.Instance.GetBannerPosition();
			int num4 = AdsManager.Instance.CalcBannerHeight(num);
			if (bannerPosition == BannerPosition.Bottom)
			{
				this.bannerBottomBackground.gameObject.SetActive(true);
				this.bannerBottomBackground.anchoredPosition = new Vector2(this.bannerBottomBackground.anchoredPosition.x, this.bannerBottomBackground.anchoredPosition.y + (float)num2);
				this.bannerBottomBackground.sizeDelta = new Vector2(this.bannerBottomBackground.sizeDelta.x, (float)num4);
				num2 += num4;
			}
			else if (bannerPosition == BannerPosition.Top)
			{
				flag2 = true;
				this.bannerTopBackground.gameObject.SetActive(true);
				int num5 = (!flag) ? 0 : num3;
				this.bannerTopBackground.sizeDelta = new Vector2(this.bannerTopBackground.sizeDelta.x, (float)(num4 + num5));
				num3 += num4;
			}
		}
		this.root.offsetMin = new Vector2(this.root.offsetMin.x, (float)num2);
		this.root.offsetMax = new Vector2(this.root.offsetMax.x, (float)(-(float)num3));
		for (int j = 0; j < this.bgExt.Length; j++)
		{
			this.bgExt[j].offsetMin += new Vector2(0f, (float)(-(float)num2));
			this.bgExt[j].offsetMax += new Vector2(0f, (float)num3);
		}
		if (num3 > 0)
		{
			this.settingsPopup.SetSafeLayoutOffset(num3);
		}
		if (num2 > 0)
		{
			this.startGameSlidePopup.SetSafeLayoutOffset(num2);
		}
		if (flag && !flag2)
		{
			this.catFilterBackground.offsetMax = new Vector2(this.root.offsetMax.x, (float)num3);
		}
	}

	public RectTransform root;

	public GameObject[] notchCovers;

	public RectTransform bannerTopBackground;

	public RectTransform bannerBottomBackground;

	public RectTransform[] bgExt;

	[SerializeField]
	private SettingsPopup settingsPopup;

	[SerializeField]
	private StartGameSlidePopup startGameSlidePopup;

	[SerializeField]
	private RectTransform catFilterBackground;
}

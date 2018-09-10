// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GameSafeLayout : MonoBehaviour
{
	public void ShowBannerBackground()
	{
		if (AdsManager.Instance.GetBannerPosition() == BannerPosition.Bottom)
		{
			this.bannerBottomBackground.gameObject.SetActive(true);
		}
		else if (AdsManager.Instance.GetBannerPosition() == BannerPosition.Top)
		{
			this.bannerTopBackground.gameObject.SetActive(true);
		}
	}

	public void HideBannerBackground()
	{
		if (AdsManager.Instance.GetBannerPosition() == BannerPosition.Bottom)
		{
			this.bannerBottomBackground.gameObject.SetActive(false);
		}
		else if (AdsManager.Instance.GetBannerPosition() == BannerPosition.Top)
		{
			this.bannerTopBackground.gameObject.SetActive(false);
		}
	}

	public void ApplySafeArea()
	{
		int num = (int)this.root.rect.height;
		bool flag = !GeneralSettings.AdsDisabled && AdsManager.Instance.GetBannerPosition() != BannerPosition.None && (AdsManager.Instance.HasBannerPlacement(BannerPlacement.Gameboard) || AdsManager.Instance.HasBannerPlacement(BannerPlacement.Solved));
		int num2 = SafeLayout.GetMaxBottomCanvasOffset(num);
		int num3 = SafeLayout.GetMinTopCanvasOffset(num);
		bool flag2 = num2 > 0 && num3 > 0;
		int num4 = AdsManager.Instance.CalcBannerHeight(num);
		if (flag)
		{
			num3 = ((AdsManager.Instance.GetBannerPosition() != BannerPosition.Top) ? SafeLayout.GetMinTopCanvasOffset(num) : SafeLayout.GetMaxTopCanvasOffset(num));
			if (AdsManager.Instance.GetBannerPosition() == BannerPosition.Bottom)
			{
				this.bannerBottomBackground.anchoredPosition = new Vector2(this.bannerBottomBackground.anchoredPosition.x, this.bannerBottomBackground.anchoredPosition.y + (float)num2);
				this.bannerBottomBackground.sizeDelta = new Vector2(this.bannerBottomBackground.sizeDelta.x, (float)num4);
				num2 += num4;
				if (AdsManager.Instance.HasBannerPlacement(BannerPlacement.Gameboard))
				{
					this.bottomControls.anchoredPosition += new Vector2(0f, (float)num2);
				}
			}
			else if (AdsManager.Instance.GetBannerPosition() == BannerPosition.Top)
			{
				int num5 = (!flag2) ? 0 : num3;
				this.bannerTopBackground.sizeDelta = new Vector2(this.bannerTopBackground.sizeDelta.x, (float)(num4 + num5));
				num3 += num4;
				if (AdsManager.Instance.HasBannerPlacement(BannerPlacement.Gameboard))
				{
					this.topControls.anchoredPosition += new Vector2(0f, (float)(-(float)num3));
				}
				if (AdsManager.Instance.HasBannerPlacement(BannerPlacement.Gameboard) || AdsManager.Instance.HasBannerPlacement(BannerPlacement.Solved))
				{
					this.toastRoot.offsetMax = new Vector2(this.root.offsetMax.x, (float)(-(float)((int)((float)num3 * 0.5f))));
				}
			}
			if (AdsManager.Instance.HasBannerPlacement(BannerPlacement.Solved))
			{
				this.solvedPageControls.SetSafeLayoutOffset(AdsManager.Instance.GetBannerPosition(), num3, num2);
			}
		}
		else
		{
			this.bottomControls.anchoredPosition += new Vector2(0f, (float)num2);
		}
		if (flag2)
		{
			int num6 = 10;
			if (GeneralSettings.IsOldDesign)
			{
				for (int i = 0; i < this.topLeftButtons.Length; i++)
				{
					if (this.topLeftButtons[i] != null)
					{
						this.topLeftButtons[i].anchoredPosition += new Vector2((float)num6, (float)(-(float)num6));
					}
				}
				for (int j = 0; j < this.topRightButtons.Length; j++)
				{
					if (this.topRightButtons[j] != null)
					{
						this.topRightButtons[j].anchoredPosition += new Vector2((float)(-(float)num6), (float)(-(float)num6));
					}
				}
			}
			if (!flag || !AdsManager.Instance.HasBannerPlacement(BannerPlacement.Solved) || AdsManager.Instance.GetBannerPosition() != BannerPosition.Bottom)
			{
				this.solvedPageControls.SetSafeLayoutExtraBottomOffset(SafeLayout.GetMaxBottomCanvasOffset(num) + 30);
			}
		}
	}

	public RectTransform root;

	[SerializeField]
	private RectTransform bottomControls;

	[SerializeField]
	private RectTransform toastRoot;

	[SerializeField]
	private RectTransform topControls;

	[SerializeField]
	private SolvedPageControls solvedPageControls;

	public RectTransform bannerTopBackground;

	public RectTransform bannerBottomBackground;

	[SerializeField]
	private RectTransform[] topLeftButtons;

	[SerializeField]
	private RectTransform[] topRightButtons;
}

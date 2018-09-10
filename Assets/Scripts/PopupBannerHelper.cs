// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PopupBannerHelper : MonoBehaviour
{
	private void OnEnable()
	{
		if (AdsManager.Instance.CurrentBannerPlacement != BannerPlacement.Unknown)
		{
			AdsManager.Instance.HideBanner(false);
		}
	}

	private void OnDisable()
	{
		if (AdsManager.Instance.CurrentBannerPlacement != BannerPlacement.Unknown)
		{
			AdsManager.Instance.ShowBanner(AdsManager.Instance.CurrentBannerPlacement);
		}
	}
}

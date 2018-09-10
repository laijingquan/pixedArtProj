// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FAdsIOS
{
	static FAdsIOS()
	{
		FAdsIOS.InitManager();
	}

	protected static void InitManager()
	{
		Type typeFromHandle = typeof(FAdsManager);
		new GameObject("FAdsManager", new Type[]
		{
			typeFromHandle
		}).GetComponent<FAdsManager>();
	}

	public static void SetBannerPosition(BannerPosition bannerPosition)
	{
	}

	public static void InitializeSdk()
	{
	}

	public static void SetAdUnits(string banner, string interstitial, string reward)
	{
		FAdsManager.Instance.FakeInit();
	}

	public static void SetInterstitialDelays(double stepDelay, double postRewardDelay)
	{
	}

	public static void ShowBanner()
	{
	}

	public static void HideBanner()
	{
	}

	public static bool HasInterstitial()
	{
		return false;
	}

	public static void ShowInterstitial()
	{
	}

	public static void LoadRewarded()
	{
	}

	public static bool HasRewarded()
	{
		return true;
	}

	public static void ShowRewarded()
	{
	}

	public static int CurrentConsentStatus()
	{
		return 0;
	}

	public static void GrantConsent()
	{
	}

	public static bool ShouldShowConsentDialog()
	{
		return false;
	}

	public static int IsGDPRApplicable()
	{
		return 0;
	}

	public static void DisableAds()
	{
	}

	public static void SetTestMode()
	{
	}

	public static void SetBannerPrecacheDelays(int showTime, int preCacheDelay)
	{
	}
}

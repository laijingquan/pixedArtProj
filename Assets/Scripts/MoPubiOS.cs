// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoPubiOS : MoPubBase
{
	static MoPubiOS()
	{
		MoPubBase.InitManager();
	}

	public static void InitializeSdk(string anyAdUnitId)
	{
		MoPubBase.ValidateAdUnitForSdkInit(anyAdUnitId);
		MoPubiOS.InitializeSdk(new MoPubBase.SdkConfiguration
		{
			AdUnitId = anyAdUnitId
		});
	}

	public static void InitializeSdk(MoPubBase.SdkConfiguration sdkConfiguration)
	{
		MoPubBase.ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
	}

	public static void LoadBannerPluginsForAdUnits(string[] adUnitIds)
	{
		MoPubiOS.LoadPluginsForAdUnits(adUnitIds);
	}

	public static void LoadInterstitialPluginsForAdUnits(string[] adUnitIds)
	{
		MoPubiOS.LoadPluginsForAdUnits(adUnitIds);
	}

	public static void LoadRewardedVideoPluginsForAdUnits(string[] adUnitIds)
	{
		MoPubiOS.LoadPluginsForAdUnits(adUnitIds);
	}

	public static bool IsSdkInitialized
	{
		get
		{
			return true;
		}
	}

	public static bool AdvancedBiddingEnabled
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static void EnableLocationSupport(bool shouldUseLocation)
	{
	}

	public static void ReportApplicationOpen(string iTunesAppId = null)
	{
	}

	protected static string GetSdkName()
	{
		return "iOS SDK v";
	}

	private static void LoadPluginsForAdUnits(string[] adUnitIds)
	{
		//foreach (string text in adUnitIds)
		//{
		//	UnityEngine.Debug.Log(adUnitIds.Length + " AdUnits loaded for plugins:\n" + string.Join(", ", adUnitIds));
		//}
	}

	public static MoPubBase.LogLevel SdkLogLevel
	{
		get
		{
			return MoPubBase.LogLevel.MPLogLevelAll;
		}
		set
		{
		}
	}

	public static void ForceWKWebView(bool shouldForce)
	{
	}

	public static void CreateBanner(string adUnitId, MoPubBase.AdPosition position, MoPubBase.BannerType bannerType = MoPubBase.BannerType.Size320x50)
	{
	}

	public static void ShowBanner(string adUnitId, bool shouldShow)
	{
	}

	public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
	{
	}

	public void SetAutorefresh(string adUnitId, bool enabled)
	{
	}

	public void ForceRefresh(string adUnitId)
	{
	}

	public static void DestroyBanner(string adUnitId)
	{
	}

	public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
	{
	}

	public static void ShowInterstitialAd(string adUnitId)
	{
	}

	public bool IsInterstialReady(string adUnitId)
	{
		return false;
	}

	public void DestroyInterstitialAd(string adUnitId)
	{
	}

	public static void RequestRewardedVideo(string adUnitId, List<MoPubBase.MediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
	}

	public static void ShowRewardedVideo(string adUnitId, string customData = null)
	{
	}

	public static bool HasRewardedVideo(string adUnitId)
	{
		return false;
	}

	public static List<MoPubBase.Reward> GetAvailableRewards(string adUnitId)
	{
		return null;
	}

	public static void SelectReward(string adUnitId, MoPubBase.Reward selectedReward)
	{
	}

	public static bool CanCollectPersonalInfo
	{
		get
		{
			return false;
		}
	}

	public static MoPubBase.Consent.Status CurrentConsentStatus
	{
		get
		{
			return MoPubBase.Consent.Status.Unknown;
		}
	}

	public static bool ShouldShowConsentDialog
	{
		get
		{
			return true;
		}
	}

	public static void LoadConsentDialog()
	{
	}

	public static bool IsConsentDialogLoaded
	{
		get
		{
			return false;
		}
	}

	public static void ShowConsentDialog()
	{
	}

	public static bool? IsGdprApplicable
	{
		get
		{
			int num = 1;
			return (num != 0) ? ((num <= 0) ? new bool?(false) : new bool?(true)) : null;
		}
	}

	public static class PartnerApi
	{
		public static void GrantConsent()
		{
		}

		public static void RevokeConsent()
		{
		}

		public static Uri CurrentConsentPrivacyPolicyUrl
		{
			get
			{
				string empty = string.Empty;
				return MoPubBase.UrlFromString(empty);
			}
		}

		public static Uri CurrentVendorListUrl
		{
			get
			{
				string empty = string.Empty;
				return MoPubBase.UrlFromString(empty);
			}
		}

		public static string CurrentConsentIabVendorListFormat
		{
			get
			{
				return string.Empty;
			}
		}

		public static string CurrentConsentPrivacyPolicyVersion
		{
			get
			{
				return string.Empty;
			}
		}

		public static string CurrentConsentVendorListVersion
		{
			get
			{
				return string.Empty;
			}
		}

		public static string PreviouslyConsentedIabVendorListFormat
		{
			get
			{
				return string.Empty;
			}
		}

		public static string PreviouslyConsentedPrivacyPolicyVersion
		{
			get
			{
				return string.Empty;
			}
		}

		public static string PreviouslyConsentedVendorListVersion
		{
			get
			{
				return string.Empty;
			}
		}
	}
}

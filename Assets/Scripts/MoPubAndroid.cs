// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoPubAndroid : MoPubBase
{
	static MoPubAndroid()
	{
		MoPubBase.InitManager();
	}

	public static void InitializeSdk(string anyAdUnitId)
	{
		MoPubBase.ValidateAdUnitForSdkInit(anyAdUnitId);
		MoPubAndroid.InitializeSdk(new MoPubBase.SdkConfiguration
		{
			AdUnitId = anyAdUnitId
		});
	}

	public static void InitializeSdk(MoPubBase.SdkConfiguration sdkConfiguration)
	{
		MoPubBase.ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
		MoPubAndroid.PluginClass.CallStatic("initializeSdk", new object[]
		{
			sdkConfiguration.AdUnitId,
			sdkConfiguration.AdvancedBiddersString,
			sdkConfiguration.MediationSettingsJson,
			sdkConfiguration.NetworksToInitString
		});
	}

	public static void LoadBannerPluginsForAdUnits(string[] bannerAdUnitIds)
	{
		foreach (string text in bannerAdUnitIds)
		{
			MoPubAndroid.BannerPluginsDict.Add(text, new MoPubAndroidBanner(text));
		}
		UnityEngine.Debug.Log(bannerAdUnitIds.Length + " banner AdUnits loaded for plugins:\n" + string.Join(", ", bannerAdUnitIds));
	}

	public static void LoadInterstitialPluginsForAdUnits(string[] interstitialAdUnitIds)
	{
		foreach (string text in interstitialAdUnitIds)
		{
			MoPubAndroid.InterstitialPluginsDict.Add(text, new MoPubAndroidInterstitial(text));
		}
		UnityEngine.Debug.Log(interstitialAdUnitIds.Length + " interstitial AdUnits loaded for plugins:\n" + string.Join(", ", interstitialAdUnitIds));
	}

	public static void LoadRewardedVideoPluginsForAdUnits(string[] rewardedVideoAdUnitIds)
	{
		foreach (string text in rewardedVideoAdUnitIds)
		{
			MoPubAndroid.RewardedVideoPluginsDict.Add(text, new MoPubAndroidRewardedVideo(text));
		}
		UnityEngine.Debug.Log(rewardedVideoAdUnitIds.Length + " rewarded video AdUnits loaded for plugins:\n" + string.Join(", ", rewardedVideoAdUnitIds));
	}

	public static bool IsSdkInitialized
	{
		get
		{
			return MoPubAndroid.PluginClass.CallStatic<bool>("isSdkInitialized", new object[0]);
		}
	}

	public static bool AdvancedBiddingEnabled
	{
		get
		{
			return MoPubAndroid.PluginClass.CallStatic<bool>("isAdvancedBiddingEnabled", new object[0]);
		}
		set
		{
			MoPubAndroid.PluginClass.CallStatic("setAdvancedBiddingEnabled", new object[]
			{
				value
			});
		}
	}

	public static void EnableLocationSupport(bool shouldUseLocation)
	{
		MoPubAndroid.PluginClass.CallStatic("setLocationAwareness", new object[]
		{
			MoPubAndroid.LocationAwareness.NORMAL.ToString()
		});
	}

	public static void ReportApplicationOpen(string iTunesAppId = null)
	{
		MoPubAndroid.PluginClass.CallStatic("reportApplicationOpen", new object[0]);
	}

	protected static string GetSdkName()
	{
		return "Android SDK v" + MoPubAndroid.PluginClass.CallStatic<string>("getSDKVersion", new object[0]);
	}

	public static void AddFacebookTestDeviceId(string hashedDeviceId)
	{
		MoPubAndroid.PluginClass.CallStatic("addFacebookTestDeviceId", new object[]
		{
			hashedDeviceId
		});
	}

	public static void CreateBanner(string adUnitId, MoPubBase.AdPosition position)
	{
		MoPubAndroidBanner moPubAndroidBanner;
		if (MoPubAndroid.BannerPluginsDict.TryGetValue(adUnitId, out moPubAndroidBanner))
		{
			moPubAndroidBanner.CreateBanner(position);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void ShowBanner(string adUnitId, bool shouldShow)
	{
		MoPubAndroidBanner moPubAndroidBanner;
		if (MoPubAndroid.BannerPluginsDict.TryGetValue(adUnitId, out moPubAndroidBanner))
		{
			moPubAndroidBanner.ShowBanner(shouldShow);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
	{
		MoPubAndroidBanner moPubAndroidBanner;
		if (MoPubAndroid.BannerPluginsDict.TryGetValue(adUnitId, out moPubAndroidBanner))
		{
			moPubAndroidBanner.RefreshBanner(keywords, userDataKeywords);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public void SetAutorefresh(string adUnitId, bool enabled)
	{
		MoPubAndroidBanner moPubAndroidBanner;
		if (MoPubAndroid.BannerPluginsDict.TryGetValue(adUnitId, out moPubAndroidBanner))
		{
			moPubAndroidBanner.SetAutorefresh(enabled);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public void ForceRefresh(string adUnitId)
	{
		MoPubAndroidBanner moPubAndroidBanner;
		if (MoPubAndroid.BannerPluginsDict.TryGetValue(adUnitId, out moPubAndroidBanner))
		{
			moPubAndroidBanner.ForceRefresh();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void DestroyBanner(string adUnitId)
	{
		MoPubAndroidBanner moPubAndroidBanner;
		if (MoPubAndroid.BannerPluginsDict.TryGetValue(adUnitId, out moPubAndroidBanner))
		{
			moPubAndroidBanner.DestroyBanner();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
	{
		MoPubAndroidInterstitial moPubAndroidInterstitial;
		if (MoPubAndroid.InterstitialPluginsDict.TryGetValue(adUnitId, out moPubAndroidInterstitial))
		{
			moPubAndroidInterstitial.RequestInterstitialAd(keywords, userDataKeywords);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void ShowInterstitialAd(string adUnitId)
	{
		MoPubAndroidInterstitial moPubAndroidInterstitial;
		if (MoPubAndroid.InterstitialPluginsDict.TryGetValue(adUnitId, out moPubAndroidInterstitial))
		{
			moPubAndroidInterstitial.ShowInterstitialAd();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public bool IsInterstialReady(string adUnitId)
	{
		MoPubAndroidInterstitial moPubAndroidInterstitial;
		if (MoPubAndroid.InterstitialPluginsDict.TryGetValue(adUnitId, out moPubAndroidInterstitial))
		{
			return moPubAndroidInterstitial.IsInterstitialReady;
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return false;
	}

	public void DestroyInterstitialAd(string adUnitId)
	{
		MoPubAndroidInterstitial moPubAndroidInterstitial;
		if (MoPubAndroid.InterstitialPluginsDict.TryGetValue(adUnitId, out moPubAndroidInterstitial))
		{
			moPubAndroidInterstitial.DestroyInterstitialAd();
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void RequestRewardedVideo(string adUnitId, List<MoPubBase.MediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
		MoPubAndroidRewardedVideo moPubAndroidRewardedVideo;
		if (MoPubAndroid.RewardedVideoPluginsDict.TryGetValue(adUnitId, out moPubAndroidRewardedVideo))
		{
			moPubAndroidRewardedVideo.RequestRewardedVideo(mediationSettings, keywords, userDataKeywords, latitude, longitude, customerId);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static void ShowRewardedVideo(string adUnitId, string customData = null)
	{
		MoPubAndroidRewardedVideo moPubAndroidRewardedVideo;
		if (MoPubAndroid.RewardedVideoPluginsDict.TryGetValue(adUnitId, out moPubAndroidRewardedVideo))
		{
			moPubAndroidRewardedVideo.ShowRewardedVideo(customData);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static bool HasRewardedVideo(string adUnitId)
	{
		MoPubAndroidRewardedVideo moPubAndroidRewardedVideo;
		if (MoPubAndroid.RewardedVideoPluginsDict.TryGetValue(adUnitId, out moPubAndroidRewardedVideo))
		{
			return moPubAndroidRewardedVideo.HasRewardedVideo();
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return false;
	}

	public static List<MoPubBase.Reward> GetAvailableRewards(string adUnitId)
	{
		MoPubAndroidRewardedVideo moPubAndroidRewardedVideo;
		if (MoPubAndroid.RewardedVideoPluginsDict.TryGetValue(adUnitId, out moPubAndroidRewardedVideo))
		{
			return moPubAndroidRewardedVideo.GetAvailableRewards();
		}
		MoPubBase.ReportAdUnitNotFound(adUnitId);
		return null;
	}

	public static void SelectReward(string adUnitId, MoPubBase.Reward selectedReward)
	{
		MoPubAndroidRewardedVideo moPubAndroidRewardedVideo;
		if (MoPubAndroid.RewardedVideoPluginsDict.TryGetValue(adUnitId, out moPubAndroidRewardedVideo))
		{
			moPubAndroidRewardedVideo.SelectReward(selectedReward);
		}
		else
		{
			MoPubBase.ReportAdUnitNotFound(adUnitId);
		}
	}

	public static bool CanCollectPersonalInfo
	{
		get
		{
			return MoPubAndroid.PluginClass.CallStatic<bool>("canCollectPersonalInfo", new object[0]);
		}
	}

	public static MoPubBase.Consent.Status CurrentConsentStatus
	{
		get
		{
			return MoPubBase.Consent.FromString(MoPubAndroid.PluginClass.CallStatic<string>("getPersonalInfoConsentState", new object[0]));
		}
	}

	public static bool ShouldShowConsentDialog
	{
		get
		{
			return MoPubAndroid.PluginClass.CallStatic<bool>("shouldShowConsentDialog", new object[0]);
		}
	}

	public static void LoadConsentDialog()
	{
		MoPubAndroid.PluginClass.CallStatic("loadConsentDialog", new object[0]);
	}

	public static bool IsConsentDialogLoaded
	{
		get
		{
			return MoPubAndroid.PluginClass.CallStatic<bool>("isConsentDialogLoaded", new object[0]);
		}
	}

	public static void ShowConsentDialog()
	{
		MoPubAndroid.PluginClass.CallStatic("showConsentDialog", new object[0]);
	}

	public static bool? IsGdprApplicable
	{
		get
		{
			int num = MoPubAndroid.PluginClass.CallStatic<int>("gdprApplies", new object[0]);
			return (num != 0) ? ((num <= 0) ? new bool?(false) : new bool?(true)) : null;
		}
	}

	private static readonly AndroidJavaClass PluginClass = new AndroidJavaClass("com.mopub.unity.MoPubUnityPlugin");

	private static readonly Dictionary<string, MoPubAndroidBanner> BannerPluginsDict = new Dictionary<string, MoPubAndroidBanner>();

	private static readonly Dictionary<string, MoPubAndroidInterstitial> InterstitialPluginsDict = new Dictionary<string, MoPubAndroidInterstitial>();

	private static readonly Dictionary<string, MoPubAndroidRewardedVideo> RewardedVideoPluginsDict = new Dictionary<string, MoPubAndroidRewardedVideo>();

	public enum LocationAwareness
	{
		TRUNCATED,
		DISABLED,
		NORMAL
	}

	public static class PartnerApi
	{
		public static void GrantConsent()
		{
			MoPubAndroid.PluginClass.CallStatic("grantConsent", new object[0]);
		}

		public static void RevokeConsent()
		{
			MoPubAndroid.PluginClass.CallStatic("revokeConsent", new object[0]);
		}

		public static Uri CurrentConsentPrivacyPolicyUrl
		{
			get
			{
				return MoPubBase.UrlFromString(MoPubAndroid.PluginClass.CallStatic<string>("getCurrentPrivacyPolicyLink", new object[]
				{
					MoPubBase.ConsentLanguageCode
				}));
			}
		}

		public static Uri CurrentVendorListUrl
		{
			get
			{
				return MoPubBase.UrlFromString(MoPubAndroid.PluginClass.CallStatic<string>("getCurrentVendorListLink", new object[]
				{
					MoPubBase.ConsentLanguageCode
				}));
			}
		}

		public static string CurrentConsentIabVendorListFormat
		{
			get
			{
				return MoPubAndroid.PluginClass.CallStatic<string>("getCurrentVendorListIabFormat", new object[0]);
			}
		}

		public static string CurrentConsentPrivacyPolicyVersion
		{
			get
			{
				return MoPubAndroid.PluginClass.CallStatic<string>("getCurrentPrivacyPolicyVersion", new object[0]);
			}
		}

		public static string CurrentConsentVendorListVersion
		{
			get
			{
				return MoPubAndroid.PluginClass.CallStatic<string>("getCurrentVendorListVersion", new object[0]);
			}
		}

		public static string PreviouslyConsentedIabVendorListFormat
		{
			get
			{
				return MoPubAndroid.PluginClass.CallStatic<string>("getConsentedVendorListIabFormat", new object[0]);
			}
		}

		public static string PreviouslyConsentedPrivacyPolicyVersion
		{
			get
			{
				return MoPubAndroid.PluginClass.CallStatic<string>("getConsentedPrivacyPolicyVersion", new object[0]);
			}
		}

		public static string PreviouslyConsentedVendorListVersion
		{
			get
			{
				return MoPubAndroid.PluginClass.CallStatic<string>("getConsentedVendorListVersion", new object[0]);
			}
		}
	}
}

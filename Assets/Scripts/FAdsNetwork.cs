// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class FAdsNetwork : AdsManager.IAds, IConsentProvider
{
	 
	public event Action ConsentInitialized;

	public string PolicyURL { get; private set; }

	public string VendorsURL { get; private set; }

	 
	public event EventHandler RewardedComplete;

	public void Configure(bool test, int bShowTime, int bPrecacheDelay, int fsDelayOnReward)
	{
		this.testMode = test;
		this.bannePrecacheDelay = bPrecacheDelay;
		this.bannerShowTime = bShowTime;
		this.fsIntervalOnReward = fsDelayOnReward;
	}

	public void Initialize(string bAdUnit, BannerPosition pos, string fsAdUnit, string rewardAdUnit)
	{
		this.InternalInit();
		if (!string.IsNullOrEmpty(bAdUnit))
		{
			try
			{
				this.bannerAdUnit = bAdUnit;
				this.bannerEnabled = true;
				this.bannerPosition = pos;
				FAdsIOS.SetBannerPosition(pos);
			}
			catch (Exception ex)
			{
				this.bannerEnabled = false;
				FMLogger.vAds("failed to SetBannerPosition fads banner. error: " + ex.Message);
			}
		}
		if (!string.IsNullOrEmpty(fsAdUnit))
		{
			this.interstitialAdUnit = fsAdUnit;
			this.fsEnabled = true;
		}
		if (!string.IsNullOrEmpty(rewardAdUnit))
		{
			this.rewardUnit = rewardAdUnit;
			this.rewardedEnabled = true;
		}
		try
		{
			if (this.testMode)
			{
				FAdsIOS.SetTestMode();
				FMLogger.vCore("FAds testMode");
			}
			FAdsIOS.SetAdUnits(this.bannerAdUnit, this.interstitialAdUnit, this.rewardUnit);
			FAdsIOS.SetBannerPrecacheDelays(this.bannerShowTime, this.bannePrecacheDelay);
			FAdsIOS.SetInterstitialDelays(5.0, (double)this.fsIntervalOnReward);
			FMLogger.vCore(string.Concat(new object[]
			{
				"FAds set delays bst:",
				this.bannerShowTime,
				" bpd:",
				this.bannePrecacheDelay,
				" fdr:",
				this.fsIntervalOnReward
			}));
		}
		catch (Exception ex2)
		{
			FMLogger.vCore("FAds fail to set adUnits. " + ex2.Message);
		}
	}

	public void InitializeBanner(string r, BannerPosition pos)
	{
	}

	public void RequestBanner()
	{
	}

	public void ShowBanner()
	{
		if (!this.bannerEnabled)
		{
			return;
		}
		FMLogger.vAds("show banner inv");
		FAdsIOS.ShowBanner();
	}

	public void HideBanner()
	{
		if (!this.bannerEnabled)
		{
			return;
		}
		FMLogger.vAds("hide banner inv");
		FAdsIOS.HideBanner();
	}

	public void UtilizeBanner()
	{
		if (!this.bannerEnabled)
		{
			return;
		}
		this.bannerEnabled = false;
		FAdsIOS.HideBanner();
	}

	public void RequestInterstitial()
	{
	}

	public bool HasInterstitial()
	{
		return this.fsEnabled && FAdsIOS.HasInterstitial();
	}

	public void ShowInterstitial(AdPlacement fsPlacement)
	{
		if (!this.fsEnabled)
		{
			return;
		}
		this.fsAdPlacement = AdConfig.AdPlacementToString(fsPlacement);
		FMLogger.vAds("fs show inv");
		FAdsIOS.ShowInterstitial();
	}

	public void InitializeRewarded(string rewardAdUnit)
	{
		if (this.rewardedEnabled)
		{
			return;
		}
		if (!string.IsNullOrEmpty(rewardAdUnit))
		{
			this.rewardUnit = rewardAdUnit;
			this.rewardedEnabled = true;
		}
		try
		{
			FAdsIOS.SetAdUnits(this.bannerAdUnit, this.interstitialAdUnit, this.rewardUnit);
		}
		catch (Exception ex)
		{
			FMLogger.vCore("FAds. reward init. fail to set adUnits. " + ex.Message);
		}
	}

	public void RequestRewardedVideo()
	{
		if (!this.rewardedEnabled)
		{
			return;
		}
		this.autoloadRewarded = true;
		FMLogger.vAds("start reward request");
		FAdsIOS.LoadRewarded();
	}

	public void SingleRequestRewardedVideo()
	{
		if (!this.rewardedEnabled)
		{
			return;
		}
		if (this.autoloadRewarded)
		{
			return;
		}
		this.RequestRewardedVideo();
	}

	public bool HasRewardedVideo()
	{
		return this.rewardedEnabled && FAdsIOS.HasRewarded();
	}

	public void ShowRewardedVideo()
	{
		if (!this.rewardedEnabled)
		{
			return;
		}
		FMLogger.vAds("show rewarded");
		AdsManager.Instance.rewardInfo.IterateImpression();
		FAdsIOS.ShowRewarded();
	}

	private void OnRewardCompleted()
	{
		if (this.RewardedComplete != null)
		{
			this.RewardedComplete(null, EventArgs.Empty);
		}
	}

	public void Utilize()
	{
		FAdsIOS.DisableAds();
	}

	private void InternalInit()
	{
		if (this.inted)
		{
			return;
		}
		this.inted = true;
		FAdsManager.RewardedCompleted += this.OnRewardCompleted;
		FAdsManager.Initialized += this.OnInitialized;
		FAdsManager.AdsEventReceived += this.OnAdsEvent;
		FAdsIOS.InitializeSdk();
		FAdsIOS.SetInterstitialDelays(5.0, 5.0);
	}

	private void OnAdsEvent(FAdsEventData data)
	{
		if (data == null)
		{
			return;
		}
		if (data.eventName == "ad_banner_request")
		{
			return;
		}
		bool firebaseOnly = false;
		Dictionary<string, string> dictionary = data.EventDataToDictionary();
		string eventName = data.eventName;
		switch (eventName)
		{
		case "ad_banner_impression":
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, string>();
			}
			dictionary.Add("banner_shown_type", (!AdsManager.Instance.IsTabletAdUnit) ? "phone" : "tablet");
			dictionary.Add("banner_position", (this.bannerPosition != BannerPosition.Bottom) ? "top" : "bottom");
			break;
		case "ad_interstitial_request":
			firebaseOnly = true;
			break;
		case "ad_interstitial_show":
		case "ad_interstitial_click":
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, string>();
			}
			dictionary.Add("placement", this.fsAdPlacement);
			break;
		case "ad_rewarded_show":
		{
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, string>();
			}
			dictionary.Add("place", AdsManager.Instance.rewardInfo.place.ToString().ToLower());
			dictionary.Add("count_d", AdsManager.Instance.rewardInfo.ImpDay.ToString());
			Dictionary<string, string> dictionary2 = dictionary;
			string key = "count_s";
			int impSession = AdsManager.Instance.rewardInfo.ImpSession;
			dictionary2.Add(key, impSession.ToString());
			break;
		}
		case "ad_rewarded_request":
			firebaseOnly = true;
			AdsManager.Instance.rewardInfo.IterateRequest();
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, string>();
			}
			dictionary.Add("count_d", AdsManager.Instance.rewardInfo.ReqDay.ToString());
			dictionary.Add("count_s", AdsManager.Instance.rewardInfo.ReqSession.ToString());
			break;
		case "ad_rewarded_cached":
			AdsManager.Instance.rewardInfo.IterateLoaded();
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, string>();
			}
			dictionary.Add("count_d", AdsManager.Instance.rewardInfo.LoadedDay.ToString());
			dictionary.Add("count_s", AdsManager.Instance.rewardInfo.LoadedSession.ToString());
			break;
		}
		if (dictionary != null)
		{
			AnalyticsManager.AdsEventLog(data.eventName, dictionary, firebaseOnly);
		}
		else
		{
			AnalyticsManager.AdsEventLog(data.eventName, firebaseOnly);
		}
	}

	private void OnInitialized(FAdsInitData data)
	{
		this.PolicyURL = ((data == null) ? string.Empty : data.privacyPolicyUrl);
		this.VendorsURL = ((data == null) ? string.Empty : data.vendorListUrl);
		if (this.ConsentInitialized != null)
		{
			this.ConsentInitialized();
		}
	}

	public void InitForConsent(string fsAdsUnit)
	{
		this.InternalInit();
		FAdsIOS.SetAdUnits(null, fsAdsUnit, null);
	}

	public FAdsNetwork.ConsentStatus CurrentConsentStatus
	{
		get
		{
			FAdsNetwork.ConsentStatus result = FAdsNetwork.ConsentStatus.Unknown;
			int num = FAdsIOS.CurrentConsentStatus();
			if (Enum.IsDefined(typeof(FAdsNetwork.ConsentStatus), num))
			{
				result = (FAdsNetwork.ConsentStatus)num;
			}
			return result;
		}
	}

	public void GrantConsent()
	{
		FAdsIOS.GrantConsent();
	}

	public bool ShouldShowConsentDialog
	{
		get
		{
			return FAdsIOS.ShouldShowConsentDialog();
		}
	}

	public bool? IsGDPRApplicable
	{
		get
		{
			int num = FAdsIOS.IsGDPRApplicable();
			return (num != 0) ? ((num <= 0) ? new bool?(false) : new bool?(true)) : null;
		}
	}

	private bool bannerEnabled;

	private bool fsEnabled;

	private bool rewardedEnabled;

	private string bannerAdUnit;

	private string interstitialAdUnit;

	private string rewardUnit;

	private string fsAdPlacement;

	private bool autoloadRewarded;

	private BannerPosition bannerPosition;

	private int bannerShowTime;

	private int bannePrecacheDelay;

	private int fsIntervalOnReward;

	private bool testMode;

	private bool inted;

	public enum ConsentStatus
	{
		Unknown,
		Denied,
		DoNotTrack,
		PotentialWhitelist,
		Consented
	}
}

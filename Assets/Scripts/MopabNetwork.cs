// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class MopabNetwork : AdsManager.IAds
{
	public MopabNetwork(Func<IEnumerator, Coroutine> coroutineProxy)
	{
		this.coroutineProxy = coroutineProxy;
	}

	 
	public event EventHandler RewardedComplete;

	public void Initialize(string bAdUnit, BannerPosition bannerPos, string fsAdUnit, string rewardAdUnit)
	{
		MoPubAndroid.EnableLocationSupport(true);
		MoPubManager.OnAdLoadedEvent += this.onAdLoadedEvent;
		MoPubManager.OnAdFailedEvent += this.onAdFailedEvent;
		MoPubManager.OnAdClickedEvent += this.onAdClickedEvent;
		MoPubManager.OnAdExpandedEvent += this.onAdExpandedEvent;
		MoPubManager.OnAdCollapsedEvent += this.onAdCollapsedEvent;
		MoPubManager.OnInterstitialLoadedEvent += this.onInterstitialLoadedEvent;
		MoPubManager.OnInterstitialFailedEvent += this.onInterstitialFailedEvent;
		MoPubManager.OnInterstitialShownEvent += this.onInterstitialShownEvent;
		MoPubManager.OnInterstitialClickedEvent += this.onInterstitialClickedEvent;
		MoPubManager.OnInterstitialDismissedEvent += this.onInterstitialDismissedEvent;
		MoPubManager.OnInterstitialExpiredEvent += this.onInterstitialExpiredEvent;
		MoPubManager.OnRewardedVideoLoadedEvent += this.onRewardedVideoLoadedEvent;
		MoPubManager.OnRewardedVideoFailedEvent += this.onRewardedVideoFailedEvent;
		MoPubManager.OnRewardedVideoExpiredEvent += this.onRewardedVideoExpiredEvent;
		MoPubManager.OnRewardedVideoShownEvent += this.onRewardedVideoShownEvent;
		MoPubManager.OnRewardedVideoClickedEvent += this.onRewardedVideoClickedEvent;
		MoPubManager.OnRewardedVideoFailedToPlayEvent += this.onRewardedVideoFailedToPlayEvent;
		MoPubManager.OnRewardedVideoReceivedRewardEvent += this.onRewardedVideoReceivedRewardEvent;
		MoPubManager.OnRewardedVideoClosedEvent += this.onRewardedVideoClosedEvent;
		MoPubManager.OnRewardedVideoLeavingApplicationEvent += this.onRewardedVideoLeavingApplicationEvent;
		if (!string.IsNullOrEmpty(bAdUnit))
		{
			try
			{
				this.bannerAdUnit = bAdUnit;
				this.bannerEnabled = true;
				this.bannerPosition = ((bannerPos != BannerPosition.Bottom) ? MoPubBase.AdPosition.TopCenter : MoPubBase.AdPosition.BottomCenter);
				this.InternalInit(this.bannerAdUnit);
				MoPubAndroid.LoadBannerPluginsForAdUnits(new string[]
				{
					this.bannerAdUnit
				});
			}
			catch (Exception ex)
			{
				this.bannerEnabled = false;
				FMLogger.vAds("failed to init mopab banner. error: " + ex.Message);
			}
		}
		if (!string.IsNullOrEmpty(fsAdUnit))
		{
			try
			{
				this.interstitialAdUnit = fsAdUnit;
				this.fsEnabled = true;
				this.InternalInit(this.interstitialAdUnit);
				MoPubAndroid.LoadInterstitialPluginsForAdUnits(new string[]
				{
					this.interstitialAdUnit
				});
			}
			catch (Exception ex2)
			{
				this.fsEnabled = false;
				FMLogger.vAds("failed to init mopab interstitial. error: " + ex2.Message);
			}
		}
		if (!string.IsNullOrEmpty(rewardAdUnit))
		{
			try
			{
				this.rewardUnit = rewardAdUnit;
				this.rewardedEnabled = true;
				this.InternalInit(this.rewardUnit);
				MoPubAndroid.LoadRewardedVideoPluginsForAdUnits(new string[]
				{
					this.rewardUnit
				});
			}
			catch (Exception ex3)
			{
				this.rewardUnit = null;
				this.rewardedEnabled = false;
				FMLogger.Log("failed to init rewarded. error: " + ex3.Message);
			}
		}
		if (this.bannerEnabled || this.fsEnabled || this.rewardedEnabled)
		{
			ApplovinHelper.InitializeSdk();
		}
		this.RequestBanner();
		this.RequestInterstitial();
	}

	public void Configure(bool testMode, int bannerShowTime, int bannerPrechadeDelay, int fsDelayOnReward)
	{
	}

	private void InternalInit(string anyAdUnit)
	{
		if (this.inited || string.IsNullOrEmpty(anyAdUnit))
		{
			return;
		}
		MoPubAndroid.InitializeSdk(anyAdUnit);
		this.inited = true;
	}

	public void InitializeBanner(string bAdUnit, BannerPosition bannerPos)
	{
		if (this.bannerEnabled)
		{
			return;
		}
		if (!string.IsNullOrEmpty(bAdUnit))
		{
			try
			{
				this.bannerAdUnit = bAdUnit;
				this.bannerEnabled = true;
				this.bannerPosition = ((bannerPos != BannerPosition.Bottom) ? MoPubBase.AdPosition.TopCenter : MoPubBase.AdPosition.BottomCenter);
				this.InternalInit(this.bannerAdUnit);
				MoPubAndroid.LoadBannerPluginsForAdUnits(new string[]
				{
					this.bannerAdUnit
				});
			}
			catch (Exception ex)
			{
				this.bannerEnabled = false;
				FMLogger.vAds("failed to init mopab banner. error: " + ex.Message);
			}
			this.RequestBanner();
		}
	}

	public void RequestBanner()
	{
		if (!this.bannerEnabled)
		{
			return;
		}
		MoPubAndroid.CreateBanner(this.bannerAdUnit, this.bannerPosition);
		if (!this.bannerVisable)
		{
			this.coroutineProxy(this.BannerHideWorkaround());
		}
		FMLogger.vAds("start banner request");
	}

	private IEnumerator BannerHideWorkaround()
	{
		yield return 0;
		FMLogger.vAds("hide banner after req on next frame. workaround");
		MoPubAndroid.ShowBanner(this.bannerAdUnit, false);
		yield break;
	}

	public void ShowBanner()
	{
		if (!this.bannerEnabled)
		{
			return;
		}
		FMLogger.vAds("show banner inv");
		MoPubAndroid.ShowBanner(this.bannerAdUnit, true);
		this.bannerVisable = true;
		if (this.userBannerImpression)
		{
			this.userBannerImpression = false;
			AnalyticsManager.BannerImpression(this.bannerAdUnit, this.currentBannerGUID, AdsManager.Instance.IsTabletAdUnit, this.bannerPosition == MoPubBase.AdPosition.BottomCenter);
		}
	}

	public void HideBanner()
	{
		if (!this.bannerEnabled)
		{
			return;
		}
		FMLogger.vAds("hide banner inv");
		MoPubAndroid.ShowBanner(this.bannerAdUnit, false);
		this.bannerVisable = false;
	}

	public void UtilizeBanner()
	{
		if (!this.bannerEnabled)
		{
			return;
		}
		this.bannerEnabled = false;
		FMLogger.vAds("utilize banner inv");
		MoPubAndroid.DestroyBanner(this.bannerAdUnit);
	}

	public void RequestInterstitial()
	{
		if (!this.fsEnabled)
		{
			return;
		}
		this.fsLoadReqTime = DateTime.Now;
		MoPubAndroid.RequestInterstitialAd(this.interstitialAdUnit, string.Empty, string.Empty);
		FMLogger.vAds("start fs request");
	}

	public bool HasInterstitial()
	{
		return this.fsEnabled && this.hasLoadedInterstitial;
	}

	public void ShowInterstitial(AdPlacement fsPlacement)
	{
		if (!this.fsEnabled)
		{
			return;
		}
		this.fsAdPlacement = AdConfig.AdPlacementToString(fsPlacement);
		FMLogger.vAds("fs show inv");
		MoPubAndroid.ShowInterstitialAd(this.interstitialAdUnit);
		this.hasLoadedInterstitial = false;
		AnalyticsManager.FsImpression(this.interstitialAdUnit, this.fsAdPlacement);
	}

	private IEnumerator FsReloadTimeout()
	{
		FMLogger.vAds("start fs reload timer");
		yield return new WaitForSeconds(15f);
		FMLogger.vAds("start fs reload timeout. requesting new");
		this.RequestInterstitial();
		yield break;
	}

	private IEnumerator FsClickDelayEvent(string adUnit, string placement)
	{
		yield return 0;
		yield return 0;
		AnalyticsManager.FsClick(adUnit, placement);
		FMLogger.vAds("FsClick event sent");
		yield break;
	}

	public void InitializeRewarded(string rewardAdUnit)
	{
		MoPubAndroid.EnableLocationSupport(true);
		if (!string.IsNullOrEmpty(rewardAdUnit))
		{
			try
			{
				this.rewardUnit = rewardAdUnit;
				this.rewardedEnabled = true;
				this.InternalInit(this.rewardUnit);
				MoPubAndroid.LoadRewardedVideoPluginsForAdUnits(new string[]
				{
					this.rewardUnit
				});
			}
			catch (Exception ex)
			{
				this.rewardUnit = null;
				this.rewardedEnabled = false;
				FMLogger.Log("failed to init rewarded. error: " + ex.Message);
			}
		}
	}

	public void RequestRewardedVideo()
	{
		if (!this.rewardedEnabled)
		{
			return;
		}
		this.autoloadRewarded = true;
		this.InternalRewardedRequest();
		FMLogger.vAds("start reward request");
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
		this.InternalRewardedRequest();
		FMLogger.vAds("start single reward request");
	}

	public bool HasRewardedVideo()
	{
		return this.rewardedEnabled && MoPubAndroid.HasRewardedVideo(this.rewardUnit);
	}

	public void ShowRewardedVideo()
	{
		if (!this.rewardedEnabled)
		{
			return;
		}
		FMLogger.vAds("show rewarded");
		this.rewardReceived = false;
		MoPubAndroid.ShowRewardedVideo(this.rewardUnit, null);
		AdsManager.Instance.rewardInfo.IterateImpression();
		AnalyticsManager.RewardImpression(this.rewardUnit, AdsManager.Instance.rewardInfo);
	}

	private IEnumerator RewardReloadTimeout()
	{
		FMLogger.vAds("start reward reload timer");
		yield return new WaitForSeconds(16f);
		FMLogger.vAds("start reward reload timeout. requesting new");
		this.InternalRewardedRequest();
		yield break;
	}

	private void InternalRewardedRequest()
	{
		this.rewardLoadReqTime = DateTime.Now;
		MoPubAndroid.RequestRewardedVideo(this.rewardUnit, null, null, null, 99999.0, 99999.0, null);
		AdsManager.Instance.rewardInfo.IterateRequest();
		AnalyticsManager.RewardRequest(AdsManager.Instance.rewardInfo);
	}

	private IEnumerator CheckIfRewardedCanceled(string adUnit)
	{
		yield return new WaitForSeconds(0.5f);
		if (!this.rewardReceived)
		{
			FMLogger.vAds("rewarded cancel");
			AnalyticsManager.RewardCancel(adUnit);
		}
		yield break;
	}

	public void Utilize()
	{
	}

	private void onAdLoadedEvent(string adUnit, float height)
	{
		this.currentBannerGUID = AnalyticsUtils.GenerateGUID();
		this.userBannerImpression = true;
		FMLogger.vAds("onAdLoadedEvent. height: " + height);
		if (this.bannerVisable)
		{
			this.userBannerImpression = false;
			AnalyticsManager.BannerImpression(this.bannerAdUnit, this.currentBannerGUID, AdsManager.Instance.IsTabletAdUnit, this.bannerPosition == MoPubBase.AdPosition.BottomCenter);
		}
		else
		{
			FMLogger.vAds("hide banner loaded event. workaround");
			MoPubAndroid.ShowBanner(this.bannerAdUnit, false);
		}
	}

	private void onAdFailedEvent(string adUnit, string errorMsg)
	{
		FMLogger.vAds("onAdFailedEvent: " + errorMsg);
	}

	private void onAdClickedEvent(string adUnitId)
	{
		FMLogger.vAds("onAdClickedEvent: " + adUnitId);
		AppState.AdsShowPause();
		AnalyticsManager.BannerClick(adUnitId, this.currentBannerGUID);
	}

	private void onAdExpandedEvent(string adUnitId)
	{
		FMLogger.vAds("onAdExpandedEvent: " + adUnitId);
	}

	private void onAdCollapsedEvent(string adUnitId)
	{
		AppState.AdsShowPause();
		FMLogger.vAds("onAdCollapsedEvent: " + adUnitId);
	}

	private void onInterstitialLoadedEvent(string adUnitId)
	{
		FMLogger.vAds("onInterstitialLoadedEvent: " + adUnitId);
		this.hasLoadedInterstitial = true;
		if (!this.fsLoadTimeEventSent)
		{
			this.fsLoadTimeEventSent = true;
			int num = (int)(DateTime.Now - this.fsLoadReqTime).TotalMilliseconds;
			num /= 100;
			num = Mathf.RoundToInt((float)num / 5f) * 5;
			float time = (float)num / 10f;
			AnalyticsManager.FsFirstLoaded(time);
		}
	}

	private void onInterstitialFailedEvent(string adUnit, string errorMsg)
	{
		FMLogger.vAds("onInterstitialFailedEvent: " + errorMsg);
		this.coroutineProxy(this.FsReloadTimeout());
	}

	private void onInterstitialShownEvent(string adUnitId)
	{
		FMLogger.vAds("onInterstitialShownEvent: " + adUnitId);
		AnalyticsManager.FsImpressionCallback(adUnitId, this.fsAdPlacement);
	}

	private void onInterstitialClickedEvent(string adUnitId)
	{
		FMLogger.vAds("onInterstitialClickedEvent: " + adUnitId);
		this.coroutineProxy(this.FsClickDelayEvent(adUnitId, this.fsAdPlacement));
	}

	private void onInterstitialDismissedEvent(string adUnitId)
	{
		FMLogger.vAds("onInterstitialDismissedEvent: " + adUnitId);
		this.RequestInterstitial();
	}

	private void onInterstitialExpiredEvent(string adUnitId)
	{
		FMLogger.vAds("onInterstitialExpiredEvent: " + adUnitId);
		this.RequestInterstitial();
	}

	private void onRewardedVideoLoadedEvent(string adUnitId)
	{
		FMLogger.vAds("onRewardedVideoLoadedEvent: " + adUnitId);
		AdsManager.Instance.rewardInfo.IterateLoaded();
		AdsManager.Instance.rewardInfo.LoadTime = (int)(DateTime.Now - this.rewardLoadReqTime).TotalSeconds;
		AnalyticsManager.RewardLoaded(AdsManager.Instance.rewardInfo);
	}

	private void onRewardedVideoFailedEvent(string adUnit, string errorMsg)
	{
		FMLogger.vAds("onRewardedVideoFailedEvent: " + errorMsg);
		this.coroutineProxy(this.RewardReloadTimeout());
	}

	private void onRewardedVideoExpiredEvent(string adUnitId)
	{
		FMLogger.vAds("onRewardedVideoExpiredEvent: " + adUnitId);
		this.coroutineProxy(this.RewardReloadTimeout());
	}

	private void onRewardedVideoShownEvent(string adUnitId)
	{
		FMLogger.vAds("onRewardedVideoShownEvent: " + adUnitId);
	}

	private void onRewardedVideoClickedEvent(string adUnitId)
	{
		FMLogger.vAds("onRewardedVideoClickedEvent: " + adUnitId);
		AnalyticsManager.RewardClick(adUnitId);
	}

	private void onRewardedVideoFailedToPlayEvent(string adUnit, string errorMsg)
	{
		FMLogger.vAds("onRewardedVideoFailedToPlayEvent: " + errorMsg);
	}

	private void onRewardedVideoReceivedRewardEvent(string adUnitId, string label, float amount)
	{
		this.rewardReceived = true;
		if (this.RewardedComplete != null)
		{
			this.RewardedComplete(null, EventArgs.Empty);
		}
		FMLogger.vAds("onRewardedVideoReceivedRewardEvent: " + adUnitId);
		AnalyticsManager.RewardFinish(adUnitId);
	}

	private void onRewardedVideoClosedEvent(string adUnitId)
	{
		FMLogger.vAds("onRewardedVideoClosedEvent: " + adUnitId);
		if (this.autoloadRewarded)
		{
			this.InternalRewardedRequest();
		}
		this.rewardReceived = true;
		if (this.RewardedComplete != null)
		{
			this.RewardedComplete(null, EventArgs.Empty);
		}
		FMLogger.vAds("onRewardedVideoClosedEvent success workaround: " + adUnitId);
		AnalyticsManager.RewardFinish(adUnitId);
	}

	private void onRewardedVideoLeavingApplicationEvent(string adUnitId)
	{
		FMLogger.vAds("onRewardedVideoLeavingApplicationEvent: " + adUnitId);
	}

	private bool bannerEnabled;

	private bool fsEnabled;

	private bool rewardedEnabled;

	private DateTime fsLoadReqTime;

	private DateTime rewardLoadReqTime;

	private bool fsLoadTimeEventSent;

	private string bannerAdUnit;

	private string interstitialAdUnit;

	private string rewardUnit;

	private string fsAdPlacement;

	private string currentBannerGUID = string.Empty;

	private MoPubBase.AdPosition bannerPosition;

	private Func<IEnumerator, Coroutine> coroutineProxy;

	private bool inited;

	private bool bannerVisable;

	private bool userBannerImpression;

	private bool hasLoadedInterstitial;

	private bool rewardReceived;

	private bool autoloadRewarded;
}

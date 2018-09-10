// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
	public event Action RewardedVideoComplete;

	public BannerPlacement CurrentBannerPlacement
	{
		get
		{
			return this.currentBannerPlacement;
		}
	}

	public float AppLaunchPlacementDelay
	{
		get
		{
			return (this.config == null) ? 0f : this.config.appLaunchDelay;
		}
	}

	public RewardConfig RewardConfig
	{
		get
		{
			if (this.config != null && this.config.rewardConfig != null && this.config.rewardConfig.IsValid())
			{
				return this.config.rewardConfig;
			}
			UnityEngine.Debug.LogError("***ALERT. Missing reward config");
			return AdConfig.DefaultRewardConfig();
		}
	}

	public bool IsTabletAdUnit
	{
		get
		{
			return this.isTablet;
		}
	}

	public static AdsManager Instance
	{
		get
		{
			return AdsManager.instance;
		}
	}

	private void Awake()
	{
		if (AdsManager.instance != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		AdsManager.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void SetParams(string adsId, int adsLimited)
	{
		this.isTablet = ScreenUtils.CanFitTabletBanner();
		this.config = this.LoadDefaultConfig();
		this.adsUrlQueue = this.PrepareAdsUrlQueue(adsId, adsLimited);
		this.rewardInfo = RewardEventData.LoadEventData();
	}

	public void Init()
	{
		if (BuildConfig.HIDE_ADS || this.adsDisabled)
		{
			return;
		}
		this.GetServerConfig();
		this.ads = this.CreateAdsNetwork();
		this.bannerDisabled = !this.config.BannerIsOn();
		this.fsDisabled = !this.config.FsIsOn();
		string b = this.bannerDisabled ? null : this.config.bannerAdUnit;
		string fs = this.fsDisabled ? null : this.config.fsAdUnit;
		string rewardAdUnit = this.config.rewardAdUnit;
		FMLogger.vAds(string.Concat(new object[]
		{
			"init. b: ",
			!this.bannerDisabled,
			" fs: ",
			!this.fsDisabled
		}));
		this.ads.Configure(this.config.adsTest, this.config.bannerShowTime, this.config.bannerLoadDelay, this.config.fsInternalOnReward);
		this.ads.Initialize(b, this.config.bannerPos, fs, rewardAdUnit);
		this.ads.RewardedComplete += delegate(object sender, EventArgs e)
		{
			if (this.RewardedVideoComplete != null)
			{
				this.RewardedVideoComplete();
			}
		};
	}

	private AdsManager.IAds CreateAdsNetwork()
	{
		return new MopabNetwork(new Func<IEnumerator, Coroutine>(base.StartCoroutine));
	}

	private string PrepareAdsUrlQueue(string adsId, int adsLimited)
	{
		string text = (!this.isTablet) ? "phone" : "tablet";
		int num;
		try
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build$VERSION"))
			{
				num = androidJavaClass.GetStatic<int>("SDK_INT");
			}
		}
		catch (Exception)
		{
			num = 15;
		}
		string text2 = "Android" + num;
		string text3 = (!string.IsNullOrEmpty(adsId)) ? adsId : string.Empty;
		string text4 = (adsLimited != -1) ? adsLimited.ToString() : "1";
		string text5 = string.Concat(new string[]
		{
			"?app_id=coloring&device_type=",
			text,
			"&app_ver=",
			Application.version,
			"&software_name=",
			text2,
			"&advertising_id=",
			text3,
			"&isLimitAdTrackingEnabled=",
			text4,
			"&alt_id=",
			SystemInfo.deviceUniqueIdentifier,
			"&package_name=",
			SystemUtils.GetAppPackage()
		});
		text5 = Uri.EscapeUriString(text5);
		FMLogger.vAds(text5);
		return text5;
	}

	private void InitRewarded()
	{
		if (BuildConfig.HIDE_ADS || this.ads != null)
		{
			return;
		}
		if (this.ads == null)
		{
			this.ads = this.CreateAdsNetwork();
		}
		if (!this.configRequested)
		{
			this.GetServerConfig();
		}
		string rewardAdUnit = this.config.rewardAdUnit;
		if (string.IsNullOrEmpty(rewardAdUnit))
		{
			FMLogger.vAds("Init rewarded fail. Empty ad unit");
			return;
		}
		this.ads.InitializeRewarded(rewardAdUnit);
		this.ads.RewardedComplete += delegate(object sender, EventArgs e)
		{
			if (this.RewardedVideoComplete != null)
			{
				this.RewardedVideoComplete();
			}
		};
		FMLogger.vAds("Init rewarded only");
	}

	public BannerPosition GetBannerPosition()
	{
		if (!this.IsEnabled())
		{
			return BannerPosition.None;
		}
		if (this.bannerDisabled)
		{
			return BannerPosition.None;
		}
		if (this.config != null)
		{
			return this.config.bannerPos;
		}
		return BannerPosition.None;
	}

	public bool HasBannerPlacement(BannerPlacement bannerPlacement)
	{
		return this.IsEnabled() && !this.bannerDisabled && this.config != null && this.config.HasBannerPlacement(bannerPlacement);
	}

	public bool HasInterstitialPlacement(AdPlacement placement)
	{
		return this.IsEnabled() && !this.bannerDisabled && this.config != null && this.config.HasPlacement(placement);
	}

	public void ShowBanner(BannerPlacement placement)
	{
		if (!this.IsEnabled())
		{
			return;
		}
		if (this.bannerDisabled)
		{
			return;
		}
		this.currentBannerPlacement = placement;
		this.ads.ShowBanner();
	}

	public void HideBanner(bool resetPlacement = true)
	{
		if (!this.IsEnabled())
		{
			return;
		}
		if (resetPlacement)
		{
			this.currentBannerPlacement = BannerPlacement.Unknown;
		}
		this.ads.HideBanner();
	}

	public void ShowInterstitial(AdPlacement placement)
	{
		if (!this.IsEnabled())
		{
			return;
		}
		AppState.AdsShowPause();
		this.StepFsAdsInterval();
		this.ads.ShowInterstitial(placement);
	}

	public bool WantToShowAppLaunchInterstitial()
	{
		return this.IsEnabled() && (!this.fsDisabled && this.config.HasPlacement(AdPlacement.AppLaunch)) && this.config.WantToShowPlacement(AdPlacement.AppLaunch);
	}

	public bool AppLaunchInterstitialReady()
	{
		return this.IsEnabled() && !this.fsDisabled && this.ads.HasInterstitial();
	}

	public bool HasInterstitial(AdPlacement placement)
	{
		if (!this.IsEnabled())
		{
			return false;
		}
		if (this.fsDisabled)
		{
			return false;
		}
		if (this.config.WantToShowPlacement(placement))
		{
			float num = -1f;
			if (this.lastShownTime > 0f)
			{
				num = Time.realtimeSinceStartup - this.lastShownTime;
				this.wantToShow = (num > this.fsInterval);
			}
			else
			{
				this.StartFsInterval();
				this.wantToShow = false;
			}
			if (placement == AdPlacement.AppLaunch)
			{
				this.wantToShow = true;
			}
			bool flag = this.wantToShow && this.ads.HasInterstitial();
			FMLogger.vAds(string.Concat(new object[]
			{
				"d: ",
				num,
				" lst",
				this.lastShownTime,
				" w: ",
				this.wantToShow,
				" would show: ",
				flag
			}));
			return flag;
		}
		return false;
	}

	public void LoadRewardedVideo()
	{
		if (this.ads != null && !this.rewardVideoLoad)
		{
			FMLogger.vAds("start load reward video");
			this.rewardVideoLoad = true;
			this.ads.RequestRewardedVideo();
		}
		else if (this.ads == null)
		{
			this.InitRewarded();
			if (this.ads != null && !this.rewardVideoLoad)
			{
				FMLogger.vAds("start load ONLY reward video");
				this.rewardVideoLoad = true;
				this.ads.RequestRewardedVideo();
			}
		}
	}

	public void SingleRewardedRequest()
	{
		if (this.ads != null && !this.rewardVideoLoad)
		{
			FMLogger.vAds("start load *single reward video");
			this.ads.SingleRequestRewardedVideo();
		}
		else if (this.ads == null)
		{
			this.InitRewarded();
			if (this.ads != null && !this.rewardVideoLoad)
			{
				FMLogger.vAds("start load ONLY *single reward video");
				this.ads.SingleRequestRewardedVideo();
			}
		}
		else
		{
			FMLogger.vAds("single rewarded request ignored, rewarded already inited for autoloading");
		}
	}

	public bool HasRewardedVideo()
	{
		return this.ads != null && this.ads.HasRewardedVideo();
	}

	public void ShowRewardedVideo()
	{
		if (this.ads == null)
		{
			return;
		}
		AppState.AdsShowPause();
		this.ads.ShowRewardedVideo();
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (this.ads == null)
		{
			return;
		}
		if (!hasFocus)
		{
			if (this.currentBannerPlacement != BannerPlacement.Unknown)
			{
				this.HideBanner(false);
			}
			if (this.rewardInfo != null)
			{
				this.rewardInfo.SaveEventData();
			}
		}
		else if (this.currentBannerPlacement != BannerPlacement.Unknown)
		{
			this.ShowBanner(this.currentBannerPlacement);
		}
	}

	public void DisableAds()
	{
		this.HideBanner(true);
		if (this.ads != null)
		{
			this.ads.UtilizeBanner();
			this.ads.Utilize();
		}
		this.adsDisabled = true;
		FMLogger.vAds("AdsManager - disable ads");
	}

	private bool IsEnabled()
	{
		return this.ads != null && !this.adsDisabled;
	}

	private void OnDestroy()
	{
		if (this.ads == null)
		{
			return;
		}
		if (this.IsEnabled())
		{
			this.ads.Utilize();
		}
	}

	private void StepFsAdsInterval()
	{
		this.lastShownTime = Time.realtimeSinceStartup;
		this.fsInterval = (float)this.config.interval;
		FMLogger.vAds("Iterate fs interval " + this.fsInterval);
	}

	public void StartFsInterval()
	{
		this.lastShownTime = Time.realtimeSinceStartup;
		this.fsInterval = (float)this.config.startInterval;
		FMLogger.vAds("Start Fs Interval " + this.fsInterval);
	}

	private bool IsTablet()
	{
		float num = Mathf.Max(160f, Screen.dpi);
		float f = (float)Screen.width / num;
		float f2 = (float)Screen.height / num;
		float num2 = Mathf.Sqrt(Mathf.Pow(f, 2f) + Mathf.Pow(f2, 2f));
		return num2 > 7.8f;
	}

	private AdConfig LoadDefaultConfig()
	{
		AdConfig adConfig = null;
		string @string = PlayerPrefs.GetString("adsconfig", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			adConfig = JsonUtility.FromJson<AdConfig>(@string);
		}
		FMLogger.vCore("cfg str: " + @string);
		if (adConfig == null)
		{
			adConfig = ((!this.isTablet) ? AdConfig.DefaultPhone() : AdConfig.DefaultTablet());
		}
		if (string.IsNullOrEmpty(adConfig.bannerAdUnit))
		{
			adConfig.bannerAdUnit = ((!this.isTablet) ? "f22e60410d82403aa5e0fb791ef9c153" : "1253b5589a4d45869288611de14229d6");
			FMLogger.vAds("cfg fix banner unit");
		}
		if (string.IsNullOrEmpty(adConfig.fsAdUnit))
		{
			adConfig.fsAdUnit = ((!this.isTablet) ? "72bb0678c400487b8d1a941944fa6888" : "8cae328870984a9987623f9c6e52b25d");
			FMLogger.vAds("cfg fix fs unit");
		}
		if (string.IsNullOrEmpty(adConfig.rewardAdUnit))
		{
			adConfig.rewardAdUnit = ((!this.isTablet) ? "6bc3898062484e71a114d0ab59cb1c78" : "0543e571406140dd96252ac1351b99f5");
			FMLogger.vAds("cfg fix reward unit");
		}
		this.SaveConfig(adConfig);
		FMLogger.vAds("loaded saved cfg: " + adConfig);
		return adConfig;
	}

	public int CalcBannerHeight(int canvasHeight = -1)
	{
		int num = 0;
		if (!this.IsEnabled())
		{
			FMLogger.vAds("bh = 0. ads is disabled");
			return num;
		}
		if (this.bannerDisabled || string.IsNullOrEmpty(this.config.bannerAdUnit))
		{
			FMLogger.vAds("config is empty");
			return num;
		}
		int num2 = (!this.isTablet) ? 50 : 90;
		if (canvasHeight == -1)
		{
			canvasHeight = ((!this.isTablet) ? 1920 : 2927);
		}
		try
		{
			float num3 = (float)Mathf.Min(Screen.width, Screen.height);
			int screenDpWidth = ScreenUtils.GetScreenDpWidth();
			int num4 = (int)(num3 / (float)screenDpWidth * (float)num2);
			num = (int)((float)(num4 * canvasHeight) / (float)Screen.height);
			FMLogger.vAds(string.Concat(new object[]
			{
				"cal android banner canvas height: ",
				num,
				" pxHeight: ",
				num4,
				string.Empty
			}));
		}
		catch (Exception)
		{
			FMLogger.vAds("banner height ex. fallback calc");
			num = this.FallbackBannerHeight(canvasHeight);
		}
		return num;
	}

	private int FallbackBannerHeight(int canvasHeight = -1)
	{
		int num = (!this.isTablet) ? 50 : 90;
		float num2 = (float)num * (Screen.dpi / 160f);
		if (canvasHeight == -1)
		{
			canvasHeight = ((!this.isTablet) ? 1920 : 2927);
		}
		float num3 = (float)canvasHeight / (float)Screen.height;
		return Mathf.RoundToInt(num2 * num3);
	}

	private void SaveConfig(AdConfig c)
	{
		if (c == null)
		{
			return;
		}
		PlayerPrefs.SetString("adsconfig", JsonUtility.ToJson(c));
		FMLogger.vAds("saving new cfg: " + c);
	}

	private void GetServerConfig()
	{
		this.configRequested = true;
		string url = this.adsUrl + this.adsUrlQueue;
		WebGetTask task = new WebGetTask(url, delegate(bool success, string text)
		{
			if (!this.IsEnabled())
			{
				FMLogger.vAds("recieved ads config, but ads is off");
				return;
			}
			if (success && !string.IsNullOrEmpty(text))
			{
				try
				{
					byte[] bytes = Convert.FromBase64String(text);
					text = Encoding.UTF8.GetString(bytes);
					char[] array = text.ToCharArray();
					Array.Reverse(array);
					text = new string(array);
					byte[] bytes2 = Convert.FromBase64String(text);
					text = Encoding.UTF8.GetString(bytes2);
					AdsServerResponse adsServerResponse = JsonUtility.FromJson<AdsServerResponse>(text);
					if (adsServerResponse != null)
					{
						if (adsServerResponse.ad_module_active == 0)
						{
							FMLogger.vAds("disable ads from config");
							this.DisableAds();
							this.config.DisableAds();
							this.config.UpdateReward(AdConfig.FromRespone(adsServerResponse.rewarded_config, true), adsServerResponse.adUnit_rewarded);
							this.SaveConfig(this.config);
						}
						else
						{
							AdConfig adConfig = AdConfig.FromResponse(adsServerResponse, this.isTablet);
							this.SaveConfig(adConfig);
							this.config.UpdateFsParamsFromServer(adConfig);
							this.fsDisabled = !this.config.FsIsOn();
							this.bannerDisabled = !adConfig.BannerIsOn();
						}
					}
				}
				catch (Exception ex)
				{
					FMLogger.vAds("config parse ex. msg:" + ex.Message);
				}
			}
			else
			{
				FMLogger.vAds("config  req server error. reschedule request");
				base.StartCoroutine(this.DelayAction(20f, new Action(this.GetServerConfig)));
			}
		});
		WebLoader.Instance.LoadText(task);
	}

	private IEnumerator DelayAction(float delay, Action a)
	{
		yield return new WaitForSeconds(delay);
		a();
		yield break;
	}

	public RewardEventData rewardInfo;

	private AdConfig config;

	private bool configRequested;

	private BannerPlacement currentBannerPlacement;

	private float fsInterval;

	private float lastShownTime = -1f;

	private bool wantToShow = true;

	private static AdsManager instance;

	private bool rewardVideoLoad;

	private bool adsDisabled;

	private bool bannerDisabled = true;

	private bool fsDisabled;

	private bool isTablet;

	private string adsUrl = "https://ad.x-flow.app/ad.php";

	private string adsUrlQueue = string.Empty;

	private AdsManager.IAds ads;

	public interface IAds
	{
		event EventHandler RewardedComplete;

		void Configure(bool testMode, int bannerShowTime, int bannerPrechadeDelay, int fsDelayOnReward);

		void Initialize(string b, BannerPosition pos, string fs, string r);

		void InitializeRewarded(string r);

		void InitializeBanner(string r, BannerPosition pos);

		void RequestBanner();

		void ShowBanner();

		void HideBanner();

		void RequestInterstitial();

		bool HasInterstitial();

		void ShowInterstitial(AdPlacement fsPlacement);

		void RequestRewardedVideo();

		void SingleRequestRewardedVideo();

		bool HasRewardedVideo();

		void ShowRewardedVideo();

		void Utilize();

		void UtilizeBanner();
	}
}

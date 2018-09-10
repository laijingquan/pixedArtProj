// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
//using Facebook.Unity;
//using Firebase.Messaging;
//using SA.Common.Pattern;
using UnityEngine;

public class AppManager : MonoBehaviour
{
	 
	public event Action Loaded;

	public static bool Inited
	{
		get
		{
			return AppManager.inited;
		}
	}

	private void Awake()
	{
		this.DummyTest();
		GeneralSettings.AppLaunchCounter++;
		this.InitDesign();
		SafeLayout.Init();
		Application.targetFrameRate = 60;
	}

	private void DummyTest()
	{
	}

	private void InitDesign()
	{
		if (GeneralSettings.CurrentDesign == GeneralSettings.DesignType.Undef)
		{
			GeneralSettings.CurrentDesign = GeneralSettings.DesignType.Neo;
			GeneralSettings.CanUseLegacyDesign = (GeneralSettings.AppLaunchCounter > 1);
			if (GeneralSettings.CanUseLegacyDesign)
			{
				AppState.ShowDesignUpdDialog = true;
			}
		}
	}

	//private void OnNotificationMessageReceived(object sender, MessageReceivedEventArgs args)
	//{
	//	UnityEngine.Debug.Log("Push received");
	//	AppState.PushNotificationLaunch = true;
	//	if (args != null && args.Message != null)
	//	{
	//		AppState.PushNotificationId = ((!string.IsNullOrEmpty(args.Message.MessageId)) ? args.Message.MessageId : "null");
	//	}
	//}

	private void OnFBDeeplinkOpened(string bonusCode)
	{
		if (string.IsNullOrEmpty(bonusCode))
		{
			return;
		}
		base.StartCoroutine(this.BonusCodeHandleOnInit(bonusCode));
	}

	private IEnumerator BonusCodeHandleOnInit(string bonusCode)
	{
		while (!AppManager.inited)
		{
			yield return null;
		}
		this.HandleBonusCode(bonusCode);
		yield return null;
		yield break;
	}

	private void HandleBonusCode(string bonusCode)
	{
		FMLogger.vCore("deeplink bonus code " + bonusCode);
		BonusCodeData bonusCodeData = new BonusCodeData
		{
			BonusCode = bonusCode,
			ClaimTime = DateTime.UtcNow
		};
		if (!SharedData.Instance.AddBonusCode(bonusCodeData))
		{
			bonusCodeData.alreadyActivated = true;
			AnalyticsManager.BonusContentClaimUsedCode();
		}
		else
		{
			AnalyticsManager.BonusContentClaim(bonusCodeData.BonusCode);
		}
		AppState.BonusCode = bonusCodeData;
	}

	private void Start()
	{
		this.AdjustInit();
		this.FirebaseInit();
		this.CheckDailyBonus();
		try
		{
			PlayTimeEventTracker.AppResume();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("PlayTimeEventTracker ex. " + ex.Message);
		}
		this.deepLinker = new DeeplinkController(base.GetComponent<FMDeepLink>());
		this.deepLinker.BonusCodeReceived += this.OnFBDeeplinkOpened;
		AppState.ContentReqTime = DateTime.Now;
		base.StartCoroutine(this.LoadSysParameters(delegate
		{
			AdsManager.Instance.SetParams(this.adsId, this.adsLimitied);
			if (GeneralSettings.AdsDisabled)
			{
				FMLogger.vAds("casual disable ads on startup");
				AdsManager.Instance.DisableAds();
			}
			else
			{
				FMLogger.vAds("ads init");
				AdsManager.Instance.Init();
				AdsManager.Instance.StartFsInterval();
			}
			TGFModule.Instance.Init(this.adsId, this.adsLimitied, this.countryCode, this.langCode);
			ImageManager.Instance.Init();
			SharedData.Instance.Init();
			//FB.Init(null, null, null);
			AppState.LaunchTime = DateTime.Now;
			this.fairyController.StartTimer();
			this.deepLinker.Check();
			TGFModule.Instance.PrecachePages();
			try
			{
				AnalyticsManager.SetUserDeviceTypeProperty(SafeLayout.IsTablet);
			}
			catch (Exception ex2)
			{
				FMLogger.vCore("SetUserDeviceTypeProperty crash. " + ex2.Message);
			}
			float delay = 0.5f;
			if (AppManager.__f__mg_cache0 == null)
			{
				AppManager.__f__mg_cache0 = new Action(UserLifecycle.AppLaunch);
			}
			base.StartCoroutine(this.DelayAction(delay, AppManager.__f__mg_cache0));
			AppManager.inited = true;
			if (this.Loaded != null)
			{
				this.Loaded();
			}
		}));
	}

	private void AdjustInit()
	{
		AdjustHelper.Init();
	}

	private void FirebaseInit()
	{
		try
		{
			//FirebaseMessaging.MessageReceived += this.OnNotificationMessageReceived;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("FirebaseMessaging.MessageReceived ex. " + ex.Message);
			AnalyticsManager.FirebaseMessagingError(SystemInfo.operatingSystem);
		}
		try
		{
			//FirebaseMessaging.TokenReceived += this.OnFirebaseTokenReceived;
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.Log("FirebaseMessaging.TokenReceived ex. " + ex2.Message);
		}
	}

	//private void OnFirebaseTokenReceived(object sender, TokenReceivedEventArgs e)
	//{
	//	FMLogger.Log("firebase token received.");
	//	if (e != null && !string.IsNullOrEmpty(e.Token))
	//	{
	//		AdjustHelper.SetUninstallToken(e.Token);
	//	}
	//	FMLogger.vCore("firebase token " + e.Token);
	//}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus)
		{
			this.CheckDailyBonus();
			base.StartCoroutine(this.DelayAction(0.1f, delegate
			{
				if (this.deepLinker != null)
				{
					this.deepLinker.Check();
				}
			}));
			float delay = 1f;
			if (AppManager.__f__mg_cache1 == null)
			{
				AppManager.__f__mg_cache1 = new Action(AppState.ResetPauseState);
			}
			base.StartCoroutine(this.DelayAction(delay, AppManager.__f__mg_cache1));
		}
		else
		{
			AppState.ValidatePauseState();
		}
		if (pauseStatus)
		{
			PlayTimeEventTracker.AppPause();
			UserLifecycle.AppPause();
		}
		else
		{
			UserLifecycle.AppResume();
			PlayTimeEventTracker.AppResume();
		}
	}

	private IEnumerator DelayAction(float delay, Action a)
	{
		yield return new WaitForSeconds(delay);
		a();
		yield break;
	}

	private void CheckDailyBonus()
	{
	}

	private IEnumerator LoadSysParameters(Action loaded)
	{
		Application.RequestAdvertisingIdentifierAsync(new Application.AdvertisingIdentifierCallback(this.OnAdvertisingIdentifierLoaded));
		//UM_Location.OnLocaleLoaded += this.OnLocaleLoaded;
		//Singleton<UM_Location>.Instance.GetLocale();
		float timeout = 1f;
		while (timeout > 0f && (!this.localeReceived || !this.advtIdReceived))
		{
			timeout -= Time.deltaTime;
			yield return null;
		}
		if (!this.localeReceived)
		{
			//UM_Location.OnLocaleLoaded -= this.OnLocaleLoaded;
			this.countryCode = null;
			this.langCode = null;
			this.localeReceived = true;
		}
		if (!this.advtIdReceived)
		{
			this.adsId = string.Empty;
			this.adsLimitied = -1;
			this.advtIdReceived = true;
		}
		if (loaded != null)
		{
			loaded();
		}
		yield break;
	}

	//private void OnLocaleLoaded(UM_LocaleInfo locale)
	//{
	//	if (this.localeReceived)
	//	{
	//		return;
	//	}
	//	if (locale != null)
	//	{
	//		this.countryCode = locale.CountryCode;
	//		this.langCode = locale.LanguageCode;
	//	}
	//	this.localeReceived = true;
	//}

	private void OnAdvertisingIdentifierLoaded(string id, bool limited, string error)
	{
		if (this.advtIdReceived)
		{
			return;
		}
		if (!string.IsNullOrEmpty(error))
		{
			return;
		}
		if (string.IsNullOrEmpty(id))
		{
			return;
		}
		this.adsId = id;
		this.adsLimitied = ((!limited) ? 0 : 1);
		this.advtIdReceived = true;
	}

	public string getSystemInfo()
	{
		string text = "<color=red>SYSTEM INFO</color>";
		text = text + "\n[system info]" + SystemInfo.deviceModel;
		text = text + "\n[type]" + SystemInfo.deviceType;
		text = text + "\n[os version]" + SystemInfo.operatingSystem;
		text = text + "\n[system memory size]" + SystemInfo.systemMemorySize;
		string text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			"\n[graphic device name]",
			SystemInfo.graphicsDeviceName,
			" (version ",
			SystemInfo.graphicsDeviceVersion,
			")"
		});
		text = text + "\n[graphic memory size]" + SystemInfo.graphicsMemorySize;
		text = text + "\n[graphic max texSize]" + SystemInfo.maxTextureSize;
		text = text + "\n[graphic shader level]" + SystemInfo.graphicsShaderLevel;
		text = text + "\n[support compute shader]" + SystemInfo.supportsComputeShaders;
		text = text + "\n[processor count]" + SystemInfo.processorCount;
		text = text + "\n[processor type]" + SystemInfo.processorType;
		text = text + "\n[support 3d texture]" + SystemInfo.supports3DTextures;
		text = text + "\n[support shadow]" + SystemInfo.supportsShadows;
		text = text + "\n[platform] " + Application.platform;
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"\n[screen size] ",
			Screen.width,
			" x ",
			Screen.height
		});
		return text + "\n[screen pixel density dpi] " + Screen.dpi;
	}

	public FairyController fairyController;

	private DeeplinkController deepLinker;

	private static bool inited;

	private bool advtIdReceived;

	private bool localeReceived;

	private string countryCode;

	private string langCode;

	private string adsId;

	private int adsLimitied;

	[CompilerGenerated]
	private static Action __f__mg_cache0;

	[CompilerGenerated]
	private static Action __f__mg_cache1;
}

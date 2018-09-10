// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using com.adjust.sdk;

public static class AdjustHelper
{
	public static void Init()
	{
		if (AdjustHelper.inited)
		{
			return;
		}
		try
		{
			AdjustEnvironment environment = (!AdjustHelper.IsSandboxMode()) ? AdjustEnvironment.Production : AdjustEnvironment.Sandbox;
			AdjustLogLevel logLevel = (!BuildConfig.LOG_ENABLED) ? AdjustLogLevel.Error : AdjustLogLevel.Verbose;
			AdjustConfig adjustConfig = new AdjustConfig("6652dk67rxj4", environment);
			adjustConfig.setLogLevel(logLevel);
			AdjustConfig adjustConfig2 = adjustConfig;
			if (AdjustHelper.__f__mg_cache0 == null)
			{
				AdjustHelper.__f__mg_cache0 = new Action<string>(FMLogger.vCore);
			}
			adjustConfig2.setLogDelegate(AdjustHelper.__f__mg_cache0);
			adjustConfig.setEventBufferingEnabled(true);
			adjustConfig.setLaunchDeferredDeeplink(true);
			AdjustConfig adjustConfig3 = adjustConfig;
			if (AdjustHelper.__f__mg_cache1 == null)
			{
				AdjustHelper.__f__mg_cache1 = new Action<AdjustEventSuccess>(AdjustHelper.EventSuccessCallback);
			}
			adjustConfig3.setEventSuccessDelegate(AdjustHelper.__f__mg_cache1, "Adjust");
			AdjustConfig adjustConfig4 = adjustConfig;
			if (AdjustHelper.__f__mg_cache2 == null)
			{
				AdjustHelper.__f__mg_cache2 = new Action<AdjustEventFailure>(AdjustHelper.EventFailureCallback);
			}
			adjustConfig4.setEventFailureDelegate(AdjustHelper.__f__mg_cache2, "Adjust");
			AdjustConfig adjustConfig5 = adjustConfig;
			if (AdjustHelper.__f__mg_cache3 == null)
			{
				AdjustHelper.__f__mg_cache3 = new Action<AdjustSessionSuccess>(AdjustHelper.SessionSuccessCallback);
			}
			adjustConfig5.setSessionSuccessDelegate(AdjustHelper.__f__mg_cache3, "Adjust");
			AdjustConfig adjustConfig6 = adjustConfig;
			if (AdjustHelper.__f__mg_cache4 == null)
			{
				AdjustHelper.__f__mg_cache4 = new Action<AdjustSessionFailure>(AdjustHelper.SessionFailureCallback);
			}
			adjustConfig6.setSessionFailureDelegate(AdjustHelper.__f__mg_cache4, "Adjust");
			AdjustConfig adjustConfig7 = adjustConfig;
			if (AdjustHelper.__f__mg_cache5 == null)
			{
				AdjustHelper.__f__mg_cache5 = new Action<string>(AdjustHelper.DeferredDeeplinkCallback);
			}
			adjustConfig7.setDeferredDeeplinkDelegate(AdjustHelper.__f__mg_cache5, "Adjust");
			AdjustConfig adjustConfig8 = adjustConfig;
			if (AdjustHelper.__f__mg_cache6 == null)
			{
				AdjustHelper.__f__mg_cache6 = new Action<AdjustAttribution>(AdjustHelper.AttributionChangedCallback);
			}
			adjustConfig8.setAttributionChangedDelegate(AdjustHelper.__f__mg_cache6, "Adjust");
			Adjust.start(adjustConfig);
			AdjustHelper.inited = true;
		}
		catch (Exception ex)
		{
			AdjustHelper.inited = false;
			FMLogger.vCore("Adjust init error. " + ex.Message);
		}
	}

	public static void SetUninstallToken(string token)
	{
		Adjust.setDeviceToken(token);
	}

	public static void SetUninstallToken(byte[] bytes)
	{
		string uninstallToken = BitConverter.ToString(bytes).Replace("-", string.Empty);
		AdjustHelper.SetUninstallToken(uninstallToken);
	}

	private static void EventSuccessCallback(AdjustEventSuccess eventSuccess)
	{
	}

	private static void EventFailureCallback(AdjustEventFailure eventFailure)
	{
	}

	private static void SessionSuccessCallback(AdjustSessionSuccess sessionSuccess)
	{
	}

	private static void SessionFailureCallback(AdjustSessionFailure sessionFailure)
	{
	}

	private static void DeferredDeeplinkCallback(string data)
	{
	}

	private static void AttributionChangedCallback(AdjustAttribution atr)
	{
		if (atr != null)
		{
			AnalyticsManager.UpdateConversionDataProperty(atr);
		}
	}

	public static void LogEvent(string eventName, Dictionary<string, string> dict)
	{
		if (!AdjustHelper.inited)
		{
			return;
		}
		string text = AdjustHelper.EventNameToToken(eventName);
		if (!string.IsNullOrEmpty(text))
		{
			AdjustEvent adjustEvent = new AdjustEvent(text);
			if (dict != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in dict)
				{
					adjustEvent.addPartnerParameter(keyValuePair.Key, keyValuePair.Value);
					adjustEvent.addCallbackParameter(keyValuePair.Key, keyValuePair.Value);
				}
			}
			Adjust.trackEvent(adjustEvent);
		}
	}

	private static string EventNameToToken(string eventName)
	{
		Dictionary<string, string> dictionary = (!AdjustHelper.IsSandboxMode()) ? AdjustHelper.ProductionEventMap : AdjustHelper.SandboxEventMap;
		string result;
		if (dictionary.TryGetValue(eventName, out result))
		{
			return result;
		}
		return null;
	}

	private static bool IsSandboxMode()
	{
		return BuildConfig.TGF_DEBUG_URL;
	}

	private const string appToken = "6652dk67rxj4";

	private static Dictionary<string, string> ProductionEventMap = new Dictionary<string, string>
	{
		{
			"ad_assert",
			"pyu08a"
		},
		{
			"ad_banner_click",
			"v8uyzf"
		},
		{
			"ad_banner_impression",
			"r50nsh"
		},
		{
			"ad_interstitial_click",
			"wfewu9"
		},
		{
			"ad_interstitial_impression",
			"x794nx"
		},
		{
			"ad_interstitial_impression_callback",
			"duckix"
		},
		{
			"ad_interstitial_loaded1st",
			"iwp0hd"
		},
		{
			"ad_interstitial_potential_gameScreen",
			"rrka40"
		},
		{
			"ad_removed",
			"uu346s"
		},
		{
			"ad_rewarded_canceled",
			"g2bmq8"
		},
		{
			"ad_rewarded_click",
			"13bgpe"
		},
		{
			"ad_rewarded_finished",
			"1rtfvv"
		},
		{
			"ad_rewarded_impression",
			"xh8ty1"
		},
		{
			"ad_rewarded_loaded",
			"utke4z"
		},
		{
			"ad_rewarded_request",
			"40tt8k"
		},
		{
			"AD_TIPS_BONUS_SHOWN",
			"9yelnv"
		},
		{
			"APPLICATION_OPEN",
			"na1a48"
		},
		{
			"coloring_animation_skip",
			"fhn5y3"
		},
		{
			"coloring_continue",
			"7wdojd"
		},
		{
			"coloring_finished",
			"htk8ea"
		},
		{
			"coloring_finished_daily",
			"8g075v"
		},
		{
			"coloring_restart",
			"cl6x72"
		},
		{
			"coloring_shared",
			"idvg67"
		},
		{
			"coloring_start",
			"fhszdx"
		},
		{
			"coloring_start_daily",
			"ex25qv"
		},
		{
			"dailyList_featured_daily",
			"yk4ty1"
		},
		{
			"dailyList_tap",
			"ytx2vu"
		},
		{
			"db_creation_error",
			"nvpowx"
		},
		{
			"deepLink_bonusClaimed",
			"vtkso3"
		},
		{
			"deepLink_bonusError",
			"fvipdt"
		},
		{
			"deepLink_queueError",
			"5gyig7"
		},
		{
			"error_firebase_messaging",
			"2rxnf0"
		},
		{
			"gameScreen_hint",
			"cfpfaj"
		},
		{
			"gameScreen_magicHint",
			"we735i"
		},
		{
			"INAPP_PURCHASED",
			"lxdpj0"
		},
		{
			"m_mins_a_day_10",
			"5ptmip"
		},
		{
			"m_mins_a_day_20",
			"l3sm95"
		},
		{
			"m_mins_a_day_40",
			"myfgx6"
		},
		{
			"m_mins_a_day_60",
			"oyu5kx"
		},
		{
			"m_open_inrow_14d",
			"j76sj6"
		},
		{
			"m_open_inrow_28d",
			"8ba7my"
		},
		{
			"m_open_inrow_3d",
			"ik4bza"
		},
		{
			"m_open_inrow_5d",
			"e7jpao"
		},
		{
			"m_open_inrow_7d",
			"niq788"
		},
		{
			"m_pics_finished_100",
			"2yhn8k"
		},
		{
			"m_pics_finished_15",
			"df14bj"
		},
		{
			"m_pics_finished_30",
			"pfvn9d"
		},
		{
			"m_pics_finished_5",
			"7t5b8k"
		},
		{
			"m_pics_finished_50",
			"2j3aq6"
		},
		{
			"m_watch_end_10",
			"1mpdv0"
		},
		{
			"mainList_bonusEmpty_findMore",
			"sfu15q"
		},
		{
			"mainList_featured_daily",
			"o9ziz0"
		},
		{
			"mainList_featured_editors",
			"c95xko"
		},
		{
			"mainList_featured_link",
			"yrj0ez"
		},
		{
			"mainList_filter_categoryBonus",
			"t241gi"
		},
		{
			"mainList_filter_categroy",
			"1433n3"
		},
		{
			"mainList_filter_findMore",
			"rrk6hx"
		},
		{
			"mainList_filter_hide_off",
			"63afuz"
		},
		{
			"mainList_filter_hide_on",
			"3u2x3o"
		},
		{
			"mainList_tap",
			"k9wzyp"
		},
		{
			"mainList_tapBonus",
			"os6c3z"
		},
		{
			"mainList_tapSearch",
			"fdoxp2"
		},
		{
			"rateit_fromMenu",
			"5t1rd8"
		},
		{
			"rateit_popup",
			"v2h0nx"
		},
		{
			"rateit_popup_cancel",
			"njlz3v"
		},
		{
			"rateit_popup_confirm",
			"jr9vbl"
		},
		{
			"menuTab_news",
			"vwfc8f"
		}
	};

	private static Dictionary<string, string> SandboxEventMap = new Dictionary<string, string>
	{
		{
			"ad_assert",
			"pyu08a"
		},
		{
			"ad_banner_click",
			"v8uyzf"
		},
		{
			"ad_banner_impression",
			"r50nsh"
		},
		{
			"ad_interstitial_click",
			"wfewu9"
		},
		{
			"ad_interstitial_impression",
			"x794nx"
		},
		{
			"ad_interstitial_impression_callback",
			"duckix"
		},
		{
			"ad_interstitial_loaded1st",
			"iwp0hd"
		},
		{
			"ad_interstitial_potential_gameScreen",
			"rrka40"
		},
		{
			"ad_removed",
			"uu346s"
		},
		{
			"ad_rewarded_canceled",
			"g2bmq8"
		},
		{
			"ad_rewarded_click",
			"13bgpe"
		},
		{
			"ad_rewarded_finished",
			"1rtfvv"
		},
		{
			"ad_rewarded_impression",
			"xh8ty1"
		},
		{
			"ad_rewarded_loaded",
			"utke4z"
		},
		{
			"ad_rewarded_request",
			"40tt8k"
		},
		{
			"AD_TIPS_BONUS_SHOWN",
			"9yelnv"
		},
		{
			"APPLICATION_OPEN",
			"na1a48"
		},
		{
			"coloring_animation_skip",
			"fhn5y3"
		},
		{
			"coloring_continue",
			"7wdojd"
		},
		{
			"coloring_finished",
			"htk8ea"
		},
		{
			"coloring_finished_daily",
			"8g075v"
		},
		{
			"coloring_restart",
			"cl6x72"
		},
		{
			"coloring_shared",
			"idvg67"
		},
		{
			"coloring_start",
			"fhszdx"
		},
		{
			"coloring_start_daily",
			"ex25qv"
		},
		{
			"dailyList_featured_daily",
			"yk4ty1"
		},
		{
			"dailyList_tap",
			"ytx2vu"
		},
		{
			"db_creation_error",
			"nvpowx"
		},
		{
			"deepLink_bonusClaimed",
			"vtkso3"
		},
		{
			"deepLink_bonusError",
			"fvipdt"
		},
		{
			"deepLink_queueError",
			"5gyig7"
		},
		{
			"error_firebase_messaging",
			"2rxnf0"
		},
		{
			"gameScreen_hint",
			"cfpfaj"
		},
		{
			"gameScreen_magicHint",
			"we735i"
		},
		{
			"INAPP_PURCHASED",
			"lxdpj0"
		},
		{
			"m_mins_a_day_10",
			"5ptmip"
		},
		{
			"m_mins_a_day_20",
			"l3sm95"
		},
		{
			"m_mins_a_day_40",
			"myfgx6"
		},
		{
			"m_mins_a_day_60",
			"oyu5kx"
		},
		{
			"m_open_inrow_14d",
			"j76sj6"
		},
		{
			"m_open_inrow_28d",
			"8ba7my"
		},
		{
			"m_open_inrow_3d",
			"ik4bza"
		},
		{
			"m_open_inrow_5d",
			"e7jpao"
		},
		{
			"m_open_inrow_7d",
			"niq788"
		},
		{
			"m_pics_finished_100",
			"2yhn8k"
		},
		{
			"m_pics_finished_15",
			"df14bj"
		},
		{
			"m_pics_finished_30",
			"pfvn9d"
		},
		{
			"m_pics_finished_5",
			"7t5b8k"
		},
		{
			"m_pics_finished_50",
			"2j3aq6"
		},
		{
			"m_watch_end_10",
			"1mpdv0"
		},
		{
			"mainList_bonusEmpty_findMore",
			"sfu15q"
		},
		{
			"mainList_featured_daily",
			"o9ziz0"
		},
		{
			"mainList_featured_editors",
			"c95xko"
		},
		{
			"mainList_featured_link",
			"yrj0ez"
		},
		{
			"mainList_filter_categoryBonus",
			"t241gi"
		},
		{
			"mainList_filter_categroy",
			"1433n3"
		},
		{
			"mainList_filter_findMore",
			"rrk6hx"
		},
		{
			"mainList_filter_hide_off",
			"63afuz"
		},
		{
			"mainList_filter_hide_on",
			"3u2x3o"
		},
		{
			"mainList_tap",
			"k9wzyp"
		},
		{
			"mainList_tapBonus",
			"os6c3z"
		},
		{
			"mainList_tapSearch",
			"fdoxp2"
		},
		{
			"rateit_fromMenu",
			"5t1rd8"
		},
		{
			"rateit_popup",
			"v2h0nx"
		},
		{
			"rateit_popup_cancel",
			"njlz3v"
		},
		{
			"rateit_popup_confirm",
			"jr9vbl"
		},
		{
			"menuTab_news",
			"vwfc8f"
		}
	};

	private static bool inited = false;

	[CompilerGenerated]
	private static Action<string> __f__mg_cache0;

	[CompilerGenerated]
	private static Action<AdjustEventSuccess> __f__mg_cache1;

	[CompilerGenerated]
	private static Action<AdjustEventFailure> __f__mg_cache2;

	[CompilerGenerated]
	private static Action<AdjustSessionSuccess> __f__mg_cache3;

	[CompilerGenerated]
	private static Action<AdjustSessionFailure> __f__mg_cache4;

	[CompilerGenerated]
	private static Action<string> __f__mg_cache5;

	[CompilerGenerated]
	private static Action<AdjustAttribution> __f__mg_cache6;
}

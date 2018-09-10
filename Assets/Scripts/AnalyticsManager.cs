// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using com.adjust.sdk;
using Fabric.Answers;
//using Firebase.Analytics;
using KHD;
using UnityEngine;

public class AnalyticsManager
{
	private static void Log(string eventName, bool firebaseOnly = false)
	{
		if (BuildConfig.DISABLE_ANALYTICS)
		{
			return;
		}
		Dictionary<string, string> dict = AnalyticsManager.DefaultParamsDict();
		AnalyticsManager.InternalFirebaseLog(eventName, dict);
		if (!firebaseOnly)
		{
			AnalyticsManager.InternalFlurryLog(eventName, dict);
			AnalyticsManager.InternalFabricAnswersLog(eventName, dict);
			AnalyticsManager.InternalAdjustLog(eventName, dict);
		}
	}

	private static void Log(string eventName, Dictionary<string, string> dict, bool firebaseOnly = false)
	{
		if (BuildConfig.DISABLE_ANALYTICS)
		{
			return;
		}
		if (dict != null)
		{
			AnalyticsManager.AddDefParametersToDict(dict);
		}
		else
		{
			dict = AnalyticsManager.DefaultParamsDict();
		}
		AnalyticsManager.InternalFirebaseLog(eventName, dict);
		if (!firebaseOnly)
		{
			AnalyticsManager.InternalFlurryLog(eventName, dict);
			AnalyticsManager.InternalFabricAnswersLog(eventName, dict);
			AnalyticsManager.InternalAdjustLog(eventName, dict);
		}
	}

	private static Dictionary<string, string> DefaultParamsDict()
	{
		if (AnalyticsManager.defParamsDict == null)
		{
			AnalyticsManager.defParamsDict = new Dictionary<string, string>();
			AnalyticsManager.defParamsDict.Add("DATE_TIME", string.Empty);
			AnalyticsManager.defParamsDict.Add("APP_SESSION_UUID", string.Empty);
			AnalyticsManager.defParamsDict.Add("USER_ID", string.Empty);
		}
		AnalyticsManager.SetDefParamsInDict(AnalyticsManager.defParamsDict);
		return AnalyticsManager.defParamsDict;
	}

	private static void AddDefParametersToDict(Dictionary<string, string> dict)
	{
		if (!dict.ContainsKey("DATE_TIME"))
		{
			dict.Add("DATE_TIME", string.Empty);
			dict.Add("APP_SESSION_UUID", string.Empty);
			dict.Add("USER_ID", string.Empty);
		}
		AnalyticsManager.SetDefParamsInDict(dict);
	}

	private static void SetDefParamsInDict(Dictionary<string, string> dict)
	{
		dict["DATE_TIME"] = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss");
		dict["USER_ID"] = UserLifecycle.UserId;
		dict["APP_SESSION_UUID"] = UserLifecycle.SessionId;
	}

	private static void InternalFlurryLog(string eventName, Dictionary<string, string> dict)
	{
		SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.LogEventWithParameters(eventName, dict);
	}

	private static void InternalFirebaseLog(string eventName, Dictionary<string, string> dict)
	{
		//Parameter[] array = null;
		//if (dict != null)
		//{
		//	array = new Parameter[dict.Count];
		//	int num = 0;
		//	foreach (KeyValuePair<string, string> keyValuePair in dict)
		//	{
		//		array[num] = new Parameter(keyValuePair.Key, keyValuePair.Value);
		//		num++;
		//	}
		//}
		//FirebaseAnalytics.LogEvent(eventName, array);
	}

	private static void InternalFabricAnswersLog(string eventName, Dictionary<string, string> dict)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (dict != null)
		{
			foreach (KeyValuePair<string, string> keyValuePair in dict)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		Answers.LogCustom(eventName, dictionary);
	}

	private static void InternalAdjustLog(string eventName, Dictionary<string, string> dict)
	{
		AdjustHelper.LogEvent(eventName, dict);
	}

	public static void UpdateUserLibProgressProperty(int percent)
	{
		//FirebaseAnalytics.SetUserProperty("user_progress_lib", percent.ToString());
	}

	public static void UpdateConversionDataProperty(AdjustAttribution attribution)
	{
		string property = (!string.IsNullOrEmpty(attribution.network)) ? attribution.network : "null";
		string property2 = (!string.IsNullOrEmpty(attribution.campaign)) ? attribution.campaign : "null";
		string property3 = (!string.IsNullOrEmpty(attribution.adgroup)) ? attribution.adgroup : "null";
		string property4 = (!string.IsNullOrEmpty(attribution.creative)) ? attribution.creative : "null";
		string property5 = (!string.IsNullOrEmpty(attribution.adid)) ? attribution.adid : "null";
		//FirebaseAnalytics.SetUserProperty("adj_n", property);
		//FirebaseAnalytics.SetUserProperty("adj_c", property2);
		//FirebaseAnalytics.SetUserProperty("adj_ag", property3);
		//FirebaseAnalytics.SetUserProperty("adj_ac", property4);
		//FirebaseAnalytics.SetUserProperty("adj_adid", property5);
	}

	public static void UpdateDesignProperty(bool isOldDesign)
	{
		//FirebaseAnalytics.SetUserProperty("user_design_type", (!isOldDesign) ? "new" : "old");
	}

	public static void UpdateUserDailyProgressProperty(int percent)
	{
		//FirebaseAnalytics.SetUserProperty("user_progress_daily", percent.ToString());
	}

	public static void SetUserDeviceTypeProperty(bool isTablet)
	{
		//FirebaseAnalytics.SetUserProperty("user_device_type", (!isTablet) ? "phone" : "tablet");
	}

	public static void PromoSudoku()
	{
		AnalyticsManager.Log("menu_playSudoku", false);
	}

	public static void PromoSolitaire()
	{
		AnalyticsManager.Log("menu_playSolitaire", false);
	}

	public static void RateMenu()
	{
		AnalyticsManager.Log("rateit_fromMenu", new Dictionary<string, string>
		{
			{
				"count",
				GeneralSettings.RateDisplayCount.ToString()
			}
		}, false);
	}

	public static void RestoreClick()
	{
	}

	public static void RateShow()
	{
		AnalyticsManager.Log("rateit_popup", new Dictionary<string, string>
		{
			{
				"count",
				GeneralSettings.RateDisplayCount.ToString()
			}
		}, false);
	}

	public static void RateConfirm()
	{
		AnalyticsManager.Log("rateit_popup_confirm", new Dictionary<string, string>
		{
			{
				"count",
				GeneralSettings.RateDisplayCount.ToString()
			}
		}, false);
	}

	public static void RateCancel()
	{
		AnalyticsManager.Log("rateit_popup_cancel", new Dictionary<string, string>
		{
			{
				"count",
				GeneralSettings.RateDisplayCount.ToString()
			}
		}, false);
	}

	public static void OnSharePic(int picId, string shareTarget)
	{
		AnalyticsManager.Log("coloring_shared", new Dictionary<string, string>
		{
			{
				"id",
				picId.ToString()
			},
			{
				"name",
				shareTarget
			}
		}, false);
	}

	public static void SelectPicClick(int id, int itemPos, int categoryId, bool isFilterCompleted)
	{
		AnalyticsManager.Log("mainList_tap", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			},
			{
				"count",
				itemPos.ToString()
			},
			{
				"categoryId",
				categoryId.ToString()
			},
			{
				"isHideCompleted",
				(!isFilterCompleted) ? "0" : "1"
			}
		}, false);
	}

	public static void StartNewColoring(int id, int size, FillAlgorithm fillType)
	{
		AnalyticsManager.Log("coloring_start", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			},
			{
				"size",
				size.ToString()
			},
			{
				"filltype",
				fillType.ToString().ToLower()
			}
		}, false);
		TGFModule.Instance.GameStarted(id, size, fillType);
		Answers.LogLevelStart(id.ToString(), null);
	}

	public static void ContinuePic(int id, int progress, string screenFrom, int size, FillAlgorithm fillType)
	{
		AnalyticsManager.Log("coloring_continue", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			},
			{
				"percent",
				progress.ToString()
			},
			{
				"from",
				screenFrom
			},
			{
				"size",
				size.ToString()
			},
			{
				"filltype",
				fillType.ToString().ToLower()
			}
		}, false);
		TGFModule.Instance.GameContinue(id, size, fillType);
	}

	public static void RestartPic(int id, int progress, string screenFrom, int size, FillAlgorithm fillType)
	{
		AnalyticsManager.Log("coloring_restart", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			},
			{
				"percent",
				progress.ToString()
			},
			{
				"from",
				screenFrom
			},
			{
				"size",
				size.ToString()
			},
			{
				"filltype",
				fillType.ToString().ToLower()
			}
		}, false);
		TGFModule.Instance.GameStarted(id, size, fillType);
		Answers.LogLevelStart(id.ToString(), null);
	}

	public static void CompletePic(int id, int time, int size, FillAlgorithm fillType, int hintsUsed)
	{
		AnalyticsManager.Log("coloring_finished", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			},
			{
				"time_5s",
				time.ToString()
			},
			{
				"size",
				size.ToString()
			},
			{
				"hints_used",
				hintsUsed.ToString()
			},
			{
				"filltype",
				fillType.ToString().ToLower()
			}
		}, false);
		TGFModule.Instance.GameFinish(id, time, size, fillType);
		Answers.LogLevelEnd(id.ToString(), null, null, null);
	}

	public static void DailyTabPicClick(int id, int itemPos, int day, int month, int year)
	{
		string text = (month >= 10) ? month.ToString() : ("0" + month);
		string text2 = (day >= 10) ? day.ToString() : ("0" + day);
		AnalyticsManager.Log("dailyList_tap", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			},
			{
				"count",
				itemPos.ToString()
			},
			{
				"date",
				string.Concat(new object[]
				{
					year,
					"-",
					text,
					"-",
					text2
				})
			},
			{
				"month",
				year + "-" + text
			}
		}, false);
	}

	public static void CompleteDailyPic(int id, int time, int size, FillAlgorithm fillType, int hintsUsed)
	{
		AnalyticsManager.Log("coloring_finished_daily", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			},
			{
				"time_5s",
				time.ToString()
			},
			{
				"size",
				size.ToString()
			},
			{
				"hints_used",
				hintsUsed.ToString()
			},
			{
				"filltype",
				fillType.ToString().ToLower()
			}
		}, false);
		TGFModule.Instance.GameFinish(id, time, size, fillType);
	}

	public static void BannerImpression(string adUnit, string guid, bool isTablet, bool bannerOnBottom)
	{
		AnalyticsManager.Log("ad_banner_impression", new Dictionary<string, string>
		{
			{
				AnalyticsManager.adUnitKey,
				adUnit
			},
			{
				"banner_shown_type",
				(!isTablet) ? "phone" : "tablet"
			},
			{
				"banner_position",
				(!bannerOnBottom) ? "top" : "bottom"
			},
			{
				"banner_shown_ID",
				guid
			}
		}, false);
	}

	public static void BannerClick(string adUnit, string guid)
	{
		AnalyticsManager.Log("ad_banner_click", new Dictionary<string, string>
		{
			{
				AnalyticsManager.adUnitKey,
				adUnit
			},
			{
				"banner_clicked_id",
				guid
			}
		}, false);
	}

	public static void FsImpression(string adUnit, string placement)
	{
		if (AnalyticsManager.adsDictEx == null)
		{
			AnalyticsManager.adsDictEx = new Dictionary<string, string>();
			AnalyticsManager.adsDictEx.Add(AnalyticsManager.adUnitKey, string.Empty);
			AnalyticsManager.adsDictEx.Add(AnalyticsManager.placementKey, string.Empty);
		}
		AnalyticsManager.adsDictEx[AnalyticsManager.adUnitKey] = adUnit;
		AnalyticsManager.adsDictEx[AnalyticsManager.placementKey] = placement;
		AnalyticsManager.Log("ad_interstitial_impression", AnalyticsManager.adsDictEx, false);
	}

	public static void FsImpressionCallback(string adUnit, string placement)
	{
		if (AnalyticsManager.adsDictEx == null)
		{
			AnalyticsManager.adsDictEx = new Dictionary<string, string>();
			AnalyticsManager.adsDictEx.Add(AnalyticsManager.adUnitKey, string.Empty);
			AnalyticsManager.adsDictEx.Add(AnalyticsManager.placementKey, string.Empty);
		}
		AnalyticsManager.adsDictEx[AnalyticsManager.adUnitKey] = adUnit;
		AnalyticsManager.adsDictEx[AnalyticsManager.placementKey] = placement;
		AnalyticsManager.Log("ad_interstitial_impression_callback", AnalyticsManager.adsDictEx, false);
	}

	public static void FsClick(string adUnit, string placement)
	{
		if (AnalyticsManager.adsDictEx == null)
		{
			AnalyticsManager.adsDictEx = new Dictionary<string, string>();
			AnalyticsManager.adsDictEx.Add(AnalyticsManager.adUnitKey, string.Empty);
			AnalyticsManager.adsDictEx.Add(AnalyticsManager.placementKey, string.Empty);
		}
		AnalyticsManager.adsDictEx[AnalyticsManager.adUnitKey] = adUnit;
		AnalyticsManager.adsDictEx[AnalyticsManager.placementKey] = placement;
		AnalyticsManager.Log("ad_interstitial_click", AnalyticsManager.adsDictEx, false);
	}

	public static void FsFirstLoaded(float time)
	{
		AnalyticsManager.Log("ad_interstitial_loaded1st", new Dictionary<string, string>
		{
			{
				"time",
				time.ToString()
			}
		}, false);
	}

	public static void RewardRequest(RewardEventData r)
	{
		AnalyticsManager.Log("ad_rewarded_request", new Dictionary<string, string>
		{
			{
				AnalyticsManager.countDayKey,
				r.ReqDay.ToString()
			},
			{
				AnalyticsManager.countSessionKey,
				r.ReqSession.ToString()
			}
		}, true);
	}

	public static void RewardLoaded(RewardEventData r)
	{
		AnalyticsManager.Log("ad_rewarded_loaded", new Dictionary<string, string>
		{
			{
				AnalyticsManager.countDayKey,
				r.LoadedDay.ToString()
			},
			{
				AnalyticsManager.countSessionKey,
				r.LoadedSession.ToString()
			},
			{
				"time",
				r.LoadTime.ToString()
			}
		}, false);
	}

	public static void RewardImpression(string adUnit, RewardEventData r)
	{
		AnalyticsManager.Log("ad_rewarded_impression", new Dictionary<string, string>
		{
			{
				AnalyticsManager.adUnitKey,
				adUnit
			},
			{
				AnalyticsManager.countDayKey,
				r.ImpDay.ToString()
			},
			{
				AnalyticsManager.countSessionKey,
				r.ImpSession.ToString()
			},
			{
				"place",
				r.place.ToString().ToLower()
			}
		}, false);
	}

	public static void RewardClick(string adUnit)
	{
		if (AnalyticsManager.adsDict == null)
		{
			AnalyticsManager.adsDict = new Dictionary<string, string>();
			AnalyticsManager.adsDict.Add(AnalyticsManager.adUnitKey, string.Empty);
		}
		AnalyticsManager.adsDict[AnalyticsManager.adUnitKey] = adUnit;
		AnalyticsManager.Log("ad_rewarded_click", AnalyticsManager.adsDict, false);
	}

	public static void RewardCancel(string adUnit)
	{
		if (AnalyticsManager.adsDict == null)
		{
			AnalyticsManager.adsDict = new Dictionary<string, string>();
			AnalyticsManager.adsDict.Add(AnalyticsManager.adUnitKey, string.Empty);
		}
		AnalyticsManager.adsDict[AnalyticsManager.adUnitKey] = adUnit;
		AnalyticsManager.Log("ad_rewarded_canceled", AnalyticsManager.adsDict, false);
	}

	public static void RewardFinish(string adUnit)
	{
		if (AnalyticsManager.adsDict == null)
		{
			AnalyticsManager.adsDict = new Dictionary<string, string>();
			AnalyticsManager.adsDict.Add(AnalyticsManager.adUnitKey, string.Empty);
		}
		AnalyticsManager.adsDict[AnalyticsManager.adUnitKey] = adUnit;
		AnalyticsManager.Log("ad_rewarded_finished", AnalyticsManager.adsDict, false);
	}

	public static void PotentialInterstitialShow(int time)
	{
		AnalyticsManager.Log("ad_interstitial_potential_gameScreen", new Dictionary<string, string>
		{
			{
				"time",
				time.ToString()
			}
		}, false);
	}

	public static void AdsRemoved()
	{
		AnalyticsManager.Log("ad_removed", false);
	}

	public static void InAppPurchased(string inAppId)
	{
		AnalyticsManager.Log("INAPP_PURCHASED", new Dictionary<string, string>
		{
			{
				"sku",
				inAppId
			}
		}, false);
	}

	public static void SysCleanup(int sysPicsCount)
	{
		AnalyticsManager.Log("content_synced", new Dictionary<string, string>
		{
			{
				"imgs",
				sysPicsCount.ToString()
			}
		}, false);
	}

	public static void DbCreationError(string db, string msg)
	{
		AnalyticsManager.Log("db_creation_error", new Dictionary<string, string>
		{
			{
				"db",
				db
			},
			{
				"msg",
				msg
			}
		}, false);
		TGFModule.Instance.DbCreationError(db, msg);
	}

	public static void DbValidationError(string msg)
	{
		AnalyticsManager.Log("db_validation_error", new Dictionary<string, string>
		{
			{
				"msg",
				msg
			}
		}, true);
	}

	public static void DbRecovery(string db)
	{
		AnalyticsManager.Log("db_recreation", new Dictionary<string, string>
		{
			{
				"db",
				db
			}
		}, true);
	}

	public static void PossibleProgressLost(int amount)
	{
		AnalyticsManager.Log("db_progress_error", new Dictionary<string, string>
		{
			{
				"amount",
				amount.ToString()
			}
		}, true);
	}

	public static void DbTransactionError(string db, string msg)
	{
		AnalyticsManager.Log("db_transaction_error", new Dictionary<string, string>
		{
			{
				"db",
				db
			},
			{
				"msg",
				msg
			}
		}, true);
	}

	public static void FairyPopupShow()
	{
		AnalyticsManager.Log("gameScreen_magicHint_popup", false);
	}

	public static void FairyPopupConfirm()
	{
		AnalyticsManager.Log("gameScreen_magicHint_popup_ok", false);
	}

	public static void FairyPopupCancel()
	{
		AnalyticsManager.Log("gameScreen_magicHint_popup_cancel", false);
	}

	public static void FairyButtonClick(float time, string guid, int rewardAmount)
	{
		time = (float)(Mathf.RoundToInt(time / 2f) * 2);
		AnalyticsManager.Log("gameScreen_magicHint", new Dictionary<string, string>
		{
			{
				"time_2",
				time.ToString()
			},
			{
				"banner_shown_id",
				guid
			},
			{
				"ad_bonus_tips_shown_tips_count",
				rewardAmount.ToString()
			}
		}, false);
	}

	public static void HintPopupShow()
	{
		AnalyticsManager.Log("gameScreen_hint_popup", false);
	}

	public static void HintPopupConfirm()
	{
		AnalyticsManager.Log("gameScreen_hint_popup_ok", false);
	}

	public static void HintPopupCancel()
	{
		AnalyticsManager.Log("gameScreen_hint_popup_cancel", false);
	}

	public static void HintUse(int picUses, int totalUses, int picId, string guid)
	{
		AnalyticsManager.Log("gameScreen_hint", new Dictionary<string, string>
		{
			{
				"countPic",
				picUses.ToString()
			},
			{
				"countLT",
				totalUses.ToString()
			},
			{
				"level_id",
				picId.ToString()
			},
			{
				"level_try_uuid",
				guid
			}
		}, false);
	}

	public static void MenuCategoryFilterClick()
	{
		AnalyticsManager.Log("mainList_tapSearch", false);
	}

	public static void MenuCategoryHideCompletedClick(bool isOn)
	{
		if (isOn)
		{
			AnalyticsManager.Log("mainList_filter_hide_on", false);
		}
		else
		{
			AnalyticsManager.Log("mainList_filter_hide_off", false);
		}
	}

	public static void MenuCategoryFilterByCategory(int id)
	{
		AnalyticsManager.Log("mainList_filter_categroy", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			}
		}, false);
	}

	public static void FeaturedDailyClick(int id, int order)
	{
		AnalyticsManager.Log("mainList_featured_daily", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			},
			{
				"order",
				order.ToString()
			}
		}, false);
	}

	public static void FeaturePromoPicClick(int id, int order)
	{
		AnalyticsManager.Log("mainList_featured_editors", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			},
			{
				"order",
				order.ToString()
			}
		}, false);
	}

	public static void DailyTabFeaturedClick(int id)
	{
		AnalyticsManager.Log("dailyList_featured_daily", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			}
		}, false);
	}

	public static void SettingsNewDesign()
	{
		AnalyticsManager.Log("settings_new_design", false);
	}

	public static void SettingsOldDesign()
	{
		AnalyticsManager.Log("settings_old_design", false);
	}

	public static void FirebaseMessagingError(string os)
	{
		AnalyticsManager.Log("error_firebase_messaging", new Dictionary<string, string>
		{
			{
				"os",
				os
			}
		}, false);
	}

	public static void GetMoreBonusContentCatBar()
	{
		AnalyticsManager.Log("mainList_filter_findMore", false);
	}

	public static void GetMoreBonusContentEmptyCat()
	{
		AnalyticsManager.Log("mainList_bonusEmpty_findMore", false);
	}

	public static void DeeplinkQueueError()
	{
		AnalyticsManager.Log("deepLink_queueError", false);
	}

	public static void BonusContentClaim(string code)
	{
		AnalyticsManager.Log("deepLink_bonusClaimed", new Dictionary<string, string>
		{
			{
				"id",
				code
			}
		}, false);
	}

	public static void BonusContentClaimUsedCode()
	{
		AnalyticsManager.Log("deepLink_bonusError", false);
	}

	public static void AnimationSkip(int id)
	{
		AnalyticsManager.Log("coloring_animation_skip", new Dictionary<string, string>
		{
			{
				"id",
				id.ToString()
			}
		}, false);
	}

	public static void BonusCategorySelected(int picsInCat)
	{
		AnalyticsManager.Log("mainList_filter_categoryBonus", new Dictionary<string, string>
		{
			{
				"count",
				picsInCat.ToString()
			}
		}, false);
	}

	public static void BonusPictureStartColoring(int picId)
	{
		AnalyticsManager.Log("mainList_tapBonus", new Dictionary<string, string>
		{
			{
				"id",
				picId.ToString()
			}
		}, false);
	}

	public static void DailyStartColoring(int picId)
	{
		AnalyticsManager.Log("coloring_start_daily", new Dictionary<string, string>
		{
			{
				"id",
				picId.ToString()
			}
		}, false);
	}

	public static void FeaturedExternalLink(int blockId)
	{
		AnalyticsManager.Log("mainList_featured_link", new Dictionary<string, string>
		{
			{
				"id",
				blockId.ToString()
			}
		}, false);
	}

	public static void AppOpenStreak(int daysInARow)
	{
		string eventName = "m_open_inrow_" + daysInARow + "d";
		AnalyticsManager.Log(eventName, false);
	}

	public static void DailyPlayTime(int time)
	{
		string eventName = "m_mins_a_day_" + time;
		AnalyticsManager.Log(eventName, false);
	}

	public static void PictureSolvedCount(int count)
	{
		string eventName = "m_pics_finished_" + count;
		AnalyticsManager.Log(eventName, false);
	}

	public static void AnimWatchedCount(int count)
	{
		string eventName = "m_watch_end_" + count;
		AnalyticsManager.Log(eventName, false);
	}

	public static void AppOpened(bool isRemotePush, string remotePushId, bool isLocalPush)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string value;
		if (isRemotePush)
		{
			value = remotePushId;
		}
		else if (isLocalPush)
		{
			value = "local";
		}
		else
		{
			value = "null";
		}
		dictionary.Add("PUSH_TYPE_ID", value);
		AnalyticsManager.Log("APPLICATION_OPEN", dictionary, false);
	}

	public static void LevelTryStarted(int picId, string guid)
	{
		if (BuildConfig.DISABLE_ANALYTICS)
		{
			return;
		}
		string eventName = "LEVEL_TRY_STARTED";
		if (AnalyticsManager.levelTryDict == null)
		{
			AnalyticsManager.levelTryDict = new Dictionary<string, string>();
			AnalyticsManager.AddDefParametersToDict(AnalyticsManager.levelTryDict);
			AnalyticsManager.levelTryDict.Add("level_try_uuid", string.Empty);
			AnalyticsManager.levelTryDict.Add("level_id", string.Empty);
		}
		else
		{
			AnalyticsManager.SetDefParamsInDict(AnalyticsManager.levelTryDict);
		}
		AnalyticsManager.levelTryDict["level_try_uuid"] = guid;
		AnalyticsManager.levelTryDict["level_id"] = picId.ToString();
		AnalyticsManager.InternalFirebaseLog(eventName, AnalyticsManager.levelTryDict);
	}

	public static void LevelTrySucceed(int picId, string guid)
	{
		if (BuildConfig.DISABLE_ANALYTICS)
		{
			return;
		}
		string eventName = "LEVEL_TRY_SUCCEED";
		if (AnalyticsManager.levelTryDict == null)
		{
			AnalyticsManager.levelTryDict = new Dictionary<string, string>();
			AnalyticsManager.AddDefParametersToDict(AnalyticsManager.levelTryDict);
			AnalyticsManager.levelTryDict.Add("level_try_uuid", string.Empty);
			AnalyticsManager.levelTryDict.Add("level_id", string.Empty);
		}
		else
		{
			AnalyticsManager.SetDefParamsInDict(AnalyticsManager.levelTryDict);
		}
		AnalyticsManager.levelTryDict["level_try_uuid"] = guid;
		AnalyticsManager.levelTryDict["level_id"] = picId.ToString();
		AnalyticsManager.InternalFirebaseLog(eventName, AnalyticsManager.levelTryDict);
	}

	public static void LevelTryExit(int picId, string guid)
	{
		if (BuildConfig.DISABLE_ANALYTICS)
		{
			return;
		}
		string eventName = "LEVEL_TRY_ENDED";
		if (AnalyticsManager.levelTryDict == null)
		{
			AnalyticsManager.levelTryDict = new Dictionary<string, string>();
			AnalyticsManager.AddDefParametersToDict(AnalyticsManager.levelTryDict);
			AnalyticsManager.levelTryDict.Add("level_try_uuid", string.Empty);
			AnalyticsManager.levelTryDict.Add("level_id", string.Empty);
		}
		else
		{
			AnalyticsManager.SetDefParamsInDict(AnalyticsManager.levelTryDict);
		}
		AnalyticsManager.levelTryDict["level_try_uuid"] = guid;
		AnalyticsManager.levelTryDict["level_id"] = picId.ToString();
		AnalyticsManager.InternalFirebaseLog(eventName, AnalyticsManager.levelTryDict);
	}

	public static void LevelProgress(int picId, string guid, int zoneId, int order)
	{
		if (BuildConfig.DISABLE_ANALYTICS)
		{
			return;
		}
		string eventName = "LEVEL_PROGRESS";
		if (AnalyticsManager.levelProgressDict == null)
		{
			AnalyticsManager.levelProgressDict = new Dictionary<string, string>();
			AnalyticsManager.AddDefParametersToDict(AnalyticsManager.levelProgressDict);
			AnalyticsManager.levelProgressDict.Add("level_progress_order", string.Empty);
			AnalyticsManager.levelProgressDict.Add("level_progress_region_id", string.Empty);
			AnalyticsManager.levelProgressDict.Add("level_try_uuid", string.Empty);
			AnalyticsManager.levelProgressDict.Add("level_id", string.Empty);
		}
		else
		{
			AnalyticsManager.SetDefParamsInDict(AnalyticsManager.levelProgressDict);
		}
		AnalyticsManager.levelProgressDict["level_progress_order"] = order.ToString();
		AnalyticsManager.levelProgressDict["level_progress_region_id"] = zoneId.ToString();
		AnalyticsManager.levelProgressDict["level_try_uuid"] = guid;
		AnalyticsManager.levelProgressDict["level_id"] = picId.ToString();
		AnalyticsManager.InternalFirebaseLog(eventName, AnalyticsManager.levelProgressDict);
	}

	public static void FairyShown(string guid, int rewardAmount)
	{
		AnalyticsManager.Log("AD_TIPS_BONUS_SHOWN", new Dictionary<string, string>
		{
			{
				"banner_shown_id",
				guid
			},
			{
				"ad_bonus_tips_shown_tips_count",
				rewardAmount.ToString()
			},
			{
				"banner_position",
				"left"
			}
		}, false);
	}

	public static void FeaturedItemVisable(int id, string typeName)
	{
		if (BuildConfig.DISABLE_ANALYTICS)
		{
			return;
		}
		string eventName = "LEVEL_MINI_SHOWN";
		if (AnalyticsManager.featuredShowDict == null)
		{
			AnalyticsManager.featuredShowDict = new Dictionary<string, string>();
			AnalyticsManager.AddDefParametersToDict(AnalyticsManager.featuredShowDict);
			AnalyticsManager.featuredShowDict.Add("level_mini_shown_region", string.Empty);
			AnalyticsManager.featuredShowDict.Add("level_mini_shown_id", string.Empty);
		}
		else
		{
			AnalyticsManager.SetDefParamsInDict(AnalyticsManager.featuredShowDict);
		}
		AnalyticsManager.featuredShowDict["level_mini_shown_region"] = typeName;
		AnalyticsManager.featuredShowDict["level_mini_shown_id"] = id.ToString();
		AnalyticsManager.InternalFirebaseLog(eventName, AnalyticsManager.featuredShowDict);
	}

	public static void AdsEventLog(string eventName, bool firebaseOnly)
	{
		AnalyticsManager.Log(eventName, firebaseOnly);
	}

	public static void AdsEventLog(string eventName, Dictionary<string, string> dict, bool firebaseOnly)
	{
		AnalyticsManager.Log(eventName, dict, firebaseOnly);
	}

	public static void NewTabOpened()
	{
		AnalyticsManager.Log("menuTab_news", false);
	}

	private static Dictionary<string, string> defParamsDict;

	private const string def_param_date_time_key = "DATE_TIME";

	private const string def_param_session_id_key = "APP_SESSION_UUID";

	private const string def_param_user_id_key = "USER_ID";

	private static Dictionary<string, string> emptyDictionary = new Dictionary<string, string>();

	private static Dictionary<string, string> adsDict;

	private static readonly string adUnitKey = "adUnit";

	private static readonly string countDayKey = "count_d";

	private static readonly string countSessionKey = "count_s";

	private static Dictionary<string, string> adsDictEx;

	private static readonly string placementKey = "placement";

	private static Dictionary<string, string> levelTryDict;

	private static Dictionary<string, string> levelProgressDict;

	private static Dictionary<string, string> featuredShowDict;
}

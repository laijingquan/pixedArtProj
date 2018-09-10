// dnSpy decompiler from Assembly-CSharp.dll
using System;

public static class AppState
{
	public static bool HasDailyBonus { get; private set; }

	public static bool TimingBonusReady { get; set; }

	public static bool IntentionalPause
	{
		get
		{
			return AppState.ads || AppState.sys || AppState.inapp;
		}
	}

	public static bool ShowDesignUpdDialog { get; set; }

	public static bool LocalNotificationLaunch { get; set; }

	public static bool PushNotificationLaunch { get; set; }

	public static string PushNotificationId { get; set; }

	public static bool IsGiftCodeLaunch
	{
		get
		{
			return AppState.BonusCode != null && !string.IsNullOrEmpty(AppState.BonusCode.BonusCode);
		}
	}

	public static BonusCodeData BonusCode { get; set; }

	public static DateTime LaunchTime { get; set; }

	public static DateTime ContentReqTime { get; set; }

	public static void AdsShowPause()
	{
		AppState.adsPauseTime = DateTime.Now;
		AppState.ads = true;
	}

	public static void SystemUtilsPause()
	{
		AppState.sysPauseTime = DateTime.Now;
		AppState.sys = true;
	}

	public static void InAppPause()
	{
		AppState.SystemUtilsPause();
	}

	public static void ResetPauseState()
	{
		AppState.ads = false;
		AppState.sys = false;
	}

	public static void ValidatePauseState()
	{
		if (AppState.ads)
		{
			double totalSeconds = (DateTime.Now - AppState.adsPauseTime).TotalSeconds;
			if (totalSeconds > 6.0)
			{
				AppState.ads = false;
			}
		}
		if (AppState.sys)
		{
			double totalSeconds2 = (DateTime.Now - AppState.sysPauseTime).TotalSeconds;
			if (totalSeconds2 > 6.0)
			{
				AppState.sys = false;
			}
		}
		if (AppState.inapp)
		{
			AppState.inappPauseCount++;
			if (AppState.inappPauseCount > 2)
			{
				AppState.inapp = false;
			}
			else
			{
				double totalSeconds3 = (DateTime.Now - AppState.inAppPauseTime).TotalSeconds;
				if (totalSeconds3 > 60.0)
				{
					AppState.inapp = false;
				}
			}
		}
	}

	public static bool IsServerContentExpired()
	{
		double totalMinutes = (DateTime.Now - AppState.ContentReqTime).TotalMinutes;
		FMLogger.Log("check exp time. t:" + totalMinutes);
		return totalMinutes > 240.0;
	}

	public static void AddDailyBonus()
	{
		AppState.HasDailyBonus = true;
	}

	public static void ConsumeDailyBonus()
	{
		AppState.HasDailyBonus = false;
	}

	private static bool ads;

	private static bool sys;

	private static bool inapp;

	private static DateTime adsPauseTime;

	private static DateTime sysPauseTime;

	private static DateTime inAppPauseTime;

	private static int inappPauseCount;

	private const int RESET_CACHE_MINUTES = 240;
}

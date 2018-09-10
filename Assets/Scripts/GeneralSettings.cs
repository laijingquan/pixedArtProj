// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GeneralSettings
{
	public static bool IsOldDesign
	{
		get
		{
			return GeneralSettings.CurrentDesign == GeneralSettings.DesignType.Legacy;
		}
	}

	public static GeneralSettings.DesignType CurrentDesign
	{
		get
		{
			return (GeneralSettings.DesignType)PlayerPrefs.GetInt("design_type", 0);
		}
		set
		{
			PlayerPrefs.SetInt("design_type", (int)value);
		}
	}

	public static bool CanUseLegacyDesign
	{
		get
		{
			return PlayerPrefs.GetInt("can_use_legacy_design", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("can_use_legacy_design", (!value) ? 0 : 1);
		}
	}

	public static bool FilterCompleted
	{
		get
		{
			return PlayerPrefs.GetInt("filter_completed", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("filter_completed", (!value) ? 0 : 1);
		}
	}

	public static int SavesCount
	{
		get
		{
			return PlayerPrefs.GetInt("saves_count", 0);
		}
		set
		{
			PlayerPrefs.SetInt("saves_count", value);
		}
	}

	public static int LastDailyItemId
	{
		get
		{
			return PlayerPrefs.GetInt("last_daily_pic_id", -1);
		}
		set
		{
			PlayerPrefs.SetInt("last_daily_pic_id", value);
		}
	}

	public static int NumSize
	{
		get
		{
			return PlayerPrefs.GetInt("gb_num_size", NumberController.DefaultNumSize());
		}
		set
		{
			PlayerPrefs.SetInt("gb_num_size", value);
		}
	}

	public static int HintsCount
	{
		get
		{
			return PlayerPrefs.GetInt("coinsKey", 3);
		}
		private set
		{
			PlayerPrefs.SetInt("coinsKey", value);
		}
	}

	public static void UpdateCoins(int bonusCoins)
	{
		GeneralSettings.HintsCount += bonusCoins;
	}

	public static void HintUsed()
	{
		GeneralSettings.HintsUsed++;
		GeneralSettings.HintsCount--;
	}

	public static bool IsHintFirstRewarded
	{
		get
		{
			return GeneralSettings.HintRewardUse == 0;
		}
	}

	public static int HintRewardUse
	{
		get
		{
			return PlayerPrefs.GetInt("rewardUseKey", 0);
		}
		set
		{
			PlayerPrefs.SetInt("rewardUseKey", value);
		}
	}

	public static bool IsFairyFirstUse
	{
		get
		{
			return GeneralSettings.FairyUseCount == 0;
		}
	}

	public static int FairyUseCount
	{
		get
		{
			return PlayerPrefs.GetInt("fairyUseKey", 0);
		}
		set
		{
			PlayerPrefs.SetInt("fairyUseKey", value);
		}
	}

	public static int HintsUsed
	{
		get
		{
			return PlayerPrefs.GetInt("coinsUsedKey", 0);
		}
		private set
		{
			PlayerPrefs.SetInt("coinsUsedKey", value);
		}
	}

	public static bool AdsDisabled
	{
		get
		{
			if (GeneralSettings.adsDisabled == -1)
			{
				GeneralSettings.adsDisabled = ((!GeneralSettings.AdsDisabledPrefs) ? 0 : 1);
			}
			return GeneralSettings.adsDisabled == 1;
		}
		set
		{
			if (value != (GeneralSettings.adsDisabled == 1) && value)
			{
				GeneralSettings.adsDisabled = ((!value) ? 0 : 1);
				GeneralSettings.AdsDisabledPrefs = value;
				AdsManager.Instance.DisableAds();
			}
		}
	}

	private static bool AdsDisabledPrefs
	{
		get
		{
			return PlayerPrefs.GetInt("adsKey", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("adsKey", (!value) ? 0 : 1);
			PlayerPrefs.Save();
		}
	}

	private static int RateCount
	{
		get
		{
			return PlayerPrefs.GetInt("showrateCount", 4);
		}
		set
		{
			PlayerPrefs.SetInt("showrateCount", value);
		}
	}

	public static int RateDisplayCount
	{
		get
		{
			return PlayerPrefs.GetInt("actShowrateCount", 0);
		}
		private set
		{
			PlayerPrefs.SetInt("actShowrateCount", value);
		}
	}

	private static int RateStep
	{
		get
		{
			return PlayerPrefs.GetInt("rateStepKey", 5);
		}
		set
		{
			PlayerPrefs.SetInt("rateStepKey", value);
		}
	}

	private static string RateVersion
	{
		get
		{
			return PlayerPrefs.GetString("rate_version", string.Empty);
		}
		set
		{
			PlayerPrefs.SetString("rate_version", value);
		}
	}

	private static int RateVersionImpressions
	{
		get
		{
			return PlayerPrefs.GetInt("rate_version_imp", 0);
		}
		set
		{
			PlayerPrefs.SetInt("rate_version_imp", value);
		}
	}

	internal static bool IsShowRate()
	{
		int num = GeneralSettings.RateCount;
		num++;
		int rateStep = GeneralSettings.RateStep;
		bool result = num == rateStep;
		if (num >= rateStep)
		{
			num = 0;
		}
		GeneralSettings.RateCount = num;
		GeneralSettings.RateDisplayCount++;
		return result;
	}

	internal static void MarkRateAsClicked()
	{
		GeneralSettings.RateStep = 100000;
		GeneralSettings.RateCount = 0;
	}

	public static int UpdateDialogVersion
	{
		get
		{
			return PlayerPrefs.GetInt("updDialogKey", 0);
		}
		set
		{
			PlayerPrefs.SetInt("updDialogKey", value);
		}
	}

	public static int DailyUsedTime
	{
		get
		{
			return PlayerPrefs.GetInt("dailyUsedKey", -1);
		}
		set
		{
			PlayerPrefs.SetInt("dailyUsedKey", value);
		}
	}

	public static SystemUtils.DevicePerfomance DevicePerfomance
	{
		get
		{
			return (SystemUtils.DevicePerfomance)PlayerPrefs.GetInt("f_dp", -1);
		}
		set
		{
			PlayerPrefs.SetInt("f_dp", (int)value);
		}
	}

	public static int AppLaunchCounter
	{
		get
		{
			return PlayerPrefs.GetInt("appLaunchCounterKey", 0);
		}
		set
		{
			PlayerPrefs.SetInt("appLaunchCounterKey", value);
		}
	}

	public static bool IsPreInstallAppsflyerUser()
	{
		return DatabaseManager.Version != 0 && GeneralSettings.AppLaunchCounter == 1;
	}

	public static bool SoundEnabled;

	public static bool MusicEnabled;

	public static bool NotificationsEnabled;

	private const string coinKey = "coinsKey";

	private const string coinsUsedKey = "coinsUsedKey";

	private const int DEFAULT_COINS_COUNT = 3;

	private const string rewardUseKey = "rewardUseKey";

	private const string fairydUseKey = "fairyUseKey";

	private static int adsDisabled = -1;

	private const string adsKey = "adsKey";

	private const string rateKey = "showrateCount";

	private const string rateDispKey = "actShowrateCount";

	private const string rateStepKey = "rateStepKey";

	private const int initialStep = 5;

	private const int maxStep = 100000;

	private const string updDialogKey = "updDialogKey";

	private const string dailyUsedKey = "dailyUsedKey";

	private const string appLaunchCounterKey = "appLaunchCounterKey";

	public enum DesignType
	{
		Undef,
		Legacy,
		Neo
	}
}

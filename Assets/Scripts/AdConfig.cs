// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AdConfig
{
	public AdConfig()
	{
		this.adsDisabled = false;
		this.bannerPos = BannerPosition.Bottom;
		this.bannerPlaces = new List<BannerPlacement>
		{
			BannerPlacement.Gameboard
		};
		this.rewardConfig = AdConfig.DefaultRewardConfig();
		this.fsInternalOnReward = 5;
		this.bannerLoadDelay = 10;
		this.bannerShowTime = 15;
		this.adsTest = false;
	}

	public bool BannerIsOn()
	{
		return !this.adsDisabled && !string.IsNullOrEmpty(this.bannerAdUnit) && this.bannerPlaces != null && this.bannerPos != BannerPosition.None && this.bannerPlaces.Count != 0;
	}

	public bool FsIsOn()
	{
		return !this.adsDisabled && !string.IsNullOrEmpty(this.fsAdUnit) && this.placements != null && this.placements.Count != 0 && this.chances != null;
	}

	public bool WantToShowPlacement(AdPlacement placement)
	{
		if (this.adsDisabled)
		{
			return false;
		}
		if (this.HasPlacement(placement))
		{
			int num = this.placements.IndexOf(placement);
			if (num != -1 && UnityEngine.Random.Range(0, 100) < this.chances[num])
			{
				return true;
			}
		}
		return false;
	}

	public bool HasPlacement(AdPlacement placement)
	{
		return !this.adsDisabled && this.placements != null && this.placements.Contains(placement);
	}

	public bool HasBannerPlacement(BannerPlacement placement)
	{
		return !this.adsDisabled && this.bannerPlaces != null && this.bannerPlaces.Contains(placement);
	}

	public void UpdateFsParamsFromServer(AdConfig upd)
	{
		if (upd == null)
		{
			return;
		}
		this.interval = upd.interval;
		if (GeneralSettings.AppLaunchCounter > 1)
		{
			this.startInterval = upd.startInterval;
		}
		this.appLaunchDelay = upd.appLaunchDelay;
		this.placements = upd.placements;
		this.chances = upd.chances;
	}

	public void DisableAds()
	{
		this.adsDisabled = true;
		this.placements = null;
		this.bannerPlaces = null;
	}

	public void UpdateReward(RewardConfig respConfig, string newRewardAdUnit)
	{
		if (respConfig != null && respConfig.IsValid())
		{
			this.rewardConfig = respConfig;
		}
		if (!string.IsNullOrEmpty(newRewardAdUnit))
		{
			this.rewardAdUnit = newRewardAdUnit;
		}
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"adsIsOff :",
			this.adsDisabled,
			" placeCount: ",
			this.placements.Count,
			" fsInt:",
			this.interval,
			" startFsInt:",
			this.startInterval,
			" appLaunchDelay:",
			this.appLaunchDelay,
			" bannerPos:",
			this.bannerPos
		});
	}

	private static AdPlacement ParsePlacement(string p)
	{
		if (string.IsNullOrEmpty(p))
		{
			return AdPlacement.Unknown;
		}
		if (p.Equals("game_complete"))
		{
			return AdPlacement.Solved;
		}
		if (p.Equals("gameScreen_back"))
		{
			return AdPlacement.GameboardBack;
		}
		if (p.Equals("start_game"))
		{
			return AdPlacement.GameboardStart;
		}
		if (p.Equals("tabLibrary"))
		{
			return AdPlacement.LibNav;
		}
		if (p.Equals("tabMyFeed"))
		{
			return AdPlacement.FeedNav;
		}
		if (p.Equals("tabMenu"))
		{
			return AdPlacement.MenuNav;
		}
		if (p.Equals("appResume"))
		{
			return AdPlacement.AppResume;
		}
		if (p.Equals("appLaunch"))
		{
			return AdPlacement.AppLaunch;
		}
		return AdPlacement.Unknown;
	}

	public static string AdPlacementToString(AdPlacement placement)
	{
		switch (placement)
		{
		case AdPlacement.Unknown:
			return "unknown";
		case AdPlacement.Solved:
			return "game_complete";
		case AdPlacement.GameboardBack:
			return "gameScreen_back";
		case AdPlacement.GameboardStart:
			return "start_game";
		case AdPlacement.MenuNav:
			return "tabMenu";
		case AdPlacement.FeedNav:
			return "tabMyFeed";
		case AdPlacement.LibNav:
			return "tabLibrary";
		case AdPlacement.AppResume:
			return "appResume";
		case AdPlacement.AppLaunch:
			return "appLaunch";
		default:
			return "unknown";
		}
	}

	private static BannerPlacement ParseBannerPlacement(string p)
	{
		if (string.IsNullOrEmpty(p))
		{
			return BannerPlacement.Unknown;
		}
		if (p.Equals("game_screen"))
		{
			return BannerPlacement.Gameboard;
		}
		if (p.Equals("game_result"))
		{
			return BannerPlacement.Solved;
		}
		if (p.Equals("all_tabs"))
		{
			return BannerPlacement.Menu;
		}
		return BannerPlacement.Unknown;
	}

	public static AdConfig DefaultPhone()
	{
		return new AdConfig
		{
			placements = new List<AdPlacement>
			{
				AdPlacement.Solved,
				AdPlacement.GameboardStart
			},
			chances = new List<int>
			{
				100,
				100
			},
			interval = 30,
			startInterval = 90,
			appLaunchDelay = 3f,
			fsInternalOnReward = 5,
			bannerLoadDelay = 10,
			bannerShowTime = 15,
			adsTest = false,
			bannerAdUnit = "f22e60410d82403aa5e0fb791ef9c153",
			fsAdUnit = "72bb0678c400487b8d1a941944fa6888",
			rewardAdUnit = "6bc3898062484e71a114d0ab59cb1c78",
			bannerPos = BannerPosition.Bottom,
			bannerPlaces = new List<BannerPlacement>
			{
				BannerPlacement.Gameboard
			},
			adsDisabled = false,
			rewardConfig = AdConfig.DefaultRewardConfig()
		};
	}

	public static AdConfig DefaultTablet()
	{
		return new AdConfig
		{
			placements = new List<AdPlacement>
			{
				AdPlacement.Solved,
				AdPlacement.GameboardStart
			},
			chances = new List<int>
			{
				100,
				100
			},
			interval = 30,
			startInterval = 90,
			appLaunchDelay = 3f,
			bannerAdUnit = "1253b5589a4d45869288611de14229d6",
			fsAdUnit = "8cae328870984a9987623f9c6e52b25d",
			rewardAdUnit = "0543e571406140dd96252ac1351b99f5",
			fsInternalOnReward = 5,
			bannerLoadDelay = 10,
			bannerShowTime = 15,
			adsTest = false,
			bannerPos = BannerPosition.Bottom,
			bannerPlaces = new List<BannerPlacement>
			{
				BannerPlacement.Gameboard
			},
			adsDisabled = false,
			rewardConfig = AdConfig.DefaultRewardConfig()
		};
	}

	public static RewardConfig DefaultRewardConfig()
	{
		return new RewardConfig
		{
			dailyBonus = 1,
			timingBonus = 1,
			timingDelay = 60,
			timingInterval = 300,
			timingBonusShowTime = 10
		};
	}

	public static AdConfig FromResponse(AdsServerResponse response, bool isTablet)
	{
		AdConfig adConfig = new AdConfig();
		if (!string.IsNullOrEmpty(response.adUnit_banner))
		{
			adConfig.bannerAdUnit = response.adUnit_banner;
		}
		else
		{
			adConfig.bannerAdUnit = ((!isTablet) ? "f22e60410d82403aa5e0fb791ef9c153" : "1253b5589a4d45869288611de14229d6");
		}
		if (!string.IsNullOrEmpty(response.adUnit_fullscreen))
		{
			adConfig.fsAdUnit = response.adUnit_fullscreen;
		}
		else
		{
			adConfig.fsAdUnit = ((!isTablet) ? "72bb0678c400487b8d1a941944fa6888" : "8cae328870984a9987623f9c6e52b25d");
		}
		if (!string.IsNullOrEmpty(response.adUnit_rewarded))
		{
			adConfig.rewardAdUnit = response.adUnit_rewarded;
		}
		else
		{
			adConfig.rewardAdUnit = ((!isTablet) ? "6bc3898062484e71a114d0ab59cb1c78" : "0543e571406140dd96252ac1351b99f5");
		}
		adConfig.adsDisabled = (response.ad_module_active == 0);
		adConfig.interval = response.interstitial_delay / 1000;
		adConfig.startInterval = response.interstitial_delay_onstart / 1000;
		adConfig.appLaunchDelay = (float)response.applaunch_delay / 1000f;
		adConfig.fsInternalOnReward = Mathf.Max(0, response.interstitial_delay_onreward / 1000);
		adConfig.bannerShowTime = Mathf.Max(10, response.banner_show_time / 1000);
		adConfig.bannerLoadDelay = Mathf.Max(1, response.banner_load_delay / 1000);
		adConfig.adsTest = (response.ad_module_test_mode == 1);
		adConfig.placements = new List<AdPlacement>();
		adConfig.chances = new List<int>();
		if (response.fullscreen_showing_config != null)
		{
			for (int i = 0; i < response.fullscreen_showing_config.Length; i++)
			{
				AdsFsPlacementConfig adsFsPlacementConfig = response.fullscreen_showing_config[i];
				AdPlacement adPlacement = AdConfig.ParsePlacement(adsFsPlacementConfig.id);
				if (adPlacement != AdPlacement.Unknown)
				{
					adConfig.placements.Add(adPlacement);
					adConfig.chances.Add(Mathf.Clamp(adsFsPlacementConfig.chance, 0, 100));
				}
			}
		}
		if (response.banner_showing_config != null && response.banner_showing_config.id != null)
		{
			adConfig.bannerPos = AdConfig.ParseBannerPosition(response.banner_showing_config.show);
			if (adConfig.bannerPos != BannerPosition.None)
			{
				adConfig.bannerPlaces = new List<BannerPlacement>();
				for (int j = 0; j < response.banner_showing_config.id.Length; j++)
				{
					BannerPlacement bannerPlacement = AdConfig.ParseBannerPlacement(response.banner_showing_config.id[j]);
					if (bannerPlacement != BannerPlacement.Unknown)
					{
						adConfig.bannerPlaces.Add(bannerPlacement);
					}
				}
			}
		}
		else
		{
			adConfig.bannerPos = BannerPosition.None;
		}
		adConfig.rewardConfig = AdConfig.FromRespone(response.rewarded_config, false);
		return adConfig;
	}

	public static RewardConfig FromRespone(RewardedAdResponseConfig response, bool allowNull = true)
	{
		if (response != null)
		{
			return new RewardConfig
			{
				dailyBonus = Mathf.Clamp(response.dailyBonusHints, 1, 1000),
				timingBonus = Mathf.Clamp(response.timingBonusHints, 1, 1000),
				timingDelay = Mathf.Clamp(response.timingBonusDelay / 1000, 1, int.MaxValue),
				timingBonusShowTime = Mathf.Clamp(response.timingBonusShowTime / 1000, 1, int.MaxValue),
				timingInterval = Mathf.Clamp(response.timingBonusInterval / 1000, 30, int.MaxValue)
			};
		}
		if (allowNull)
		{
			return null;
		}
		return AdConfig.DefaultRewardConfig();
	}

	private static BannerPosition ParseBannerPosition(string show)
	{
		if (string.IsNullOrEmpty(show))
		{
			return BannerPosition.None;
		}
		if (show.Equals("top"))
		{
			return BannerPosition.Top;
		}
		if (show.Equals("bottom"))
		{
			return BannerPosition.Bottom;
		}
		return BannerPosition.None;
	}

	public bool adsDisabled;

	public List<AdPlacement> placements;

	public List<int> chances;

	public int interval;

	public int startInterval;

	public float appLaunchDelay;

	public string bannerAdUnit;

	public string fsAdUnit;

	public string rewardAdUnit;

	public int fsInternalOnReward;

	public int bannerShowTime;

	public int bannerLoadDelay;

	public bool adsTest;

	public BannerPosition bannerPos;

	public List<BannerPlacement> bannerPlaces;

	public RewardConfig rewardConfig;

	public const string PHONE_BANNER = "f22e60410d82403aa5e0fb791ef9c153";

	public const string PHONE_INTERSTITIAL = "72bb0678c400487b8d1a941944fa6888";

	public const string PHONE_REWARDED = "6bc3898062484e71a114d0ab59cb1c78";

	public const string TABLET_BANNER = "1253b5589a4d45869288611de14229d6";

	public const string TABLET_INTERSTITIAL = "8cae328870984a9987623f9c6e52b25d";

	public const string TABLET_REWARDED = "0543e571406140dd96252ac1351b99f5";
}

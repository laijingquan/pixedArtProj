// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PlayTimeEventTracker
{
	private static int AnimWatchedCounter
	{
		get
		{
			return PlayerPrefs.GetInt("pte_animWatched", 0);
		}
		set
		{
			PlayerPrefs.SetInt("pte_animWatched", value);
		}
	}

	private static int PicsSolvedCounter
	{
		get
		{
			return PlayerPrefs.GetInt("pte_picsSolved", 0);
		}
		set
		{
			PlayerPrefs.SetInt("pte_picsSolved", value);
		}
	}

	private static int OpenStreakSent
	{
		get
		{
			return PlayerPrefs.GetInt("pte_openStreakSent", 0);
		}
		set
		{
			PlayerPrefs.SetInt("pte_openStreakSent", value);
		}
	}

	private static int CurrentOpenStreak
	{
		get
		{
			return PlayerPrefs.GetInt("pte_openStreak", 1);
		}
		set
		{
			PlayerPrefs.SetInt("pte_openStreak", value);
		}
	}

	private static int LastOpenDay
	{
		get
		{
			return PlayerPrefs.GetInt("pte_lastOpen", 0);
		}
		set
		{
			PlayerPrefs.SetInt("pte_lastOpen", value);
		}
	}

	private static int DailyPlayTime
	{
		get
		{
			return PlayerPrefs.GetInt("pte_playTime", 0);
		}
		set
		{
			PlayerPrefs.SetInt("pte_playTime", value);
		}
	}

	private static int DailyPlaySentStepIndex
	{
		get
		{
			return PlayerPrefs.GetInt("pte_playTimeStep", 0);
		}
		set
		{
			PlayerPrefs.SetInt("pte_playTimeStep", value);
		}
	}

	public static void AnimWatched()
	{
		PlayTimeEventTracker.AnimWatchedCounter++;
		if (PlayTimeEventTracker.AnimWatchedCounter == 10)
		{
			PlayTimeEventTracker.SendAnimWatchCount(10);
		}
	}

	public static void PictureSolved()
	{
		int[] array = new int[]
		{
			5,
			15,
			30,
			50,
			100
		};
		PlayTimeEventTracker.PicsSolvedCounter++;
		int num = PlayTimeEventTracker.PicsSolvedCounter;
		for (int i = 0; i < array.Length; i++)
		{
			if (num == array[i])
			{
				PlayTimeEventTracker.SendPicsSolvedCount(array[i]);
				break;
			}
		}
	}

	public static void AppResume()
	{
		int dayOfYear = DateTime.UtcNow.DayOfYear;
		if (dayOfYear != PlayTimeEventTracker.LastOpenDay)
		{
			if (dayOfYear - 1 == PlayTimeEventTracker.LastOpenDay)
			{
				PlayTimeEventTracker.CurrentOpenStreak++;
				int[] array = new int[]
				{
					3,
					5,
					7,
					14,
					28
				};
				for (int i = 0; i < array.Length; i++)
				{
					if (PlayTimeEventTracker.CurrentOpenStreak == array[i])
					{
						PlayTimeEventTracker.SendOpenStreak(array[i]);
						break;
					}
				}
			}
			else
			{
				PlayTimeEventTracker.CurrentOpenStreak = 1;
			}
		}
		if (dayOfYear != PlayTimeEventTracker.LastOpenDay)
		{
			PlayTimeEventTracker.DailyPlayTime = 0;
			PlayTimeEventTracker.DailyPlaySentStepIndex = -1;
		}
		PlayTimeEventTracker.launchTime = DateTime.UtcNow;
		PlayTimeEventTracker.LastOpenDay = dayOfYear;
	}

	public static void AppPause()
	{
		int num = Mathf.RoundToInt((float)(DateTime.UtcNow - PlayTimeEventTracker.launchTime).TotalSeconds);
		PlayTimeEventTracker.DailyPlayTime += num;
		int num2 = Mathf.FloorToInt((float)PlayTimeEventTracker.DailyPlayTime / 60f);
		//int num3 = Mathf.FloorToInt((float)num / 60f);
		for (int i = 0; i < PlayTimeEventTracker.playTimeSteps.Length; i++)
		{
			if (num2 <= PlayTimeEventTracker.playTimeSteps[i])
			{
				break;
			}
			if (PlayTimeEventTracker.DailyPlaySentStepIndex < i)
			{
				PlayTimeEventTracker.DailyPlaySentStepIndex = i;
				PlayTimeEventTracker.SendDailyPlayTime(PlayTimeEventTracker.playTimeSteps[i]);
			}
		}
	}

	private static void SendOpenStreak(int daysInARow)
	{
		try
		{
			AnalyticsManager.AppOpenStreak(daysInARow);
		}
		catch (Exception ex)
		{
			FMLogger.vCore("************************ERROR. AppResume SendOpenStreak ex. " + ex.Message);
		}
	}

	private static void SendDailyPlayTime(int time)
	{
		AnalyticsManager.DailyPlayTime(time);
	}

	private static void SendPicsSolvedCount(int count)
	{
		AnalyticsManager.PictureSolvedCount(count);
	}

	private static void SendAnimWatchCount(int count)
	{
		AnalyticsManager.AnimWatchedCount(count);
	}

	private const string playTimeKey = "pte_playTime";

	private const string openStreakKey = "pte_openStreak";

	private const string lastOpenKey = "pte_lastOpen";

	private const string openStreakSentKey = "pte_openStreakSent";

	private const string picsSolvedCounter = "pte_picsSolved";

	private const string animWatchedKey = "pte_animWatched";

	private const string playTimeSentStepKey = "pte_playTimeStep";

	private static DateTime launchTime;

	private static readonly int[] playTimeSteps = new int[]
	{
		10,
		20,
		40,
		60
	};
}

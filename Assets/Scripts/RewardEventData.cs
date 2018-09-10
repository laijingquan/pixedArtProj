// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class RewardEventData
{
	private RewardEventData(int date)
	{
		this.date = date;
	}

	public int ReqSession { get; private set; }

	public int ImpSession { get; private set; }

	public int LoadedSession { get; private set; }

	public int LoadTime { get; set; }

	private static string JsonStr
	{
		get
		{
			return PlayerPrefs.GetString("rewardEventData", string.Empty);
		}
		set
		{
			PlayerPrefs.SetString("rewardEventData", value);
		}
	}

	public static RewardEventData LoadEventData()
	{
		string jsonStr = RewardEventData.JsonStr;
		RewardEventData rewardEventData;
		if (!string.IsNullOrEmpty(jsonStr))
		{
			rewardEventData = JsonUtility.FromJson<RewardEventData>(jsonStr);
			if (rewardEventData != null && rewardEventData.date == RewardEventData.GetCurrentDate())
			{
				return rewardEventData;
			}
		}
		rewardEventData = new RewardEventData(RewardEventData.GetCurrentDate());
		rewardEventData.SaveEventData();
		return rewardEventData;
	}

	public void SaveEventData()
	{
		RewardEventData.JsonStr = JsonUtility.ToJson(this);
	}

	private static int GetCurrentDate()
	{
		return DateTime.UtcNow.DayOfYear;
	}

	public void SetPlace(RewardEventData.Place p)
	{
		this.place = p;
	}

	public void IterateImpression()
	{
		this.ImpSession++;
		this.ImpDay++;
		FMLogger.vAds(string.Concat(new object[]
		{
			"rewarded. imp d:",
			this.ImpDay,
			"s",
			this.ImpSession
		}));
	}

	public void IterateRequest()
	{
		this.ReqDay++;
		this.ReqSession++;
		FMLogger.vAds(string.Concat(new object[]
		{
			"rewarded. req d:",
			this.ReqDay,
			"s",
			this.ReqSession
		}));
	}

	public void IterateLoaded()
	{
		this.LoadedDay++;
		this.LoadedSession++;
		FMLogger.vAds(string.Concat(new object[]
		{
			"rewarded. loaded d:",
			this.LoadedDay,
			"s",
			this.LoadedSession
		}));
	}

	public int date;

	public int ReqDay;

	public int ImpDay;

	public int LoadedDay;

	public RewardEventData.Place place;

	public enum Place
	{
		Hint,
		Magic
	}
}

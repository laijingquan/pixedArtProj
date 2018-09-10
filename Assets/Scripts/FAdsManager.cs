// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;
using UnityEngine;

public class FAdsManager : MonoBehaviour
{
	public static FAdsManager Instance { get; private set; }

	 
	public static event Action<FAdsEventData> AdsEventReceived;

	 
	public static event Action RewardedCompleted;

	 
	public static event Action<FAdsInitData> Initialized;

	private void Awake()
	{
		if (FAdsManager.Instance == null)
		{
			FAdsManager.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void OnDestroy()
	{
		if (FAdsManager.Instance == this)
		{
			FAdsManager.Instance = null;
		}
	}

	public void AdsEvent(string argsJson)
	{
		FMLogger.vAds("FAdsManager AdsEvent received args: " + ((!string.IsNullOrEmpty(argsJson)) ? argsJson : "null"));
		FAdsEventData obj = this.ParseAdsEventData(argsJson);
		if (FAdsManager.AdsEventReceived != null)
		{
			FAdsManager.AdsEventReceived(obj);
		}
	}

	public void ShouldReward(string dummy)
	{
		FMLogger.vAds("FAdsManager ShouldReward received");
		if (FAdsManager.RewardedCompleted != null)
		{
			FAdsManager.RewardedCompleted();
		}
	}

	public void SdkInitialized(string argsJson)
	{
		FMLogger.vAds("FAdsManager SdkInitialized received");
		FAdsInitData obj = this.ParseInitEventData(argsJson);
		if (FAdsManager.Initialized != null)
		{
			FAdsManager.Initialized(obj);
		}
	}

	private FAdsEventData ParseAdsEventData(string json)
	{
		FAdsEventData fadsEventData = JsonUtility.FromJson<FAdsEventData>(json);
		if (fadsEventData == null || string.IsNullOrEmpty(fadsEventData.eventName))
		{
			return null;
		}
		return fadsEventData;
	}

	private FAdsInitData ParseInitEventData(string json)
	{
		return JsonUtility.FromJson<FAdsInitData>(json);
	}

	public void FakeInit()
	{
		FAdsInitData obj = new FAdsInitData
		{
			privacyPolicyUrl = string.Empty,
			vendorListUrl = string.Empty
		};
		if (FAdsManager.Initialized != null)
		{
			FAdsManager.Initialized(obj);
		}
	}
}

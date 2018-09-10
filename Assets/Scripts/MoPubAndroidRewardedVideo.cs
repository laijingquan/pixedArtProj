// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Linq;
using MoPubInternal.ThirdParty.MiniJSON;
using UnityEngine;

public class MoPubAndroidRewardedVideo
{
	public MoPubAndroidRewardedVideo(string adUnitId)
	{
		this._plugin = new AndroidJavaObject("com.mopub.unity.MoPubRewardedVideoUnityPlugin", new object[]
		{
			adUnitId
		});
	}

	public static void InitializeRewardedVideo()
	{
		MoPubAndroidRewardedVideo.PluginClass.CallStatic("initializeRewardedVideo", new object[0]);
	}

	public static void InitializeRewardedVideoWithSdkConfiguration(MoPubBase.SdkConfiguration sdkConfiguration)
	{
		MoPubAndroidRewardedVideo.PluginClass.CallStatic("initializeRewardedVideoWithSdkConfiguration", new object[]
		{
			sdkConfiguration.AdUnitId,
			sdkConfiguration.AdvancedBiddersString,
			sdkConfiguration.MediationSettingsJson,
			sdkConfiguration.NetworksToInitString
		});
	}

	public static void InitializeRewardedVideoWithNetworks(IEnumerable<string> networks)
	{
		MoPubAndroidRewardedVideo.PluginClass.CallStatic("initializeRewardedVideoWithNetworks", new object[]
		{
			string.Join(",", networks.ToArray<string>())
		});
	}

	public void RequestRewardedVideo(List<MoPubBase.MediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
		string text = (mediationSettings == null) ? null : Json.Serialize(mediationSettings);
		this._plugin.Call("requestRewardedVideo", new object[]
		{
			text,
			keywords,
			userDataKeywords,
			latitude,
			longitude,
			customerId
		});
	}

	public void ShowRewardedVideo(string customData)
	{
		this._plugin.Call("showRewardedVideo", new object[]
		{
			customData
		});
	}

	public bool HasRewardedVideo()
	{
		return this._plugin.Call<bool>("hasRewardedVideo", new object[0]);
	}

	public List<MoPubBase.Reward> GetAvailableRewards()
	{
		this._rewardsDict.Clear();
		using (AndroidJavaObject androidJavaObject = this._plugin.Call<AndroidJavaObject>("getAvailableRewards", new object[0]))
		{
			AndroidJavaObject[] array = AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(androidJavaObject.GetRawObject());
			if (array.Length <= 1)
			{
				return new List<MoPubBase.Reward>(this._rewardsDict.Keys);
			}
			foreach (AndroidJavaObject androidJavaObject2 in array)
			{
				this._rewardsDict.Add(new MoPubBase.Reward
				{
					Label = androidJavaObject2.Call<string>("getLabel", new object[0]),
					Amount = androidJavaObject2.Call<int>("getAmount", new object[0])
				}, androidJavaObject2);
			}
		}
		return new List<MoPubBase.Reward>(this._rewardsDict.Keys);
	}

	public void SelectReward(MoPubBase.Reward selectedReward)
	{
		AndroidJavaObject androidJavaObject;
		if (this._rewardsDict.TryGetValue(selectedReward, out androidJavaObject))
		{
			this._plugin.Call("selectReward", new object[]
			{
				androidJavaObject
			});
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Format("Selected reward {0} is not available.", selectedReward));
		}
	}

	private static readonly AndroidJavaClass PluginClass = new AndroidJavaClass("com.mopub.unity.MoPubRewardedVideoUnityPlugin");

	private readonly AndroidJavaObject _plugin;

	private readonly Dictionary<MoPubBase.Reward, AndroidJavaObject> _rewardsDict = new Dictionary<MoPubBase.Reward, AndroidJavaObject>();
}

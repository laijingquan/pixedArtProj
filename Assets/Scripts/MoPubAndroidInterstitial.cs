// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MoPubAndroidInterstitial
{
	public MoPubAndroidInterstitial(string adUnitId)
	{
		this._interstitialPlugin = new AndroidJavaObject("com.mopub.unity.MoPubInterstitialUnityPlugin", new object[]
		{
			adUnitId
		});
	}

	public void RequestInterstitialAd(string keywords = "", string userDataKeywords = "")
	{
		this._interstitialPlugin.Call("request", new object[]
		{
			keywords,
			userDataKeywords
		});
	}

	public void ShowInterstitialAd()
	{
		this._interstitialPlugin.Call("show", new object[0]);
	}

	public bool IsInterstitialReady
	{
		get
		{
			return this._interstitialPlugin.Call<bool>("isReady", new object[0]);
		}
	}

	public void DestroyInterstitialAd()
	{
		this._interstitialPlugin.Call("destroy", new object[0]);
	}

	private readonly AndroidJavaObject _interstitialPlugin;
}

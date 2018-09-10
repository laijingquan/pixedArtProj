// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MoPubAndroidBanner
{
	public MoPubAndroidBanner(string adUnitId)
	{
		this._bannerPlugin = new AndroidJavaObject("com.mopub.unity.MoPubBannerUnityPlugin", new object[]
		{
			adUnitId
		});
	}

	public void CreateBanner(MoPubBase.AdPosition position)
	{
		this._bannerPlugin.Call("createBanner", new object[]
		{
			(int)position
		});
	}

	public void ShowBanner(bool shouldShow)
	{
		this._bannerPlugin.Call("hideBanner", new object[]
		{
			!shouldShow
		});
	}

	public void RefreshBanner(string keywords, string userDataKeywords = "")
	{
		this._bannerPlugin.Call("refreshBanner", new object[]
		{
			keywords,
			userDataKeywords
		});
	}

	public void DestroyBanner()
	{
		this._bannerPlugin.Call("destroyBanner", new object[0]);
	}

	public void SetAutorefresh(bool enabled)
	{
		this._bannerPlugin.Call("setAutorefreshEnabled", new object[]
		{
			enabled
		});
	}

	public void ForceRefresh()
	{
		this._bannerPlugin.Call("forceRefresh", new object[0]);
	}

	private readonly AndroidJavaObject _bannerPlugin;
}

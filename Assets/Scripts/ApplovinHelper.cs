// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public static class ApplovinHelper
{
	public static void InitializeSdk()
	{
		try
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.applovin.sdk.AppLovinSdk");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
			androidJavaClass.CallStatic("initializeSdk", new object[]
			{
				@static
			});
			FMLogger.vAds("applovin inited");
		}
		catch (Exception)
		{
			FMLogger.vAds("failed to init applovin");
		}
	}
}

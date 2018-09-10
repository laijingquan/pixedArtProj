// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScreenUtils
{
	public static bool CanFitTabletBanner()
	{
		return ScreenUtils.CanFitTabletBannerAndroid();
	}

	public static int GetScreenDpWidth()
	{
		int result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("fmg.nutils.ScreenUtils"))
		{
			using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
				int num = androidJavaClass.CallStatic<int>("screenWidthDp", new object[]
				{
					@static
				});
				result = num;
			}
		}
		return result;
	}

	public static int GetScreenDpHeight()
	{
		int result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("fmg.nutils.ScreenUtils"))
		{
			using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
				int num = androidJavaClass.CallStatic<int>("screenHeightDp", new object[]
				{
					@static
				});
				result = num;
			}
		}
		return result;
	}

	private static bool CanFitTabletBannerAndroid()
	{
		bool result;
		try
		{
			int screenDpWidth = ScreenUtils.GetScreenDpWidth();
			if (screenDpWidth >= 728)
			{
				result = true;
			}
			else
			{
				result = false;
			}
		}
		catch (Exception)
		{
			FMLogger.vAds("banner height ex. fallback calc");
			result = ScreenUtils.IsTabletDeviceFallback();
		}
		return result;
	}

	private static bool CanFitTabletBannerIOS()
	{
		bool result;
		try
		{
			result = ScreenUtils.isTablet();
		}
		catch (Exception)
		{
			FMLogger.vAds("banner height ex. fallback calc");
			result = ScreenUtils.IsTabletDeviceFallback();
		}
		return result;
	}

	private static bool IsTabletDeviceFallback()
	{
		float num = Mathf.Max(160f, Screen.dpi);
		float f = (float)Screen.width / num;
		float f2 = (float)Screen.height / num;
		float num2 = Mathf.Sqrt(Mathf.Pow(f, 2f) + Mathf.Pow(f2, 2f));
		return num2 > 7.8f;
	}

	[DllImport("__Internal")]
	private static extern bool isTablet();

	[DllImport("__Internal")]
	private static extern float screenPointsHeight();

	[DllImport("__Internal")]
	private static extern float screenPointsWidth();
}

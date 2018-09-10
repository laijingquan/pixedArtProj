// dnSpy decompiler from Assembly-CSharp.dll
using System;
using com.adjust.sdk.test;
using UnityEngine;

namespace com.adjust.sdk
{
	public class Adjust : MonoBehaviour
	{
		private void Awake()
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
			if (!this.startManually)
			{
				AdjustConfig adjustConfig = new AdjustConfig(this.appToken, this.environment, this.logLevel == AdjustLogLevel.Suppress);
				adjustConfig.setLogLevel(this.logLevel);
				adjustConfig.setSendInBackground(this.sendInBackground);
				adjustConfig.setEventBufferingEnabled(this.eventBuffering);
				adjustConfig.setLaunchDeferredDeeplink(this.launchDeferredDeeplink);
				Adjust.start(adjustConfig);
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			if (pauseStatus)
			{
				AdjustAndroid.OnPause();
			}
			else
			{
				AdjustAndroid.OnResume();
			}
		}

		public static void start(AdjustConfig adjustConfig)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			if (adjustConfig == null)
			{
				UnityEngine.Debug.Log("Adjust: Missing config to start.");
				return;
			}
			AdjustAndroid.Start(adjustConfig);
		}

		public static void trackEvent(AdjustEvent adjustEvent)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			if (adjustEvent == null)
			{
				UnityEngine.Debug.Log("Adjust: Missing event to track.");
				return;
			}
			AdjustAndroid.TrackEvent(adjustEvent);
		}

		public static void setEnabled(bool enabled)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.SetEnabled(enabled);
		}

		public static bool isEnabled()
		{
			return !Adjust.IsEditor() && AdjustAndroid.IsEnabled();
		}

		public static void setOfflineMode(bool enabled)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.SetOfflineMode(enabled);
		}

		public static void setDeviceToken(string deviceToken)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.SetDeviceToken(deviceToken);
		}

		public static void gdprForgetMe()
		{
			AdjustAndroid.GdprForgetMe();
		}

		public static void appWillOpenUrl(string url)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.AppWillOpenUrl(url);
		}

		public static void sendFirstPackages()
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.SendFirstPackages();
		}

		public static void addSessionPartnerParameter(string key, string value)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.AddSessionPartnerParameter(key, value);
		}

		public static void addSessionCallbackParameter(string key, string value)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.AddSessionCallbackParameter(key, value);
		}

		public static void removeSessionPartnerParameter(string key)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.RemoveSessionPartnerParameter(key);
		}

		public static void removeSessionCallbackParameter(string key)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.RemoveSessionCallbackParameter(key);
		}

		public static void resetSessionPartnerParameters()
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.ResetSessionPartnerParameters();
		}

		public static void resetSessionCallbackParameters()
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.ResetSessionCallbackParameters();
		}

		public static string getAdid()
		{
			if (Adjust.IsEditor())
			{
				return string.Empty;
			}
			return AdjustAndroid.GetAdid();
		}

		public static AdjustAttribution getAttribution()
		{
			if (Adjust.IsEditor())
			{
				return null;
			}
			return AdjustAndroid.GetAttribution();
		}

		public static string getWinAdid()
		{
			if (Adjust.IsEditor())
			{
				return string.Empty;
			}
			UnityEngine.Debug.Log("Adjust: Error! Windows Advertising ID is not available on Android platform.");
			return string.Empty;
		}

		public static string getIdfa()
		{
			if (Adjust.IsEditor())
			{
				return string.Empty;
			}
			UnityEngine.Debug.Log("Adjust: Error! IDFA is not available on Android platform.");
			return string.Empty;
		}

		[Obsolete("This method is intended for testing purposes only. Do not use it.")]
		public static void setReferrer(string referrer)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.SetReferrer(referrer);
		}

		public static void getGoogleAdId(Action<string> onDeviceIdsRead)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.GetGoogleAdId(onDeviceIdsRead);
		}

		public static string getAmazonAdId()
		{
			if (Adjust.IsEditor())
			{
				return string.Empty;
			}
			return AdjustAndroid.GetAmazonAdId();
		}

		private static bool IsEditor()
		{
			return false;
		}

		public static void SetTestOptions(AdjustTestOptions testOptions)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			AdjustAndroid.SetTestOptions(testOptions);
		}

		private const string errorMsgEditor = "Adjust: SDK can not be used in Editor.";

		private const string errorMsgStart = "Adjust: SDK not started. Start it manually using the 'start' method.";

		private const string errorMsgPlatform = "Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.";

		public bool startManually = true;

		public bool eventBuffering;

		public bool sendInBackground;

		public bool launchDeferredDeeplink = true;

		public string appToken = "{Your App Token}";

		public AdjustLogLevel logLevel = AdjustLogLevel.Info;

		public AdjustEnvironment environment;
	}
}

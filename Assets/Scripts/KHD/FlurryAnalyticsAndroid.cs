// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KHD
{
	public static class FlurryAnalyticsAndroid
	{
		private static AndroidJavaClass Flurry
		{
			get
			{
				if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
				{
					return null;
				}
				if (FlurryAnalyticsAndroid._flurry == null)
				{
					FlurryAnalyticsAndroid._flurry = new AndroidJavaClass("com.flurry.android.FlurryAgent");
				}
				return FlurryAnalyticsAndroid._flurry;
			}
		}

		public static void Dispose()
		{
			if (FlurryAnalyticsAndroid._flurry != null)
			{
				FlurryAnalyticsAndroid._flurry.Dispose();
				FlurryAnalyticsAndroid._flurry = null;
			}
		}

		public static void Init(string apiKey, bool captureUncaughtExceptions = false)
		{
			FlurryAnalyticsAndroid.DebugLog(string.Concat(new object[]
			{
				"Init with key: ",
				apiKey,
				" capture exceptions: ",
				captureUncaughtExceptions
			}));
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			if (apiKey != null)
			{
				apiKey = apiKey.ToUpper();
			}
			if (string.IsNullOrEmpty(apiKey))
			{
				FlurryAnalyticsAndroid.DebugLogError("Android API key is null or empty, please provide valid API key");
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					FlurryAnalyticsAndroid.Flurry.CallStatic("setCaptureUncaughtExceptions", new object[]
					{
						captureUncaughtExceptions
					});
					FlurryAnalyticsAndroid.Flurry.CallStatic("init", new object[]
					{
						@static,
						apiKey.ToUpper()
					});
				}
			}
		}

		public static void OnStartSession()
		{
			FlurryAnalyticsAndroid.DebugLog("Session started");
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					FlurryAnalyticsAndroid.Flurry.CallStatic("onStartSession", new object[]
					{
						@static
					});
				}
			}
		}

		public static void OnEndSession()
		{
			FlurryAnalyticsAndroid.DebugLog("Session ended");
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					FlurryAnalyticsAndroid.Flurry.CallStatic("onStartSession", new object[]
					{
						@static
					});
				}
			}
		}

		public static void SetLogEnabled(bool enabled)
		{
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic("setLogEnabled", new object[]
			{
				enabled
			});
		}

		public static void SetVersionName(string versionName)
		{
			FlurryAnalyticsAndroid.DebugLog("Application version changed to: " + versionName);
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic("setVersionName", new object[]
			{
				versionName
			});
		}

		public static void SetReportLocation(bool report)
		{
			FlurryAnalyticsAndroid.DebugLog("Set report location: " + report);
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic("setReportLocation", new object[]
			{
				report
			});
		}

		public static void SetContinueSessionMillis(long milliseconds)
		{
			FlurryAnalyticsAndroid.DebugLog("Set continue session seconds to: " + milliseconds / 1000L);
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic("setContinueSessionMillis", new object[]
			{
				milliseconds
			});
		}

		public static void SetCaptureUncaughtExceptions(bool enabled)
		{
			FlurryAnalyticsAndroid.DebugLog("Set capture unchaught exceptions: " + enabled);
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic("setCaptureUncaughtExceptions", new object[]
			{
				enabled
			});
		}

		public static void SetPulseEnabled(bool enabled)
		{
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic("setPulseEnabled", new object[]
			{
				enabled
			});
		}

		public static void SetUserId(string userId)
		{
			FlurryAnalyticsAndroid.DebugLog("Set unique user id: " + userId);
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic("setUserId", new object[]
			{
				userId
			});
		}

		public static void SetGender(FlurryAnalytics.Gender gender)
		{
			FlurryAnalyticsAndroid.DebugLog("Set user gender: " + gender);
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic("setGender", new object[]
			{
				(gender != FlurryAnalytics.Gender.Male) ? 0 : 1
			});
		}

		public static void SetAge(int age)
		{
			FlurryAnalyticsAndroid.DebugLog("Set user age: " + age);
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic("setAge", new object[]
			{
				age
			});
		}

		public static void SetLocation(float latitude, float longitude)
		{
			FlurryAnalyticsAndroid.DebugLog(string.Concat(new object[]
			{
				"Set location: ",
				latitude,
				", ",
				longitude
			}));
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic("setLocation", new object[]
			{
				latitude,
				longitude
			});
		}

		public static void LogEvent(string eventName, bool isTimed = false)
		{
			FlurryAnalyticsAndroid.DebugLog(string.Concat(new object[]
			{
				"Log event: ",
				eventName,
				" isTimed: ",
				isTimed
			}));
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			FlurryAnalyticsAndroid.Flurry.CallStatic<AndroidJavaObject>("logEvent", new object[]
			{
				eventName,
				isTimed
			});
		}

		public static void LogEventWithParameters(string eventName, Dictionary<string, string> parameters, bool isTimed = false)
		{
			FlurryAnalyticsAndroid.DebugLog(string.Concat(new object[]
			{
				"Log event: ",
				eventName,
				" isTimed: ",
				isTimed,
				" with parameters"
			}));
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			using (AndroidJavaObject androidJavaObject = FlurryAnalyticsAndroid.ConvertDictionaryToJavaHashMap(parameters))
			{
				FlurryAnalyticsAndroid.Flurry.CallStatic<AndroidJavaObject>("logEvent", new object[]
				{
					eventName,
					androidJavaObject,
					isTimed
				});
			}
		}

		public static void EndTimedEvent(string eventName, Dictionary<string, string> parameters = null)
		{
			FlurryAnalyticsAndroid.DebugLog("End timed event: " + eventName);
			if (!FlurryAnalyticsAndroid.IsAndroidPlayer())
			{
				return;
			}
			if (parameters == null)
			{
				FlurryAnalyticsAndroid.Flurry.CallStatic("endTimedEvent", new object[]
				{
					eventName
				});
			}
			else
			{
				using (AndroidJavaObject androidJavaObject = FlurryAnalyticsAndroid.ConvertDictionaryToJavaHashMap(parameters))
				{
					FlurryAnalyticsAndroid.Flurry.CallStatic("endTimedEvent", new object[]
					{
						eventName,
						androidJavaObject
					});
				}
			}
		}

		public static void LogPayment(string productName, string productId, int quantity, double price, string currency, string transactionId, Dictionary<string, string> parameters)
		{
			FlurryAnalyticsAndroid.DebugLog("Log payment: " + productName);
			if (parameters == null)
			{
				parameters = new Dictionary<string, string>();
			}
			using (AndroidJavaObject androidJavaObject = FlurryAnalyticsAndroid.ConvertDictionaryToJavaHashMap(parameters))
			{
				FlurryAnalyticsAndroid.Flurry.CallStatic<AndroidJavaObject>("logPayment", new object[]
				{
					productName,
					productId,
					quantity,
					price,
					currency,
					transactionId,
					androidJavaObject
				});
			}
		}

		private static AndroidJavaObject ConvertDictionaryToJavaHashMap(Dictionary<string, string> parameters)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]);
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			foreach (KeyValuePair<string, string> keyValuePair in parameters)
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
				{
					keyValuePair.Key
				}))
				{
					using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", new object[]
					{
						keyValuePair.Value
					}))
					{
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(new object[]
						{
							androidJavaObject2,
							androidJavaObject3
						}));
					}
				}
			}
			return androidJavaObject;
		}

		private static bool EditorDebugLogEnabled()
		{
			return false;
		}

		private static bool IsAndroidPlayer()
		{
			return Application.platform == RuntimePlatform.Android;
		}

		private static void DebugLog(string log)
		{
			if (!FlurryAnalyticsAndroid.EditorDebugLogEnabled())
			{
				return;
			}
			UnityEngine.Debug.Log("[FlurryAnalyticsPlugin]: " + log);
		}

		private static void DebugLogError(string error)
		{
			UnityEngine.Debug.Log("[FlurryAnalyticsPlugin]: " + error);
		}

		private const string FLURRY_ANGENT_CLASS_NAME = "com.flurry.android.FlurryAgent";

		private const string UNITY_PLAYER_CLASS_NAME = "com.unity3d.player.UnityPlayer";

		private const string UNITY_PLAYER_ACTIVITY_NAME = "currentActivity";

		private static AndroidJavaClass _flurry;
	}
}

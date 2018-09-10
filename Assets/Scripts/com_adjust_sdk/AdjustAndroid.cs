// dnSpy decompiler from Assembly-CSharp.dll
using System;
using com.adjust.sdk.test;
using UnityEngine;

namespace com.adjust.sdk
{
	public class AdjustAndroid
	{
		public static void Start(AdjustConfig adjustConfig)
		{
			AndroidJavaObject androidJavaObject = (adjustConfig.environment != AdjustEnvironment.Sandbox) ? new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_PRODUCTION") : new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_SANDBOX");
			bool? allowSuppressLogLevel = adjustConfig.allowSuppressLogLevel;
			AndroidJavaObject androidJavaObject2;
			if (allowSuppressLogLevel != null)
			{
				androidJavaObject2 = new AndroidJavaObject("com.adjust.sdk.AdjustConfig", new object[]
				{
					AdjustAndroid.ajoCurrentActivity,
					adjustConfig.appToken,
					androidJavaObject,
					adjustConfig.allowSuppressLogLevel
				});
			}
			else
			{
				androidJavaObject2 = new AndroidJavaObject("com.adjust.sdk.AdjustConfig", new object[]
				{
					AdjustAndroid.ajoCurrentActivity,
					adjustConfig.appToken,
					androidJavaObject
				});
			}
			AdjustAndroid.launchDeferredDeeplink = adjustConfig.launchDeferredDeeplink;
			AdjustLogLevel? logLevel = adjustConfig.logLevel;
			if (logLevel != null)
			{
				AndroidJavaObject @static;
				if (adjustConfig.logLevel.Value.ToUppercaseString().Equals("SUPPRESS"))
				{
					@static = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>("SUPRESS");
				}
				else
				{
					@static = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>(adjustConfig.logLevel.Value.ToUppercaseString());
				}
				if (@static != null)
				{
					androidJavaObject2.Call("setLogLevel", new object[]
					{
						@static
					});
				}
			}
			androidJavaObject2.Call("setSdkPrefix", new object[]
			{
				"unity4.14.1"
			});
			double? delayStart = adjustConfig.delayStart;
			if (delayStart != null)
			{
				androidJavaObject2.Call("setDelayStart", new object[]
				{
					adjustConfig.delayStart
				});
			}
			bool? eventBufferingEnabled = adjustConfig.eventBufferingEnabled;
			if (eventBufferingEnabled != null)
			{
				AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.Boolean", new object[]
				{
					adjustConfig.eventBufferingEnabled.Value
				});
				androidJavaObject2.Call("setEventBufferingEnabled", new object[]
				{
					androidJavaObject3
				});
			}
			bool? sendInBackground = adjustConfig.sendInBackground;
			if (sendInBackground != null)
			{
				androidJavaObject2.Call("setSendInBackground", new object[]
				{
					adjustConfig.sendInBackground.Value
				});
			}
			if (adjustConfig.userAgent != null)
			{
				androidJavaObject2.Call("setUserAgent", new object[]
				{
					adjustConfig.userAgent
				});
			}
			if (!string.IsNullOrEmpty(adjustConfig.processName))
			{
				androidJavaObject2.Call("setProcessName", new object[]
				{
					adjustConfig.processName
				});
			}
			if (adjustConfig.defaultTracker != null)
			{
				androidJavaObject2.Call("setDefaultTracker", new object[]
				{
					adjustConfig.defaultTracker
				});
			}
			if (AdjustAndroid.IsAppSecretSet(adjustConfig))
			{
				androidJavaObject2.Call("setAppSecret", new object[]
				{
					adjustConfig.secretId.Value,
					adjustConfig.info1.Value,
					adjustConfig.info2.Value,
					adjustConfig.info3.Value,
					adjustConfig.info4.Value
				});
			}
			if (adjustConfig.isDeviceKnown != null)
			{
				androidJavaObject2.Call("setDeviceKnown", new object[]
				{
					adjustConfig.isDeviceKnown.Value
				});
			}
			if (adjustConfig.readImei != null)
			{
				androidJavaObject2.Call("setReadMobileEquipmentIdentity", new object[]
				{
					adjustConfig.readImei.Value
				});
			}
			if (adjustConfig.attributionChangedDelegate != null)
			{
				AdjustAndroid.onAttributionChangedListener = new AdjustAndroid.AttributionChangeListener(adjustConfig.attributionChangedDelegate);
				androidJavaObject2.Call("setOnAttributionChangedListener", new object[]
				{
					AdjustAndroid.onAttributionChangedListener
				});
			}
			if (adjustConfig.eventSuccessDelegate != null)
			{
				AdjustAndroid.onEventTrackingSucceededListener = new AdjustAndroid.EventTrackingSucceededListener(adjustConfig.eventSuccessDelegate);
				androidJavaObject2.Call("setOnEventTrackingSucceededListener", new object[]
				{
					AdjustAndroid.onEventTrackingSucceededListener
				});
			}
			if (adjustConfig.eventFailureDelegate != null)
			{
				AdjustAndroid.onEventTrackingFailedListener = new AdjustAndroid.EventTrackingFailedListener(adjustConfig.eventFailureDelegate);
				androidJavaObject2.Call("setOnEventTrackingFailedListener", new object[]
				{
					AdjustAndroid.onEventTrackingFailedListener
				});
			}
			if (adjustConfig.sessionSuccessDelegate != null)
			{
				AdjustAndroid.onSessionTrackingSucceededListener = new AdjustAndroid.SessionTrackingSucceededListener(adjustConfig.sessionSuccessDelegate);
				androidJavaObject2.Call("setOnSessionTrackingSucceededListener", new object[]
				{
					AdjustAndroid.onSessionTrackingSucceededListener
				});
			}
			if (adjustConfig.sessionFailureDelegate != null)
			{
				AdjustAndroid.onSessionTrackingFailedListener = new AdjustAndroid.SessionTrackingFailedListener(adjustConfig.sessionFailureDelegate);
				androidJavaObject2.Call("setOnSessionTrackingFailedListener", new object[]
				{
					AdjustAndroid.onSessionTrackingFailedListener
				});
			}
			if (adjustConfig.deferredDeeplinkDelegate != null)
			{
				AdjustAndroid.onDeferredDeeplinkListener = new AdjustAndroid.DeferredDeeplinkListener(adjustConfig.deferredDeeplinkDelegate);
				androidJavaObject2.Call("setOnDeeplinkResponseListener", new object[]
				{
					AdjustAndroid.onDeferredDeeplinkListener
				});
			}
			AdjustAndroid.ajcAdjust.CallStatic("onCreate", new object[]
			{
				androidJavaObject2
			});
			AdjustAndroid.ajcAdjust.CallStatic("onResume", new object[0]);
		}

		public static void TrackEvent(AdjustEvent adjustEvent)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.adjust.sdk.AdjustEvent", new object[]
			{
				adjustEvent.eventToken
			});
			double? revenue = adjustEvent.revenue;
			if (revenue != null)
			{
				AndroidJavaObject androidJavaObject2 = androidJavaObject;
				string methodName = "setRevenue";
				object[] array = new object[2];
				int num = 0;
				double? revenue2 = adjustEvent.revenue;
				array[num] = revenue2.Value;
				array[1] = adjustEvent.currency;
				androidJavaObject2.Call(methodName, array);
			}
			if (adjustEvent.callbackList != null)
			{
				for (int i = 0; i < adjustEvent.callbackList.Count; i += 2)
				{
					string text = adjustEvent.callbackList[i];
					string text2 = adjustEvent.callbackList[i + 1];
					androidJavaObject.Call("addCallbackParameter", new object[]
					{
						text,
						text2
					});
				}
			}
			if (adjustEvent.partnerList != null)
			{
				for (int j = 0; j < adjustEvent.partnerList.Count; j += 2)
				{
					string text3 = adjustEvent.partnerList[j];
					string text4 = adjustEvent.partnerList[j + 1];
					androidJavaObject.Call("addPartnerParameter", new object[]
					{
						text3,
						text4
					});
				}
			}
			if (adjustEvent.transactionId != null)
			{
				androidJavaObject.Call("setOrderId", new object[]
				{
					adjustEvent.transactionId
				});
			}
			AdjustAndroid.ajcAdjust.CallStatic("trackEvent", new object[]
			{
				androidJavaObject
			});
		}

		public static bool IsEnabled()
		{
			return AdjustAndroid.ajcAdjust.CallStatic<bool>("isEnabled", new object[0]);
		}

		public static void SetEnabled(bool enabled)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setEnabled", new object[]
			{
				enabled
			});
		}

		public static void SetOfflineMode(bool enabled)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setOfflineMode", new object[]
			{
				enabled
			});
		}

		public static void SendFirstPackages()
		{
			AdjustAndroid.ajcAdjust.CallStatic("sendFirstPackages", new object[0]);
		}

		public static void SetDeviceToken(string deviceToken)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setPushToken", new object[]
			{
				deviceToken,
				AdjustAndroid.ajoCurrentActivity
			});
		}

		public static string GetAdid()
		{
			return AdjustAndroid.ajcAdjust.CallStatic<string>("getAdid", new object[0]);
		}

		public static void GdprForgetMe()
		{
			AdjustAndroid.ajcAdjust.CallStatic("gdprForgetMe", new object[]
			{
				AdjustAndroid.ajoCurrentActivity
			});
		}

		public static AdjustAttribution GetAttribution()
		{
			try
			{
				AndroidJavaObject androidJavaObject = AdjustAndroid.ajcAdjust.CallStatic<AndroidJavaObject>("getAttribution", new object[0]);
				if (androidJavaObject == null)
				{
					return null;
				}
				return new AdjustAttribution
				{
					trackerName = androidJavaObject.Get<string>(AdjustUtils.KeyTrackerName),
					trackerToken = androidJavaObject.Get<string>(AdjustUtils.KeyTrackerToken),
					network = androidJavaObject.Get<string>(AdjustUtils.KeyNetwork),
					campaign = androidJavaObject.Get<string>(AdjustUtils.KeyCampaign),
					adgroup = androidJavaObject.Get<string>(AdjustUtils.KeyAdgroup),
					creative = androidJavaObject.Get<string>(AdjustUtils.KeyCreative),
					clickLabel = androidJavaObject.Get<string>(AdjustUtils.KeyClickLabel),
					adid = androidJavaObject.Get<string>(AdjustUtils.KeyAdid)
				};
			}
			catch (Exception)
			{
			}
			return null;
		}

		public static void AddSessionPartnerParameter(string key, string value)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("addSessionPartnerParameter", new object[]
			{
				key,
				value
			});
		}

		public static void AddSessionCallbackParameter(string key, string value)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("addSessionCallbackParameter", new object[]
			{
				key,
				value
			});
		}

		public static void RemoveSessionPartnerParameter(string key)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("removeSessionPartnerParameter", new object[]
			{
				key
			});
		}

		public static void RemoveSessionCallbackParameter(string key)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("removeSessionCallbackParameter", new object[]
			{
				key
			});
		}

		public static void ResetSessionPartnerParameters()
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("resetSessionPartnerParameters", new object[0]);
		}

		public static void ResetSessionCallbackParameters()
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("resetSessionCallbackParameters", new object[0]);
		}

		public static void AppWillOpenUrl(string url)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("parse", new object[]
			{
				url
			});
			AdjustAndroid.ajcAdjust.CallStatic("appWillOpenUrl", new object[]
			{
				androidJavaObject,
				AdjustAndroid.ajoCurrentActivity
			});
		}

		public static void OnPause()
		{
			AdjustAndroid.ajcAdjust.CallStatic("onPause", new object[0]);
		}

		public static void OnResume()
		{
			AdjustAndroid.ajcAdjust.CallStatic("onResume", new object[0]);
		}

		public static void SetReferrer(string referrer)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setReferrer", new object[]
			{
				referrer,
				AdjustAndroid.ajoCurrentActivity
			});
		}

		public static void GetGoogleAdId(Action<string> onDeviceIdsRead)
		{
			AdjustAndroid.DeviceIdsReadListener deviceIdsReadListener = new AdjustAndroid.DeviceIdsReadListener(onDeviceIdsRead);
			AdjustAndroid.ajcAdjust.CallStatic("getGoogleAdId", new object[]
			{
				AdjustAndroid.ajoCurrentActivity,
				deviceIdsReadListener
			});
		}

		public static string GetAmazonAdId()
		{
			return AdjustAndroid.ajcAdjust.CallStatic<string>("getAmazonAdId", new object[]
			{
				AdjustAndroid.ajoCurrentActivity
			});
		}

		public static void SetTestOptions(AdjustTestOptions testOptions)
		{
			AndroidJavaObject androidJavaObject = testOptions.ToAndroidJavaObject(AdjustAndroid.ajoCurrentActivity);
			AdjustAndroid.ajcAdjust.CallStatic("setTestOptions", new object[]
			{
				androidJavaObject
			});
		}

		private static bool IsAppSecretSet(AdjustConfig adjustConfig)
		{
			return adjustConfig.secretId != null && adjustConfig.info1 != null && adjustConfig.info2 != null && adjustConfig.info3 != null && adjustConfig.info4 != null;
		}

		private const string sdkPrefix = "unity4.14.1";

		private static bool launchDeferredDeeplink = true;

		private static AndroidJavaClass ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");

		private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

		private static AdjustAndroid.DeferredDeeplinkListener onDeferredDeeplinkListener;

		private static AdjustAndroid.AttributionChangeListener onAttributionChangedListener;

		private static AdjustAndroid.EventTrackingFailedListener onEventTrackingFailedListener;

		private static AdjustAndroid.EventTrackingSucceededListener onEventTrackingSucceededListener;

		private static AdjustAndroid.SessionTrackingFailedListener onSessionTrackingFailedListener;

		private static AdjustAndroid.SessionTrackingSucceededListener onSessionTrackingSucceededListener;

		private class AttributionChangeListener : AndroidJavaProxy
		{
			public AttributionChangeListener(Action<AdjustAttribution> pCallback) : base("com.adjust.sdk.OnAttributionChangedListener")
			{
				this.callback = pCallback;
			}

			public void onAttributionChanged(AndroidJavaObject attribution)
			{
				if (this.callback == null)
				{
					return;
				}
				AdjustAttribution adjustAttribution = new AdjustAttribution();
				adjustAttribution.trackerName = attribution.Get<string>(AdjustUtils.KeyTrackerName);
				adjustAttribution.trackerToken = attribution.Get<string>(AdjustUtils.KeyTrackerToken);
				adjustAttribution.network = attribution.Get<string>(AdjustUtils.KeyNetwork);
				adjustAttribution.campaign = attribution.Get<string>(AdjustUtils.KeyCampaign);
				adjustAttribution.adgroup = attribution.Get<string>(AdjustUtils.KeyAdgroup);
				adjustAttribution.creative = attribution.Get<string>(AdjustUtils.KeyCreative);
				adjustAttribution.clickLabel = attribution.Get<string>(AdjustUtils.KeyClickLabel);
				adjustAttribution.adid = attribution.Get<string>(AdjustUtils.KeyAdid);
				this.callback(adjustAttribution);
			}

			private Action<AdjustAttribution> callback;
		}

		private class DeferredDeeplinkListener : AndroidJavaProxy
		{
			public DeferredDeeplinkListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeeplinkResponseListener")
			{
				this.callback = pCallback;
			}

			public bool launchReceivedDeeplink(AndroidJavaObject deeplink)
			{
				if (this.callback == null)
				{
					return AdjustAndroid.launchDeferredDeeplink;
				}
				string obj = deeplink.Call<string>("toString", new object[0]);
				this.callback(obj);
				return AdjustAndroid.launchDeferredDeeplink;
			}

			private Action<string> callback;
		}

		private class EventTrackingSucceededListener : AndroidJavaProxy
		{
			public EventTrackingSucceededListener(Action<AdjustEventSuccess> pCallback) : base("com.adjust.sdk.OnEventTrackingSucceededListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedEventTrackingSucceeded(AndroidJavaObject eventSuccessData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (eventSuccessData == null)
				{
					return;
				}
				AdjustEventSuccess adjustEventSuccess = new AdjustEventSuccess();
				adjustEventSuccess.Adid = eventSuccessData.Get<string>(AdjustUtils.KeyAdid);
				adjustEventSuccess.Message = eventSuccessData.Get<string>(AdjustUtils.KeyMessage);
				adjustEventSuccess.Timestamp = eventSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
				adjustEventSuccess.EventToken = eventSuccessData.Get<string>(AdjustUtils.KeyEventToken);
				try
				{
					AndroidJavaObject androidJavaObject = eventSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					adjustEventSuccess.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustEventSuccess);
			}

			private Action<AdjustEventSuccess> callback;
		}

		private class EventTrackingFailedListener : AndroidJavaProxy
		{
			public EventTrackingFailedListener(Action<AdjustEventFailure> pCallback) : base("com.adjust.sdk.OnEventTrackingFailedListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedEventTrackingFailed(AndroidJavaObject eventFailureData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (eventFailureData == null)
				{
					return;
				}
				AdjustEventFailure adjustEventFailure = new AdjustEventFailure();
				adjustEventFailure.Adid = eventFailureData.Get<string>(AdjustUtils.KeyAdid);
				adjustEventFailure.Message = eventFailureData.Get<string>(AdjustUtils.KeyMessage);
				adjustEventFailure.WillRetry = eventFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
				adjustEventFailure.Timestamp = eventFailureData.Get<string>(AdjustUtils.KeyTimestamp);
				adjustEventFailure.EventToken = eventFailureData.Get<string>(AdjustUtils.KeyEventToken);
				try
				{
					AndroidJavaObject androidJavaObject = eventFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					adjustEventFailure.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustEventFailure);
			}

			private Action<AdjustEventFailure> callback;
		}

		private class SessionTrackingSucceededListener : AndroidJavaProxy
		{
			public SessionTrackingSucceededListener(Action<AdjustSessionSuccess> pCallback) : base("com.adjust.sdk.OnSessionTrackingSucceededListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedSessionTrackingSucceeded(AndroidJavaObject sessionSuccessData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (sessionSuccessData == null)
				{
					return;
				}
				AdjustSessionSuccess adjustSessionSuccess = new AdjustSessionSuccess();
				adjustSessionSuccess.Adid = sessionSuccessData.Get<string>(AdjustUtils.KeyAdid);
				adjustSessionSuccess.Message = sessionSuccessData.Get<string>(AdjustUtils.KeyMessage);
				adjustSessionSuccess.Timestamp = sessionSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
				try
				{
					AndroidJavaObject androidJavaObject = sessionSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					adjustSessionSuccess.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustSessionSuccess);
			}

			private Action<AdjustSessionSuccess> callback;
		}

		private class SessionTrackingFailedListener : AndroidJavaProxy
		{
			public SessionTrackingFailedListener(Action<AdjustSessionFailure> pCallback) : base("com.adjust.sdk.OnSessionTrackingFailedListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedSessionTrackingFailed(AndroidJavaObject sessionFailureData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (sessionFailureData == null)
				{
					return;
				}
				AdjustSessionFailure adjustSessionFailure = new AdjustSessionFailure();
				adjustSessionFailure.Adid = sessionFailureData.Get<string>(AdjustUtils.KeyAdid);
				adjustSessionFailure.Message = sessionFailureData.Get<string>(AdjustUtils.KeyMessage);
				adjustSessionFailure.WillRetry = sessionFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
				adjustSessionFailure.Timestamp = sessionFailureData.Get<string>(AdjustUtils.KeyTimestamp);
				try
				{
					AndroidJavaObject androidJavaObject = sessionFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					adjustSessionFailure.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustSessionFailure);
			}

			private Action<AdjustSessionFailure> callback;
		}

		private class DeviceIdsReadListener : AndroidJavaProxy
		{
			public DeviceIdsReadListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeviceIdsRead")
			{
				this.onPlayAdIdReadCallback = pCallback;
			}

			public void onGoogleAdIdRead(string playAdId)
			{
				if (this.onPlayAdIdReadCallback == null)
				{
					return;
				}
				this.onPlayAdIdReadCallback(playAdId);
			}

			public void onGoogleAdIdRead(AndroidJavaObject ajoAdId)
			{
				if (ajoAdId == null)
				{
					string playAdId = null;
					this.onGoogleAdIdRead(playAdId);
					return;
				}
				this.onGoogleAdIdRead(ajoAdId.Call<string>("toString", new object[0]));
			}

			private Action<string> onPlayAdIdReadCallback;
		}
	}
}

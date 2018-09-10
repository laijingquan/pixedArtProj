// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace KHD
{
	[AddComponentMenu("")]
	public class FlurryAnalytics : SingletonCrossSceneAutoCreate<FlurryAnalytics>
	{
		public bool replicateDataToUnityAnalytics { get; set; }

		protected override void OnDestroy()
		{
			base.OnDestroy();
			FlurryAnalyticsAndroid.Dispose();
		}

		public void StartSession(string iOSApiKey, string androidApiKey, bool sendErrorsToFlurry)
		{
			FlurryAnalyticsAndroid.Init(androidApiKey, sendErrorsToFlurry);
			FlurryAnalyticsAndroid.OnStartSession();
		}

		public void SetDebugLogEnabled(bool enabled)
		{
			FlurryAnalyticsAndroid.SetLogEnabled(enabled);
		}

		public void SetAppVersion(string appVersion)
		{
			FlurryAnalyticsAndroid.SetVersionName(appVersion);
		}

		public void SetSessionContinueSeconds(int seconds)
		{
			FlurryAnalyticsAndroid.SetContinueSessionMillis((long)(seconds * 1000));
		}

		public void SetPulseEnabled(bool enabled)
		{
			FlurryAnalyticsAndroid.SetPulseEnabled(enabled);
		}

		public void SetUserId(string userId)
		{
			FlurryAnalyticsAndroid.SetUserId(userId);
			this.ReplicateUserIdToUnityAnalytics(userId);
		}

		public void SetAge(int age)
		{
			FlurryAnalyticsAndroid.SetAge(age);
		}

		public void SetGender(FlurryAnalytics.Gender gender)
		{
			FlurryAnalyticsAndroid.SetGender(gender);
			this.ReplicateUserGenderToUnityAnalytics(gender);
		}

		public void LogEvent(string eventName)
		{
			FlurryAnalyticsAndroid.LogEvent(eventName, false);
			this.ReplicateEventToUnityAnalytics(eventName, null);
		}

		public void LogEvent(string eventName, bool isTimed)
		{
			FlurryAnalyticsAndroid.LogEvent(eventName, isTimed);
			this.ReplicateEventToUnityAnalytics(eventName, null);
		}

		public void LogEventWithParameters(string eventName, Dictionary<string, string> parameters)
		{
			FlurryAnalyticsAndroid.LogEventWithParameters(eventName, parameters, false);
			this.ReplicateEventToUnityAnalytics(eventName, parameters);
		}

		public void LogEventWithParameters(string eventName, Dictionary<string, string> parameters, bool isTimed)
		{
			FlurryAnalyticsAndroid.LogEventWithParameters(eventName, parameters, isTimed);
			this.ReplicateEventToUnityAnalytics(eventName, parameters);
		}

		public void EndTimedEvent(string eventName, Dictionary<string, string> parameters = null)
		{
			FlurryAnalyticsAndroid.EndTimedEvent(eventName, parameters);
		}

		private void ReplicateUserIdToUnityAnalytics(string userId)
		{
			if (!this.replicateDataToUnityAnalytics)
			{
				return;
			}
			Analytics.SetUserId(userId);
		}

		private void ReplicateUserGenderToUnityAnalytics(FlurryAnalytics.Gender gender)
		{
			if (!this.replicateDataToUnityAnalytics)
			{
				return;
			}
			Analytics.SetUserGender((gender != FlurryAnalytics.Gender.Male) ? UnityEngine.Analytics.Gender.Female : UnityEngine.Analytics.Gender.Male);
		}

		private void ReplicateEventToUnityAnalytics(string eventName, Dictionary<string, string> parameters = null)
		{
			if (!this.replicateDataToUnityAnalytics)
			{
				return;
			}
			Dictionary<string, object> dictionary = null;
			if (parameters != null && parameters.Count > 0)
			{
				dictionary = new Dictionary<string, object>();
				foreach (KeyValuePair<string, string> keyValuePair in parameters)
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			Analytics.CustomEvent(eventName, dictionary);
		}

		private void DataReplicationToUnityAnalyticsWarning()
		{
			UnityEngine.Debug.LogWarning("Unity Analytics is disabled, please turn off data replication.To enable Unity Analytics please use this guide http://docs.unity3d.com/Manual/UnityAnalyticsOverview.html");
		}

		public enum Gender
		{
			Female,
			Male
		}
	}
}

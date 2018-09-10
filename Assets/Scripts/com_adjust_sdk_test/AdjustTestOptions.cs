// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

namespace com.adjust.sdk.test
{
	public class AdjustTestOptions
	{
		public string BaseUrl { get; set; }

		public string GdprUrl { get; set; }

		public string BasePath { get; set; }

		public string GdprPath { get; set; }

		public bool? Teardown { get; set; }

		public bool? DeleteState { get; set; }

		public bool? UseTestConnectionOptions { get; set; }

		public bool? NoBackoffWait { get; set; }

		public long? TimerIntervalInMilliseconds { get; set; }

		public long? TimerStartInMilliseconds { get; set; }

		public long? SessionIntervalInMilliseconds { get; set; }

		public long? SubsessionIntervalInMilliseconds { get; set; }

		public AndroidJavaObject ToAndroidJavaObject(AndroidJavaObject ajoCurrentActivity)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.adjust.sdk.AdjustTestOptions", new object[0]);
			androidJavaObject.Set<string>("baseUrl", this.BaseUrl);
			androidJavaObject.Set<string>("gdprUrl", this.GdprUrl);
			if (!string.IsNullOrEmpty(this.BasePath))
			{
				androidJavaObject.Set<string>("basePath", this.BasePath);
			}
			if (!string.IsNullOrEmpty(this.GdprPath))
			{
				androidJavaObject.Set<string>("gdprPath", this.GdprPath);
			}
			if (this.DeleteState.GetValueOrDefault(false) && ajoCurrentActivity != null)
			{
				androidJavaObject.Set<AndroidJavaObject>("context", ajoCurrentActivity);
			}
			if (this.UseTestConnectionOptions != null)
			{
				AndroidJavaObject val = new AndroidJavaObject("java.lang.Boolean", new object[]
				{
					this.UseTestConnectionOptions.Value
				});
				androidJavaObject.Set<AndroidJavaObject>("useTestConnectionOptions", val);
			}
			if (this.TimerIntervalInMilliseconds != null)
			{
				AndroidJavaObject val2 = new AndroidJavaObject("java.lang.Long", new object[]
				{
					this.TimerIntervalInMilliseconds.Value
				});
				androidJavaObject.Set<AndroidJavaObject>("timerIntervalInMilliseconds", val2);
			}
			if (this.TimerStartInMilliseconds != null)
			{
				AndroidJavaObject val3 = new AndroidJavaObject("java.lang.Long", new object[]
				{
					this.TimerStartInMilliseconds.Value
				});
				androidJavaObject.Set<AndroidJavaObject>("timerStartInMilliseconds", val3);
			}
			if (this.SessionIntervalInMilliseconds != null)
			{
				AndroidJavaObject val4 = new AndroidJavaObject("java.lang.Long", new object[]
				{
					this.SessionIntervalInMilliseconds.Value
				});
				androidJavaObject.Set<AndroidJavaObject>("sessionIntervalInMilliseconds", val4);
			}
			if (this.SubsessionIntervalInMilliseconds != null)
			{
				AndroidJavaObject val5 = new AndroidJavaObject("java.lang.Long", new object[]
				{
					this.SubsessionIntervalInMilliseconds.Value
				});
				androidJavaObject.Set<AndroidJavaObject>("subsessionIntervalInMilliseconds", val5);
			}
			if (this.Teardown != null)
			{
				AndroidJavaObject val6 = new AndroidJavaObject("java.lang.Boolean", new object[]
				{
					this.Teardown.Value
				});
				androidJavaObject.Set<AndroidJavaObject>("teardown", val6);
			}
			if (this.NoBackoffWait != null)
			{
				AndroidJavaObject val7 = new AndroidJavaObject("java.lang.Boolean", new object[]
				{
					this.NoBackoffWait.Value
				});
				androidJavaObject.Set<AndroidJavaObject>("noBackoffWait", val7);
			}
			return androidJavaObject;
		}
	}
}

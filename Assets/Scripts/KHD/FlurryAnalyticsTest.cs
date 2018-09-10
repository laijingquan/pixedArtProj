// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KHD
{
	public class FlurryAnalyticsTest : MonoBehaviour
	{
		private void Start()
		{
			if (!this.startSessionFromCode)
			{
				return;
			}
			SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.SetDebugLogEnabled(true);
			SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.StartSession("DHQ7W44SGF7D4N7R672C", "ZPP8WWPDGBJR49CF2VTC", true);
		}

		private void OnGUI()
		{
			int num = 0;
			if (this.Button("Log Event", num++))
			{
				SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.LogEvent("KHD Sample Event");
			}
			if (this.Button("Log Event Wit Parameters", num++))
			{
				SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.LogEventWithParameters("KHD Sample Event with parameters", new Dictionary<string, string>
				{
					{
						"Param1",
						"Value1"
					},
					{
						"Param2",
						"Value2"
					},
					{
						"Param3",
						"Value3"
					}
				});
			}
			if (this.Button("Log Timed Event", num++))
			{
				SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.LogEvent("KHD Sample Timed Event New", true);
			}
			if (this.Button("End Timed Event", num++))
			{
				SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.EndTimedEvent("KHD Sample Timed Event New", null);
			}
			if (this.Button("Log Payment", num++))
			{
				System.Random random = new System.Random();
				FlurryAnalyticsAndroid.LogPayment("Test Payment", "com.khd.testpayment", 1, 0.99, "USD", SystemInfo.deviceUniqueIdentifier + random.Next(), null);
			}
		}

		private bool Button(string label, int index)
		{
			float num = (float)Screen.width * 0.8f;
			float num2 = (float)Screen.height * 0.07f;
			Rect position = new Rect(((float)Screen.width - num) * 0.5f, (float)index * num2 + num2 * 0.5f, num, num2);
			return GUI.Button(position, label);
		}

		public bool startSessionFromCode = true;
	}
}

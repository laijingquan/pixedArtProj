// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KHD
{
	public static class FlurryAnalyticsIOS
	{
		public static void StartSession(string apiKey, bool sendCrashReports)
		{
		}

		public static void SetDebugLogEnabled(bool enabled)
		{
		}

		public static void SetAppVersion(string appVersion)
		{
		}

		public static void SetSessionContinueSeconds(int seconds)
		{
		}

		public static void SetPulseEnabled(bool enabled)
		{
		}

		public static void SetIAPReportingEnabled(bool enabled)
		{
		}

		public static void SetUserId(string userId)
		{
		}

		public static void SetAge(int age)
		{
		}

		public static void SetGender(FlurryAnalytics.Gender gender)
		{
		}

		public static void SetLocation(float latitude, float longitude, float horizontalAccuracy, float verticalAccuracy)
		{
		}

		public static void LogEvent(string eventName, bool isTimed = false)
		{
		}

		public static void LogEventWithParameters(string eventName, Dictionary<string, string> parameters, bool isTimed = false)
		{
		}

		public static void EndTimedEvent(string eventName, Dictionary<string, string> parameters = null)
		{
		}

		public static void SetSessionReportsOnCloseEnabled(bool enabled)
		{
		}

		public static void SetSessionReportsOnPauseEnabled(bool enabled)
		{
		}

		private static string ConvertParameters(Dictionary<string, string> parameters)
		{
			if (parameters == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{\n");
			bool flag = true;
			foreach (KeyValuePair<string, string> keyValuePair in parameters)
			{
				if (!flag)
				{
					stringBuilder.Append(',');
				}
				FlurryAnalyticsIOS.SerializeString(stringBuilder, keyValuePair.Key);
				stringBuilder.Append(":");
				FlurryAnalyticsIOS.SerializeString(stringBuilder, keyValuePair.Value);
				flag = false;
			}
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		private static void SerializeString(StringBuilder builder, string str)
		{
			builder.Append('"');
			char[] array = str.ToCharArray();
			foreach (char c in array)
			{
				switch (c)
				{
				case '\b':
					builder.Append("\\b");
					break;
				case '\t':
					builder.Append("\\t");
					break;
				case '\n':
					builder.Append("\\n");
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							int num = Convert.ToInt32(c);
							if (num >= 32 && num <= 126)
							{
								builder.Append(c);
							}
							else
							{
								builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
							}
						}
						else
						{
							builder.Append("\\\\");
						}
					}
					else
					{
						builder.Append("\\\"");
					}
					break;
				case '\f':
					builder.Append("\\f");
					break;
				case '\r':
					builder.Append("\\r");
					break;
				}
			}
			builder.Append('"');
		}

		private static bool IsIPhonePlayer()
		{
			return Application.platform == RuntimePlatform.IPhonePlayer;
		}

		private static bool EditorDebugLogEnabled()
		{
			return false;
		}

		private static void DebugLog(string log)
		{
			if (!FlurryAnalyticsIOS.EditorDebugLogEnabled())
			{
				return;
			}
			UnityEngine.Debug.Log("[FlurryAnalyticsPlugin]: " + log);
		}

		private static void DebugLogError(string error)
		{
			UnityEngine.Debug.Log("[FlurryAnalyticsPlugin]: " + error);
		}
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Fabric.Internal.Runtime;

namespace Fabric.Crashlytics.Internal
{
	internal class Impl
	{
		public static Impl Make()
		{
			return new AndroidImpl();
		}

		public virtual void SetDebugMode(bool mode)
		{
		}

		public virtual void Crash()
		{
			Utils.Log("Crashlytics", "Method Crash () is unimplemented on this platform");
		}

		public virtual void ThrowNonFatal()
		{
			string text = null;
			string message = text.ToLower();
			Utils.Log("Crashlytics", message);
		}

		public virtual void Log(string message)
		{
			Utils.Log("Crashlytics", "Would log custom message if running on a physical device: " + message);
		}

		public virtual void SetKeyValue(string key, string value)
		{
			Utils.Log("Crashlytics", "Would set key-value if running on a physical device: " + key + ":" + value);
		}

		public virtual void SetUserIdentifier(string identifier)
		{
			Utils.Log("Crashlytics", "Would set user identifier if running on a physical device: " + identifier);
		}

		public virtual void SetUserEmail(string email)
		{
			Utils.Log("Crashlytics", "Would set user email if running on a physical device: " + email);
		}

		public virtual void SetUserName(string name)
		{
			Utils.Log("Crashlytics", "Would set user name if running on a physical device: " + name);
		}

		public virtual void RecordCustomException(string name, string reason, StackTrace stackTrace)
		{
			Utils.Log("Crashlytics", "Would record custom exception if running on a physical device: " + name + ", " + reason);
		}

		public virtual void RecordCustomException(string name, string reason, string stackTraceString)
		{
			Utils.Log("Crashlytics", "Would record custom exception if running on a physical device: " + name + ", " + reason);
		}

		private static Dictionary<string, string> ParseFrameString(string regex, string frameString)
		{
			MatchCollection matchCollection = Regex.Matches(frameString, regex);
			if (matchCollection.Count < 1)
			{
				return null;
			}
			Match match = matchCollection[0];
			if (!match.Groups["class"].Success || !match.Groups["method"].Success)
			{
				return null;
			}
			string text = (!match.Groups["file"].Success) ? match.Groups["class"].Value : match.Groups["file"].Value;
			string value = (!match.Groups["line"].Success) ? "0" : match.Groups["line"].Value;
			if (text == Impl.MonoFilenameUnknownString)
			{
				text = match.Groups["class"].Value;
				value = "0";
			}
			return new Dictionary<string, string>
			{
				{
					"class",
					match.Groups["class"].Value
				},
				{
					"method",
					match.Groups["method"].Value
				},
				{
					"file",
					text
				},
				{
					"line",
					value
				}
			};
		}

		protected static Dictionary<string, string>[] ParseStackTraceString(string stackTraceString)
		{
			List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			string[] array = stackTraceString.Split(Impl.StringDelimiters, StringSplitOptions.None);
			if (array.Length < 1)
			{
				return list.ToArray();
			}
			string regex;
			if (Regex.Matches(array[0], Impl.FrameRegexWithFileInfo).Count == 1)
			{
				regex = Impl.FrameRegexWithFileInfo;
			}
			else
			{
				if (Regex.Matches(array[0], Impl.FrameRegexWithoutFileInfo).Count != 1)
				{
					return list.ToArray();
				}
				regex = Impl.FrameRegexWithoutFileInfo;
			}
			foreach (string frameString in array)
			{
				Dictionary<string, string> dictionary = Impl.ParseFrameString(regex, frameString);
				if (dictionary != null)
				{
					list.Add(dictionary);
				}
			}
			return list.ToArray();
		}

		protected const string KitName = "Crashlytics";

		private static readonly string FrameArgsRegex = "\\s?\\(.*\\)";

		private static readonly string FrameRegexWithoutFileInfo = "(?<class>[^\\s]+)\\.(?<method>[^\\s\\.]+)" + Impl.FrameArgsRegex;

		private static readonly string FrameRegexWithFileInfo = Impl.FrameRegexWithoutFileInfo + " .*[/|\\\\](?<file>.+):(?<line>\\d+)";

		private static readonly string MonoFilenameUnknownString = "<filename unknown>";

		private static readonly string[] StringDelimiters = new string[]
		{
			Environment.NewLine
		};

		private delegate Dictionary<string, string> FrameParser(string frameString);
	}
}

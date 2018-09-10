// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace com.adjust.sdk
{
	public static class AdjustLogLevelExtension
	{
		public static string ToLowercaseString(this AdjustLogLevel AdjustLogLevel)
		{
			switch (AdjustLogLevel)
			{
			case AdjustLogLevel.Verbose:
				return "verbose";
			case AdjustLogLevel.Debug:
				return "debug";
			case AdjustLogLevel.Info:
				return "info";
			case AdjustLogLevel.Warn:
				return "warn";
			case AdjustLogLevel.Error:
				return "error";
			case AdjustLogLevel.Assert:
				return "assert";
			case AdjustLogLevel.Suppress:
				return "suppress";
			default:
				return "unknown";
			}
		}

		public static string ToUppercaseString(this AdjustLogLevel AdjustLogLevel)
		{
			switch (AdjustLogLevel)
			{
			case AdjustLogLevel.Verbose:
				return "VERBOSE";
			case AdjustLogLevel.Debug:
				return "DEBUG";
			case AdjustLogLevel.Info:
				return "INFO";
			case AdjustLogLevel.Warn:
				return "WARN";
			case AdjustLogLevel.Error:
				return "ERROR";
			case AdjustLogLevel.Assert:
				return "ASSERT";
			case AdjustLogLevel.Suppress:
				return "SUPPRESS";
			default:
				return "UNKNOWN";
			}
		}
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace com.adjust.sdk
{
	public static class AdjustEnvironmentExtension
	{
		public static string ToLowercaseString(this AdjustEnvironment adjustEnvironment)
		{
			if (adjustEnvironment == AdjustEnvironment.Sandbox)
			{
				return "sandbox";
			}
			if (adjustEnvironment != AdjustEnvironment.Production)
			{
				return "unknown";
			}
			return "production";
		}
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;
using Fabric.Crashlytics.Internal;

namespace Fabric.Crashlytics
{
	public class Crashlytics
	{
		public static void SetDebugMode(bool debugMode)
		{
			Crashlytics.impl.SetDebugMode(debugMode);
		}

		public static void Crash()
		{
			Crashlytics.impl.Crash();
		}

		public static void ThrowNonFatal()
		{
			Crashlytics.impl.ThrowNonFatal();
		}

		public static void Log(string message)
		{
			Crashlytics.impl.Log(message);
		}

		public static void SetKeyValue(string key, string value)
		{
			Crashlytics.impl.SetKeyValue(key, value);
		}

		public static void SetUserIdentifier(string identifier)
		{
			Crashlytics.impl.SetUserIdentifier(identifier);
		}

		public static void SetUserEmail(string email)
		{
			Crashlytics.impl.SetUserEmail(email);
		}

		public static void SetUserName(string name)
		{
			Crashlytics.impl.SetUserName(name);
		}

		public static void RecordCustomException(string name, string reason, StackTrace stackTrace)
		{
			Crashlytics.impl.RecordCustomException(name, reason, stackTrace);
		}

		public static void RecordCustomException(string name, string reason, string stackTraceString)
		{
			Crashlytics.impl.RecordCustomException(name, reason, stackTraceString);
		}

		private static readonly Impl impl = Impl.Make();
	}
}

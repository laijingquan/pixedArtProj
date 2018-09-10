// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Runtime.CompilerServices;
using Fabric.Crashlytics;
using Fabric.Internal.Runtime;
using UnityEngine;

namespace Fabric.Internal.Crashlytics
{
	public class CrashlyticsInit : MonoBehaviour
	{
		private void Awake()
		{
			if (CrashlyticsInit.instance == null)
			{
				this.AwakeOnce();
				CrashlyticsInit.instance = this;
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
			else if (CrashlyticsInit.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void AwakeOnce()
		{
			CrashlyticsInit.RegisterExceptionHandlers();
		}

		private static void RegisterExceptionHandlers()
		{
			if (CrashlyticsInit.IsSDKInitialized())
			{
				Utils.Log(CrashlyticsInit.kitName, "Registering exception handlers");
				AppDomain currentDomain = AppDomain.CurrentDomain;
				if (CrashlyticsInit.__f__mg_cache0 == null)
				{
					CrashlyticsInit.__f__mg_cache0 = new UnhandledExceptionEventHandler(CrashlyticsInit.HandleException);
				}
				currentDomain.UnhandledException += CrashlyticsInit.__f__mg_cache0;
				if (CrashlyticsInit.__f__mg_cache1 == null)
				{
					CrashlyticsInit.__f__mg_cache1 = new Application.LogCallback(CrashlyticsInit.HandleLog);
				}
				Application.logMessageReceived += CrashlyticsInit.__f__mg_cache1;
			}
			else
			{
				Utils.Log(CrashlyticsInit.kitName, "Did not register exception handlers: Crashlytics SDK was not initialized");
			}
		}

		private static bool IsSDKInitialized()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.crashlytics.android.Crashlytics");
			AndroidJavaObject androidJavaObject = null;
			try
			{
				androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			}
			catch
			{
				androidJavaObject = null;
			}
			return androidJavaObject != null;
		}

		private static void HandleException(object sender, UnhandledExceptionEventArgs eArgs)
		{
			Exception ex = (Exception)eArgs.ExceptionObject;
			CrashlyticsInit.HandleLog(ex.Message.ToString(), ex.StackTrace.ToString(), LogType.Exception);
		}

		private static void HandleLog(string message, string stackTraceString, LogType type)
		{
			if (type == LogType.Exception)
			{
				Utils.Log(CrashlyticsInit.kitName, "Recording exception: " + message);
				Utils.Log(CrashlyticsInit.kitName, "Exception stack trace: " + stackTraceString);
				//string[] messageParts = CrashlyticsInit.getMessageParts(message);
				//Crashlytics.RecordCustomException(messageParts[0], messageParts[1], stackTraceString);
			}
		}

		private static string[] getMessageParts(string message)
		{
			char[] separator = new char[]
			{
				':'
			};
			string[] array = message.Split(separator, 2, StringSplitOptions.None);
			foreach (string text in array)
			{
				text.Trim();
			}
			if (array.Length == 2)
			{
				return array;
			}
			return new string[]
			{
				"Exception",
				message
			};
		}

		private static readonly string kitName = "Crashlytics";

		private static CrashlyticsInit instance;

		[CompilerGenerated]
		private static UnhandledExceptionEventHandler __f__mg_cache0;

		[CompilerGenerated]
		private static Application.LogCallback __f__mg_cache1;
	}
}

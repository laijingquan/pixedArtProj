// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FMLogger
{
	public static void vGb(string msg)
	{
		FMLogger.v(msg, FMLogger.gbLayer);
	}

	public static void vAds(string msg)
	{
		FMLogger.v(msg, FMLogger.adsLayer);
	}

	public static void vStat(string msg)
	{
		FMLogger.v(msg, FMLogger.analiticsLayer);
	}

	public static void vCore(string msg)
	{
		FMLogger.v(msg, FMLogger.coreLayer);
	}

	public static void v(string msg, int layer = 0)
	{
		if (FMLogger.disabled)
		{
			return;
		}
		if (layer == 0)
		{
			FMLogger.DeubLog(msg);
		}
		else
		{
			if ((layer & FMLogger.showLayer) == FMLogger.gbLayer)
			{
				FMLogger.DeubLog(msg);
			}
			if ((layer & FMLogger.showLayer) == FMLogger.analiticsLayer)
			{
				FMLogger.DeubLog(msg);
			}
			if ((layer & FMLogger.showLayer) == FMLogger.adsLayer)
			{
				FMLogger.DeubLog(msg);
			}
			if ((layer & FMLogger.showLayer) == FMLogger.coreLayer)
			{
				FMLogger.DeubLog(msg);
			}
		}
	}

	private static void DeubLog(string msg)
	{
		UnityEngine.Debug.Log(msg);
	}

	public static void e(string msg)
	{
		UnityEngine.Debug.LogError(msg);
	}

	public static void Log(UnityEngine.Object msg)
	{
		FMLogger.log = msg + "\n" + FMLogger.log;
		if (FMLogger.log.Length > 3000)
		{
			FMLogger.log = FMLogger.log.Substring(0, 2000);
		}
	}

	public static void Log(string msg)
	{
		if (BuildConfig.LOG_FILE)
		{
			FMLogger.log = msg + "\n" + FMLogger.log;
			if (FMLogger.log.Length > 3000)
			{
				FMLogger.log = FMLogger.log.Substring(0, 2000);
			}
		}
	}

	public static void Log(int msg)
	{
		if (BuildConfig.LOG_FILE)
		{
			FMLogger.log = msg + "\n" + FMLogger.log;
			if (FMLogger.log.Length > 3000)
			{
				FMLogger.log = FMLogger.log.Substring(0, 2000);
			}
		}
	}

	public static void LogError(UnityEngine.Object msg)
	{
		if (BuildConfig.LOG_FILE)
		{
			FMLogger.log = string.Concat(new object[]
			{
				"ERROR*****: ",
				msg,
				"\n",
				FMLogger.log
			});
			if (FMLogger.log.Length > 3000)
			{
				FMLogger.log = FMLogger.log.Substring(0, 2000);
			}
		}
	}

	public static void LogError(string msg)
	{
		if (BuildConfig.LOG_FILE)
		{
			FMLogger.log = "ERROR*****: " + msg + "\n" + FMLogger.log;
			if (FMLogger.log.Length > 3000)
			{
				FMLogger.log = FMLogger.log.Substring(0, 2000);
			}
		}
	}

	public static string GetLogStr()
	{
		return FMLogger.log;
	}

	public static string log = string.Empty;

	private static bool disabled = !BuildConfig.LOG_ENABLED;

	private static int gbLayer = 1;

	private static int analiticsLayer = 2;

	private static int adsLayer = 4;

	private static int coreLayer = 8;

	private static int showLayer = 12;
}

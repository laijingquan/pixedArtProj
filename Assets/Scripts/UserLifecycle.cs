// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public static class UserLifecycle
{
	public static string UserId
	{
		get
		{
			if (UserLifecycle.userId == null)
			{
				UserLifecycle.userId = PlayerPrefs.GetString("ul_user_id", string.Empty);
				if (string.IsNullOrEmpty(UserLifecycle.userId))
				{
					UserLifecycle.userId = UserLifecycle.GenerateGUID();
					FMLogger.vCore("UL. generate user id");
					PlayerPrefs.SetString("ul_user_id", UserLifecycle.userId);
					PlayerPrefs.Save();
				}
			}
			return UserLifecycle.userId;
		}
	}

	public static string SessionId
	{
		get
		{
			if (UserLifecycle.sessionId == null)
			{
				UserLifecycle.sessionId = PlayerPrefs.GetString("ul_session_id", string.Empty);
				if (string.IsNullOrEmpty(UserLifecycle.sessionId))
				{
					UserLifecycle.sessionId = UserLifecycle.GenerateGUID();
					FMLogger.vCore("UL. generate session id fallback");
					PlayerPrefs.SetString("ul_session_id", UserLifecycle.sessionId);
				}
			}
			return UserLifecycle.sessionId;
		}
	}

	public static void AppLaunch()
	{
		UserLifecycle.sessionId = UserLifecycle.GenerateGUID();
		UserLifecycle.SendAppOpen();
	}

	public static void AppPause()
	{
		UserLifecycle.pauseTime = DateTime.Now;
	}

	public static void AppResume()
	{
		int num = Mathf.FloorToInt((float)(DateTime.Now - UserLifecycle.pauseTime).TotalHours);
		if (num >= 5)
		{
			UserLifecycle.sessionId = UserLifecycle.GenerateGUID();
			UserLifecycle.SendAppOpen();
		}
	}

	private static string GenerateGUID()
	{
		return Guid.NewGuid().ToString();
	}

	private static void SendAppOpen()
	{
		if (!string.IsNullOrEmpty(AppState.PushNotificationId))
		{
			string pushNotificationId = AppState.PushNotificationId;
			AppState.PushNotificationId = null;
			FMLogger.vCore("App open from push notif event " + pushNotificationId);
			AnalyticsManager.AppOpened(true, pushNotificationId, false);
		}
		else if (AppState.LocalNotificationLaunch)
		{
			AppState.LocalNotificationLaunch = false;
			FMLogger.vCore("App open from local notif event");
			AnalyticsManager.AppOpened(false, string.Empty, true);
		}
		else
		{
			FMLogger.vCore("App open normal event");
			AnalyticsManager.AppOpened(false, string.Empty, false);
		}
	}

	private const string usedIdKey = "ul_user_id";

	private const string sessionIdKey = "ul_session_id";

	private static string userId;

	private static string sessionId;

	private static DateTime pauseTime;
}

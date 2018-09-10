// dnSpy decompiler from Assembly-CSharp.dll
using System;
using Assets.SimpleAndroidNotifications;
using Assets.SimpleAndroidNotifications.Data;
using Assets.SimpleAndroidNotifications.Enums;
using Assets.SimpleAndroidNotifications.Helpers;
using UnityEngine;

public class LocalPushController : MonoBehaviour
{
	private void Awake()
	{
		try
		{
			NotificationCallback notificationCallback = NotificationManager.GetNotificationCallback();
			if (notificationCallback != null && notificationCallback.Id != this.lastNotifId)
			{
				this.lastNotifId = notificationCallback.Id;
				AppState.LocalNotificationLaunch = true;
				FMLogger.vCore("local notif app launch");
			}
			NotificationManager.CancelAll();
		}
		catch (Exception ex)
		{
			FMLogger.vCore("NotificationManager check and cancel ex " + ex.Message);
		}
	}

	private void Start()
	{
	}

	private void OnApplicationPause(bool isPause)
	{
		try
		{
			if (isPause)
			{
				int num = UnityEngine.Random.Range(1, 5);
				string textByKey = LocalizationService.Instance.GetTextByKey("localNotification_0" + num + "_title");
				string textByKey2 = LocalizationService.Instance.GetTextByKey("localNotification_0" + num + "_body");
				NotificationParams notificationParams = new NotificationParams
				{
					Id = NotificationIdHandler.GetNotificationId(),
					Delay = TimeSpan.FromHours(24.0),
					Title = textByKey,
					Message = textByKey2,
					Ticker = textByKey,
					Sound = true,
					Vibrate = true,
					Light = true,
					SmallIcon = NotificationIcon.Bell,
					SmallIconColor = new Color(0.63f, 0.63f, 0.63f),
					LargeIcon = "app_icon",
					ExecuteMode = NotificationExecuteMode.Inexact,
					Importance = NotificationImportance.Max,
					Repeat = true,
					RepeatInterval = TimeSpan.FromHours(24.0),
					ChannelId = "coloring.local",
					ChannelName = "Miscellaneous"
				};
				NotificationManager.SendCustom(notificationParams);
			}
			else
			{
				NotificationCallback notificationCallback = NotificationManager.GetNotificationCallback();
				if (notificationCallback != null && notificationCallback.Id != this.lastNotifId)
				{
					this.lastNotifId = notificationCallback.Id;
					AppState.LocalNotificationLaunch = true;
					FMLogger.vCore("local notif app resume");
				}
				NotificationManager.CancelAll();
			}
		}
		catch (Exception ex)
		{
			FMLogger.vCore("NotificationManager resume handler ex. " + ex.Message);
		}
	}

	private int lastNotifId = int.MinValue;
}

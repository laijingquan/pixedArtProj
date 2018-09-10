// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.SimpleAndroidNotifications.Helpers
{
	public static class NotificationIdHandler
	{
		public static List<int> GetScheduledNotificaions()
		{
			List<int> result;
			if (PlayerPrefs.HasKey("NotificationHelper.Scheduled"))
			{
				IEnumerable<string> source = from i in PlayerPrefs.GetString("NotificationHelper.Scheduled").Split(new char[]
				{
					'|'
				})
				where i != string.Empty
				select i;
				if (NotificationIdHandler.__f__mg_cache0 == null)
				{
					NotificationIdHandler.__f__mg_cache0 = new Func<string, int>(int.Parse);
				}
				result = source.Select(NotificationIdHandler.__f__mg_cache0).ToList<int>();
			}
			else
			{
				result = new List<int>();
			}
			return result;
		}

		public static void SetScheduledNotificaions(List<int> scheduledNotificaions)
		{
			PlayerPrefs.SetString("NotificationHelper.Scheduled", string.Join("|", (from i in scheduledNotificaions
			select i.ToString()).ToArray<string>()));
		}

		public static void AddScheduledNotificaion(int notificationId)
		{
			List<int> scheduledNotificaions = NotificationIdHandler.GetScheduledNotificaions();
			scheduledNotificaions.Add(notificationId);
			NotificationIdHandler.SetScheduledNotificaions(scheduledNotificaions);
		}

		public static void RemoveScheduledNotificaion(int id)
		{
			List<int> scheduledNotificaions = NotificationIdHandler.GetScheduledNotificaions();
			scheduledNotificaions.RemoveAll((int i) => i == id);
			NotificationIdHandler.SetScheduledNotificaions(scheduledNotificaions);
		}

		public static void RemoveAllScheduledNotificaions()
		{
			NotificationIdHandler.SetScheduledNotificaions(new List<int>());
		}

		public static int GetNotificationId()
		{
			List<int> scheduledNotificaions = NotificationIdHandler.GetScheduledNotificaions();
			int num;
			do
			{
				num = UnityEngine.Random.Range(0, int.MaxValue);
			}
			while (scheduledNotificaions.Contains(num));
			return num;
		}

		private const string PlayerPrefsKey = "NotificationHelper.Scheduled";

		[CompilerGenerated]
		private static Func<string, int> __f__mg_cache0;
	}
}

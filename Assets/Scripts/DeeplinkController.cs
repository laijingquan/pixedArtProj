// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DeeplinkController
{
	public DeeplinkController(FMDeepLink deepLinker)
	{
		if (deepLinker != null)
		{
			this.fmDeepLink = deepLinker;
			deepLinker.Initialize(new FMDeepLink.DeeplinkDataReceived(this.FMDeepLinkCallback), false);
		}
		else
		{
			UnityEngine.Debug.LogError("Check FMDeepLink component on AppManager game object");
		}
	}

	 
	public event Action<string> BonusCodeReceived;

	private void FMDeepLinkCallback(string data)
	{
		FMLogger.vCore("FMDeepLinkCallback");
		this.HandleLaunchData(data);
	}

	public void Check()
	{
		FMLogger.vCore("deeplink check.");
		if (this.fmDeepLink != null)
		{
			this.fmDeepLink.CheckForMessage();
		}
		else
		{
			UnityEngine.Debug.LogError("FMDeepLink ref is null");
		}
	}

	private void HandleLaunchData(string launchStr)
	{
		if (!string.IsNullOrEmpty(launchStr))
		{
			FMLogger.vCore("dp launch uri: " + launchStr);
			try
			{
				Uri uri = new Uri(launchStr);
				FMLogger.vCore("parse deeplink uri:" + uri.Query);
				this.HandleQueryString(uri.Query);
			}
			catch (Exception ex)
			{
				FMLogger.vCore("failed to parse deeplink url. msg:" + ex.Message);
			}
		}
		else
		{
			FMLogger.vCore("dp launch uri is empty");
		}
	}

	private void HandleQueryString(string query)
	{
		if (string.IsNullOrEmpty(query))
		{
			FMLogger.vCore("deeplink empty queue");
			return;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (string text in query.TrimStart(new char[]
		{
			'?'
		}).Split(new char[]
		{
			'&'
		}, StringSplitOptions.RemoveEmptyEntries))
		{
			string[] array2 = text.Split(new char[]
			{
				'='
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length == 2)
			{
				dictionary[array2[0].Trim()] = WWW.UnEscapeURL(array2[1]).Trim();
			}
			else
			{
				dictionary[array2[0].Trim()] = string.Empty;
			}
		}
		bool flag = false;
		foreach (KeyValuePair<string, string> keyValuePair in dictionary)
		{
			if (keyValuePair.Key.Equals("bonus_code"))
			{
				flag = true;
				if (this.BonusCodeReceived != null)
				{
					this.BonusCodeReceived(keyValuePair.Value);
				}
			}
		}
		if (!flag)
		{
			AnalyticsManager.DeeplinkQueueError();
		}
	}

	private FMDeepLink fmDeepLink;
}

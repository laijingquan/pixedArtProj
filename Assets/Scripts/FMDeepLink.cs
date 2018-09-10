// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FMDeepLink : MonoBehaviour
{
	public void Initialize(FMDeepLink.DeeplinkDataReceived deeplinkCallback, bool autoCheckOnResume = false)
	{
		if (FMDeepLink.inited)
		{
			return;
		}
		this.autoCheck = autoCheckOnResume;
		this.deepLinkDelegate = deeplinkCallback;
		this.deepLinkHandler = new FMDeepLinkAndroid(base.gameObject.name, "onDeeplinkOpened");
		this.deepLinkHandler.CheckForMessage();
		FMDeepLink.inited = true;
	}

	public void CheckForMessage()
	{
		if (!FMDeepLink.inited)
		{
			UnityEngine.Debug.LogError("FMDeepLink must be initialized first");
		}
		this.deepLinkHandler.CheckForMessage();
	}

	public void OnApplicationPause(bool paused)
	{
		if (!FMDeepLink.inited)
		{
			return;
		}
		if (!paused)
		{
			this.deepLinkHandler.CheckForMessage();
		}
	}

	private void onDeeplinkOpened(string url)
	{
		if (this.deepLinkDelegate != null)
		{
			this.deepLinkDelegate(url);
		}
	}

	private IDeepLinkHandler deepLinkHandler;

	private FMDeepLink.DeeplinkDataReceived deepLinkDelegate;

	private const string methodName = "onDeeplinkOpened";

	private static bool inited;

	private bool autoCheck;

	public delegate void DeeplinkDataReceived(string data);
}

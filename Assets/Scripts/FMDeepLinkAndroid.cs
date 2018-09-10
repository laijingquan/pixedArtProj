// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.IO;
using UnityEngine;

public class FMDeepLinkAndroid : IDeepLinkHandler
{
	public FMDeepLinkAndroid(string gameListenerName, string methodName)
	{
		try
		{
			this.deepLinker = new AndroidJavaObject("fmp.deeplinker.DeeplinkProxy", new object[]
			{
				gameListenerName,
				methodName
			});
		}
		catch (Exception)
		{
			throw new FileNotFoundException("USP deeplink jar not found. Do you have it in your plugin folder?");
		}
	}

	public void CheckForMessage()
	{
		if (this.deepLinker != null)
		{
			this.deepLinker.Call("CheckIfDeeplinkOpened", new object[0]);
		}
		else
		{
			UnityEngine.Debug.LogError("Deeplink CheckForMessage Error. Check if jar file is added");
		}
	}

	private AndroidJavaObject deepLinker;
}

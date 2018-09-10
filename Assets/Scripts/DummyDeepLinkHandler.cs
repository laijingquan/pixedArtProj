// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DummyDeepLinkHandler : IDeepLinkHandler
{
	public void CheckForMessage()
	{
		UnityEngine.Debug.Log("Deeplink check. Editor stub");
	}
}

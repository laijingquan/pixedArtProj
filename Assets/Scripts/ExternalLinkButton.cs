// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ExternalLinkButton : MonoBehaviour
{
	public string URL { get; private set; }

	public string Scheme { get; private set; }

	public void Init(string scheme, string url)
	{
		this.URL = url;
		this.Scheme = scheme;
	}
}

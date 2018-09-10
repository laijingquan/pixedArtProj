// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class IconDownloadTask
{
	public IconDownloadTask(string[] baseURls, string relativeUrl, string localKey, Action<bool, Texture2D> h)
	{
		this.IsCanceled = false;
		this.Completed = false;
		this.BaseUrls = baseURls;
		this.RelativeUrl = relativeUrl;
		this.CacheKey = localKey;
		this.handler = h;
	}

	public string[] BaseUrls { get; private set; }

	public string RelativeUrl { get; private set; }

	public string CacheKey { get; private set; }

	public bool IsCanceled { get; private set; }

	public bool Completed { get; private set; }

	public void Cancel()
	{
		this.IsCanceled = true;
		this.handler = null;
	}

	public bool IsRunning
	{
		get
		{
			return !this.IsCanceled && !this.Completed;
		}
	}

	public void Result(bool success, Texture2D tex)
	{
		this.Completed = true;
		if (this.handler != null)
		{
			this.handler(success, tex);
		}
		this.handler = null;
		this.fallbackHandler = null;
	}

	public void AddHandlerOnCancelation(Action<string, Texture2D> fh)
	{
		this.fallbackHandler = fh;
	}

	public void FallbackCancelation(Texture2D tex)
	{
		if (this.fallbackHandler != null)
		{
			FMLogger.Log("Fallback icon dl handle: " + this.CacheKey);
			this.fallbackHandler(this.CacheKey, tex);
		}
		this.fallbackHandler = null;
	}

	private Action<bool, Texture2D> handler;

	private Action<string, Texture2D> fallbackHandler;
}

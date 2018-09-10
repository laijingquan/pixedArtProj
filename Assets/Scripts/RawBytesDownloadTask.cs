// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class RawBytesDownloadTask
{
	public RawBytesDownloadTask(string[] baseUrls, string relativeUrl, Action<bool, byte[]> h)
	{
		this.BaseUrls = baseUrls;
		this.RelativeUrl = relativeUrl;
		this.handler = h;
		this.IsCanceled = false;
	}

	public string[] BaseUrls { get; private set; }

	public string RelativeUrl { get; private set; }

	public bool IsCanceled { get; private set; }

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

	public bool Completed { get; private set; }

	public void Result(bool success, byte[] result)
	{
		if (this.IsCanceled)
		{
			return;
		}
		this.Completed = true;
		if (this.handler != null)
		{
			this.handler(success, result);
		}
		this.handler = null;
	}

	private Action<bool, byte[]> handler;
}

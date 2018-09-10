// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class WebGetTask
{
	public WebGetTask(string url, Action<bool, string> handler)
	{
		this.handler = handler;
		this.BaseUrls = new string[]
		{
			url
		};
		this.RelativeUrl = string.Empty;
		this.IsCanceled = false;
	}

	public WebGetTask(string[] baseUrls, string relativeUrl, Action<bool, string> handler)
	{
		this.handler = handler;
		this.BaseUrls = baseUrls;
		this.RelativeUrl = relativeUrl;
		this.IsCanceled = false;
	}

	public string[] BaseUrls { get; private set; }

	public string RelativeUrl { get; private set; }

	public bool IsCanceled { get; private set; }

	public bool Completed { get; private set; }

	public string Responce { get; private set; }

	public bool Success { get; private set; }

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

	public void Result(bool success, string result)
	{
		if (!this.IsRunning)
		{
			return;
		}
		this.Responce = result;
		this.Success = success;
		this.Completed = true;
		this.IsCanceled = true;
		if (this.handler != null)
		{
			this.handler(success, result);
		}
		this.handler = null;
	}

	private Action<bool, string> handler;
}

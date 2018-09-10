// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class PageContentTask
{
	public PageContentTask(int page, Action<bool, PageContentInfo> h, Action longLoadWarning)
	{
		this.IsCanceled = false;
		this.Page = page;
		this.handler = h;
		this.longLoadWarning = longLoadWarning;
	}

	public int Page { get; private set; }

	public bool IsCanceled { get; private set; }

	public bool Completed { get; private set; }

	public bool IsRunning
	{
		get
		{
			return !this.IsCanceled && !this.Completed;
		}
	}

	public void Cancel()
	{
		this.IsCanceled = true;
		this.handler = null;
		this.longLoadWarning = null;
	}

	public void Result(bool success, PageContentInfo result)
	{
		this.longLoadWarning = null;
		this.Completed = true;
		if (this.handler != null)
		{
			this.handler(success, result);
		}
		this.handler = null;
	}

	public void TriggerLongLoading()
	{
		if (!this.IsRunning)
		{
			return;
		}
		if (this.longLoadWarning != null)
		{
			this.longLoadWarning();
		}
	}

	private Action<bool, PageContentInfo> handler;

	private Action longLoadWarning;
}

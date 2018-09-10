// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class DailyPageContentTask
{
	public DailyPageContentTask(int page, Action<bool, DailyTabInfo> h, Action longLoadingWarning)
	{
		this.IsCanceled = false;
		this.Page = page;
		this.handler = h;
		this.longLoadingWarning = longLoadingWarning;
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
	}

	public void Result(bool success, DailyTabInfo result)
	{
		this.Completed = true;
		if (this.handler != null)
		{
			this.handler(success, result);
		}
	}

	public void TriggerWarning()
	{
		if (!this.IsRunning)
		{
			return;
		}
		if (this.longLoadingWarning != null)
		{
			this.longLoadingWarning();
		}
	}

	private Action<bool, DailyTabInfo> handler;

	private Action longLoadingWarning;
}

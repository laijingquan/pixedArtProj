// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;

public class TGFContentSyncTask
{
	public TGFContentSyncTask(Action<SysSyncContentResponce> h)
	{
		this.IsCanceled = false;
		this.handler += h;
	}

	public float Timeout { get; private set; }

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

	public void Result(SysSyncContentResponce result)
	{
		this.Completed = true;
		if (this.handler != null)
		{
			this.handler(result);
		}
		this.handler = null;
	}

	 
	private event Action<SysSyncContentResponce> handler;

	public void AddHandler(Action<SysSyncContentResponce> h)
	{
		this.handler += h;
	}
}

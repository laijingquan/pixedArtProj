// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TimeoutTask
{
	public TimeoutTask(float timeout, Action callback)
	{
		this.Timeout = timeout;
		this.handler = callback;
	}

	public float Timeout { get; private set; }

	public bool Completed { get; private set; }

	public void Cancel()
	{
		if (this.coroutine != null)
		{
			TimeoutTaskRunner.Instance.StopHandler(this.coroutine);
		}
		this.Completed = true;
		this.handler = null;
		this.coroutine = null;
	}

	private void FireAlarm()
	{
		this.Completed = true;
		this.coroutine = null;
		if (this.handler != null)
		{
			this.handler();
		}
		this.handler = null;
	}

	public void Run()
	{
		this.coroutine = TimeoutTaskRunner.Instance.Run(this.Timeout, new Action(this.OnTimeout));
	}

	private void OnTimeout()
	{
		if (!this.Completed)
		{
			this.FireAlarm();
		}
	}

	private Action handler;

	private Coroutine coroutine;
}

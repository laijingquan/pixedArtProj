// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class DownloadPictureTask
{
	public DownloadPictureTask(PictureData pd, Action<bool, PictureData> resultHandler, Action longLoadHandler, Action extraLongLoadHandler)
	{
		this.PictureData = pd;
		this.IsCanceled = false;
		this.resultHandler = resultHandler;
		this.longLoadHandler = longLoadHandler;
		this.extraLongLoadHandler = extraLongLoadHandler;
		this.Timeout = 90f;
		this.FirstSlowNotifTimeout = 5f;
		this.SecondSlowNotifTimeout = 15f;
	}

	public PictureData PictureData { get; private set; }

	public float Timeout { get; private set; }

	public float FirstSlowNotifTimeout { get; private set; }

	public float SecondSlowNotifTimeout { get; private set; }

	public bool IsCanceled { get; private set; }

	public void Cancel()
	{
		this.Completed = true;
		this.IsCanceled = true;
		this.resultHandler = null;
		this.longLoadHandler = null;
		this.extraLongLoadHandler = null;
		if (this.cancelHandler != null)
		{
			this.cancelHandler();
		}
		this.cancelHandler = null;
	}

	public bool IsRunning
	{
		get
		{
			return !this.IsCanceled && !this.Completed;
		}
	}

	public bool IsRunningExtraLong
	{
		get
		{
			return this.IsRunning && this.extraLongTriggered;
		}
	}

	public bool Completed { get; private set; }

	public void SetTimeouts(float global, float firstWarning, float secondwarning)
	{
		this.Timeout = global;
		this.FirstSlowNotifTimeout = firstWarning;
		this.SecondSlowNotifTimeout = secondwarning;
	}

	public void AddCancelationHandler(Action cancelCallback)
	{
		this.cancelHandler = cancelCallback;
	}

	public void Result(bool success, PictureData result)
	{
		if (this.Completed)
		{
			return;
		}
		this.Completed = true;
		if (this.resultHandler != null)
		{
			this.resultHandler(success, result);
		}
		this.resultHandler = null;
		this.cancelHandler = null;
	}

	public void TriggerLongLoad()
	{
		if (this.longLoadHandler != null)
		{
			this.longLoadHandler();
		}
		this.longLoadHandler = null;
	}

	public void TriggerExtraLongLoad()
	{
		this.extraLongTriggered = true;
		if (this.extraLongLoadHandler != null)
		{
			this.extraLongLoadHandler();
		}
		this.extraLongLoadHandler = null;
	}

	private bool extraLongTriggered;

	private Action longLoadHandler;

	private Action extraLongLoadHandler;

	private Action cancelHandler;

	private Action<bool, PictureData> resultHandler;
}

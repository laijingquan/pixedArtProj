// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class BonusCodeContentTask
{
	public BonusCodeContentTask(string giftCode, Action<bool, List<PictureData>> h)
	{
		this.IsCanceled = false;
		this.GiftCode = giftCode;
		this.handler = h;
	}

	public string GiftCode { get; private set; }

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

	public void Result(bool success, List<PictureData> result)
	{
		this.Completed = true;
		if (this.handler != null)
		{
			this.handler(success, result);
		}
	}

	private Action<bool, List<PictureData>> handler;
}

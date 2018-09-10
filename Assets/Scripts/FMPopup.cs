// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FMPopup : MonoBehaviour
{
	public void Open()
	{
		if (this.IsAutoclosingPopup)
		{
			this.currentShowTime = this.showTime;
		}
		this.StartOpenAnimation();
	}

	public void Close()
	{
		if (this.IsAutoclosingPopup)
		{
			this.currentShowTime = 0f;
		}
		this.StartClosingAnimation();
	}

	protected virtual void StartOpenAnimation()
	{
	}

	protected virtual void StartClosingAnimation()
	{
	}

	public bool IsAutoclosingPopup;

	public float showTime;

	public float currentShowTime;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MyPopupManager : FMPopupManager
{
	public void OpenRoot()
	{
		base.Open(this.rootPopup, FMPopupManager.FMPopupPriority.Normal);
	}

	public void CloseRoot()
	{
		this.CloseActive();
	}

	public void OpenNested()
	{
		base.Open(this.nestedPopup, FMPopupManager.FMPopupPriority.High);
		base.CloseActivePopup(true);
	}

	public void CloseNested()
	{
		base.Open(this.rootPopup, FMPopupManager.FMPopupPriority.High);
		base.CloseActivePopup(true);
	}

	public void OpenForced()
	{
		base.Open(this.forcedPopup, FMPopupManager.FMPopupPriority.ForceOpen);
	}

	public void OpenDelayed()
	{
		base.Open(this.timeOutPopup, FMPopupManager.FMPopupPriority.Low);
	}

	internal void OpenTimeoutPopup()
	{
		base.Open(this.timeoutPopup, FMPopupManager.FMPopupPriority.Normal);
	}

	public void CloseActive()
	{
		base.CloseActivePopup(true);
	}

	[SerializeField]
	private FMPopup rootPopup;

	[SerializeField]
	private FMPopup nestedPopup;

	[SerializeField]
	private FMPopup timeOutPopup;

	[SerializeField]
	private FMPopup forcedPopup;

	[SerializeField]
	private FMPopup timeoutPopup;
}

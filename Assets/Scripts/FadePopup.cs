// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FadePopup : FMPopup
{
	protected override void StartOpenAnimation()
	{
		base.StartOpenAnimation();
		base.GetComponent<Animation>().Play(this.openAnimationClipName);
	}

	protected override void StartClosingAnimation()
	{
		base.StartClosingAnimation();
		base.GetComponent<Animation>().Play(this.closeAnimationClipName);
	}

	public string openAnimationClipName;

	public string closeAnimationClipName;
}

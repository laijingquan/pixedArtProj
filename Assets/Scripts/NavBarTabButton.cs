// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class NavBarTabButton : MonoBehaviour
{
	public bool IsOn { get; private set; }

	public void SetState(bool isOn, bool animated = true)
	{
		if (this.IsOn == isOn)
		{
			return;
		}
		this.IsOn = isOn;
		this.onBtn.SetActive(this.IsOn);
		this.offBtn.SetActive(!this.IsOn);
		this.anim.PlayOneShot(!isOn);
	}

	[SerializeField]
	private GameObject onBtn;

	[SerializeField]
	private GameObject offBtn;

	[SerializeField]
	private AdvancedImageAnimation anim;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugNumIndicator : MonoBehaviour
{
	private void Awake()
	{
		this.label.text = "Font: " + GeneralSettings.NumSize;
	}

	public Text label;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PageIndicatorItem : MonoBehaviour
{
	public void Select()
	{
		this.checkmark.gameObject.SetActive(true);
	}

	public void Deselect()
	{
		this.checkmark.gameObject.SetActive(false);
	}

	[SerializeField]
	private RectTransform checkmark;
}

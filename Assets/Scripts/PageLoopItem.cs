// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PageLoopItem : MonoBehaviour
{
	public RectTransform rt { get; private set; }

	public void Init(int num)
	{
		this.Page = num;
		this.rt = (RectTransform)base.transform;
	}

	public int Page;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ScrollItem : MonoBehaviour
{
	public virtual void OnBecomeVisable(int row)
	{
		this.Row = row;
	}

	public virtual void OnBecomeInvinsable()
	{
	}

	public int Row;
}

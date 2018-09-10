// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LoadIndicator : MonoBehaviour
{
	private void Update()
	{
		base.transform.Rotate(0f, 0f, 180f * Time.deltaTime);
	}
}

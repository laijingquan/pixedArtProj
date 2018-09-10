// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ColorUtils
{
	public static bool IsSameColors(Color a, Color b)
	{
		float num = Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b);
		return num < 0.04f;
	}

	public static bool IsSameColors(Color32 a, Color b)
	{
		Color color = new Color((float)a.r / 255f, (float)a.g / 255f, (float)a.b / 255f, 1f);
		float num = Mathf.Abs(color.r - b.r) + Mathf.Abs(color.g - b.g) + Mathf.Abs(color.b - b.b);
		return num < 0.04f;
	}
}

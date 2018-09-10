// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SafeLayout
{
	public static bool IsTablet { get; private set; }

	public static float ScreenSize { get; private set; }

	public static void Init()
	{
		SafeLayout.ScreenSize = SafeLayout.GetScreenSize();
		SafeLayout.IsTablet = SafeLayout.IsTabletDevice(SafeLayout.ScreenSize);
		FMLogger.vCore("tablet:" + SafeLayout.IsTablet);
	}

	public static int GetMinTopCanvasOffset(int height)
	{
		return 0;
	}

	public static int GetMaxTopCanvasOffset(int height)
	{
		return 0;
	}

	public static int GetMaxBottomCanvasOffset(int height)
	{
		return 0;
	}

	private static bool IsTabletDevice(float screenSize)
	{
		return screenSize > 6.9f;
	}

	public static float GetScreenSize()
	{
		float num = Mathf.Max(160f, Screen.dpi);
		float f = (float)Screen.width / num;
		float f2 = (float)Screen.height / num;
		return Mathf.Sqrt(Mathf.Pow(f, 2f) + Mathf.Pow(f2, 2f));
	}

	private static bool fakeTablet;

	public static bool FakeX;
}

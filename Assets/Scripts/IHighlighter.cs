// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public interface IHighlighter
{
	void HighlightColor(int cId);

	void DehighlightForIcon();

	void HighlightLast();

	void FillColor(Point p, Color32 color, int markTag);

	void Clean();
}

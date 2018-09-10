// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPaintFill
{
	FillAlgorithm FillType { get; }

	Texture2D DrawTex { get; }

	Texture2D LineTex { get; }

	short[] SourceMask { get; }

	int TexWidth { get; }

	int TexHeight { get; }

	Point TextureArrayIndexToMatrix(int index);

	void UpdateDrawTex();

	List<Texture2D> Clean();

	bool IsLineInPixel(Point p);

	bool IsPainted(Point pColor);

	Color ExpectedColorInPixel(Point p);

	Color32 CurrentColorInPixel(Point p);
}

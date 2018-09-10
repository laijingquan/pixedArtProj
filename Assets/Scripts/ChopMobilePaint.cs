// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChopMobilePaint
{
	public Point TextureArrayIndexToMatrix(int index)
	{
		Point result = default(Point);
		result.i = Mathf.FloorToInt((float)index / (float)this.texWidth);
		result.j = index - result.i * this.texWidth;
		return result;
	}

	public List<List<int>> zones;

	public byte[] pixels;

	public short[] source;

	public Color32 paintColor;

	public int texWidth;

	public int texHeight;
}

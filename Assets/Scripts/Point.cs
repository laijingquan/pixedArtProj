// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public struct Point
{
	public Point(int x, int y)
	{
		this.i = x;
		this.j = y;
	}

	public override string ToString()
	{
		return string.Format("({0},{1})", this.i, this.j);
	}

	public int i;

	public int j;
}

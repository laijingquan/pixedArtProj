// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public struct SaveStep
{
	public SaveStep(int id, Point p)
	{
		this.colorId = id;
		this.point = p;
	}

	public int colorId;

	public Point point;
}

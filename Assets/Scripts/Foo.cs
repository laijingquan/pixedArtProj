// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public class Foo
{
	public Foo()
	{
		this.bPlacement = BannerPlacement.Solved;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			this.val,
			" ",
			this.bPos,
			" ",
			this.bPlacement
		});
	}

	public int val;

	public BannerPosition bPos;

	public BannerPlacement bPlacement;
}

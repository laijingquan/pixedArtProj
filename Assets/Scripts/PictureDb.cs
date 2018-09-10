// dnSpy decompiler from Assembly-CSharp.dll
using System;
using SimpleSQL;

public class PictureDb
{
	[PrimaryKey]
	public int PicId { get; set; }

	[Default(0)]
	public int FillType { get; set; }

	[Default(0)]
	public int PackId { get; set; }

	[Default(0)]
	public int HasSave { get; set; }

	[Default(0)]
	public int Solved { get; set; }

	[Default(0)]
	public int PicClass { get; set; }
}

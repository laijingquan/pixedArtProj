// dnSpy decompiler from Assembly-CSharp.dll
using System;
using SimpleSQL;

public class SaveDb
{
	[PrimaryKey]
	public int PicId { get; set; }

	[Default(0)]
	public int PackId { get; set; }

	public string IconPath { get; set; }

	[Default(0)]
	public float TimeSpent { get; set; }

	[Default(0)]
	public long TimeStamp { get; set; }

	[Default(0)]
	public int Progress { get; set; }

	public string StepsJsonStr { get; set; }

	[Default(0)]
	public int HintsUsed { get; set; }
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using SimpleSQL;

public class GiftDb
{
	[PrimaryKey]
	public string GiftId { get; set; }

	[Default(0)]
	public long TimeStamp { get; set; }
}

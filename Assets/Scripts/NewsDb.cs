// dnSpy decompiler from Assembly-CSharp.dll
using System;
using SimpleSQL;

public class NewsDb
{
	[PrimaryKey]
	public int Id { get; set; }

	[Default(0)]
	public int NewsType { get; set; }
}

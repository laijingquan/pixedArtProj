// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public struct PaletteData
{
	public bool IsEmpty()
	{
		return this.entities == null || this.entities.Length == 0;
	}

	public PaletteEntity[] entities;
}

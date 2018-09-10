// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

[Serializable]
public class PackCollectionIndex
{
	public bool AddPackIndex(int packId)
	{
		if (this.packIds.Contains(packId))
		{
			return false;
		}
		this.packIds.Add(packId);
		return true;
	}

	public static string IndexToKey(int id)
	{
		return "p" + id + "Key";
	}

	public List<int> packIds = new List<int>();
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

[Serializable]
public class SysSyncContentResponce
{
	public bool IsValid()
	{
		if (string.IsNullOrEmpty(this.path))
		{
			return false;
		}
		if (this.content == null)
		{
			return false;
		}
		for (int i = 0; i < this.content.Count; i++)
		{
			if (!this.content[i].IsValid())
			{
				return false;
			}
		}
		return true;
	}

	public void PreparePathes()
	{
		for (int i = 0; i < this.content.Count; i++)
		{
			this.content[i].path = this.path;
		}
	}

	public string path;

	public List<SysPicData> content;
}

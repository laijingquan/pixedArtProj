// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public class BonusCodeResponse
{
	public bool IsValid()
	{
		if (this.paths == null || this.paths.Length < 1)
		{
			return false;
		}
		bool flag = false;
		for (int i = 0; i < this.paths.Length; i++)
		{
			if (string.IsNullOrEmpty(this.paths[i]))
			{
				this.paths[i] = string.Empty;
			}
			else
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (this.content == null)
		{
			return false;
		}
		for (int j = 0; j < this.content.Length; j++)
		{
			if (!this.content[j].IsValid())
			{
				return false;
			}
		}
		return true;
	}

	public string[] paths;

	public WebPicData[] content;
}

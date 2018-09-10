// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public class SysPicData
{
	public bool IsValid()
	{
		return !string.IsNullOrEmpty(this.lineart) && !string.IsNullOrEmpty(this.icon) && !string.IsNullOrEmpty(this.colored) && !string.IsNullOrEmpty(this.json);
	}

	public string path;

	public int id;

	public string lineart;

	public string icon;

	public string colored;

	public string json;
}

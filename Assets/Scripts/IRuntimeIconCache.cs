// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public interface IRuntimeIconCache
{
	void RecycleWeb(string key, Texture2D tex);

	void RecycleLocal(string key, Texture2D tex);

	void GetCopy(string key);
}

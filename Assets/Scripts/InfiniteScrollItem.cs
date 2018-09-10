// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public abstract class InfiniteScrollItem : MonoBehaviour
{
	public int Row { get; set; }

	public abstract void ReloadFailedTextures();

	public abstract void ReloadTextures();

	public abstract void UnloadTextures();

	protected abstract void Clean();
}

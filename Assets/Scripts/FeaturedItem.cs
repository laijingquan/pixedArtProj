// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public abstract class FeaturedItem : MonoBehaviour
{
	public FeaturedItem.ItemType Type { get; protected set; }

	public int Id { get; protected set; }

	public abstract void Reset();

	public abstract void ReloadTextures();

	public abstract void UnloadTextures();

	public abstract void ReloadFailedTextures();

	public enum ItemType
	{
		Daily,
		PromoPic,
		ExternalLink
	}
}

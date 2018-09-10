// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PaletterButton : MonoBehaviour
{
	public int Id { get; private set; }

	public Color Color { get; private set; }

	public bool Completed { get; private set; }

	public void Init(int id, Color color)
	{
		this.Id = id;
		this.Color = color;
		this.InternalInit(id, color);
	}

	protected virtual void InternalInit(int id, Color color)
	{
	}

	public virtual void MarkComplete()
	{
		this.Completed = true;
	}

	public virtual void Select()
	{
	}

	public virtual void Deselect()
	{
	}

	protected static int Brightness(Color c)
	{
		return (int)Mathf.Sqrt(c.r * 255f * 255f * c.r * 0.29f + c.g * 255f * 255f * c.g * 0.58f + c.b * 255f * 255f * c.b * 0.114f);
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LocalIconTask
{
	public LocalIconTask(string path, Action<bool, Texture2D> h)
	{
		this.IsCanceled = false;
		this.Completed = false;
		this.Path = path;
		this.handler = h;
	}

	public string Path { get; private set; }

	public bool IsCanceled { get; private set; }

	public bool Completed { get; private set; }

	public void Cancel()
	{
		this.IsCanceled = true;
		this.handler = null;
	}

	public bool IsRunning
	{
		get
		{
			return !this.IsCanceled && !this.Completed;
		}
	}

	public void Result(bool success, Texture2D tex)
	{
		this.Completed = true;
		if (this.handler != null)
		{
			this.handler(success, tex);
		}
		this.handler = null;
	}

	private Action<bool, Texture2D> handler;
}

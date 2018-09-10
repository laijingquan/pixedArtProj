// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
	 
	public event Action LoopStarted;

	public float AnimDuration { get; private set; }

	public bool IsPlaying
	{
		get
		{
			return this.animCoroutine != null;
		}
	}

	public bool IsPaused { get; private set; }

	public void Play()
	{
		if (this.IsPlaying)
		{
			this.Stop(true);
		}
		this.Init();
		this.IsPaused = false;
		this.stopOnNextLoop = false;
		this.animCoroutine = base.StartCoroutine(this.Anim());
	}

	public void Pause()
	{
		this.IsPaused = true;
	}

	public void Resume()
	{
		this.IsPaused = false;
	}

	public void Stop(bool immidiate = true)
	{
		if (immidiate)
		{
			if (this.animCoroutine != null)
			{
				base.StopCoroutine(this.animCoroutine);
				this.animCoroutine = null;
				this.stopOnNextLoop = false;
				this.img.sprite = this.sprites[0];
			}
		}
		else
		{
			this.stopOnNextLoop = true;
		}
		this.IsPaused = false;
	}

	private void Init()
	{
		if (this.inited)
		{
			return;
		}
		this.inited = true;
		this.AnimDuration = (float)this.sprites.Length * (1f / (float)this.animTargetFps);
		this.step = this.AnimDuration / (float)this.sprites.Length;
	}

	private void OnEnable()
	{
		if (!this.ManualControl)
		{
			this.Play();
		}
	}

	private void OnDisable()
	{
		this.Stop(true);
	}

	private IEnumerator Anim()
	{
		do
		{
			if (this.LoopStarted != null)
			{
				this.LoopStarted();
			}
			for (int i = 0; i < this.sprites.Length; i++)
			{
				if (this.IsPaused)
				{
					yield return null;
				}
				else
				{
					this.img.sprite = this.sprites[i];
					yield return new WaitForSeconds(this.step);
				}
			}
			if (this.idleTime > 0f)
			{
				yield return new WaitForSeconds(this.idleTime);
			}
		}
		while (!this.stopOnNextLoop);
		this.Stop(true);
		//yield break;
	}

	public bool ManualControl;

	public int animTargetFps = 30;

	public float idleTime;

	private bool stopOnNextLoop;

	[SerializeField]
	private Sprite[] sprites;

	[SerializeField]
	private Image img;

	private float step;

	private bool inited;

	private Coroutine animCoroutine;
}

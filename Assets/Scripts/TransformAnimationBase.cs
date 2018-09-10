// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public abstract class TransformAnimationBase : MonoBehaviour
{
	public bool IsPlaying
	{
		get
		{
			return this.animCoroutine != null;
		}
	}

	public bool IsPaused { get; private set; }

	public void Play(bool isOneShoot = false)
	{
		if (!this.inited)
		{
			this.Init(this.animDuration, this.idleTime);
			UnityEngine.Debug.Log("RectTransformAnimation");
		}
		if (this.IsPlaying)
		{
			this.Stop();
		}
		this.oneShoot = isOneShoot;
		this.IsPaused = false;
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

	public void Stop()
	{
		if (this.animCoroutine != null)
		{
			base.StopCoroutine(this.animCoroutine);
			this.animCoroutine = null;
		}
		this.IsPaused = false;
	}

	public void Init(float duration, float idle)
	{
		this.idleTime = idle;
		this.animDuration = duration;
		this.inited = true;
	}

	private void OnEnable()
	{
		if (!this.ManualControl)
		{
			this.Play(false);
		}
	}

	private void OnDisable()
	{
		this.Stop();
	}

	private IEnumerator Anim()
	{
		if (this.oneShoot)
		{
			yield return this.DoStaff();
			yield break;
		}
		for (;;)
		{
			yield return this.DoStaff();
			if (this.idleTime > 0f)
			{
				yield return new WaitForSeconds(this.idleTime);
			}
		}
	}

	protected abstract IEnumerator DoStaff();

	public bool ManualControl;

	[SerializeField]
	private float idleTime;

	[SerializeField]
	protected float animDuration;

	private bool oneShoot;

	private bool inited;

	private Coroutine animCoroutine;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class ConnectionResumeHelper : MonoBehaviour
{
	public void SafeResume(Action action)
	{
		if (this.currentAttempt == 0)
		{
			this.currentAttempt++;
			action();
		}
		else
		{
			if ((DateTime.UtcNow - this.lastAttemp).TotalSeconds > 5.0)
			{
				this.Reset();
			}
			else
			{
				this.StepInterval();
			}
			if (this.reloadCr == null)
			{
				this.reloadCr = base.StartCoroutine(this.DelayAction(this.interval, action));
			}
		}
		this.lastAttemp = DateTime.UtcNow;
	}

	private void Reset()
	{
		this.currentAttempt = 0;
		this.interval = 0f;
	}

	private void StepInterval()
	{
		this.currentAttempt++;
		if (this.currentAttempt > 2)
		{
			this.interval += 0.5f;
			this.interval = Mathf.Clamp(this.interval, 0f, 5f);
		}
	}

	private IEnumerator DelayAction(float delay, Action a)
	{
		yield return new WaitForSeconds(delay);
		a();
		this.reloadCr = null;
		yield break;
	}

	private int currentAttempt;

	private const int initialAttemps = 2;

	private DateTime lastAttemp = DateTime.MinValue;

	private Coroutine reloadCr;

	private float interval;
}

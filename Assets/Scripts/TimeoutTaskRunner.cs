// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class TimeoutTaskRunner : MonoBehaviour
{
	public static TimeoutTaskRunner Instance
	{
		get
		{
			if (TimeoutTaskRunner.instance == null)
			{
				GameObject gameObject = new GameObject("TimeoutTaskRunner");
				TimeoutTaskRunner.instance = gameObject.AddComponent<TimeoutTaskRunner>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			return TimeoutTaskRunner.instance;
		}
	}

	public Coroutine Run(float delay, Action callback)
	{
		return base.StartCoroutine(this.DelayAction(delay, callback));
	}

	public void StopHandler(Coroutine coroutine)
	{
		if (coroutine != null)
		{
			base.StopCoroutine(coroutine);
		}
	}

	private IEnumerator DelayAction(float delay, Action callback)
	{
		yield return new WaitForSeconds(delay);
		if (callback != null)
		{
			callback();
		}
		yield break;
	}

	private static TimeoutTaskRunner instance;
}

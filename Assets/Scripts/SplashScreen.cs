// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.DelayLoad());
	}

	private IEnumerator DelayLoad()
	{
		AsyncOperation ao = SceneManager.LoadSceneAsync("start", LoadSceneMode.Additive);
		ao.allowSceneActivation = false;
		base.StartCoroutine(this.Anim(ao));
		yield return ao;
		yield break;
	}

	private IEnumerator Anim(AsyncOperation ao)
	{
		for (;;)
		{
			UnityEngine.Debug.Log("start spin");
			if (ao != null)
			{
				ao.allowSceneActivation = false;
			}
			yield return this.Spin(this.icon.transform, 0.5f);
			UnityEngine.Debug.Log("spin done");
			if (ao != null)
			{
				ao.allowSceneActivation = true;
			}
			yield return new WaitForSeconds(0.5f);
		}

	}

	private IEnumerator Spin(Transform tr, float animDuration)
	{
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			tr.rotation = Quaternion.Euler(0f, 0f, i * 360f);
			yield return 0;
		}
		tr.rotation = Quaternion.identity;
		yield break;
	}

	public Image icon;
}

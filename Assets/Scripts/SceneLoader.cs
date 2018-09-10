// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	private void Start()
	{
		if (this.autoload)
		{
			this.Close();
		}
		SplashCanvas.Instance.CloseButtonClick += this.OnCloseClick;
	}

	private void OnDisable()
	{
		SplashCanvas.Instance.CloseButtonClick -= this.OnCloseClick;
	}

	public void ShowSlowConnectionLabel()
	{
		SplashCanvas.Instance.ShowError();
	}

	public void ShowCloseButton(Action clickCallback)
	{
		this.closeClickCallback = clickCallback;
		SplashCanvas.Instance.ShowErrorBtn();
	}

	public void Close()
	{
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(false, this.duration, false));
	}

	public void Open(bool showAnimation = false)
	{
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(true, this.duration, showAnimation));
	}

	public void LoadSceneImmediate(string sceneName)
	{
		if (!SplashCanvas.Instance.gameObject.activeSelf)
		{
			SplashCanvas.Instance.gameObject.SetActive(true);
		}
		SplashCanvas.Instance.InstantFade(true);
		SceneManager.LoadScene(sceneName);
	}

	public void LoadScene(string sceneName, Action callBackOnLoad, bool showAnimation = false)
	{
		base.StartCoroutine(this.SceneLoad(sceneName, callBackOnLoad, showAnimation));
	}

	private IEnumerator SceneLoad(string sceneName, Action callBackOnLoad, bool showAnimation = false)
	{
		yield return this.FadeCoroutine(true, this.duration, showAnimation);
		yield return null;
		if (callBackOnLoad != null)
		{
			callBackOnLoad();
		}
		SceneManager.LoadScene(sceneName);
		yield break;
	}

	protected IEnumerator FadeCoroutine(bool open, float animDuration, bool showAnimation = false)
	{
		if (open)
		{
			SplashCanvas.Instance.gameObject.SetActive(true);
		}
		yield return SplashCanvas.Instance.Fade(open, animDuration, showAnimation);
		if (!open)
		{
			SplashCanvas.Instance.gameObject.SetActive(false);
		}
		this.fadeCoroutine = null;
		yield break;
	}

	private void OnCloseClick()
	{
		if (this.closeClickCallback != null)
		{
			this.closeClickCallback();
		}
		this.closeClickCallback = null;
	}

	public float duration = 0.1f;

	public bool autoload = true;

	private Coroutine fadeCoroutine;

	private Action closeClickCallback;
}

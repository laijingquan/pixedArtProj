// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

internal class ZoomButton : MonoBehaviour
{
	public void Open()
	{
		if (this.opened)
		{
			return;
		}
		this.opened = true;
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(true, 0.15f));
	}

	public void Close()
	{
		if (!this.opened)
		{
			return;
		}
		this.opened = false;
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(false, 0.15f));
	}

	protected IEnumerator FadeCoroutine(bool open, float animDuration)
	{
		float from;
		float to;
		if (open)
		{
			from = this.canvas.alpha;
			to = 1f;
		}
		else
		{
			from = this.canvas.alpha;
			to = 0f;
		}
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			this.canvas.alpha = Mathf.Lerp(from, to, i);
			yield return 0;
		}
		this.canvas.alpha = to;
		if (open)
		{
			this.canvas.interactable = true;
			this.canvas.blocksRaycasts = true;
		}
		else
		{
			this.canvas.interactable = false;
			this.canvas.blocksRaycasts = false;
		}
		yield return 0;
		this.fadeCoroutine = null;
		yield break;
	}

	[SerializeField]
	private CanvasGroup canvas;

	private Coroutine fadeCoroutine;

	private bool opened;
}

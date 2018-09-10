// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class FadeView : FMPopup
{
	protected override void StartOpenAnimation()
	{
		base.StartOpenAnimation();
		if (this.turnOnOffCanvas)
		{
			this.canvasGroup.gameObject.SetActive(true);
		}
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(0f, 1f, this.duration, this.delay, this.canvasGroup));
	}

	protected override void StartClosingAnimation()
	{
		base.StartClosingAnimation();
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(1f, 0f, this.duration, 0f, this.canvasGroup));
	}

	protected IEnumerator FadeCoroutine(float from, float to, float animDuration, float d, CanvasGroup canvas)
	{
		if (d > 0f)
		{
			yield return new WaitForSeconds(d);
		}
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			canvas.alpha = Mathf.Lerp(from, to, i);
			yield return 0;
		}
		canvas.alpha = to;
		if ((double)Mathf.Abs(to) < 0.01 && this.turnOnOffCanvas && this.turnOnOffCanvas)
		{
			this.canvasGroup.gameObject.SetActive(false);
		}
		this.fadeCoroutine = null;
		yield break;
	}

	public CanvasGroup canvasGroup;

	public bool turnOnOffCanvas;

	public float duration;

	public float delay;

	private Coroutine fadeCoroutine;
}

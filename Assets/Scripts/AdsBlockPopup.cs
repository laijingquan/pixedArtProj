// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class AdsBlockPopup : FMPopup
{
	public void SetBlockMode(bool blockView)
	{
		this.isFullViewOverlay = blockView;
	}

	protected override void StartOpenAnimation()
	{
		base.StartOpenAnimation();
		if (this.turnOnOffCanvas)
		{
			this.canvasGroup.gameObject.SetActive(true);
		}
		if (this.isFullViewOverlay)
		{
			this.canvasGroup.alpha = 1f;
		}
		else
		{
			this.canvasGroup.alpha = 0f;
		}
	}

	protected override void StartClosingAnimation()
	{
		base.StartClosingAnimation();
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		if (this.isFullViewOverlay)
		{
			this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(1f, 0f, this.duration, this.showTime, this.canvasGroup));
		}
		else
		{
			this.canvasGroup.gameObject.SetActive(false);
		}
	}

	private IEnumerator FadeCoroutine(float from, float to, float animDuration, float d, CanvasGroup canvas)
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

	private bool isFullViewOverlay = true;

	private Coroutine fadeCoroutine;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class FadeSlideView : FMPopup
{
	protected override void StartOpenAnimation()
	{
		base.StartOpenAnimation();
		if (this.turnOnOffCanvas)
		{
			this.canvasGroup.gameObject.SetActive(true);
		}
		base.StartCoroutine(this.FadeCoroutine(0f, 1f, this.duration, this.delay, this.canvasGroup));
		base.StartCoroutine(this.MoveCoroutine((!(this.mRt != null)) ? ((RectTransform)base.transform) : this.mRt, this.closedPosition, this.openPosition, this.duration, this.delay));
	}

	protected override void StartClosingAnimation()
	{
		base.StartClosingAnimation();
		base.StartCoroutine(this.FadeCoroutine(1f, 0f, this.duration, 0f, this.canvasGroup));
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
		yield break;
	}

	protected IEnumerator MoveCoroutine(RectTransform rt, Vector2 from, Vector2 to, float animDuration, float d = 0f)
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
			rt.anchoredPosition = Vector2.LerpUnclamped(from, to, this.curve.Evaluate(i));
			yield return 0;
		}
		rt.anchoredPosition = Vector2.Lerp(from, to, 1f);
		yield break;
	}

	public RectTransform mRt;

	public CanvasGroup canvasGroup;

	public bool turnOnOffCanvas;

	public AnimationCurve curve;

	public Vector2 closedPosition;

	public Vector2 openPosition;

	public float duration;

	public float delay;
}

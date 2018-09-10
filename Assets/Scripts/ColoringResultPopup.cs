// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class ColoringResultPopup : FMPopup
{
	protected override void StartOpenAnimation()
	{
		base.StartOpenAnimation();
		this.root.gameObject.SetActive(true);
		this.background.gameObject.SetActive(false);
		this.canvasGroup.alpha = 0f;
		if (this.scaleCoroutine != null)
		{
			base.StopCoroutine(this.scaleCoroutine);
		}
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.WillBecomeVisible();
		this.mRt.localScale = new Vector3(this.scaleFrom, this.scaleFrom, 1f);
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(0f, 1f, this.fadeDuration, this.fadeDelay, this.canvasGroup));
		this.scaleCoroutine = base.StartCoroutine(this.ScaleCoroutine(this.mRt, this.scaleFrom, 1f, this.scaleDuration, this.scaleDelay));
	}

	protected override void StartClosingAnimation()
	{
		base.StartClosingAnimation();
		if (this.scaleCoroutine != null)
		{
			base.StopCoroutine(this.scaleCoroutine);
		}
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.WillBecomeInvisable();
		this.mRt.localScale = Vector3.one;
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(1f, 0f, this.fadeDuration, 0f, this.canvasGroup));
	}

	protected virtual void WillBecomeVisible()
	{
	}

	protected virtual void BecomeVisable()
	{
	}

	protected virtual void WillBecomeInvisable()
	{
	}

	protected virtual void BecomeInvisable()
	{
	}

	private IEnumerator FadeCoroutine(float from, float to, float animDuration, float d, CanvasGroup canvas)
	{
		yield return 0;
		if (d > 0f)
		{
			yield return new WaitForSeconds(d);
		}
		if ((double)Mathf.Abs(to) > 0.98)
		{
			this.background.gameObject.SetActive(true);
		}
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			canvas.alpha = Mathf.Lerp(from, to, this.fadeCurve.Evaluate(i));
			yield return 0;
		}
		canvas.alpha = to;
		if ((double)Mathf.Abs(to) < 0.01)
		{
			this.BecomeInvisable();
			this.root.gameObject.SetActive(false);
			this.background.gameObject.SetActive(false);
		}
		if (to > 0.98f)
		{
			this.BecomeVisable();
		}
		this.fadeCoroutine = null;
		yield break;
	}

	private IEnumerator ScaleCoroutine(RectTransform rt, float from, float to, float animDuration, float d = 0f)
	{
		yield return null;
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
			float s = Mathf.LerpUnclamped(from, to, this.curve.Evaluate(i));
			rt.localScale = new Vector3(s, s, 1f);
			yield return 0;
		}
		rt.localScale = new Vector3(to, to, 1f);
		this.scaleCoroutine = null;
		yield break;
	}

	public RectTransform background;

	public RectTransform root;

	public RectTransform mRt;

	public CanvasGroup canvasGroup;

	public bool turnOnOffCanvas;

	public AnimationCurve fadeCurve;

	public AnimationCurve curve;

	public float fadeDuration = 0.2f;

	public float fadeDelay;

	public float scaleDelay;

	public float scaleDuration = 0.3f;

	public float scaleFrom = 0.5f;

	private Coroutine fadeCoroutine;

	private Coroutine scaleCoroutine;
}

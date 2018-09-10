// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class ContexPopup : FMPopup
{
	protected override void StartOpenAnimation()
	{
		base.StartOpenAnimation();
		if (this.turnOnOffCanvas)
		{
			this.canvasGroup.gameObject.SetActive(true);
		}
		this.canvasGroup.alpha = 0f;
		this.mRt.anchoredPosition = new Vector2(this.mRt.anchoredPosition.x, Mathf.Clamp(this.refHeight.anchoredPosition.y, 0f, (float)this.refHeightOffset.SectionHeight) - (float)this.refHeightOffset.SectionHeight);
		if (this.scaleCoroutine != null)
		{
			base.StopCoroutine(this.scaleCoroutine);
		}
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(0f, 1f, this.duration, this.delay, this.canvasGroup));
		this.scaleCoroutine = base.StartCoroutine(this.ScaleCoroutine(this.mRt, this.mRt.localScale.x, 1f, this.duration, this.delay));
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
		this.mRt.localScale = Vector3.one;
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(1f, 0f, this.duration, 0f, this.canvasGroup));
		this.scaleCoroutine = base.StartCoroutine(this.ScaleCoroutine(this.mRt, this.mRt.localScale.x, 0f, this.duration, this.delay));
	}

	protected IEnumerator FadeCoroutine(float from, float to, float animDuration, float d, CanvasGroup canvas)
	{
		yield return 0;
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

	protected IEnumerator ScaleCoroutine(RectTransform rt, float from, float to, float animDuration, float d = 0f)
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
			float s = Mathf.LerpUnclamped(from, to, this.curve.Evaluate(i));
			rt.localScale = new Vector3(s, s, 1f);
			yield return 0;
		}
		rt.localScale = new Vector3(to, to, 1f);
		this.scaleCoroutine = null;
		yield break;
	}

	public RectTransform mRt;

	public CanvasGroup canvasGroup;

	public bool turnOnOffCanvas;

	public AnimationCurve curve;

	public float duration;

	public float delay;

	private Coroutine fadeCoroutine;

	private Coroutine scaleCoroutine;

	[SerializeField]
	private FeaturedMenuBar refHeightOffset;

	[SerializeField]
	private RectTransform refHeight;
}

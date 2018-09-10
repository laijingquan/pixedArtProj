// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class SlidePoppup : FMPopup
{
	protected override void StartOpenAnimation()
	{
		base.StartOpenAnimation();
		this.root.gameObject.SetActive(true);
		this.WillBecomeVisible();
		base.StartCoroutine(this.FrameDelay(delegate
		{
			base.StartCoroutine(this.FadeCoroutine(0f, 1f, this.duration, this.delay, this.canvasGroup));
			base.StartCoroutine(this.MoveCoroutine(this.mRt, this.closedPosition, this.openedPosition, this.duration, 0f));
		}));
	}

	protected override void StartClosingAnimation()
	{
		base.StartClosingAnimation();
		this.WillBecomeInvisable();
		base.StartCoroutine(this.FadeCoroutine(1f, 0f, this.duration, 0f, this.canvasGroup));
		base.StartCoroutine(this.MoveCoroutine(this.mRt, this.mRt.anchoredPosition, this.closedPosition, this.duration, 0f));
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

	private IEnumerator FrameDelay(Action a)
	{
		yield return 0;
		a();
		yield break;
	}

	private IEnumerator FadeCoroutine(float from, float to, float animDuration, float d, CanvasGroup canvas)
	{
		if (to < 0.01f)
		{
			canvas.interactable = false;
		}
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
		if (to > 0.98f)
		{
			canvas.interactable = true;
			this.BecomeVisable();
		}
		if (to < 0.01f)
		{
			this.BecomeInvisable();
			this.root.gameObject.SetActive(false);
		}
		yield break;
	}

	private IEnumerator MoveCoroutine(RectTransform rt, Vector2 from, Vector2 to, float animDuration, float d = 0f)
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

	[SerializeField]
	protected Vector2 closedPosition;

	[SerializeField]
	protected Vector2 openedPosition;

	public RectTransform root;

	public RectTransform mRt;

	public CanvasGroup canvasGroup;

	public AnimationCurve curve;

	public float duration;

	public float delay;
}

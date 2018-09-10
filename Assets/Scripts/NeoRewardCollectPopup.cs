// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NeoRewardCollectPopup : MonoBehaviour
{
	public void Prepare(int amount)
	{
		this.label.text = "+" + amount;
	}

	private void OnEnable()
	{
		this.bulbRt.anchoredPosition = Vector2.zero;
		this.bulbRt.localScale = Vector2.zero;
		base.StartCoroutine(this.Anim());
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	private IEnumerator Anim()
	{
		yield return new WaitForSeconds(1f);
		yield return this.Appear();
		yield return new WaitForSeconds(0.1f);
		yield return this.MoveScaleCoroutine(this.bulbRt, 1f, 0f, this.moveTime, this.moveCurve, 0f);
		this.popupManager.CloseActive();
		this.hintBtn.RewardReceived();
		yield break;
	}

	private IEnumerator Appear()
	{
		base.StartCoroutine(this.ScaleCoroutine(this.bulbRt, 0f, 1f, this.appearTime, this.appearCurve, 0f));
		yield return base.StartCoroutine(this.FadeCoroutine(this.bulbCanvas, 0f, 1f, this.appearTime, 0f));
		yield break;
	}

	private IEnumerator MoveScaleCoroutine(RectTransform rt, float from, float to, float animDuration, AnimationCurve curve, float d = 0f)
	{
		if (d > 0f)
		{
			yield return new WaitForSeconds(d);
		}
		float i = 0f;
		float currentTime = 0f;
		Vector2 posFrom = rt.anchoredPosition;
		Vector2 posTo = this.switchToRectTransform(rt, this.hintBtn.transform as RectTransform);
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			float s = Mathf.LerpUnclamped(from, to, i);
			rt.localScale = new Vector3(s, s, 1f);
			rt.anchoredPosition = Vector2.LerpUnclamped(posFrom, posTo, curve.Evaluate(i));
			yield return 0;
		}
		rt.localScale = new Vector3(to, to, 1f);
		rt.anchoredPosition = posTo;
		yield break;
	}

	private IEnumerator DelayAction(float delay, Action a)
	{
		yield return new WaitForSeconds(delay);
		a();
		yield break;
	}

	protected IEnumerator FadeCoroutine(CanvasGroup canvas, float from, float to, float animDuration, float d = 0f)
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
		yield break;
	}

	private IEnumerator ScaleCoroutine(RectTransform rt, float from, float to, float animDuration, AnimationCurve curve, float d = 0f)
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
			float s = Mathf.LerpUnclamped(from, to, curve.Evaluate(i));
			rt.localScale = new Vector3(s, s, 1f);
			yield return 0;
		}
		rt.localScale = new Vector3(to, to, 1f);
		yield break;
	}

	private Vector2 switchToRectTransform(RectTransform from, RectTransform to)
	{
		RectTransform rectTransform = (RectTransform)from.parent;
		Vector2 result = new Vector2(rectTransform.rect.width / 2f - Mathf.Abs(to.anchoredPosition.x), rectTransform.rect.height / 2f - Mathf.Abs(to.anchoredPosition.y));
		return result;
	}

	[SerializeField]
	private Text label;

	[SerializeField]
	private HintBtn hintBtn;

	[SerializeField]
	private GamePopupManager popupManager;

	[SerializeField]
	private float appearTime = 0.8f;

	[SerializeField]
	private float moveTime = 0.4f;

	[SerializeField]
	private AnimationCurve appearCurve;

	[SerializeField]
	private AnimationCurve moveCurve;

	[SerializeField]
	private CanvasGroup bulbCanvas;

	[SerializeField]
	private RectTransform bulbRt;
}

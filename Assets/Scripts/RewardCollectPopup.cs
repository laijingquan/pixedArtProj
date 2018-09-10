// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardCollectPopup : MonoBehaviour
{
	public void Prepare(int amount)
	{
		this.label.text = "+" + amount;
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.Anim());
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	private IEnumerator Anim()
	{
		this.bulbRt.anchoredPosition = Vector2.zero;
		this.bulbRt.localScale = Vector2.one;
		this.raysRt.GetComponent<CanvasGroup>().alpha = 1f;
		base.StartCoroutine(this.Spin(this.raysRt));
		base.StartCoroutine(this.DelayAction(1.41f, delegate
		{
			this.popupManager.CloseActive();
		}));
		yield return this.MoveScaleCoroutine(this.bulbRt, 1f, 0f, 0.25f, 1f);
		this.hintBtn.RewardReceived();
		yield break;
	}

	private IEnumerator MoveScaleCoroutine(RectTransform rt, float from, float to, float animDuration, float d = 0f)
	{
		if (d > 0f)
		{
			yield return new WaitForSeconds(d);
		}
		float i = 0f;
		float currentTime = 0f;
		base.StartCoroutine(this.FadeCoroutine(this.raysRt.GetComponent<CanvasGroup>(), 1f, 0f, 0.2f, 0f));
		Vector2 posFrom = rt.anchoredPosition;
		Vector2 posTo = new Vector2(((RectTransform)rt.parent).rect.size.x / 2f, ((RectTransform)rt.parent).rect.size.y / 2f);
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			float s = Mathf.Lerp(from, to, i);
			rt.localScale = new Vector3(s, s, 1f);
			rt.anchoredPosition = Vector2.Lerp(posFrom, posTo, i * i);
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

	private IEnumerator Spin(RectTransform rt)
	{
		for (;;)
		{
			rt.Rotate(Vector3.forward, this.raySpeed * Time.deltaTime);
			yield return 0;
		}
		//yield break;
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

	[SerializeField]
	private Text label;

	[SerializeField]
	private RectTransform raysRt;

	[SerializeField]
	private float raySpeed = 100f;

	[SerializeField]
	private HintBtn hintBtn;

	[SerializeField]
	private GamePopupManager popupManager;

	[SerializeField]
	private RectTransform bulbRt;
}

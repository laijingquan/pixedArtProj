// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NeoPaletterButton : PaletterButton
{
	protected override void InternalInit(int id, Color color)
	{
		this.label.text = (id + 1).ToString();
		this.label.color = ((PaletterButton.Brightness(color) <= 130) ? Color.white : Color.black);
		this.imgColor.color = color;
		this.completeMask.color = color;
	}

	public override void MarkComplete()
	{
		base.MarkComplete();
		this.label.gameObject.SetActive(false);
		this.imgColor.gameObject.SetActive(false);
		this.selectorRt.gameObject.SetActive(false);
		this.completeMask.gameObject.SetActive(true);
	}

	public override void Select()
	{
		if (this.selectCoroutine != null)
		{
			base.StopCoroutine(this.selectCoroutine);
		}
		this.selectCoroutine = base.StartCoroutine(this.ScaleCoroutine(this.selectorRt, this.selectorRt.localScale.x, 1f, this.selectTime, 0f));
	}

	public override void Deselect()
	{
		if (this.selectCoroutine != null)
		{
			base.StopCoroutine(this.selectCoroutine);
		}
		this.selectCoroutine = base.StartCoroutine(this.ScaleCoroutine(this.selectorRt, this.selectorRt.localScale.x, 0.8f, this.selectTime, 0f));
	}

	private IEnumerator ScaleCoroutine(RectTransform rt, float from, float to, float animDuration, float d = 0f)
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
			float s = Mathf.LerpUnclamped(from, to, i);
			rt.localScale = new Vector3(s, s, 1f);
			yield return 0;
		}
		rt.localScale = new Vector3(to, to, 1f);
		this.selectCoroutine = null;
		yield break;
	}

	[SerializeField]
	private RectTransform selectorRt;

	[SerializeField]
	private Text label;

	[SerializeField]
	private Image completeMask;

	[SerializeField]
	private Image imgColor;

	private float selectTime = 0.3f;

	private Coroutine selectCoroutine;
}

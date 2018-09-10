// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartContexPopup : FMPopup
{
	protected override void StartOpenAnimation()
	{
		base.StartOpenAnimation();
		if (this.turnOnOffCanvas)
		{
			this.canvasGroup.gameObject.SetActive(true);
		}
		this.canvasGroup.alpha = 0f;
		if (this.scaleCoroutine != null)
		{
			base.StopCoroutine(this.scaleCoroutine);
		}
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(0f, 1f, this.duration, this.delay, this.canvasGroup));
		this.scaleCoroutine = base.StartCoroutine(this.ScaleCoroutine(this.mRt, this.scaleFrom, 1f, this.duration, this.delay));
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
		if (this.iconUpdate != null)
		{
			base.StopCoroutine(this.iconUpdate);
		}
		this.mRt.localScale = Vector3.one;
		this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(1f, 0f, this.duration, 0f, this.canvasGroup));
		this.scaleCoroutine = base.StartCoroutine(this.ScaleCoroutine(this.mRt, 1f, this.scaleFrom, this.duration, this.delay));
	}

	private IEnumerator FadeCoroutine(float from, float to, float animDuration, float d, CanvasGroup canvas)
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
			float s = Mathf.LerpUnclamped(from, to, this.curve.Evaluate(i));
			rt.localScale = new Vector3(s, s, 1f);
			yield return 0;
		}
		rt.localScale = new Vector3(to, to, 1f);
		this.scaleCoroutine = null;
		yield break;
	}

	public void SetIcon(PicItem item)
	{
		this.icon.texture = item.GetIconTex();
		this.save.texture = item.GetSaveTex();
		this.iconUpdate = base.StartCoroutine(this.IconUpdate(item));
	}

	public void SetButtons(bool cont, bool rest, bool del)
	{
		this.buttons[0].SetActive(cont);
		this.buttons[1].SetActive(rest);
		this.buttons[2].SetActive(del);
		int num = 0;
		if (cont)
		{
			num += this.buttonOffset;
		}
		if (rest)
		{
			num += this.buttonOffset;
		}
		if (del)
		{
			num += this.buttonOffset;
		}
		this.content.sizeDelta = new Vector2(this.content.sizeDelta.x, (float)(this.emptyHeight + num));
	}

	private IEnumerator IconUpdate(PicItem item)
	{
		bool hasIcon = this.icon.texture != null;
		bool hasSave = this.save.texture != null;
		while (!hasIcon || !hasSave)
		{
			if (!hasIcon)
			{
				Texture iconTex = item.GetIconTex();
				if (iconTex != null)
				{
					this.icon.texture = iconTex;
					hasIcon = true;
				}
			}
			if (!hasSave)
			{
				Texture saveTex = item.GetSaveTex();
				if (saveTex != null)
				{
					this.save.texture = saveTex;
					hasSave = true;
				}
			}
			this.iconUpdate = null;
			yield return 0;
		}
		yield break;
	}

	public RawImage icon;

	public RawImage save;

	public GameObject[] buttons;

	public int buttonOffset = 140;

	public int emptyHeight = 550;

	public RectTransform content;

	public RectTransform mRt;

	public CanvasGroup canvasGroup;

	public bool turnOnOffCanvas;

	public AnimationCurve curve;

	public float duration;

	public float delay;

	public float scaleFrom;

	private Coroutine fadeCoroutine;

	private Coroutine scaleCoroutine;

	private Coroutine iconUpdate;
}

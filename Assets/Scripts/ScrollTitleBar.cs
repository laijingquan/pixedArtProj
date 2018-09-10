// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class ScrollTitleBar : MonoBehaviour
{
	public float InitialOffset
	{
		get
		{
			return this.initialPos.y;
		}
	}

	public void AddTopOffset(int offset)
	{
		this.initialPos += new Vector2(0f, (float)(-(float)offset));
	}

	private void Awake()
	{
		this.rt = (RectTransform)base.transform;
		this.outsideParent = base.transform.parent.parent;
		this.initialPos = this.rt.anchoredPosition;
	}

	private void Update()
	{
		if ((this.initialPos + this.scrollContent.anchoredPosition).y > 0f)
		{
			if (this.isScrollable)
			{
				this.isScrollable = false;
				this.rt.SetParent(this.outsideParent);
				this.rt.anchoredPosition = Vector2.zero;
				if (this.fadeCoroutine != null)
				{
					base.StopCoroutine(this.fadeCoroutine);
				}
				this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(this.backbroundCanvas.alpha, 1f, 0.15f, 0f, this.backbroundCanvas));
			}
		}
		else if (!this.isScrollable)
		{
			this.isScrollable = true;
			this.rt.SetParent(this.scrollContent);
			this.rt.anchoredPosition = this.initialPos;
			if (this.fadeCoroutine != null)
			{
				base.StopCoroutine(this.fadeCoroutine);
			}
			this.fadeCoroutine = base.StartCoroutine(this.FadeCoroutine(this.backbroundCanvas.alpha, 0f, 0.15f, 0f, this.backbroundCanvas));
		}
	}

	private IEnumerator Parent()
	{
		Transform currentParent = this.rt.parent;
		this.rt.SetParent(this.scrollContent);
		bool rected = true;
		for (;;)
		{
			if ((this.initialPos + this.scrollContent.anchoredPosition).y > 0f)
			{
				if (rected)
				{
					rected = false;
					this.rt.SetParent(currentParent);
					this.rt.anchoredPosition = Vector2.zero;
				}
			}
			else if (!rected)
			{
				rected = true;
				this.rt.SetParent(this.scrollContent);
				this.rt.anchoredPosition = this.initialPos;
			}
			yield return 0;
		}
	}

	private IEnumerator Pos()
	{
		for (;;)
		{
			Vector2 pos = this.initialPos + this.scrollContent.anchoredPosition;
			if (pos.y > 0f)
			{
				pos.y = 0f;
			}
			this.rt.anchoredPosition = pos;
			yield return new WaitForEndOfFrame();
		}
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
		this.fadeCoroutine = null;
		yield break;
	}

	[SerializeField]
	private CanvasGroup backbroundCanvas;

	[SerializeField]
	private RectTransform scrollContent;

	private RectTransform rt;

	private Vector2 initialPos;

	private Transform outsideParent;

	private bool isScrollable = true;

	private Coroutine fadeCoroutine;
}

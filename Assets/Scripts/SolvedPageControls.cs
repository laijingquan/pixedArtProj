// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class SolvedPageControls : FMPopup
{
	public void SetSafeLayoutOffset(BannerPosition bannerPosition, int topOffset, int bottomOffset)
	{
		if (bannerPosition != BannerPosition.Bottom)
		{
			if (bannerPosition == BannerPosition.Top)
			{
				this.topControls.anchoredPosition += new Vector2(0f, (float)(-(float)topOffset));
				for (int i = 0; i < this.images.Length; i++)
				{
					this.images[i].anchoredPosition += new Vector2(0f, (float)(-(float)topOffset));
				}
			}
		}
		else
		{
			this.bottomControls.anchoredPosition += new Vector2(0f, (float)bottomOffset);
			this.bottomControlsPreAnim.anchoredPosition += new Vector2(0f, (float)bottomOffset);
		}
		if (((RectTransform)base.transform).rect.height < 1630f)
		{
			for (int j = 0; j < this.images.Length; j++)
			{
				this.images[j].sizeDelta = new Vector2(800f, 800f);
			}
		}
	}

	public void SetSafeLayoutExtraBottomOffset(int extraOffset)
	{
		this.bottomControls.anchoredPosition += new Vector2(0f, (float)extraOffset);
		this.bottomControlsPreAnim.anchoredPosition += new Vector2(0f, (float)extraOffset);
	}

	protected override void StartOpenAnimation()
	{
		base.StartOpenAnimation();
		this.canvasGroup.gameObject.SetActive(true);
		this.canvasGroup.alpha = 0f;
		base.StartCoroutine(this.FadeCoroutine(0f, 1f, this.duration, this.delay, this.canvasGroup));
	}

	protected override void StartClosingAnimation()
	{
		base.StartClosingAnimation();
		this.canvasGroup.gameObject.SetActive(false);
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
		yield break;
	}

	public CanvasGroup canvasGroup;

	public float duration;

	public float delay;

	public RectTransform topControls;

	public RectTransform[] images;

	public RectTransform bottomControls;

	public RectTransform bottomControlsPreAnim;
}

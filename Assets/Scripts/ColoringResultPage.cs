// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoringResultPage : MonoBehaviour, ISolvePage
{
	public bool IsAnimating
	{
		get
		{
			return this.coloringAnimation.IsAnimating;
		}
	}

	public bool IsOpened { get; private set; }

	public void Clean()
	{
		this.coloringAnimation.Clean();
		if (this.texForShare != null)
		{
			UnityEngine.Object.Destroy(this.texForShare);
			this.texForShare = null;
		}
	}

	public void SetData(List<SaveStep> saveSteps, IPaintFill pFill, Func<int, Color> idToColor)
	{
		this.coloringAnimation.Init(saveSteps, pFill, idToColor);
	}

	public void Replay()
	{
		this.isReplay = true;
		this.coloringAnimation.Replay();
		this.HideControls();
	}

	public void CutShareableTexture(Action<Texture2D> callback)
	{
		if (this.texForShare == null)
		{
			Texture2D texture2D = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.wrapMode = TextureWrapMode.Clamp;
			ImageManager.Instance.ShareTexture(this.coloringAnimation.DrawTex, this.coloringAnimation.LineTex, texture2D);
			this.texForShare = texture2D;
		}
		callback(this.texForShare);
	}

	public Vector2 GetBrnPosNormilized()
	{
		Transform parent = this.shareBtnRt.parent;
		this.shareBtnRt.SetParent(this.mainAppRt);
		Vector2 result = this.shareBtnRt.anchoredPosition + new Vector2(0f, this.shareBtnRt.rect.height / 2f);
		this.shareBtnRt.SetParent(parent);
		float height = this.mainAppRt.rect.height;
		float width = this.mainAppRt.rect.width;
		float y;
		if (result.y > 0f)
		{
			y = (height / 2f - result.y) / height;
		}
		else
		{
			y = (height / 2f + Mathf.Abs(result.y)) / height;
		}
		float x;
		if (result.x > 0f)
		{
			x = (width / 2f + result.x) / width;
		}
		else
		{
			x = (width / 2f - Mathf.Abs(result.x)) / width;
		}
		result = new Vector2(x, y);
		return result;
	}

	public void ForceFinishAnimation()
	{
		this.coloringAnimation.ForceFinishAnimation();
		this.OpenControls();
	}

	private void OnEnable()
	{
		this.isReplay = false;
		this.coloringAnimation.Completed += this.OpenControls;
		this.coloringAnimation.StartAnimation(3f, 0.7f);
		this.IsOpened = true;
	}

	private void OnDisable()
	{
		this.coloringAnimation.Completed -= this.OpenControls;
		this.IsOpened = false;
	}

	private void OpenControls()
	{
		if (!this.isReplay)
		{
			if (GeneralSettings.IsShowRate())
			{
				this.popupManager.OpenRate();
				AnalyticsManager.RateShow();
			}
			else if (AdsManager.Instance.HasInterstitial(AdPlacement.Solved))
			{
				AdsManager.Instance.ShowInterstitial(AdPlacement.Solved);
			}
		}
		PlayTimeEventTracker.AnimWatched();
		base.StartCoroutine(this.FadeCoroutine(0f, 1f, 0.2f, 0.2f, this.mediaBtn));
		base.StartCoroutine(this.FadeCoroutine(0f, 1f, 0.2f, 0.4f, this.exitBtn));
		base.StartCoroutine(this.FadeCoroutine(1f, 0f, 0.2f, 0f, this.skipBtn));
	}

	private void HideControls()
	{
		base.StartCoroutine(this.FadeCoroutine(1f, 0f, 0.2f, 0f, this.mediaBtn));
		base.StartCoroutine(this.FadeCoroutine(1f, 0f, 0.2f, 0f, this.exitBtn));
		base.StartCoroutine(this.FadeCoroutine(0f, 1f, 0.2f, 0.2f, this.skipBtn));
	}

	private IEnumerator FadeCoroutine(float from, float to, float animDuration, float d, CanvasGroup canvas)
	{
		if (d > 0f)
		{
			yield return new WaitForSeconds(d);
		}
		if ((double)from <= 0.1)
		{
			canvas.gameObject.SetActive(true);
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
		if ((double)Mathf.Abs(to) < 0.01)
		{
			canvas.gameObject.SetActive(false);
		}
		yield break;
	}

	[SerializeField]
	private GamePopupManager popupManager;

	[SerializeField]
	private ColoringAnimation coloringAnimation;

	[SerializeField]
	private RectTransform shareBtnRt;

	[SerializeField]
	private RectTransform mainAppRt;

	[SerializeField]
	private CanvasGroup skipBtn;

	[SerializeField]
	private CanvasGroup mediaBtn;

	[SerializeField]
	private CanvasGroup exitBtn;

	private Texture2D texForShare;

	private bool isReplay;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolvedPage : MonoBehaviour, ISolvePage
{
	public bool IsAnimating { get; private set; }

	public bool IsOpened { get; private set; }

	private void OnEnable()
	{
		this.IsOpened = true;
		if (SafeLayout.IsTablet)
		{
			this.drawImg.rectTransform.sizeDelta = new Vector2((float)((int)(this.drawImg.rectTransform.sizeDelta.x * 1.4f)), (float)((int)(this.drawImg.rectTransform.sizeDelta.y * 1.4f)));
			this.lineImg.rectTransform.sizeDelta = this.drawImg.rectTransform.sizeDelta;
		}
		this.colorCoroutine = base.StartCoroutine(this.ColorStepByStep());
	}

	private void OnDisable()
	{
		this.IsOpened = false;
		if (this.colorCoroutine != null)
		{
			base.StopCoroutine(this.colorCoroutine);
		}
		this.popup.Close();
	}

	public void Clean()
	{
		UnityEngine.Object.Destroy(this.drawTex);
		this.drawTex = null;
		this.lineImg.texture = null;
		this.steps = null;
		if (this.texForShare != null)
		{
			UnityEngine.Object.Destroy(this.texForShare);
			this.texForShare = null;
		}
	}

	public void SetData(List<SaveStep> saveSteps, IPaintFill pFill, Func<int, Color> idToColor)
	{
		this.IdToColor = idToColor;
		this.lineImg.texture = pFill.LineTex;
		this.paintFill = pFill;
		this.drawTex = new Texture2D(this.paintFill.TexWidth, this.paintFill.TexHeight, TextureFormat.RGBA32, false);
		this.drawTex.filterMode = FilterMode.Bilinear;
		this.drawTex.wrapMode = TextureWrapMode.Clamp;
		this.pixels = new byte[this.paintFill.TexWidth * this.paintFill.TexHeight * 4];
		if (pFill is ChopFill)
		{
			((ChopFill)pFill).PrepareCopy(this.pixels);
		}
		this.drawTex.LoadRawTextureData(this.pixels);
		this.drawTex.Apply(false);
		this.drawImg.texture = this.drawTex;
		this.steps = saveSteps;
		this.IsAnimating = true;
	}

	public void CutShareableTexture(Action<Texture2D> callback)
	{
		if (this.texForShare != null)
		{
			callback(this.texForShare);
		}
		else
		{
			base.StartCoroutine(this.TextureCutCoroutine(callback));
		}
	}

	public void ForceFinishAnimation()
	{
		if (!this.IsAnimating)
		{
			return;
		}
		this.IsAnimating = false;
		if (this.colorCoroutine != null)
		{
			base.StopCoroutine(this.colorCoroutine);
		}
		for (int i = this.colorStepIndex; i < this.steps.Count; i++)
		{
			FillAlgorithm fillType = this.paintFill.FillType;
			if (fillType != FillAlgorithm.Flood)
			{
				if (fillType == FillAlgorithm.Chop)
				{
					(this.paintFill as ChopFill).FillOnCopy(this.steps[i].point, this.IdToColor(this.steps[i].colorId), this.pixels);
				}
			}
			else
			{
				(this.paintFill as FloodFill).FillOnCopy(this.steps[i].point, this.IdToColor(this.steps[i].colorId), this.pixels);
			}
		}
		this.drawTex.LoadRawTextureData(this.pixels);
		this.drawTex.Apply(false);
		this.OpenControls();
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

	private IEnumerator TextureCutCoroutine(Action<Texture2D> callback)
	{
		this.ForceFinishAnimation();
		yield return new WaitForEndOfFrame();
		RenderTexture oldRT = RenderTexture.active;
		int sWidth = Screen.width;
		int sHeight = Screen.height;
		float cWidth = ((RectTransform)base.transform).rect.width;
		float i = (float)sWidth / cWidth;
		int tWidth = (int)(this.lineImg.rectTransform.rect.width * i);
		int tHeight = (int)(this.lineImg.rectTransform.rect.height * i);
		RenderTexture rt = new RenderTexture(tWidth, tHeight, 16);
		Texture2D tex = new Texture2D(tWidth, tHeight);
		RenderTexture.active = rt;
		yield return new WaitForEndOfFrame();
		float yOffset = this.lineImg.rectTransform.anchoredPosition.y;
		tex.ReadPixels(new Rect((float)sWidth / 2f - (float)tWidth / 2f, (float)sHeight / 2f - (float)tHeight / 2f + yOffset * i, (float)tWidth, (float)tWidth), 0, 0);
		tex.Apply();
		yield return new WaitForEndOfFrame();
		RenderTexture.active = oldRT;
		this.texForShare = tex;
		callback(tex);
		yield break;
	}

	private IEnumerator ColorStepByStep()
	{
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < this.steps.Count; i++)
		{
			FillAlgorithm fillType = this.paintFill.FillType;
			if (fillType != FillAlgorithm.Flood)
			{
				if (fillType == FillAlgorithm.Chop)
				{
					(this.paintFill as ChopFill).FillOnCopy(this.steps[i].point, this.IdToColor(this.steps[i].colorId), this.pixels);
				}
			}
			else
			{
				(this.paintFill as FloodFill).FillOnCopy(this.steps[i].point, this.IdToColor(this.steps[i].colorId), this.pixels);
			}
			this.drawTex.LoadRawTextureData(this.pixels);
			this.drawTex.Apply(false);
			this.colorStepIndex = i;
			yield return new WaitForSeconds(0.04f);
		}
		yield return 0;
		this.colorCoroutine = null;
		this.IsAnimating = false;
		PlayTimeEventTracker.AnimWatched();
		this.OpenControls();
		yield break;
	}

	private void OpenControls()
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
		this.popup.Open();
	}

	private Texture2D texForShare;

	private int colorStepIndex;

	[SerializeField]
	private RectTransform shareBtnRt;

	[SerializeField]
	private RectTransform mainAppRt;

	[SerializeField]
	private GamePopupManager popupManager;

	[SerializeField]
	private RawImage drawImg;

	[SerializeField]
	private RawImage lineImg;

	private Texture2D drawTex;

	private byte[] pixels;

	private Func<int, Color> IdToColor;

	private IPaintFill paintFill;

	private List<SaveStep> steps;

	private Coroutine colorCoroutine;

	[SerializeField]
	private FMPopup popup;

	[SerializeField]
	private GameObject skipButton;
}

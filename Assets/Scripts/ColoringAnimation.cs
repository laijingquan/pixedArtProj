// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ColoringAnimation : MonoBehaviour
{
	public bool IsAnimating { get; private set; }

	public Texture2D DrawTex
	{
		get
		{
			return this.drawTex;
		}
	}

	public Texture2D LineTex
	{
		get
		{
			return this.paintFill.LineTex;
		}
	}

	 
	public event Action Completed;

	public void Init(List<SaveStep> saveSteps, IPaintFill pFill, Func<int, Color> idToColor)
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

	public void InitAsPreview(Texture2D dtex, byte[] pxs, List<SaveStep> saveSteps, IPaintFill pFill, Func<int, Color> idToColor)
	{
		this.IdToColor = idToColor;
		this.paintFill = pFill;
		this.drawTex = dtex;
		this.pixels = pxs;
		this.steps = saveSteps;
		this.IsAnimating = true;
	}

	public void StartAnimation(float delay = 0.5f, float postDelay = 0f)
	{
		this.colorCoroutine = base.StartCoroutine(this.ColorStepByStep(delay, postDelay));
	}

	public void Replay()
	{
		this.pixels = new byte[this.paintFill.TexWidth * this.paintFill.TexHeight * 4];
		if (this.paintFill is ChopFill)
		{
			((ChopFill)this.paintFill).PrepareCopy(this.pixels);
		}
		this.drawTex.LoadRawTextureData(this.pixels);
		this.drawTex.Apply(false);
		this.colorStepIndex = 0;
		this.IsAnimating = true;
		this.StartAnimation(0.5f, 0f);
	}

	public void StopAnimation()
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
	}

	public void Clean()
	{
		UnityEngine.Object.Destroy(this.drawTex);
		this.drawTex = null;
		this.lineImg.texture = null;
		this.steps = null;
		this.pixels = null;
	}

	private IEnumerator ColorStepByStep(float delay = 0f, float postDelay = 0f)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
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
		if (postDelay > 0f)
		{
			float currentTime = 0f;
			while (currentTime < postDelay)
			{
				currentTime += Time.deltaTime;
				yield return null;
			}
		}
		yield return 0;
		this.colorCoroutine = null;
		this.IsAnimating = false;
		if (this.Completed != null)
		{
			this.Completed();
		}
		yield break;
	}

	[SerializeField]
	private RawImage drawImg;

	[SerializeField]
	private RawImage lineImg;

	private Texture2D drawTex;

	private byte[] pixels;

	private Func<int, Color> IdToColor;

	private IPaintFill paintFill;

	private List<SaveStep> steps;

	private int colorStepIndex;

	private Coroutine colorCoroutine;
}

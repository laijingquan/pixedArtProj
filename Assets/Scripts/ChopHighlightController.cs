// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopHighlightController : MonoBehaviour, IHighlighter
{
	public void Init(IPaintFill pFill, PaletteData data, NumberController numberController)
	{
		this.pd = data;
		this.chopFill = (ChopFill)pFill;
		this.numController = numberController;
	}

	public void HighlightColor(int colorId)
	{
		if (this.currentColorId == colorId)
		{
			UnityEngine.Debug.Log("Rehighlight. Skip");
			return;
		}
		if (this.highlightCoroutine != null)
		{
			if (this.prev != null)
			{
				for (int i = 0; i < this.prev.zoneIds.Count; i++)
				{
					this.chopFill.FillId(this.prev.startColor, this.prev.zoneIds[i]);
				}
				this.numController.DehighlightVisable(this.prev.colorId);
			}
			base.StopCoroutine(this.highlightCoroutine);
		}
		this.currentColorId = colorId;
		PaletteEntity paletteEntity = this.pd.entities[colorId];
		if (this.current != null)
		{
			this.prev = this.current;
		}
		this.current = new ChopHighlightController.HState();
		this.current.colorId = colorId;
		this.current.color = paletteEntity.color;
		if (ColorUtils.IsSameColors(paletteEntity.color, this.highlightColor))
		{
			this.current.highlightColor = this.highlightBackupColor;
			this.current.startColor = this.startBackupColor;
		}
		else
		{
			this.current.highlightColor = this.highlightColor;
			this.current.startColor = this.startColor;
		}
		this.current.zoneIds = new List<short>();
		short num = 0;
		while ((int)num < this.chopFill.ColorMap.Count)
		{
			if (this.chopFill.ColorMap[(int)num] == colorId && !this.chopFill.HasColorInId(paletteEntity.color, num))
			{
				this.current.zoneIds.Add(num);
			}
			num += 1;
		}
		if (this.prev != null)
		{
			for (int j = this.prev.zoneIds.Count - 1; j >= 0; j--)
			{
				if (this.chopFill.HasColorInId(this.prev.color, this.prev.zoneIds[j]))
				{
					this.prev.zoneIds.RemoveAt(j);
				}
			}
		}
		this.highlightCoroutine = base.StartCoroutine(this.HighlightCoroutine(this.duration));
	}

	public void FillColor(Point p, Color32 color, int markTag)
	{
		this.fill = new ChopHighlightController.HState();
		this.fill.startColor = this.chopFill.CurrentColorInPixel(p);
		this.fill.highlightColor = color;
		this.fill.zoneIds = new List<short>
		{
			(short)markTag
		};
		base.StartCoroutine(this.FillCoroutine(0.2f));
	}

	public void DehighlightForIcon()
	{
		if (this.current != null)
		{
			for (int i = 0; i < this.current.zoneIds.Count; i++)
			{
				if (!this.chopFill.HasColorInId(this.current.color, this.current.zoneIds[i]))
				{
					this.chopFill.FillId(this.current.startColor, this.current.zoneIds[i]);
				}
			}
			this.numController.DehighlightVisable(this.current.colorId);
		}
		if (this.prev != null)
		{
			for (int j = 0; j < this.prev.zoneIds.Count; j++)
			{
				this.chopFill.FillId(this.prev.startColor, this.prev.zoneIds[j]);
			}
			this.prev = null;
		}
		this.chopFill.UpdateDrawTex();
	}

	public void HighlightLast()
	{
		if (this.current != null)
		{
			for (int i = 0; i < this.current.zoneIds.Count; i++)
			{
				if (!this.chopFill.HasColorInId(this.current.color, this.current.zoneIds[i]))
				{
					this.chopFill.FillId(this.current.highlightColor, this.current.zoneIds[i]);
				}
			}
			this.chopFill.UpdateDrawTex();
			this.numController.HighlighVisable(this.current.colorId);
		}
	}

	public void Clean()
	{
	}

	private IEnumerator HighlightCoroutine(float animDuration)
	{
		yield return 0;
		float i = 0f;
		float currentTime = 0f;
		this.numController.HighlighVisable(this.current.colorId);
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			Color32 colorUp = Color32.Lerp(this.current.startColor, this.current.highlightColor, i);
			for (int j = 0; j < this.current.zoneIds.Count; j++)
			{
				this.chopFill.FillId(colorUp, this.current.zoneIds[j]);
			}
			if (this.prev != null)
			{
				Color32 color = Color32.Lerp(this.prev.highlightColor, this.prev.startColor, i);
				for (int k = 0; k < this.prev.zoneIds.Count; k++)
				{
					this.chopFill.FillId(color, this.prev.zoneIds[k]);
				}
			}
			this.chopFill.UpdateDrawTex();
			yield return 0;
		}
		yield return 0;
		if (this.prev != null)
		{
			this.numController.DehighlightVisable(this.prev.colorId);
			this.prev.zoneIds.Clear();
			this.prev = null;
		}
		yield return 0;
		this.highlightCoroutine = null;
		yield break;
	}

	private IEnumerator FillCoroutine(float animDuration)
	{
		yield return 0;
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			Color32 color = Color32.Lerp(this.fill.startColor, this.fill.highlightColor, i);
			for (int j = 0; j < this.fill.zoneIds.Count; j++)
			{
				this.chopFill.FillId(color, this.fill.zoneIds[j]);
			}
			this.chopFill.UpdateDrawTex();
			yield return 0;
		}
		yield return 0;
		this.fillCoroutine = null;
		yield break;
	}

	[SerializeField]
	private float duration = 0.15f;

	private PaletteData pd;

	private int currentColorId = -1;

	private ChopFill chopFill;

	private NumberController numController;

	private ChopHighlightController.HState current;

	private ChopHighlightController.HState prev;

	private Color32 startColor = new Color32(210, 210, 210, 0);

	private Color32 highlightColor = new Color32(210, 210, 210, byte.MaxValue);

	private Color32 highlightBackupColor = new Color32(200, 200, 200, byte.MaxValue);

	private Color32 startBackupColor = new Color32(200, 200, 200, 0);

	private Coroutine highlightCoroutine;

	private Coroutine fillCoroutine;

	private ChopHighlightController.HState fill;

	private class HState
	{
		public int colorId;

		public Color32 color;

		public Color32 highlightColor;

		public Color32 startColor;

		public List<short> zoneIds;
	}
}

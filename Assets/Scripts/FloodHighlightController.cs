// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloodHighlightController : MonoBehaviour, IHighlighter
{
	public void Init(AdvancedMobilePaint paintData, PaletteData data, Texture2D drawTexture)
	{
		this.drawImg.gameObject.SetActive(true);
		this.pd = data;
		this.paintEngine = paintData;
		this.highlightEngine = new AdvancedMobilePaint();
		this.highlightEngine.pixels = new byte[this.paintEngine.pixels.Length];
		this.highlightEngine.source = this.paintEngine.source;
		this.highlightEngine.texWidth = this.paintEngine.texWidth;
		this.highlightEngine.texHeight = this.paintEngine.texHeight;
		this.drawText = new Texture2D(drawTexture.width, drawTexture.height, TextureFormat.RGBA32, false);
		this.drawText.LoadRawTextureData(this.highlightEngine.pixels);
		this.drawText.Apply();
		this.drawImg.texture = this.drawText;
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
				for (int i = 0; i < this.prev.zones.Count; i++)
				{
					List<int> indexes = this.prev.zones[i];
					FloodAlgorithm.FillIndexes(indexes, this.highlightEngine, this.startColor);
				}
			}
			base.StopCoroutine(this.highlightCoroutine);
		}
		this.currentColorId = colorId;
		PaletteEntity paletteEntity = this.pd.entities[colorId];
		Color32 newColor = this.startColor;
		if (this.current != null)
		{
			this.prev = this.current;
		}
		this.current = new FloodHighlightController.HState();
		this.current.colorId = colorId;
		this.current.color = paletteEntity.color;
		this.current.zones = new List<List<int>>();
		for (int j = 0; j < paletteEntity.indexes.Length; j++)
		{
			int num = paletteEntity.indexes[j];
			if (!this.IsPainted(num, paletteEntity.color))
			{
				Point point = default(Point);
				point.i = Mathf.FloorToInt((float)num / (float)this.paintEngine.texWidth);
				point.j = num - point.i * this.paintEngine.texWidth;
				byte b = this.paintEngine.source[num];
				if (b < 253)
				{
					b += 1;
				}
				else
				{
					b = 1;
				}
				List<int> item = FloodAlgorithm.FloodFillExtended(point.j, point.i, this.highlightEngine, newColor, b);
				this.current.zones.Add(item);
			}
		}
		if (this.prev != null)
		{
			for (int k = this.prev.zones.Count - 1; k >= 0; k--)
			{
				if (this.IsPainted(this.prev.zones[k][0], this.prev.color))
				{
					this.prev.zones.RemoveAt(k);
				}
			}
		}
		this.highlightCoroutine = base.StartCoroutine(this.HighlightCoroutine(this.duration));
	}

	public void FillColor(Point p, Color32 color, int markTag)
	{
	}

	public void DehighlightForIcon()
	{
		if (this.current != null)
		{
			for (int i = 0; i < this.current.zones.Count; i++)
			{
				List<int> indexes = this.current.zones[i];
				if (!this.IsPainted(this.current.zones[i][0], this.current.color))
				{
					FloodAlgorithm.FillIndexes(indexes, this.highlightEngine, this.startColor);
				}
				this.drawText.LoadRawTextureData(this.highlightEngine.pixels);
			}
		}
	}

	public void HighlightLast()
	{
	}

	public void Clean()
	{
		UnityEngine.Object.Destroy(this.drawText);
		this.drawText = null;
	}

	private IEnumerator HighlightCoroutine(float animDuration)
	{
		yield return 0;
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			Color32 colorUp = Color32.Lerp(this.startColor, this.highlightColor, i);
			Color32 colorDown = Color32.Lerp(this.highlightColor, this.startColor, i);
			for (int j = 0; j < this.current.zones.Count; j++)
			{
				List<int> indexes = this.current.zones[j];
				FloodAlgorithm.FillIndexes(indexes, this.highlightEngine, colorUp);
			}
			if (this.prev != null)
			{
				for (int k = 0; k < this.prev.zones.Count; k++)
				{
					List<int> indexes2 = this.prev.zones[k];
					FloodAlgorithm.FillIndexes(indexes2, this.highlightEngine, colorDown);
				}
			}
			this.drawText.LoadRawTextureData(this.highlightEngine.pixels);
			this.drawText.Apply();
			yield return 0;
		}
		yield return 0;
		if (this.prev != null)
		{
			this.prev.zones.Clear();
			this.prev = null;
		}
		yield return 0;
		this.highlightCoroutine = null;
		yield break;
	}

	private bool IsPainted(int index, Color color)
	{
		if (this.paintEngine.source[index] == 0)
		{
			return false;
		}
		Color32 a = new Color32(this.paintEngine.pixels[index * 4], this.paintEngine.pixels[index * 4 + 1], this.paintEngine.pixels[index * 4 + 2], byte.MaxValue);
		return ColorUtils.IsSameColors(a, color);
	}

	[SerializeField]
	private RawImage drawImg;

	[SerializeField]
	private float duration = 0.15f;

	private PaletteData pd;

	private int currentColorId = -1;

	private AdvancedMobilePaint paintEngine;

	private AdvancedMobilePaint highlightEngine;

	private Texture2D drawText;

	private FloodHighlightController.HState current;

	private FloodHighlightController.HState prev;

	private Color32 startColor = new Color32(210, 210, 210, 0);

	private Color32 highlightColor = new Color32(210, 210, 210, byte.MaxValue);

	private Coroutine highlightCoroutine;

	private class HState
	{
		public int colorId;

		public Color32 color;

		public List<List<int>> zones;
	}
}

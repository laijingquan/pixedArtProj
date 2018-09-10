// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class NumberController : MonoBehaviour
{
	public float ScaleRequiredForNum { get; private set; }

	public static int DefaultNumSize()
	{
		int result = 30;
		if (SafeLayout.IsTablet)
		{
			if ((double)SafeLayout.ScreenSize < 8.2)
			{
				result = Mathf.CeilToInt(35f);
			}
			else
			{
				result = Mathf.CeilToInt(31.25f);
			}
		}
		return result;
	}

	public void Init(Func<int, Vector2> toCanvas, float k)
	{
		this.ToCanvasPosition = toCanvas;
		this.textureScaleRatio = k;
		this.numFontSize = GeneralSettings.NumSize;
		this.numInitialSize = Mathf.CeilToInt((float)this.numFontSize / 25f * 30f);
	}

	public void Create(PaletteData pd)
	{
		PaletteData paletteData = pd;
		int num = int.MaxValue;
		for (int i = 0; i < paletteData.entities.Length; i++)
		{
			paletteData.entities[i].color.a = byte.MaxValue;
			PaletteEntity paletteEntity = paletteData.entities[i];
			for (int j = 0; j < paletteEntity.indexes.Length; j++)
			{
				Vector2 position = this.ToCanvasPosition(paletteEntity.indexes[j]);
				int num2 = Mathf.RoundToInt((float)paletteEntity.sizes[j] * this.textureScaleRatio);
				if (num2 < num)
				{
					num = num2;
				}
				Num item = this.CreateNumView(position, i, num2);
				this.numbers.Add(item);
			}
		}
		this.ScaleRequiredForNum = (float)this.numInitialSize / (float)num + 0.05f;
	}

	private Num CreateNumView(Vector2 position, int value, int size)
	{
		Num entity = this.numPool.GetEntity<Num>(this.parent);
		entity.Init(value, (float)size, this.numFontSize, (float)this.numInitialSize);
		((RectTransform)entity.transform).anchoredPosition = position;
		return entity;
	}

	public void UpdateNums(float scaleFactor)
	{
		if (scaleFactor > 0.98f)
		{
			for (int i = 0; i < this.numbers.Count; i++)
			{
				this.numbers[i].UpdateZoomableState(scaleFactor);
			}
		}
		else
		{
			for (int j = 0; j < this.numbers.Count; j++)
			{
				this.numbers[j].Hide();
			}
		}
	}

	public void Hide()
	{
		for (int i = 0; i < this.numbers.Count; i++)
		{
			this.numbers[i].Hide();
		}
	}

	public void HighlighVisable(int id)
	{
		for (int i = 0; i < this.numbers.Count; i++)
		{
			if (this.numbers[i].Id == id)
			{
				this.numbers[i].transform.SetParent(this.visableParent);
			}
		}
	}

	public void DehighlightVisable(int id)
	{
		for (int i = 0; i < this.numbers.Count; i++)
		{
			if (this.numbers[i].Id == id)
			{
				this.numbers[i].transform.SetParent(this.parent);
			}
		}
	}

	public void FilledZone(int zoneId)
	{
		this.numbers[zoneId].MarkFilled();
	}

	private const float minScaleForNumbers = 0.98f;

	private const int NUM_MIN_SIZE = 30;

	private const int FONT_MIN_SIZE = 25;

	private const int FONT_DEF_SIZE_PHONE = 30;

	private int numInitialSize;

	private int numFontSize;

	[SerializeField]
	private RectTransform visableParent;

	[SerializeField]
	private RectTransform parent;

	[SerializeField]
	private ObjectContainer numPool;

	private List<Num> numbers = new List<Num>();

	private Func<int, Vector2> ToCanvasPosition;

	private float textureScaleRatio;
}

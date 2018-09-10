// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class FloodFill : IPaintFill
{
	public FillAlgorithm FillType
	{
		get
		{
			return FillAlgorithm.Flood;
		}
	}

	public Texture2D DrawTex
	{
		get
		{
			return this.drawText;
		}
	}

	public Texture2D LineTex
	{
		get
		{
			return this.lineContourText;
		}
	}

	public Texture2D ColorTex
	{
		get
		{
			return this.colorSourceText;
		}
	}

	public short[] SourceMask
	{
		get
		{
			return null;
		}
	}

	public int TexWidth
	{
		get
		{
			return this.paintEngine.texWidth;
		}
	}

	public int TexHeight
	{
		get
		{
			return this.paintEngine.texHeight;
		}
	}

	public AdvancedMobilePaint Paint
	{
		get
		{
			return this.paintEngine;
		}
	}

	public void Create(Texture2D l, Texture2D c, PaletteData p)
	{
		this.lineContourText = l;
		this.paletteData = p;
		this.colorSourceText = c;
		Color[] pixels = this.colorSourceText.GetPixels();
		byte[] array = new byte[pixels.Length];
		if (this.paintEngine == null)
		{
			this.paintEngine = new AdvancedMobilePaint();
		}
		this.paintEngine.pixels = new byte[this.colorSourceText.width * this.colorSourceText.height * 4];
		this.paintEngine.texHeight = this.colorSourceText.height;
		this.paintEngine.texWidth = this.colorSourceText.width;
		this.paintEngine.source = array;
		this.drawText = new Texture2D(this.colorSourceText.width, this.colorSourceText.height, TextureFormat.RGBA32, false);
		this.drawText.LoadRawTextureData(this.paintEngine.pixels);
		this.drawText.filterMode = FilterMode.Bilinear;
		this.drawText.wrapMode = TextureWrapMode.Clamp;
		this.drawText.Apply();
		float num = 0.06f;
		for (int i = 0; i < pixels.Length; i++)
		{
			if (pixels[i].r < num && pixels[i].g < num && pixels[i].b < num)
			{
				array[i] = byte.MaxValue;
			}
			else
			{
				array[i] = 0;
			}
		}
	}

	public void FillPoint(Point pColor, Color32 color, bool writeData = true)
	{
		int num = this.colorSourceText.width * pColor.i + pColor.j;
		byte b = this.paintEngine.source[num];
		if (b < 253)
		{
			b += 1;
		}
		else
		{
			b = 1;
		}
		this.paintEngine.paintColor = color;
		FloodAlgorithm.FloodFill(pColor.j, pColor.i, this.paintEngine, b);
		if (writeData)
		{
			UnityEngine.Debug.Log("save fill");
			this.drawText.LoadRawTextureData(this.paintEngine.pixels);
			this.drawText.Apply(false);
		}
	}

	public void FillOnCopy(Point pColor, Color32 color, byte[] pixels)
	{
		int num = this.colorSourceText.width * pColor.i + pColor.j;
		byte b = this.paintEngine.source[num];
		if (b < 253)
		{
			b += 1;
		}
		else
		{
			b = 1;
		}
		this.paintEngine.paintColor = color;
		FloodAlgorithm.FloodFillCopy(pColor.j, pColor.i, this.paintEngine, pixels, b);
	}

	public void UpdateDrawTex()
	{
		this.drawText.LoadRawTextureData(this.paintEngine.pixels);
		this.drawText.Apply();
	}

	public Point TextureArrayIndexToMatrix(int index)
	{
		Point result = default(Point);
		result.i = Mathf.FloorToInt((float)index / (float)this.paintEngine.texWidth);
		result.j = index - result.i * this.paintEngine.texWidth;
		return result;
	}

	public List<Texture2D> Clean()
	{
		return new List<Texture2D>
		{
			this.drawText,
			this.lineContourText,
			this.colorSourceText
		};
	}

	public bool IsLineInPixel(Point pColor)
	{
		int num = this.TexWidth * pColor.i + pColor.j;
		return this.paintEngine.source[num] == byte.MaxValue;
	}

	public Color ExpectedColorInPixel(Point pColor)
	{
		return this.colorSourceText.GetPixel(pColor.j, pColor.i);
	}

	public Color32 CurrentColorInPixel(Point pColor)
	{
		int num = this.TexWidth * pColor.i + pColor.j;
		Color32 result = new Color32(this.paintEngine.pixels[num * 4], this.paintEngine.pixels[num * 4 + 1], this.paintEngine.pixels[num * 4 + 2], this.paintEngine.pixels[num * 4 + 3]);
		return result;
	}

	public bool IsPainted(Point pColor)
	{
		return ColorUtils.IsSameColors(this.ExpectedColorInPixel(pColor), this.CurrentColorInPixel(pColor));
	}

	private Texture2D lineContourText;

	private Texture2D colorSourceText;

	private Texture2D drawText;

	private PaletteData paletteData;

	private AdvancedMobilePaint paintEngine;
}

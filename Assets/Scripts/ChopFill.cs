// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChopFill : IPaintFill
{
	public FillAlgorithm FillType
	{
		get
		{
			return FillAlgorithm.Chop;
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
			return this.lineTex;
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

	public short[] SourceMask
	{
		get
		{
			return this.paintEngine.source;
		}
	}

	public byte[] Pixels
	{
		get
		{
			return this.paintEngine.pixels;
		}
	}

	public List<int> ColorMap
	{
		get
		{
			return this.colorMap;
		}
	}

	public void Create(Texture2D l, PaletteData p, bool writeData = true)
	{
		this.paletteData = p;
		this.lineTex = l;
		Color[] pixels = this.lineTex.GetPixels();
		short[] array = new short[pixels.Length];
		this.paintEngine = new ChopMobilePaint();
		this.paintEngine.pixels = new byte[this.lineTex.width * this.lineTex.height * 4];
		this.paintEngine.texHeight = this.lineTex.height;
		this.paintEngine.texWidth = this.lineTex.width;
		this.paintEngine.source = array;
		this.paintEngine.zones = new List<List<int>>();
		this.drawText = new Texture2D(this.lineTex.width, this.lineTex.height, TextureFormat.RGBA32, false);
		this.drawText.filterMode = FilterMode.Bilinear;
		this.drawText.wrapMode = TextureWrapMode.Clamp;
		for (int i = 0; i < pixels.Length; i++)
		{
			if (pixels[i].a >= 0.99f)
			{
				array[i] = short.MaxValue;
			}
			else
			{
				array[i] = this.emptyPixelTag;
			}
		}
		this.paintEngine.paintColor = Color.black;
		ChopFillAlgorithm.FindAndFillId(this.paintEngine, this.paintEngine.pixels, short.MaxValue);
		if (writeData)
		{
			this.drawText.LoadRawTextureData(this.paintEngine.pixels);
			this.drawText.Apply();
		}
		PaletteData paletteData = p;
		short num = 0;
		for (int j = 0; j < paletteData.entities.Length; j++)
		{
			for (int k = 0; k < paletteData.entities[j].indexes.Length; k++)
			{
				Point point = this.paintEngine.TextureArrayIndexToMatrix(paletteData.entities[j].indexes[k]);
				List<int> list = ChopFillAlgorithm.MarkArea(point.j, point.i, this.paintEngine, this.emptyPixelTag, num);
				if (list != null && list.Count > 0)
				{
					this.paintedMap.Add(false);
					this.colorMap.Add(j);
					this.paintEngine.zones.Add(list);
					num += 1;
				}
				else
				{
					UnityEngine.Debug.Log("Error marking area");
				}
			}
		}
	}

	public int FillPoint(Point pColor, Color32 color, bool writeData = true)
	{
		int num = this.paintEngine.texWidth * pColor.i + pColor.j;
		short num2 = this.paintEngine.source[num];
		this.paintEngine.paintColor = color;
		ChopFillAlgorithm.Fill(pColor.j, pColor.i, this.paintEngine, num2);
		if (writeData)
		{
			this.UpdateDrawTex();
		}
		this.paintedMap[(int)num2] = true;
		return (int)num2;
	}

	public int MarkFilled(Point pColor)
	{
		int num = this.paintEngine.texWidth * pColor.i + pColor.j;
		int num2 = (int)this.paintEngine.source[num];
		this.paintedMap[num2] = true;
		return num2;
	}

	public void PrepareCopy(byte[] pixels)
	{
		Color32 paintColor = this.paintEngine.paintColor;
		this.paintEngine.paintColor = Color.black;
		ChopFillAlgorithm.FindAndFillId(this.paintEngine, pixels, short.MaxValue);
		this.paintEngine.paintColor = paintColor;
	}

	public void FillOnCopy(Point pColor, Color32 color, byte[] pixels)
	{
		int num = this.paintEngine.texWidth * pColor.i + pColor.j;
		short markTag = this.paintEngine.source[num];
		this.paintEngine.paintColor = color;
		ChopFillAlgorithm.FillCopy(pColor.j, pColor.i, this.paintEngine, pixels, markTag);
	}

	public void FillId(Color32 color, short id)
	{
		this.paintEngine.paintColor = color;
		ChopFillAlgorithm.Fill(0, 0, this.paintEngine, id);
	}

	public void UpdateDrawTex()
	{
		this.drawText.LoadRawTextureData(this.paintEngine.pixels);
		this.drawText.Apply();
	}

	public Point TextureArrayIndexToMatrix(int index)
	{
		return this.paintEngine.TextureArrayIndexToMatrix(index);
	}

	public List<Texture2D> Clean()
	{
		List<Texture2D> list = new List<Texture2D>();
		if (this.drawText != null)
		{
			list.Add(this.drawText);
		}
		if (this.lineTex != null)
		{
			list.Add(this.lineTex);
		}
		return list;
	}

	public bool IsLineInPixel(Point pColor)
	{
		int num = this.TexWidth * pColor.i + pColor.j;
		return this.paintEngine.source[num] == short.MaxValue || this.paintEngine.source[num] == this.emptyPixelTag;
	}

	public Color ExpectedColorInPixel(Point pColor)
	{
		int num = this.TexWidth * pColor.i + pColor.j;
		int num2 = (int)this.paintEngine.source[num];
		if (num2 == (int)this.emptyPixelTag)
		{
			return new Color(0f, 0f, 0f, 0f);
		}
		return this.paletteData.entities[this.colorMap[num2]].color;
	}

	public Color32 CurrentColorInPixel(Point pColor)
	{
		int num = this.TexWidth * pColor.i + pColor.j;
		Color32 result = new Color32(this.paintEngine.pixels[num * 4], this.paintEngine.pixels[num * 4 + 1], this.paintEngine.pixels[num * 4 + 2], this.paintEngine.pixels[num * 4 + 3]);
		return result;
	}

	public bool HasColorInId(Color32 color, short id)
	{
		int num = this.paintEngine.zones[(int)id][0];
		Color32 a = new Color32(this.paintEngine.pixels[num * 4], this.paintEngine.pixels[num * 4 + 1], this.paintEngine.pixels[num * 4 + 2], this.paintEngine.pixels[num * 4 + 3]);
		return ColorUtils.IsSameColors(a, color);
	}

	public bool IsPainted(Point pColor)
	{
		int num = this.paintEngine.texWidth * pColor.i + pColor.j;
		short id = this.paintEngine.source[num];
		return this.IsPainted(id);
	}

	public bool IsPainted(short id)
	{
		return this.paintedMap[(int)id];
	}

	private Texture2D lineTex;

	private Texture2D drawText;

	private PaletteData paletteData;

	private ChopMobilePaint paintEngine;

	private List<int> colorMap = new List<int>();

	private List<bool> paintedMap = new List<bool>();

	private short emptyPixelTag = -1;
}

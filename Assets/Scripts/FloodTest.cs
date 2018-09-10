// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FloodTest : MonoBehaviour
{
	public Color drawColor
	{
		get
		{
			return this.paintEngine.paintColor;
		}
		set
		{
			if (this.paintEngine == null)
			{
				this.paintEngine = new AdvancedMobilePaint();
			}
			this.paintEngine.paintColor = value;
		}
	}

	private void Start()
	{
		this.touch.Click += this.OnClick;
		this.PrepareMandala();
	}

	private void PrepareMandala()
	{
		this.fType = FloodTest.FillType.Fast;
		this.lineContourText = this.LoadImage(FloodTest.IMG_NAME_INDEX.ToString());
		this.sourceImg.texture = this.lineContourText;
		this.colorSourceText = this.LoadImage(FloodTest.IMG_NAME_INDEX + "ccc");
		this.pixels = this.colorSourceText.GetPixels();
		this.sourceAlpha = new byte[this.pixels.Length];
		if (this.paintEngine == null)
		{
			this.paintEngine = new AdvancedMobilePaint();
		}
		this.paintEngine.pixels = new byte[this.colorSourceText.width * this.colorSourceText.height * 4];
		this.paintEngine.texHeight = this.colorSourceText.height;
		this.paintEngine.texWidth = this.colorSourceText.width;
		this.paintEngine.source = this.sourceAlpha;
		this.drawText = new Texture2D(this.colorSourceText.width, this.colorSourceText.height, TextureFormat.RGBA32, false);
		this.drawText.LoadRawTextureData(this.paintEngine.pixels);
		this.drawText.filterMode = FilterMode.Point;
		this.drawText.Apply();
		this.imageRect = this.sourceImg.rectTransform.rect;
		this.drawImg.texture = this.drawText;
		float num;
		if (FloodTest.IMG_NAME_INDEX < 104)
		{
			num = 0.09f;
		}
		else
		{
			num = 0.02f;
		}
		for (int i = 0; i < this.pixels.Length; i++)
		{
			if (this.pixels[i].r < num && this.pixels[i].g < num && this.pixels[i].b < num)
			{
				this.sourceAlpha[i] = byte.MaxValue;
			}
			else
			{
				this.sourceAlpha[i] = 0;
			}
		}
		string value = FileHelper.LoadTextAssetResource(FloodTest.IMG_NAME_INDEX + "t");
		PaletteData paletteData = JsonConvert.DeserializeObject<PaletteData>(value);
		for (int j = 0; j < paletteData.entities.Length; j++)
		{
			PaletteEntity paletteEntity = paletteData.entities[j];
			for (int k = 0; k < paletteEntity.indexes.Length; k++)
			{
				Point p = default(Point);
				p.i = Mathf.FloorToInt((float)paletteEntity.indexes[k] / (float)this.colorSourceText.width);
				p.j = paletteEntity.indexes[k] - p.i * this.colorSourceText.width;
				this.CreateNumView(this.TextureToCanvas(p), j);
			}
		}
	}

	private void PrepareMandalaTwo()
	{
		this.fType = FloodTest.FillType.Fast;
		this.lineContourText = this.LoadImage(FloodTest.IMG_NAME_INDEX.ToString());
		this.sourceImg.texture = this.lineContourText;
		this.colorSourceText = this.lineContourText;
		this.pixels = this.colorSourceText.GetPixels();
		this.sourceAlpha = new byte[this.pixels.Length];
		Texture2D texture2D = this.LoadImage(FloodTest.IMG_NAME_INDEX + "ccc");
		if (this.paintEngine == null)
		{
			this.paintEngine = new AdvancedMobilePaint();
		}
		this.paintEngine.pixels = new byte[this.colorSourceText.width * this.colorSourceText.height * 4];
		this.paintEngine.texHeight = this.colorSourceText.height;
		this.paintEngine.texWidth = this.colorSourceText.width;
		this.paintEngine.source = this.sourceAlpha;
		this.paintEngine.sourceColors = texture2D.GetPixels();
		this.drawText = new Texture2D(this.colorSourceText.width, this.colorSourceText.height, TextureFormat.RGBA32, false);
		this.drawText.LoadRawTextureData(this.paintEngine.pixels);
		this.drawText.Apply();
		this.imageRect = this.sourceImg.rectTransform.rect;
		this.drawImg.texture = this.drawText;
		for (int i = 0; i < this.pixels.Length; i++)
		{
			if (this.pixels[i].a == 1f)
			{
				this.sourceAlpha[i] = byte.MaxValue;
			}
			else
			{
				this.sourceAlpha[i] = 0;
			}
		}
		string value = FileHelper.LoadTextAssetResource(FloodTest.IMG_NAME_INDEX + "t");
		PaletteData paletteData = JsonConvert.DeserializeObject<PaletteData>(value);
		for (int j = 0; j < paletteData.entities.Length; j++)
		{
			PaletteEntity paletteEntity = paletteData.entities[j];
			for (int k = 0; k < paletteEntity.indexes.Length; k++)
			{
				Point p = default(Point);
				p.i = Mathf.FloorToInt((float)paletteEntity.indexes[k] / (float)this.colorSourceText.width);
				p.j = paletteEntity.indexes[k] - p.i * this.colorSourceText.width;
				this.CreateNumView(this.TextureToCanvas(p), j);
			}
		}
	}

	private Texture2D LoadImage(string n)
	{
		return Resources.Load<Texture2D>(n);
	}

	private void Brush(Point p)
	{
		this.drawText.SetPixel(p.i, p.j, Color.red);
		this.drawText.Apply();
	}

	public void OnClick(Vector2 position)
	{
		Point p = this.CanvasToTextureCoord(position, this.colorSourceText);
		TLogger.Instance.Log(p.ToString());
		this.time = Time.realtimeSinceStartup;
		this.ClickHandle(p);
		this.UpdateJson(p);
	}

	private void ClickHandle(Point p)
	{
		TLogger.Instance.Log(string.Concat(new object[]
		{
			"C: ",
			this.colorSourceText.GetPixel(p.i, p.j),
			" to ",
			this.drawColor
		}));
		TLogger.Instance.Log(string.Concat(new object[]
		{
			"p: ",
			p,
			" index: ",
			p.i * this.colorSourceText.width + p.j
		}));
		int num = this.paintEngine.texWidth * p.i + p.j;
		byte b = this.paintEngine.source[num];
		if (b == 255)
		{
			UnityEngine.Debug.Log("Hit line");
			return;
		}
		Color32 color = new Color32(this.paintEngine.pixels[num * 4], this.paintEngine.pixels[num * 4 + 1], this.paintEngine.pixels[num * 4 + 2], byte.MaxValue);
		if (color.r == this.paintEngine.paintColor.r && color.g == this.paintEngine.paintColor.g && color.b == this.paintEngine.paintColor.b)
		{
			UnityEngine.Debug.Log("Repaint");
			return;
		}
		if (b < 253)
		{
			b += 1;
		}
		else
		{
			b = 0;
		}
		FloodTest.FillType fillType = this.fType;
		if (fillType != FloodTest.FillType.Slow)
		{
			if (fillType == FloodTest.FillType.Fast)
			{
				FloodTest.FloodFill(p.j, p.i, this.paintEngine, b);
				this.drawText.LoadRawTextureData(this.paintEngine.pixels);
				this.drawText.Apply(false);
				TLogger.Instance.Log(Time.realtimeSinceStartup - this.time);
			}
		}
		else
		{
			base.StartCoroutine(this.SlowFill(p.j, p.i, this.paintEngine, b));
		}
	}

	private void ClickHandleTwo(Point p)
	{
		int num = this.paintEngine.texWidth * p.j + p.i;
		byte b = this.paintEngine.source[num];
		if (b == 255)
		{
			UnityEngine.Debug.Log("Hit line");
			return;
		}
		Color32 color = new Color32(this.paintEngine.pixels[num * 4], this.paintEngine.pixels[num * 4 + 1], this.paintEngine.pixels[num * 4 + 2], byte.MaxValue);
		Color color2 = this.paintEngine.sourceColors[(int)((float)this.paintEngine.texWidth / 16f * (float)p.j + (float)p.i / 4f)];
		TLogger.Instance.Log(string.Concat(new object[]
		{
			"C: ",
			color,
			" to ",
			this.drawColor,
			" exp: ",
			color2
		}));
		if (color2.r == (float)this.paintEngine.paintColor.r && color2.g == (float)this.paintEngine.paintColor.g && color2.b == (float)this.paintEngine.paintColor.b)
		{
			UnityEngine.Debug.Log("Repaint");
			return;
		}
		if (b < 253)
		{
			b += 1;
		}
		else
		{
			b = 0;
		}
		FloodTest.FillType fillType = this.fType;
		if (fillType != FloodTest.FillType.Slow)
		{
			if (fillType == FloodTest.FillType.Fast)
			{
				FloodTest.FloodFill(p.i, p.j, this.paintEngine, b);
				this.drawText.LoadRawTextureData(this.paintEngine.pixels);
				this.drawText.Apply(false);
				TLogger.Instance.Log(Time.realtimeSinceStartup - this.time);
			}
		}
		else
		{
			base.StartCoroutine(this.SlowFill(p.i, p.j, this.paintEngine, b));
		}
	}

	private void UpdateJson(Point p)
	{
		if (this.tEntities == null)
		{
			this.tEntities = new Dictionary<Color, List<int>>();
		}
		Color pixel = this.colorSourceText.GetPixel(p.i, p.j);
		if (!this.tEntities.ContainsKey(pixel))
		{
			this.tEntities.Add(pixel, new List<int>());
		}
		UnityEngine.Debug.Log(p + " to " + (this.paintEngine.texWidth * p.i + p.j));
		this.tEntities[pixel].Add(this.paintEngine.texWidth * p.i + p.j);
	}

	private Point CanvasToTextureCoord(Vector2 position, Texture2D texture)
	{
		Vector2 a = new Vector2(this.imageRect.width / 2f, this.imageRect.height / 2f);
		Vector2 vector = a + position;
		Point point = new Point((int)vector.y, (int)vector.x);
		int x = Mathf.FloorToInt((float)point.i / this.imageRect.width * (float)texture.width);
		int y = Mathf.FloorToInt((float)point.j / this.imageRect.height * (float)texture.height);
		Point result = new Point(x, y);
		return result;
	}

	private Vector2 TextureToCanvas(Point p)
	{
		int num = Mathf.FloorToInt((float)p.j / (float)this.colorSourceText.width * this.imageRect.width);
		int num2 = Mathf.FloorToInt((float)p.i / (float)this.colorSourceText.height * this.imageRect.height);
		Vector2 result = new Vector2((float)num, (float)num2);
		return result;
	}

	private IEnumerator DelayCr(float delay)
	{
		yield return new WaitForSeconds(delay);
		yield break;
	}

	public void RevisedQueueFloodFill(byte[] source, Texture2D targetTex, int hitX, int hitY, Color replaceColor, bool dontPush)
	{
		int width = targetTex.width;
		int height = targetTex.height;
		Color color = this.pixels[hitX + hitY * width];
		if (color == replaceColor)
		{
			return;
		}
		Queue<Point> queue = new Queue<Point>();
		queue.Enqueue(new Point(hitX, hitY));
		while (queue.Count > 0)
		{
			Point point = queue.Dequeue();
			if (source[point.i + point.j * width] < 1)
			{
				Point item = point;
				while (item.i > 0 && source[item.i + item.j * width] < 1)
				{
					this.pixels[item.i + item.j * width] = replaceColor;
					item.i--;
				}
				int num = item.i + 1;
				item = point;
				item.i++;
				while (item.i < width - 1 && source[item.i + item.j * width] < 1)
				{
					this.pixels[item.i + item.j * width] = replaceColor;
					item.i++;
				}
				int num2 = item.i - 1;
				item = point;
				item.j++;
				Point item2 = point;
				item2.j--;
				for (int i = num; i <= num2; i++)
				{
					item.i = (int)((short)i);
					item2.i = (int)((short)i);
					if (item.j < height - 1 && this.pixels[item.i + item.j * width] == color)
					{
						queue.Enqueue(item);
					}
					if (item2.j >= 0 && this.pixels[item2.i + item2.j * width] == color)
					{
						queue.Enqueue(item2);
					}
				}
			}
		}
		targetTex.SetPixels(this.pixels);
		targetTex.Apply();
	}

	public static void FloodFill(int x, int y, AdvancedMobilePaint paintEngine, byte newMark)
	{
		UnityEngine.Debug.Log("FloodFill");
		byte b = paintEngine.source[paintEngine.texWidth * y + x];
		Queue<int> queue = new Queue<int>();
		Queue<int> queue2 = new Queue<int>();
		queue.Enqueue(x);
		queue2.Enqueue(y);
		while (queue.Count > 0)
		{
			int num = queue.Dequeue();
			int num2 = queue2.Dequeue();
			if (num2 - 1 > -1)
			{
				int num3 = paintEngine.texWidth * (num2 - 1) + num;
				int num4 = num3 * 4;
				if (paintEngine.source[num3] == b)
				{
					queue.Enqueue(num);
					queue2.Enqueue(num2 - 1);
					paintEngine.pixels[num4] = paintEngine.paintColor.r;
					paintEngine.pixels[num4 + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[num4 + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[num4 + 3] = paintEngine.paintColor.a;
					paintEngine.source[num3] = newMark;
				}
			}
			if (num + 1 < paintEngine.texWidth)
			{
				int num3 = paintEngine.texWidth * num2 + (num + 1);
				int num4 = num3 * 4;
				if (paintEngine.source[num3] == b)
				{
					queue.Enqueue(num + 1);
					queue2.Enqueue(num2);
					paintEngine.pixels[num4] = paintEngine.paintColor.r;
					paintEngine.pixels[num4 + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[num4 + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[num4 + 3] = paintEngine.paintColor.a;
					paintEngine.source[num3] = newMark;
				}
			}
			if (num - 1 > -1)
			{
				int num3 = paintEngine.texWidth * num2 + (num - 1);
				int num4 = num3 * 4;
				if (paintEngine.source[num3] == b)
				{
					queue.Enqueue(num - 1);
					queue2.Enqueue(num2);
					paintEngine.pixels[num4] = paintEngine.paintColor.r;
					paintEngine.pixels[num4 + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[num4 + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[num4 + 3] = paintEngine.paintColor.a;
					paintEngine.source[num3] = newMark;
				}
			}
			if (num2 + 1 < paintEngine.texHeight)
			{
				int num3 = paintEngine.texWidth * (num2 + 1) + num;
				int num4 = num3 * 4;
				if (paintEngine.source[num3] == b)
				{
					queue.Enqueue(num);
					queue2.Enqueue(num2 + 1);
					paintEngine.pixels[num4] = paintEngine.paintColor.r;
					paintEngine.pixels[num4 + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[num4 + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[num4 + 3] = paintEngine.paintColor.a;
					paintEngine.source[num3] = newMark;
				}
			}
		}
	}

	public static void FloodFillTwo(int x, int y, AdvancedMobilePaint paintEngine, byte newMark)
	{
		UnityEngine.Debug.Log("FloodFill");
		byte b = paintEngine.source[paintEngine.texWidth * y + x];
		Queue<int> queue = new Queue<int>();
		Queue<int> queue2 = new Queue<int>();
		queue.Enqueue(x);
		queue2.Enqueue(y);
		while (queue.Count > 0)
		{
			int num = queue.Dequeue();
			int num2 = queue2.Dequeue();
			if (num2 - 1 > -1)
			{
				int num3 = paintEngine.texWidth * (num2 - 1) + num;
				int num4 = num3 * 4;
				if (paintEngine.source[num3] == b)
				{
					queue.Enqueue(num);
					queue2.Enqueue(num2 - 1);
					paintEngine.pixels[num4] = paintEngine.paintColor.r;
					paintEngine.pixels[num4 + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[num4 + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[num4 + 3] = paintEngine.paintColor.a;
					paintEngine.source[num3] = newMark;
				}
			}
			if (num + 1 < paintEngine.texWidth)
			{
				int num3 = paintEngine.texWidth * num2 + (num + 1);
				int num4 = num3 * 4;
				if (paintEngine.source[num3] == b)
				{
					queue.Enqueue(num + 1);
					queue2.Enqueue(num2);
					paintEngine.pixels[num4] = paintEngine.paintColor.r;
					paintEngine.pixels[num4 + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[num4 + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[num4 + 3] = paintEngine.paintColor.a;
					paintEngine.source[num3] = newMark;
				}
			}
			if (num - 1 > -1)
			{
				int num3 = paintEngine.texWidth * num2 + (num - 1);
				int num4 = num3 * 4;
				if (paintEngine.source[num3] == b)
				{
					queue.Enqueue(num - 1);
					queue2.Enqueue(num2);
					paintEngine.pixels[num4] = paintEngine.paintColor.r;
					paintEngine.pixels[num4 + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[num4 + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[num4 + 3] = paintEngine.paintColor.a;
					paintEngine.source[num3] = newMark;
				}
			}
			if (num2 + 1 < paintEngine.texHeight)
			{
				int num3 = paintEngine.texWidth * (num2 + 1) + num;
				int num4 = num3 * 4;
				if (paintEngine.source[num3] == b)
				{
					queue.Enqueue(num);
					queue2.Enqueue(num2 + 1);
					paintEngine.pixels[num4] = paintEngine.paintColor.r;
					paintEngine.pixels[num4 + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[num4 + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[num4 + 3] = paintEngine.paintColor.a;
					paintEngine.source[num3] = newMark;
				}
			}
		}
	}

	public IEnumerator SlowFill(int x, int y, AdvancedMobilePaint paintEngine, byte newMark)
	{
		UnityEngine.Debug.Log("FloodFill");
		int scanBeforeUpdate = 1000;
		int index = 0;
		byte hitColorA = paintEngine.source[paintEngine.texWidth * y + x];
		Queue<int> fillPointX = new Queue<int>();
		Queue<int> fillPointY = new Queue<int>();
		fillPointX.Enqueue(x);
		fillPointY.Enqueue(y);
		int pixel = 0;
		int sourceA = 0;
		while (fillPointX.Count > 0)
		{
			int ptsx = fillPointX.Dequeue();
			int ptsy = fillPointY.Dequeue();
			if (ptsy - 1 > -1)
			{
				sourceA = paintEngine.texWidth * (ptsy - 1) + ptsx;
				pixel = sourceA * 4;
				if (paintEngine.source[sourceA] == hitColorA)
				{
					fillPointX.Enqueue(ptsx);
					fillPointY.Enqueue(ptsy - 1);
					paintEngine.pixels[pixel] = paintEngine.paintColor.r;
					paintEngine.pixels[pixel + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[pixel + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[pixel + 3] = paintEngine.paintColor.a;
					paintEngine.source[sourceA] = newMark;
				}
			}
			if (ptsx + 1 < paintEngine.texWidth)
			{
				sourceA = paintEngine.texWidth * ptsy + (ptsx + 1);
				pixel = sourceA * 4;
				if (paintEngine.source[sourceA] == hitColorA)
				{
					fillPointX.Enqueue(ptsx + 1);
					fillPointY.Enqueue(ptsy);
					paintEngine.pixels[pixel] = paintEngine.paintColor.r;
					paintEngine.pixels[pixel + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[pixel + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[pixel + 3] = paintEngine.paintColor.a;
					paintEngine.source[sourceA] = newMark;
				}
			}
			if (ptsx - 1 > -1)
			{
				sourceA = paintEngine.texWidth * ptsy + (ptsx - 1);
				pixel = sourceA * 4;
				if (paintEngine.source[sourceA] == hitColorA)
				{
					fillPointX.Enqueue(ptsx - 1);
					fillPointY.Enqueue(ptsy);
					paintEngine.pixels[pixel] = paintEngine.paintColor.r;
					paintEngine.pixels[pixel + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[pixel + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[pixel + 3] = paintEngine.paintColor.a;
					paintEngine.source[sourceA] = newMark;
				}
			}
			if (ptsy + 1 < paintEngine.texHeight)
			{
				sourceA = paintEngine.texWidth * (ptsy + 1) + ptsx;
				pixel = sourceA * 4;
				if (paintEngine.source[sourceA] == hitColorA)
				{
					fillPointX.Enqueue(ptsx);
					fillPointY.Enqueue(ptsy + 1);
					paintEngine.pixels[pixel] = paintEngine.paintColor.r;
					paintEngine.pixels[pixel + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[pixel + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[pixel + 3] = paintEngine.paintColor.a;
					paintEngine.source[sourceA] = newMark;
				}
			}
			index++;
			if (index > scanBeforeUpdate)
			{
				index = 0;
				this.drawText.LoadRawTextureData(paintEngine.pixels);
				this.drawText.Apply(false);
				yield return 0;
			}
		}
		this.drawText.LoadRawTextureData(paintEngine.pixels);
		this.drawText.Apply(false);
		TLogger.Instance.Log(Time.realtimeSinceStartup - this.time);
		yield return 0;
		yield break;
	}

	public void OnHideOverlap(bool isOn)
	{
		this.sourceImg.gameObject.SetActive(isOn);
		this.SaveTestJSON();
	}

	private void SaveTestJSON()
	{
		this.tPaletteData.entities = new PaletteEntity[this.tEntities.Count];
		int num = 0;
		foreach (KeyValuePair<Color, List<int>> keyValuePair in this.tEntities)
		{
			Color key = keyValuePair.Key;
			this.tPaletteData.entities[num].color = key;
			this.tPaletteData.entities[num].indexes = keyValuePair.Value.ToArray();
			num++;
		}
		FileHelper.SaveStringToFile(FloodTest.IMG_NAME_INDEX + ".txt", JsonConvert.SerializeObject(this.tPaletteData, Formatting.None, new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore
		}));
	}

	public void OnFillTypeClick(bool isSlow)
	{
		if (isSlow)
		{
			this.fType = FloodTest.FillType.Slow;
		}
		else
		{
			this.fType = FloodTest.FillType.Fast;
		}
	}

	public void OnNumTestClick(bool isOn)
	{
		if (isOn)
		{
			if (this.numbers.Count > 0)
			{
				for (int i = 0; i < this.numbers.Count; i++)
				{
					this.numbers[i].gameObject.SetActive(true);
				}
			}
			int num = UnityEngine.Random.Range(30, 100);
			for (int j = 0; j < num; j++)
			{
				Text text = this.CreateNumView(new Vector2((float)UnityEngine.Random.Range(-400, 400), (float)UnityEngine.Random.Range(-400, 400)), UnityEngine.Random.Range(1, 99));
				this.numbers.Add(text.gameObject);
			}
			TLogger.Instance.Log("total  nums " + this.numbers.Count);
		}
		else
		{
			for (int k = 0; k < this.numbers.Count; k++)
			{
				this.numbers[k].gameObject.SetActive(false);
			}
		}
	}

	public void OnZoomClick(bool magnify)
	{
		if (magnify)
		{
			float d = 1.05f;
			base.transform.localScale *= d;
		}
		else
		{
			float d2 = 0.95f;
			base.transform.localScale *= d2;
		}
	}

	private Text CreateNumView(Vector2 position, int value)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.numPrefab);
		gameObject.transform.SetParent(this.numParent);
		Text component = gameObject.GetComponent<Text>();
		component.text = value.ToString();
		component.rectTransform.anchoredPosition = position;
		return component;
	}

	private void JsonTest()
	{
		UnityEngine.Debug.Log(JsonConvert.DeserializeObject<PaletteData>(FileHelper.LoadTextFromFile("example.txt")).entities[0].color);
		PaletteData paletteData = default(PaletteData);
		paletteData.entities = new PaletteEntity[3];
		paletteData.entities[0] = default(PaletteEntity);
		paletteData.entities[0].color = new Color(0.443f, 0.33f, 1f);
		paletteData.entities[0].indexes = new int[]
		{
			33,
			112,
			551,
			12312,
			44431,
			414312
		};
		paletteData.entities[1] = default(PaletteEntity);
		paletteData.entities[1].color = new Color(0f, 0f, 1f);
		paletteData.entities[1].indexes = new int[]
		{
			2,
			33,
			14341,
			76412
		};
		paletteData.entities[2] = default(PaletteEntity);
		paletteData.entities[2].color = new Color(1f, 0.93f, 0.5412f);
		paletteData.entities[2].indexes = new int[]
		{
			22,
			433,
			4341,
			96412
		};
		FileHelper.SaveStringToFile("example.txt", JsonConvert.SerializeObject(paletteData, Formatting.None, new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore
		}));
	}

	public void OnBackClick()
	{
		Resources.UnloadUnusedAssets();
		SceneManager.LoadScene("menu");
	}

	public GameObject numPrefab;

	public Transform numParent;

	public List<GameObject> numbers;

	public SingleTouchHandler touch;

	private FloodTest.FillType fType;

	public static int IMG_NAME_INDEX = 111;

	[SerializeField]
	private RawImage sourceImg;

	[SerializeField]
	private byte[] sourceAlpha;

	[SerializeField]
	private RawImage drawImg;

	private Texture2D lineContourText;

	private Texture2D colorSourceText;

	private Texture2D drawText;

	private Rect imageRect;

	private float time;

	private Color[] pixels;

	private List<NumTrigger> triggers;

	public AdvancedMobilePaint paintEngine;

	private Dictionary<Color, List<int>> tEntities;

	private PaletteData tPaletteData;

	public enum FillType
	{
		Slow,
		Fast
	}
}

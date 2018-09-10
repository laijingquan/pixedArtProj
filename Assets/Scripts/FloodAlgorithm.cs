// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class FloodAlgorithm
{
	public static void FloodFill(int x, int y, AdvancedMobilePaint paintEngine, byte newMark)
	{
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

	public static void FloodFillCopy(int x, int y, AdvancedMobilePaint paintEngine, byte[] pixels, byte newMark)
	{
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
					pixels[num4] = paintEngine.paintColor.r;
					pixels[num4 + 1] = paintEngine.paintColor.g;
					pixels[num4 + 2] = paintEngine.paintColor.b;
					pixels[num4 + 3] = paintEngine.paintColor.a;
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
					pixels[num4] = paintEngine.paintColor.r;
					pixels[num4 + 1] = paintEngine.paintColor.g;
					pixels[num4 + 2] = paintEngine.paintColor.b;
					pixels[num4 + 3] = paintEngine.paintColor.a;
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
					pixels[num4] = paintEngine.paintColor.r;
					pixels[num4 + 1] = paintEngine.paintColor.g;
					pixels[num4 + 2] = paintEngine.paintColor.b;
					pixels[num4 + 3] = paintEngine.paintColor.a;
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
					pixels[num4] = paintEngine.paintColor.r;
					pixels[num4 + 1] = paintEngine.paintColor.g;
					pixels[num4 + 2] = paintEngine.paintColor.b;
					pixels[num4 + 3] = paintEngine.paintColor.a;
					paintEngine.source[num3] = newMark;
				}
			}
		}
	}

	public static List<int> FloodFillExtended(int x, int y, AdvancedMobilePaint paintEngine, Color32 newColor, byte newMark)
	{
		byte b = paintEngine.source[paintEngine.texWidth * y + x];
		List<int> list = new List<int>();
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
					paintEngine.pixels[num4] = newColor.r;
					paintEngine.pixels[num4 + 1] = newColor.g;
					paintEngine.pixels[num4 + 2] = newColor.b;
					paintEngine.pixels[num4 + 3] = newColor.a;
					paintEngine.source[num3] = newMark;
					list.Add(num3);
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
					paintEngine.pixels[num4] = newColor.r;
					paintEngine.pixels[num4 + 1] = newColor.g;
					paintEngine.pixels[num4 + 2] = newColor.b;
					paintEngine.pixels[num4 + 3] = newColor.a;
					paintEngine.source[num3] = newMark;
					list.Add(num3);
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
					paintEngine.pixels[num4] = newColor.r;
					paintEngine.pixels[num4 + 1] = newColor.g;
					paintEngine.pixels[num4 + 2] = newColor.b;
					paintEngine.pixels[num4 + 3] = newColor.a;
					paintEngine.source[num3] = newMark;
					list.Add(num3);
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
					paintEngine.pixels[num4] = newColor.r;
					paintEngine.pixels[num4 + 1] = newColor.g;
					paintEngine.pixels[num4 + 2] = newColor.b;
					paintEngine.pixels[num4 + 3] = newColor.a;
					paintEngine.source[num3] = newMark;
					list.Add(num3);
				}
			}
		}
		return list;
	}

	public static void FillIndexes(List<int> indexes, AdvancedMobilePaint paintEngine, Color32 newColor)
	{
		for (int i = 0; i < indexes.Count; i++)
		{
			int num = indexes[i] * 4;
			paintEngine.pixels[num] = newColor.r;
			paintEngine.pixels[num + 1] = newColor.g;
			paintEngine.pixels[num + 2] = newColor.b;
			paintEngine.pixels[num + 3] = newColor.a;
		}
	}
}

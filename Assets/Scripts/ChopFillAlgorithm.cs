// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class ChopFillAlgorithm
{
	public static void FindAndFillId(ChopMobilePaint paintEngine, byte[] pixels, short markTag)
	{
		for (int i = 0; i < paintEngine.source.Length; i++)
		{
			if (paintEngine.source[i] == markTag)
			{
				int num = i * 4;
				pixels[num] = paintEngine.paintColor.r;
				pixels[num + 1] = paintEngine.paintColor.g;
				pixels[num + 2] = paintEngine.paintColor.b;
				pixels[num + 3] = paintEngine.paintColor.a;
			}
		}
	}

	public static void Fill(int x, int y, ChopMobilePaint paintEngine, short markTag)
	{
		List<int> list = paintEngine.zones[(int)markTag];
		for (int i = 0; i < list.Count; i++)
		{
			int num = list[i] * 4;
			paintEngine.pixels[num] = paintEngine.paintColor.r;
			paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
			paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
			paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
		}
	}

	public static void FillCopy(int x, int y, ChopMobilePaint paintEngine, byte[] pixels, short markTag)
	{
		List<int> list = paintEngine.zones[(int)markTag];
		for (int i = 0; i < list.Count; i++)
		{
			int num = list[i] * 4;
			pixels[num] = paintEngine.paintColor.r;
			pixels[num + 1] = paintEngine.paintColor.g;
			pixels[num + 2] = paintEngine.paintColor.b;
			pixels[num + 3] = paintEngine.paintColor.a;
		}
	}

	public static List<int> MarkArea(int x, int y, ChopMobilePaint paintEngine, short oldMark, short newMark)
	{
		short num = paintEngine.source[paintEngine.texWidth * y + x];
		if (num != oldMark)
		{
			return null;
		}
		List<int> list = new List<int>();
		Queue<int> queue = new Queue<int>();
		Queue<int> queue2 = new Queue<int>();
		queue.Enqueue(x);
		queue2.Enqueue(y);
		while (queue.Count > 0)
		{
			int num2 = queue.Dequeue();
			int num3 = queue2.Dequeue();
			if (num3 - 1 > -1)
			{
				int num4 = paintEngine.texWidth * (num3 - 1) + num2;
				//int num5 = num4 * 4;
				if (paintEngine.source[num4] == num)
				{
					queue.Enqueue(num2);
					queue2.Enqueue(num3 - 1);
					paintEngine.source[num4] = newMark;
					list.Add(num4);
				}
			}
			if (num2 + 1 < paintEngine.texWidth)
			{
				int num4 = paintEngine.texWidth * num3 + (num2 + 1);
				//int num5 = num4 * 4;
				if (paintEngine.source[num4] == num)
				{
					queue.Enqueue(num2 + 1);
					queue2.Enqueue(num3);
					paintEngine.source[num4] = newMark;
					list.Add(num4);
				}
			}
			if (num2 - 1 > -1)
			{
				int num4 = paintEngine.texWidth * num3 + (num2 - 1);
				//int num5 = num4 * 4;
				if (paintEngine.source[num4] == num)
				{
					queue.Enqueue(num2 - 1);
					queue2.Enqueue(num3);
					paintEngine.source[num4] = newMark;
					list.Add(num4);
				}
			}
			if (num3 + 1 < paintEngine.texHeight)
			{
				int num4 = paintEngine.texWidth * (num3 + 1) + num2;
				//int num5 = num4 * 4;
				if (paintEngine.source[num4] == num)
				{
					queue.Enqueue(num2);
					queue2.Enqueue(num3 + 1);
					paintEngine.source[num4] = newMark;
					list.Add(num4);
				}
			}
		}
		return list;
	}
}

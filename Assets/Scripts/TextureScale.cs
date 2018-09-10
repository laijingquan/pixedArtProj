// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TextureScale
{
	public static void Scale(Texture2D tex, int newWidth, int newHeight)
	{
		TextureScale.texColors = tex.GetPixels();
		TextureScale.newColors = new Color[newWidth * newHeight];
		TextureScale.ratioX = 1f / ((float)newWidth / (float)(tex.width - 1));
		TextureScale.ratioY = 1f / ((float)newHeight / (float)(tex.height - 1));
		TextureScale.w = tex.width;
		TextureScale.w2 = newWidth;
		TextureScale.BilinearScale(0, newHeight);
		tex.Resize(newWidth, newHeight);
		tex.SetPixels(TextureScale.newColors);
		tex.Apply();
	}

	private static void BilinearScale(int start, int end)
	{
		for (int i = start; i < end; i++)
		{
			int num = (int)Mathf.Floor((float)i * TextureScale.ratioY);
			int num2 = num * TextureScale.w;
			int num3 = (num + 1) * TextureScale.w;
			int num4 = i * TextureScale.w2;
			for (int j = 0; j < TextureScale.w2; j++)
			{
				int num5 = (int)Mathf.Floor((float)j * TextureScale.ratioX);
				float value = (float)j * TextureScale.ratioX - (float)num5;
				TextureScale.newColors[num4 + j] = TextureScale.ColorLerpUnclamped(TextureScale.ColorLerpUnclamped(TextureScale.texColors[num2 + num5], TextureScale.texColors[num2 + num5 + 1], value), TextureScale.ColorLerpUnclamped(TextureScale.texColors[num3 + num5], TextureScale.texColors[num3 + num5 + 1], value), (float)i * TextureScale.ratioY - (float)num);
			}
		}
	}

	private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
	{
		return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
	}

	public static void CopyIntoTexture(Texture2D sourceTex, Texture2D targetTex)
	{
		Rect source = new Rect(0f, 0f, (float)targetTex.width, (float)targetTex.height);
		TextureScale.GPUCopy(sourceTex, targetTex.width, targetTex.height);
		targetTex.ReadPixels(source, 0, 0, false);
		targetTex.Apply(false);
	}

	public static void CopyArrayIntoTexture(Texture2D[] sourceTexs, Texture2D targetTex)
	{
		Rect source = new Rect(0f, 0f, (float)targetTex.width, (float)targetTex.height);
		TextureScale.GPUArrayCopy(sourceTexs, targetTex.width, targetTex.height);
		targetTex.ReadPixels(source, 0, 0, false);
		targetTex.Apply(false);
	}

	private static void GPUCopy(Texture2D src, int width, int height)
	{
		RenderTexture renderTarget = new RenderTexture(width, height, 32);
		Graphics.SetRenderTarget(renderTarget);
		GL.LoadPixelMatrix(0f, 1f, 1f, 0f);
		GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
		Graphics.DrawTexture(new Rect(0f, 0f, 1f, 1f), src);
	}

	private static void GPUArrayCopy(Texture2D[] srcs, int width, int height)
	{
		RenderTexture renderTarget = new RenderTexture(width, height, 32);
		Graphics.SetRenderTarget(renderTarget);
		GL.LoadPixelMatrix(0f, 1f, 1f, 0f);
		GL.Clear(true, true, new Color(1f, 1f, 1f, 1f));
		for (int i = 0; i < srcs.Length; i++)
		{
			Graphics.DrawTexture(new Rect(0f, 0f, 1f, 1f), srcs[i]);
		}
	}

	private static Color[] texColors;

	private static Color[] newColors;

	private static int w;

	private static float ratioX;

	private static float ratioY;

	private static int w2;
}

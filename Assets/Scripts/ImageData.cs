// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class ImageData
{
	public ImageData(string[] baseUrl, string relativePath)
	{
		this.baseUrl = baseUrl;
		this.relativePath = relativePath;
	}

	public string CacheKey
	{
		get
		{
			return "elink" + this.relativePath.Replace("/", "-");
		}
	}

	public string[] baseUrl;

	public string relativePath;
}

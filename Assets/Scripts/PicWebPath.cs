// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class PicWebPath
{
	public string CacheKey
	{
		get
		{
			return this.icon.Replace("/", "-");
		}
	}

	public static PicWebPath FromWebPic(WebPicData webPic, string[] baseUrls)
	{
		return new PicWebPath
		{
			icon = webPic.icon,
			lineart = webPic.lineart,
			colored = webPic.colored,
			json = webPic.json,
			baseUrls = baseUrls
		};
	}

	public string[] baseUrls;

	public string icon;

	public string lineart;

	public string colored;

	public string json;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

[Serializable]
public class WebPicData
{
	public FillAlgorithm FillType { get; private set; }

	public bool IsValid()
	{
		this.FillType = this.ParseFillType(this.fillType);
		return !string.IsNullOrEmpty(this.lineart) && !string.IsNullOrEmpty(this.icon) && (this.FillType != FillAlgorithm.Flood || !string.IsNullOrEmpty(this.colored)) && !string.IsNullOrEmpty(this.json);
	}

	private FillAlgorithm ParseFillType(string fType)
	{
		if (string.IsNullOrEmpty(fType))
		{
			return FillAlgorithm.Flood;
		}
		if (fType.Equals("chop"))
		{
			return FillAlgorithm.Chop;
		}
		return FillAlgorithm.Flood;
	}

	public static List<PictureLabel> ParseLabels(string[] labels)
	{
		List<PictureLabel> list = new List<PictureLabel>();
		foreach (string text in labels)
		{
			if (text.Equals("new"))
			{
				list.Add(PictureLabel.New);
			}
			else if (text.Equals("daily"))
			{
				list.Add(PictureLabel.Daily);
			}
			else if (text.Equals("fb"))
			{
				list.Add(PictureLabel.Facebook);
			}
		}
		return list;
	}

	public int id;

	public int packId;

	public string lineart;

	public string icon;

	public string colored;

	public string json;

	public string[] labels;

	public int[] categories;

	public string fillType;
}

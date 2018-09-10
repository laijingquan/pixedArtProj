// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class DbDebugTest
{
	private static Texture2D GetIconText()
	{
		if (DbDebugTest.sharedTex == null)
		{
			DbDebugTest.sharedTex = new Texture2D(50, 50, TextureFormat.RGBA32, false);
			byte[] array = new byte[DbDebugTest.sharedTex.width * DbDebugTest.sharedTex.height * 4];
			for (int i = 0; i < array.Length; i++)
			{
				if (i % 4 == 0)
				{
					array[i] = byte.MaxValue;
				}
				else
				{
					array[i] = (byte)UnityEngine.Random.Range(0, 256);
				}
			}
			DbDebugTest.sharedTex.LoadRawTextureData(array);
			DbDebugTest.sharedTex.Apply(false);
			FileHelper.SaveTextureToFile(DbDebugTest.sharedTex, "0", "shared");
		}
		return DbDebugTest.sharedTex;
	}

	public static void AddRange(int count)
	{
		List<PictureData> getMainPageData = SharedData.Instance.GetMainPageData;
		int num = 0;
		for (int i = 0; i < getMainPageData.Count; i++)
		{
			if (getMainPageData[i].picType == PictureType.Web)
			{
				DbDebugTest.AddItem(getMainPageData[i]);
				num++;
				if (num == count)
				{
					break;
				}
			}
		}
	}

	private static void AddItem(PictureData pictureData)
	{
		int id = pictureData.Id;
		string filename = id + "i";
		string directory = pictureData.PackId.ToString();
		Texture2D iconText = DbDebugTest.GetIconText();
		FileHelper.SaveTextureToFile(iconText, directory, filename);
		SharedData.Instance.AddPictureToPack(pictureData);
		PictureSaveData pictureSaveData = new PictureSaveData();
		pictureSaveData.PackId = pictureData.PackId;
		pictureSaveData.Id = pictureData.Id;
		pictureSaveData.hintsUsed = UnityEngine.Random.Range(0, 5);
		pictureSaveData.iconMaskPath = "0/shared";
		if (UnityEngine.Random.Range(0, 4) == 1)
		{
			pictureSaveData.progres = 100;
		}
		else
		{
			pictureSaveData.progres = UnityEngine.Random.Range(1, 99);
		}
		pictureSaveData.timeSpent = UnityEngine.Random.Range(1f, 99999f);
		pictureSaveData.steps = new List<SaveStep>();
		int num = UnityEngine.Random.Range(400, 1000);
		for (int i = 0; i < num; i++)
		{
			SaveStep item = new SaveStep
			{
				colorId = UnityEngine.Random.Range(0, 99),
				point = new Point(UnityEngine.Random.Range(0, 2048), UnityEngine.Random.Range(0, 2048))
			};
			pictureSaveData.steps.Add(item);
		}
		SharedData.Instance.AddSave(pictureSaveData);
	}

	private static Texture2D sharedTex;

	private const string sharedSaveIconPath = "shared";
}

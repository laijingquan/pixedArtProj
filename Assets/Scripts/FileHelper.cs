// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.IO;
using UnityEngine;

public static class FileHelper
{
	public static void SaveStringToFile(string fileName, string text)
	{
		File.WriteAllText(Application.persistentDataPath + "/" + fileName, text);
	}

	public static void SaveStringToFile(string text, string directory, string filename)
	{
		Directory.CreateDirectory(Application.persistentDataPath + "/" + directory);
		File.WriteAllText(string.Concat(new string[]
		{
			Application.persistentDataPath,
			"/",
			directory,
			"/",
			filename
		}), text);
	}

	public static string LoadTextFromFile(string fileName)
	{
		string result = null;
		if (File.Exists(Application.persistentDataPath + "/" + fileName))
		{
			result = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
		}
		return result;
	}

	public static string LoadTextAssetResource(string fileName)
	{
		TextAsset textAsset = Resources.Load(fileName) as TextAsset;
		return (!(textAsset != null)) ? null : textAsset.text;
	}

	public static void SaveTextureToFile(Texture2D texture, string directory, string filename)
	{
		Directory.CreateDirectory(Application.persistentDataPath + "/" + directory);
		File.WriteAllBytes(string.Concat(new string[]
		{
			Application.persistentDataPath,
			"/",
			directory,
			"/",
			filename
		}), texture.EncodeToPNG());
	}

	public static void SaveTextureToFile(Texture2D texture, string path)
	{
		File.WriteAllBytes(path, texture.EncodeToPNG());
	}

	public static void SaveRawBytesToFile(byte[] bytes, string directory, string filename)
	{
		Directory.CreateDirectory(Application.persistentDataPath + "/" + directory);
		File.WriteAllBytes(string.Concat(new string[]
		{
			Application.persistentDataPath,
			"/",
			directory,
			"/",
			filename
		}), bytes);
	}

	public static bool FileExist(string directory, string filename)
	{
		return File.Exists(string.Concat(new string[]
		{
			Application.persistentDataPath,
			"/",
			directory,
			"/",
			filename
		}));
	}

	public static Texture2D LoadTextureFromFile(string path)
	{
		Texture2D texture2D = new Texture2D(2, 2);
		byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/" + path);
		texture2D.LoadImage(data);
		return texture2D;
	}

	public static Texture2D LoadCacheTextureFromFile(string path)
	{
		Texture2D texture2D = new Texture2D(2, 2);
		byte[] data = File.ReadAllBytes(path);
		texture2D.LoadImage(data);
		return texture2D;
	}

	public static void DeleteFile(string directory, string filename)
	{
		if (FileHelper.FileExist(directory, filename))
		{
			File.Delete(string.Concat(new string[]
			{
				Application.persistentDataPath,
				"/",
				directory,
				"/",
				filename
			}));
		}
	}

	public static void DeleteFile(string filePath)
	{
		if (File.Exists(Application.persistentDataPath + "/" + filePath))
		{
			File.Delete(Application.persistentDataPath + "/" + filePath);
		}
	}
}

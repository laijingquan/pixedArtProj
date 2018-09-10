// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalizationService : MonoBehaviour
{
	public static LocalizationService Instance
	{
		get
		{
			return LocalizationService.instance;
		}
	}

	public static string LocalizationFilePath
	{
		get
		{
			return LocalizationService.LocalizationPath + LocalizationService._localization;
		}
	}

	public string Localization
	{
		get
		{
			return LocalizationService._localization;
		}
		set
		{
			LocalizationService._localization = value;
			this.localizationLibrary = this.LoadLocalizeFileHelper();
			this.SetLocalization(value);
			this.OnChangeLocalization.SafeInvoke();
		}
	}

	private void Awake()
	{
		LocalizationService.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
		this.Initialize();
	}

	private void Initialize()
	{
		this.Localization = this.GetLocalization();
		this.localizationLibrary = this.LoadLocalizeFileHelper();
	}

	private IEnumerator GetLocalizationCoroutine(Action callback)
	{
		yield return 0;
		yield break;
	}

	private static Dictionary<string, string> ParseLocalizeFile(string[,] grid)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(grid.GetUpperBound(0) + 1);
		for (int i = 1; i <= grid.GetUpperBound(1); i++)
		{
			for (int j = 1; j <= grid.GetUpperBound(0); j++)
			{
				if (!string.IsNullOrEmpty(grid[0, i]) && !string.IsNullOrEmpty(grid[j, i]))
				{
					if (!dictionary.ContainsKey(grid[0, i]))
					{
						dictionary.Add(grid[0, i], grid[j, i]);
					}
					else
					{
						UnityEngine.Debug.LogError(string.Format("Key {0} already exist", grid[0, i]));
					}
				}
			}
		}
		return dictionary;
	}

	public string GetTextByKey(string key)
	{
		return this.GetTextByKeyWithLocalize(key, LocalizationService._localization);
	}

	public string GetTextByKeyWithLocalize(string key, string localize)
	{
		if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(localize))
		{
			return "[EMPTY]";
		}
		string result;
		if (this.localizationLibrary.TryGetValue(key, out result))
		{
			return result;
		}
		return string.Format("[ERROR KEY {0}]", key);
	}

	private string GetLocalization()
	{
		return SystemUtils.GetLanguage();
	}

	private void SetLocalization(string localize)
	{
		PlayerPrefs.SetString("localization", localize);
	}

	public string[] GetLocalizations()
	{
		string[] array = new string[this.localizationLibrary.Count];
		int num = 0;
		foreach (KeyValuePair<string, string> keyValuePair in this.localizationLibrary)
		{
			array[num] = keyValuePair.Key;
			num++;
		}
		return array;
	}

	public Dictionary<string, string> LoadLocalizeFileHelper()
	{
		TextAsset textAsset = Resources.Load(LocalizationService.LocalizationFilePath, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			if (this.Localization != "English")
			{
				this.LoadDefault();
			}
			return null;
		}
		string[,] grid = CSVReader.SplitCsvGrid(textAsset.text);
		return LocalizationService.ParseLocalizeFile(grid);
	}

	private void LoadDefault()
	{
		this.Localization = "English";
	}

	public static string[] GetLocalizationKeys()
	{
		TextAsset textAsset = Resources.Load(LocalizationService.LocalizationFilePath, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			return null;
		}
		string[,] grid = CSVReader.SplitCsvGrid(textAsset.text);
		Dictionary<string, string> dictionary = LocalizationService.ParseLocalizeFile(grid);
		return dictionary.Keys.ToArray<string>();
	}

	private static LocalizationService instance;

	private const string DefaultLocalizationName = "English";

	public static string LocalizationPath = "Localization/";

	public Action OnChangeLocalization;

	private static string _localization = "English";

	private Dictionary<string, string> localizationLibrary;
}

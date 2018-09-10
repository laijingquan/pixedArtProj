// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsentPopup : MonoBehaviour
{
	private void OnEnable()
	{
		SystemLanguage systemLanguage = Application.systemLanguage;
		int num = this.languages.IndexOf(systemLanguage);
		if (num == -1)
		{
			this.active = 0;
		}
		else
		{
			this.active = num;
		}
		this.localizations[this.active].SetActive(true);
	}

	public List<GameObject> localizations;

	private int active;

	public List<SystemLanguage> languages = new List<SystemLanguage>
	{
		SystemLanguage.English,
		SystemLanguage.French,
		SystemLanguage.German,
		SystemLanguage.Italian,
		SystemLanguage.Portuguese,
		SystemLanguage.Spanish,
		SystemLanguage.Swedish,
		SystemLanguage.Turkish
	};
}

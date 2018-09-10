// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPage : Page
{
	public void LoadContent()
	{
		this.PrepareForContnentLoad();
		this.restoreBtnSingle.SetActive(false);
		this.restoreBtnInBlock.SetActive(false);
		this.adsBtnInBlock.SetActive(false);
		this.adsBtnSingle.SetActive(true);
		if (!GeneralSettings.CanUseLegacyDesign)
		{
			this.design.SetActive(false);
		}
		this.filterCheckmark.SetActive(GeneralSettings.FilterCompleted);
		if (GeneralSettings.AdsDisabled)
		{
			this.HideAdsButton();
		}
		base.StartCoroutine(base.DelayAction(3, 0f, new Action(this.ContentLoadComplete)));
	}

	public void UpdateFilterCompletedState()
	{
		this.filterCheckmark.SetActive(GeneralSettings.FilterCompleted);
	}

	public void HideAdsButton()
	{
		this.separator.SetActive(false);
		this.adsBtnSingle.SetActive(false);
		this.adsBtnInBlock.SetActive(false);
	}

	public void DebugLang()
	{
		LocalizationService.Instance.Localization = this.langs[this.index];
		this.index++;
		if (this.index == this.langs.Count)
		{
			this.index = 0;
		}
	}

	protected override void OnBeginOpenning()
	{
	}

	protected override void OnOpened()
	{
	}

	protected override void OnBeginClosing()
	{
	}

	protected override void OnClosed()
	{
	}

	[SerializeField]
	private GameObject restoreBtnSingle;

	[SerializeField]
	private GameObject restoreBtnInBlock;

	[SerializeField]
	private GameObject adsBtnSingle;

	[SerializeField]
	private GameObject separator;

	[SerializeField]
	private GameObject adsBtnInBlock;

	[SerializeField]
	private GameObject filterCheckmark;

	[SerializeField]
	private GameObject design;

	private int index;

	private List<string> langs = new List<string>
	{
		"English",
		"Russian",
		"French",
		"ChineseTraditional",
		"ChineseSimplified",
		"German",
		"Hindi",
		"Italian",
		"Japanese",
		"Korean",
		"Portuguese",
		"Thai",
		"Spanish",
		"Swedish",
		"Turkish",
		"Vietnamese"
	};
}

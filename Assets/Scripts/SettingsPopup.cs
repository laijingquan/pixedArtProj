// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : SlidePoppup
{
	public void SetSafeLayoutOffset(int yOffset)
	{
		this.openedPosition -= new Vector2(0f, (float)yOffset);
	}

	public void UpdateFilterCompletedState()
	{
		this.filterCheckmark.SetActive(GeneralSettings.FilterCompleted);
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

	protected override void WillBecomeVisible()
	{
		if (!this.inited)
		{
			this.defaultOpenedPos = this.openedPosition;
			this.inited = true;
		}
		this.filterCheckmark.SetActive(GeneralSettings.FilterCompleted);
		this.openedPosition = this.defaultOpenedPos;
		if (GeneralSettings.AdsDisabled)
		{
			this.removeAds.SetActive(false);
			this.openedPosition += new Vector2(0f, (float)this.btnHeight);
		}
		if (!GeneralSettings.CanUseLegacyDesign)
		{
			this.design.SetActive(false);
			this.openedPosition += new Vector2(0f, (float)this.btnHeight);
		}
		else
		{
			this.lastBtnBg.offsetMin = new Vector2(this.lastBtnBg.offsetMin.x, 0f);
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			this.restore.SetActive(true);
		}
		else
		{
			this.restore.SetActive(false);
			this.openedPosition += new Vector2(0f, (float)this.btnHeight);
		}
	}

	protected override void WillBecomeInvisable()
	{
	}

	[SerializeField]
	private GameObject filterCheckmark;

	[SerializeField]
	private GameObject restore;

	[SerializeField]
	private GameObject removeAds;

	[SerializeField]
	private GameObject design;

	[SerializeField]
	private RectTransform lastBtnBg;

	private Vector2 defaultOpenedPos;

	private int btnHeight = 135;

	private bool inited;

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

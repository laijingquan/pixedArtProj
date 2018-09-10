// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPage : Page
{
	public void LoadContent()
	{
		this.soundBtns[0].SetActive(AudioManager.Instance.SoundEnabled);
		this.soundBtns[1].SetActive(!AudioManager.Instance.SoundEnabled);
		this.musicBtns[0].SetActive(AudioManager.Instance.MusicEnabled);
		this.musicBtns[1].SetActive(!AudioManager.Instance.MusicEnabled);
		this.notifBtns[0].SetActive(true);
		this.notifBtns[1].SetActive(false);
		if (BuildConfig.LOG_FILE)
		{
			this.debugLogButton.SetActive(true);
		}
		if (GeneralSettings.AdsDisabled)
		{
			this.HideAdsButton();
		}
	}

	protected override void OnBeginClosing()
	{
	}

	protected override void OnBeginOpenning()
	{
	}

	public void HideAdsButton()
	{
		this.adsButton.SetActive(false);
	}

	protected override void OnClosed()
	{
	}

	protected override void OnOpened()
	{
	}

	public void OnSoundToggle(bool isOn)
	{
		this.soundBtns[0].SetActive(isOn);
		this.soundBtns[1].SetActive(!isOn);
		AudioManager.Instance.SoundEnabled = isOn;
		AudioManager.Instance.Button();
	}

	public void OnMusicToggle(bool isOn)
	{
		this.musicBtns[0].SetActive(isOn);
		this.musicBtns[1].SetActive(!isOn);
		AudioManager.Instance.MusicEnabled = isOn;
		AudioManager.Instance.Button();
	}

	public void OnNotificationToggle(bool isOn)
	{
		this.notifBtns[0].SetActive(isOn);
		this.notifBtns[1].SetActive(!isOn);
		if (isOn)
		{
			LocalizationService.Instance.Localization = "Russian";
		}
		else
		{
			LocalizationService.Instance.Localization = "English";
		}
		AudioManager.Instance.Button();
	}

	public void DebugLang()
	{
		this.debugStr.text = this.langs[this.index];
		LocalizationService.Instance.Localization = this.langs[this.index];
		this.index++;
		if (this.index == this.langs.Count)
		{
			this.index = 0;
		}
	}

	public static int QueueSpeed
	{
		get
		{
			return PlayerPrefs.GetInt("debug_queue_speed", 0);
		}
		private set
		{
			PlayerPrefs.SetInt("debug_queue_speed", value);
		}
	}

	public void DebugQueue()
	{
	}

	public void OnDebugSendLog()
	{
		//FMNativeUtils.Instance.SendEmail("PaintLog " + DateTime.Now.ToString("T"), FMLogger.GetLogStr(), "wp.dev@yandex.ru");
	}

	[SerializeField]
	private GameObject debugLogButton;

	[SerializeField]
	private GameObject restore;

	[SerializeField]
	private Text debugStr;

	[SerializeField]
	private Text debugQueueStr;

	[SerializeField]
	private GameObject adsButton;

	[SerializeField]
	private GameObject[] soundBtns;

	[SerializeField]
	private GameObject[] musicBtns;

	[SerializeField]
	private GameObject[] notifBtns;

	[SerializeField]
	private GameObject sudokuButton;

	[SerializeField]
	private GameObject[] iosButtonReposition;

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

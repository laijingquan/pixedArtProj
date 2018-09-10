// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AudioManager : SoundManager
{
	public static AudioManager Instance
	{
		get
		{
			if (AudioManager.instance == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<AudioManager>();
				AudioManager.instance = gameObject.GetComponent<AudioManager>();
			}
			return AudioManager.instance;
		}
	}

	private static bool SoundIsOnSetting
	{
		get
		{
			return PlayerPrefs.GetInt("sound_enabled_key", 1) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("sound_enabled_key", (!value) ? 0 : 1);
		}
	}

	public bool SoundEnabled
	{
		get
		{
			return this.soundEnabled;
		}
		set
		{
			if (value != this.soundEnabled)
			{
				this.soundEnabled = value;
				AudioManager.SoundIsOnSetting = this.soundEnabled;
			}
		}
	}

	private static bool MusicIsOnSetting
	{
		get
		{
			return PlayerPrefs.GetInt("music_enabled_key", 1) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("music_enabled_key", (!value) ? 0 : 1);
		}
	}

	public bool MusicEnabled
	{
		get
		{
			return this.musicEnabled;
		}
		set
		{
			if (value != this.musicEnabled)
			{
				this.musicEnabled = value;
				AudioManager.MusicIsOnSetting = this.musicEnabled;
			}
		}
	}

	public void Button()
	{
		base.PlaySingle(this.click);
	}

	private void Awake()
	{
		if (AudioManager.instance == null)
		{
			AudioManager.instance = this;
		}
		else if (AudioManager.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.soundEnabled = AudioManager.SoundIsOnSetting;
		this.musicEnabled = AudioManager.MusicIsOnSetting;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private static AudioManager instance;

	[SerializeField]
	private AudioClip click;

	private const string soundKey = "sound_enabled_key";

	private const string musicKey = "music_enabled_key";
}

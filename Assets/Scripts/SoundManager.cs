// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public void PlaySingle(AudioClip clip)
	{
		if (!this.soundEnabled)
		{
			return;
		}
		this.efxSource.clip = clip;
		this.efxSource.Play();
	}

	public void RandomizeSfx(params AudioClip[] clips)
	{
		int num = UnityEngine.Random.Range(0, clips.Length);
		float pitch = UnityEngine.Random.Range(this.lowPitchRange, this.highPitchRange);
		this.efxSource.pitch = pitch;
		this.efxSource.clip = clips[num];
		this.efxSource.Play();
	}

	public AudioSource efxSource;

	public AudioSource musicSource;

	public float lowPitchRange = 0.95f;

	public float highPitchRange = 1.05f;

	protected bool soundEnabled;

	protected bool musicEnabled;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HintBtn : MonoBehaviour
{
	private void OnEnable()
	{
		if (GeneralSettings.AdsDisabled)
		{
			this.label.transform.parent.gameObject.SetActive(false);
		}
		this.UpdateUI();
	}

	private void OnDisable()
	{
		if (this.checkerCoroutine != null)
		{
			base.StopCoroutine(this.checkerCoroutine);
		}
	}

	public void UpdateUI()
	{
		if (GeneralSettings.AdsDisabled)
		{
			return;
		}
		int hintsCount = GeneralSettings.HintsCount;
		string text = hintsCount.ToString();
		if (hintsCount == 0)
		{
			try
			{
				this.hasRewarded = AdsManager.Instance.HasRewardedVideo();
			}
			catch (Exception)
			{
				FMLogger.vCore("hasRewarded ex.");
				this.hasRewarded = false;
			}
			text = ((!this.hasRewarded) ? "0" : this.AD_STR);
			if (!this.hasRewarded)
			{
				AdsManager.Instance.LoadRewardedVideo();
			}
			this.shouldCheckRewarded = true;
			if (this.checkerCoroutine != null)
			{
				base.StopCoroutine(this.checkerCoroutine);
			}
			this.checkerCoroutine = base.StartCoroutine(this.Checker());
		}
		else
		{
			this.shouldCheckRewarded = false;
			if (this.checkerCoroutine != null)
			{
				base.StopCoroutine(this.checkerCoroutine);
			}
		}
		this.label.text = text;
	}

	public void PlayUsedAnimation()
	{
		if (this.hintButtonAnim != null)
		{
			this.hintButtonAnim.Play();
			this.hintButtonAnim.Stop(false);
		}
	}

	public void RewardReceived()
	{
		this.UpdateUI();
		base.StartCoroutine(this.Pulse());
	}

	private IEnumerator CheckRewarded()
	{
		while (!this.hasRewarded)
		{
			this.hasRewarded = AdsManager.Instance.HasRewardedVideo();
			if (this.hasRewarded)
			{
				this.label.text = this.AD_STR;
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	private IEnumerator Checker()
	{
		FMLogger.vAds("hint btn start checker. rs:" + this.hasRewarded);
		while (this.shouldCheckRewarded)
		{
			while (!this.hasRewarded)
			{
				this.hasRewarded = AdsManager.Instance.HasRewardedVideo();
				if (this.hasRewarded)
				{
					this.label.text = this.AD_STR;
					FMLogger.vAds("hint checker upd. AD");
				}
				yield return new WaitForSeconds(1f);
			}
			while (this.hasRewarded)
			{
				this.hasRewarded = AdsManager.Instance.HasRewardedVideo();
				if (!this.hasRewarded)
				{
					this.label.text = GeneralSettings.HintsCount.ToString();
					FMLogger.vAds("hint checker upd. Rewarded GONE");
				}
				yield return new WaitForSeconds(3f);
			}
			yield return 0;
		}
		yield break;
	}

	private IEnumerator Pulse()
	{
		RectTransform rt = (RectTransform)base.transform;
		float pulseUp = 0.1f;
		float pulseDown = 0.15f;
		float maxScale = 1.08f;
		yield return this.ScaleCoroutine(rt, 1f, maxScale, pulseUp, 0f);
		yield return this.ScaleCoroutine(rt, maxScale, 1f, pulseDown, 0f);
		yield break;
	}

	private IEnumerator ScaleCoroutine(RectTransform rt, float from, float to, float animDuration, float d = 0f)
	{
		if (d > 0f)
		{
			yield return new WaitForSeconds(d);
		}
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			float s = Mathf.Lerp(from, to, i);
			rt.localScale = new Vector3(s, s, 1f);
			yield return 0;
		}
		rt.localScale = new Vector3(to, to, 1f);
		yield break;
	}

	[SerializeField]
	private Text label;

	[SerializeField]
	private ImageAnimation hintButtonAnim;

	private bool hasRewarded;

	private bool shouldCheckRewarded;

	private string AD_STR = "AD";

	private Coroutine checkerCoroutine;
}

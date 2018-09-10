// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FairyRewardButton : MonoBehaviour, IFairyButton
{
	public string currentGUID { get; private set; }

	public void Show()
	{
		if (AdsManager.Instance.HasRewardedVideo())
		{
			this.Open();
		}
	}

	public void Hide()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeCoroutine(this.canvas.alpha, 0f, 0.15f));
	}

	private void Open()
	{
		this.currentGUID = AnalyticsUtils.GenerateGUID();
		base.StartCoroutine(this.Slide());
		AnalyticsManager.FairyShown(this.currentGUID, AdsManager.Instance.RewardConfig.timingBonus);
	}

	private void Update()
	{
		if (this.active)
		{
			this.currentShowTime += Time.deltaTime;
		}
	}

	private IEnumerator Slide()
	{
		this.showDuration = (float)AdsManager.Instance.RewardConfig.timingBonusShowTime;
		this.currentShowTime = 0f;
		this.active = true;
		this.rootRt.gameObject.SetActive(true);
		this.rootRt.localScale = Vector3.one;
		this.canvas.alpha = 1f;
		this.canvas.interactable = true;
		this.timerRt.color = this.colors[0];
		this.hintsLabel.text = AdsManager.Instance.RewardConfig.timingBonus.ToString();
		base.StartCoroutine(this.Spin(this.raysRt));
		base.StartCoroutine(this.CheckRewardAvailability());
		yield return this.MoveCoroutine(this.rootRt, this.closedPos, this.openedPos, 0.2f, 0f);
		yield return this.CountdownCoroutine();
		yield break;
	}

	private IEnumerator CountdownCoroutine()
	{
		base.StartCoroutine(this.FillImage(this.timerRt, 1f, 0f, this.showDuration));
		base.StartCoroutine(this.Pulse(this.rootRt, this.showDuration));
		for (int i = 0; i < this.colors.Length - 1; i++)
		{
			yield return this.CrossFade(this.timerRt, this.colors[i], this.colors[i + 1], this.showDuration / (float)(this.colors.Length - 1));
		}
		yield return 0;
		base.StartCoroutine(this.FadeCoroutine(this.canvas.alpha, 0f, 0.15f));
		yield break;
	}

	private IEnumerator CrossFade(Image img, Color from, Color to, float animDuration)
	{
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			img.color = Color.Lerp(from, to, i);
			yield return 0;
		}
		img.color = to;
		yield break;
	}

	private IEnumerator FillImage(Image img, float from, float to, float animDuration)
	{
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			img.fillAmount = Mathf.Lerp(from, to, i);
			yield return 0;
		}
		img.fillAmount = to;
		yield break;
	}

	private IEnumerator Pulse(RectTransform rt, float duration)
	{
		float step = duration / 4f;
		float maxScale = 1.05f;
		float minScale = 0.95f;
		for (int i = 0; i < 3; i++)
		{
			yield return this.ScaleCoroutine(rt, (i != 0) ? minScale : 1f, maxScale, step / 2f, 0f);
			yield return this.ScaleCoroutine(rt, maxScale, minScale, step / 2f, 0f);
		}
		yield return this.ScaleCoroutine(rt, minScale, 1.25f, step * 0.3f, 0f);
		yield return this.ScaleCoroutine(rt, 1.25f, 1f, step * 0.7f, 0f);
		yield break;
	}

	private IEnumerator FadeCoroutine(float from, float to, float animDuration)
	{
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			this.canvas.alpha = Mathf.Lerp(from, to, i);
			yield return 0;
		}
		this.active = false;
		this.canvas.interactable = false;
		this.canvas.alpha = to;
		this.CleanStars();
		this.rootRt.gameObject.SetActive(false);
		yield break;
	}

	private IEnumerator MoveCoroutine(RectTransform rt, Vector2 from, Vector2 to, float animDuration, float d = 0f)
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
			rt.anchoredPosition = Vector2.LerpUnclamped(from, to, i);
			yield return 0;
		}
		rt.anchoredPosition = Vector2.Lerp(from, to, 1f);
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

	private IEnumerator Spin(RectTransform rt)
	{
		while (this.active)
		{
			rt.Rotate(Vector3.forward, this.raySpeed * Time.deltaTime);
			yield return 0;
		}
		yield break;
	}

	private IEnumerator SpawnStars()
	{
		while (this.active)
		{
			CanvasGroup star = this.starsPool.GetEntity<CanvasGroup>(this.starsPool.transform);
			this.stars.Add(star);
			float delay = UnityEngine.Random.Range(0.1f, 0.3f);
			base.StartCoroutine(this.StartMove(star));
			yield return new WaitForSeconds(delay);
		}
		yield break;
	}

	private IEnumerator CheckRewardAvailability()
	{
		bool fairyCanceled = false;
		while (this.active && !fairyCanceled)
		{
			if (!AdsManager.Instance.HasRewardedVideo())
			{
				fairyCanceled = true;
				this.Hide();
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield break;
	}

	private void CleanStars()
	{
		for (int i = 0; i < this.stars.Count; i++)
		{
			this.stars[i].transform.localPosition = new Vector3(-10000f, 0f, 0f);
			this.starsPool.UnUseItem(this.stars[i].gameObject);
		}
		this.stars.Clear();
	}

	private IEnumerator StartMove(CanvasGroup star)
	{
		star.alpha = 1f;
		RectTransform rt = (RectTransform)star.transform;
		float angle = (float)UnityEngine.Random.Range(0, 360) * 0.0174532924f;
		float animDuration = 1f;
		float radius = 100f;
		Vector2 to = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
		Vector2 from = Vector2.zero;
		float fadeInterval = 0.2f;
		bool fading = false;
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			if (!fading && 1f - i < fadeInterval)
			{
				fading = true;
			}
			if (fading)
			{
				float num = (i - (1f - fadeInterval)) / fadeInterval;
				star.alpha = 1f - num;
			}
			rt.anchoredPosition = Vector2.LerpUnclamped(from, to, i);
			yield return 0;
		}
		this.stars.Remove(star);
		star.transform.localPosition = new Vector3(-10000f, 0f, 0f);
		this.starsPool.UnUseItem(star.gameObject);
		yield break;
	}

	public float GetShowingTime()
	{
		return this.currentShowTime;
	}

	private float showDuration;

	private float currentShowTime;

	[SerializeField]
	private Vector2 openedPos;

	[SerializeField]
	private Vector2 closedPos;

	[SerializeField]
	private CanvasGroup canvas;

	[SerializeField]
	private RectTransform rootRt;

	[SerializeField]
	private float raySpeed = 100f;

	[SerializeField]
	private RectTransform raysRt;

	[SerializeField]
	private Color[] colors;

	[SerializeField]
	private Image timerRt;

	[SerializeField]
	private Text hintsLabel;

	[SerializeField]
	private ObjectContainer starsPool;

	private List<CanvasGroup> stars = new List<CanvasGroup>();

	private bool active;
}

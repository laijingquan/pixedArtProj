// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SplashCanvas : MonoBehaviour
{
	 
	public event Action CloseButtonClick;

	public static SplashCanvas Instance { get; private set; }

	private void Awake()
	{
		if (SplashCanvas.Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SplashCanvas.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		this.buttonObj = ((!GeneralSettings.IsOldDesign) ? this.neoCloseBtn : this.classicCloseBtn);
		if (SafeLayout.IsTablet)
		{
			this.animObj.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
			this.errorObj.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
			this.buttonObj.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
		}
		this.animObj.SetActive(true);
	}

	public void ShowErrorBtn()
	{
		this.buttonObj.SetActive(true);
	}

	public void ShowError()
	{
		this.errorObj.SetActive(true);
	}

	public void HideError()
	{
		this.errorObj.SetActive(false);
		this.buttonObj.SetActive(false);
	}

	public IEnumerator Fade(bool open, float animDuration, bool showAnimation)
	{
		this.errorObj.SetActive(false);
		this.buttonObj.SetActive(false);
		float from;
		float to;
		if (open)
		{
			this.animObj.SetActive(showAnimation);
			this.canvas.gameObject.SetActive(true);
			from = this.canvas.alpha;
			to = 1f;
		}
		else
		{
			from = this.canvas.alpha;
			to = 0f;
		}
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			this.canvas.alpha = Mathf.Lerp(from, to, i);
			yield return null;
		}
		this.canvas.alpha = to;
		if (!open)
		{
			this.canvas.gameObject.SetActive(false);
			if (this.animObj != null)
			{
				this.animObj.SetActive(false);
			}
		}
		yield return null;
		yield break;
	}

	public void InstantFade(bool open)
	{
		this.errorObj.SetActive(false);
		this.buttonObj.SetActive(false);
		if (open)
		{
			if (this.animObj != null)
			{
				this.animObj.SetActive(true);
			}
			this.canvas.gameObject.SetActive(true);
			this.canvas.alpha = 1f;
		}
		else
		{
			this.canvas.gameObject.SetActive(true);
			this.canvas.alpha = 0f;
			if (this.animObj != null)
			{
				this.animObj.SetActive(false);
			}
		}
	}

	public void OnCloseBtnClick()
	{
		if (this.CloseButtonClick != null)
		{
			this.CloseButtonClick();
		}
	}

	public CanvasGroup canvas;

	public GameObject animObj;

	public GameObject errorObj;

	[SerializeField]
	private GameObject classicCloseBtn;

	[SerializeField]
	private GameObject neoCloseBtn;

	private GameObject buttonObj;
}

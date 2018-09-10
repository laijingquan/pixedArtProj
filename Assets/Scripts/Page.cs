// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public abstract class Page : MonoBehaviour
{
	public bool IsOpened { get; private set; }

	public void SetOpened()
	{
		this.IsOpened = true;
		((RectTransform)base.transform).anchoredPosition = new Vector2(0f, this.yOffset);
		this.SetRootEnabled(true);
	}

	public void Init(AnimationCurve curve, float time)
	{
		this.duration = time;
		this.yOffset = ((RectTransform)base.transform).anchoredPosition.y;
	}

	public void Open(AnimSlideDirection direction)
	{
		this.IsOpened = true;
		this.SetRootEnabled(true);
		this.OnBeginOpenning();
		RectTransform rectTransform = (RectTransform)base.transform;
		Vector2 from;
		if (direction == AnimSlideDirection.Left)
		{
			from = new Vector2(rectTransform.rect.width, this.yOffset);
		}
		else
		{
			from = new Vector2(-rectTransform.rect.width, this.yOffset);
		}
		if (this.slideCoroutine != null)
		{
			base.StopCoroutine(this.slideCoroutine);
		}
		this.slideCoroutine = base.StartCoroutine(this.MoveCoroutine((RectTransform)base.transform, from, new Vector2(0f, this.yOffset), this.duration, true, 0f));
	}

	public void Close(AnimSlideDirection direction)
	{
		this.IsOpened = false;
		this.OnBeginClosing();
		RectTransform rectTransform = (RectTransform)base.transform;
		Vector2 to;
		if (direction == AnimSlideDirection.Left)
		{
			to = new Vector2(-rectTransform.rect.width, this.yOffset);
		}
		else
		{
			to = new Vector2(rectTransform.rect.width, this.yOffset);
		}
		if (this.slideCoroutine != null)
		{
			base.StopCoroutine(this.slideCoroutine);
		}
		this.slideCoroutine = base.StartCoroutine(this.MoveCoroutine(rectTransform, new Vector2(0f, this.yOffset), to, this.duration, false, 0f));
	}

	public void EnableLowMemoryUsage()
	{
		this.lowMemoryUse = true;
	}

	protected virtual void PrepareForContnentLoad()
	{
		this.contentLoading = true;
		this.SetRootEnabled(true);
	}

	protected virtual void ContentLoadComplete()
	{
		this.contentLoading = false;
		if (!this.IsOpened)
		{
			this.SetRootEnabled(false);
		}
	}

	protected abstract void OnBeginOpenning();

	protected abstract void OnOpened();

	protected abstract void OnBeginClosing();

	protected abstract void OnClosed();

	private void SetRootEnabled(bool isOn)
	{
		if (this.contentRoot != null)
		{
			this.contentRoot.SetActive(isOn);
		}
	}

	protected IEnumerator MoveCoroutine(RectTransform rt, Vector2 from, Vector2 to, float animDuration, bool open, float d = 0f)
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
			rt.anchoredPosition = Vector2.Lerp(from, to, i);
			yield return 0;
		}
		rt.anchoredPosition = to;
		yield return 0;
		if (open)
		{
			this.OnOpened();
		}
		else
		{
			this.OnClosed();
			rt.anchoredPosition = this.closedPosition;
			if (!this.contentLoading)
			{
				this.SetRootEnabled(false);
			}
		}
		yield break;
	}

	protected IEnumerator DelayAction(int framesDelay, float timeDelay, Action a)
	{
		if (timeDelay > 0f)
		{
			yield return new WaitForSeconds(timeDelay);
		}
		while (framesDelay > 0)
		{
			framesDelay--;
			yield return null;
		}
		a();
		yield break;
	}

	protected bool lowMemoryUse;

	[SerializeField]
	private GameObject contentRoot;

	private bool contentLoading;

	[SerializeField]
	private Vector2 closedPosition;

	private float duration;

	private Coroutine slideCoroutine;

	private float yOffset;
}

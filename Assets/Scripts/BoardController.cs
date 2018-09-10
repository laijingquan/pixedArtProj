// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class BoardController : MonoBehaviour
{
	 
	public event Action<Vector2> Click;

	 
	public event Action<float> PinchCompleted;

	public void Init(NumberController nController)
	{
		this.numController = nController;
		this.pan.Init(this.rt);
		float minZoom = (!SafeLayout.IsTablet) ? 0.8f : 1.2f;
		float scaleRequiredForNum = this.numController.ScaleRequiredForNum;
		this.pinch.Init(this.rt, minZoom, scaleRequiredForNum);
		this.initialZoom = ((!SafeLayout.IsTablet) ? 1f : 1.4f);
		this.initialPos = this.rt.anchoredPosition;
		this.rt.localScale = new Vector3(this.initialZoom, this.initialZoom, 1f);
		this.numController.UpdateNums(this.rt.localScale.x);
		this.pan.Click += delegate(Vector2 pos)
		{
			if (this.Click != null)
			{
				this.Click(pos);
			}
		};
		this.pinch.PinchCompleted += this.OnPinchCompleted;
		this.pinch.Pinching += this.OnPinch;
		this.pan.TouchActionStarted += this.OnTouchActionStarted;
		this.pinch.TouchActionStarted += this.OnTouchActionStarted;
	}

	public void ResetZoom()
	{
		this.sizeCoroutine = base.StartCoroutine(this.MoveCamera(this.initialPos, this.initialZoom, 0.3f, false));
	}

	public void ResetZoom(float duration)
	{
		this.sizeCoroutine = base.StartCoroutine(this.MoveCamera(this.initialPos, this.initialZoom, duration, false));
	}

	public void ZoomToPosition(Vector2 pos)
	{
		if (this.sizeCoroutine != null)
		{
			base.StopCoroutine(this.sizeCoroutine);
		}
		base.StartCoroutine(this.MoveCamera(pos * this.numController.ScaleRequiredForNum, this.numController.ScaleRequiredForNum, 0.3f, true));
	}

	public void UpdateZoom(bool magnify)
	{
		if (magnify)
		{
			float d = 1.05f;
			this.rt.localScale *= d;
		}
		else
		{
			float d2 = 0.95f;
			this.rt.localScale *= d2;
		}
		this.OnPinchCompleted();
	}

	private void DisableTouchInput()
	{
		this.touchBlock.SetActive(true);
	}

	private void EnableTouchInput()
	{
		this.touchBlock.SetActive(false);
	}

	private void OnPinch()
	{
		float x = this.rt.localScale.x;
		this.numController.UpdateNums(x);
	}

	private void OnPinchCompleted()
	{
		float x = this.rt.localScale.x;
		this.numController.UpdateNums(x);
		if (this.PinchCompleted != null)
		{
			this.PinchCompleted(this.rt.localScale.x);
		}
	}

	private void OnTouchActionStarted()
	{
		if (this.sizeCoroutine != null)
		{
			base.StopCoroutine(this.sizeCoroutine);
		}
	}

	private IEnumerator MoveCamera(Vector2 posTo, float scaleTo, float animDuration, bool disableInput = false)
	{
		float i = 0f;
		float currentTime = 0f;
		Vector2 posFrom = this.rt.anchoredPosition;
		float scaleFrom = this.rt.localScale.x;
		if (disableInput)
		{
			this.DisableTouchInput();
		}
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			this.rt.anchoredPosition = Vector2.Lerp(posFrom, posTo, i);
			float s = Mathf.Lerp(scaleFrom, scaleTo, i);
			this.rt.localScale = new Vector3(s, s, 1f);
			this.pinch.SetCurrentPinchZoom(s);
			this.numController.UpdateNums(s);
			yield return 0;
		}
		this.rt.anchoredPosition = posTo;
		this.rt.localScale = new Vector3(scaleTo, scaleTo, 1f);
		yield return 0;
		this.sizeCoroutine = null;
		this.OnPinchCompleted();
		if (disableInput)
		{
			this.EnableTouchInput();
		}
		yield break;
	}

	[SerializeField]
	private PinchHandler pinch;

	[SerializeField]
	private SingleTouchHandler pan;

	[SerializeField]
	private RectTransform rt;

	[SerializeField]
	private GameObject touchBlock;

	private Coroutine sizeCoroutine;

	private NumberController numController;

	private float initialZoom;

	private Vector2 initialPos;
}

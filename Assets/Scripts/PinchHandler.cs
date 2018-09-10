// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PinchHandler : MonoBehaviour, IPinchExtHandler
{
	 
	public event Action TouchActionStarted;

	 
	public event Action PinchCompleted;

	 
	public event Action Pinching;

	public void Init(RectTransform rt, float minZoom, float maxZoom)
	{
		this.content = rt;
		this._maxZoom = Mathf.Max(4f, maxZoom);
		this._minZoom = minZoom;
		base.GetComponent<Graphic>().raycastTarget = true;
	}

	public void OnPinchStarted(Vector2 posOne, Vector2 posTwo)
	{
		if (this.TouchActionStarted != null)
		{
			this.TouchActionStarted();
		}
		TLogger.Instance.Log("zoom start");
		this._startPinchDist = this.Distance(posOne, posTwo) * this.content.localScale.x;
		this._startPinchZoom = this._currentZoom;
		this._startPinchScreenPosition = (posOne + posTwo) / 2f;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.content, this._startPinchScreenPosition, null, out this._startPinchCenterPosition);
		Vector2 a = new Vector3(this.content.pivot.x * this.content.rect.size.x, this.content.pivot.y * this.content.rect.size.y);
		Vector2 vector = a + this._startPinchCenterPosition;
		PinchHandler.SetPivot(this.content, new Vector2(vector.x / this.content.rect.width, vector.y / this.content.rect.height));
	}

	public void OnPinch(Vector2 posOne, Vector2 posTwo)
	{
		float num = this.Distance(posOne, posTwo) * this.content.localScale.x;
		this._currentZoom = num / this._startPinchDist * this._startPinchZoom;
		this._currentZoom = Mathf.Clamp(this._currentZoom, this._minZoom, this._maxZoom);
		if (Mathf.Abs(this.content.localScale.x - this._currentZoom) > 0.001f)
		{
			this.content.localScale = Vector3.Lerp(this.content.localScale, Vector3.one * this._currentZoom, this._zoomLerpSpeed * Time.deltaTime);
		}
		if (this.Pinching != null)
		{
			this.Pinching();
		}
	}

	public void OnPinchEnd()
	{
		PinchHandler.SetPivot(this.content, new Vector2(0.5f, 0.5f));
		if (this.PinchCompleted != null)
		{
			this.PinchCompleted();
		}
	}

	public void SetCurrentPinchZoom(float zoom)
	{
		this._currentZoom = zoom;
	}

	private float Distance(Vector2 pos1, Vector2 pos2)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.content, pos1, null, out pos1);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.content, pos2, null, out pos2);
		return Vector2.Distance(pos1, pos2);
	}

	private static void SetPivot(RectTransform rectTransform, Vector2 pivot)
	{
		if (rectTransform == null)
		{
			return;
		}
		TLogger.Instance.Log(string.Concat(new object[]
		{
			"set pivot ",
			rectTransform.localPosition,
			" ",
			rectTransform.pivot
		}));
		Vector2 size = rectTransform.rect.size;
		Vector2 vector = rectTransform.pivot - pivot;
		Vector3 b = new Vector3(vector.x * size.x, vector.y * size.y) * rectTransform.localScale.x;
		rectTransform.pivot = pivot;
		rectTransform.localPosition -= b;
		TLogger.Instance.Log(string.Concat(new object[]
		{
			"after set pivot ",
			rectTransform.localPosition,
			" ",
			rectTransform.pivot
		}));
	}

	private float _minZoom = 0.8f;

	private float _maxZoom;

	[SerializeField]
	private float _zoomLerpSpeed = 10f;

	private float _currentZoom = 1f;

	private float _startPinchDist;

	private float _startPinchZoom;

	private Vector2 _startPinchCenterPosition;

	private Vector2 _startPinchScreenPosition;

	private RectTransform content;
}

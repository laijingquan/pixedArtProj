// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinchInputExtModule : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler
{
	private void Awake()
	{
		this.handler = (base.GetComponent(typeof(IPinchExtHandler)) as IPinchExtHandler);
	}

	public void OnPointerDown(PointerEventData data)
	{
		this.kountFingersDown++;
		if (this.currentFirstFinger == -1 && this.kountFingersDown == 1)
		{
			this.currentFirstFinger = data.pointerId;
			this.positionFirst = data.position;
			return;
		}
		if (this.currentFirstFinger != -1 && this.currentSecondFinger == -1 && this.kountFingersDown == 2)
		{
			this.currentSecondFinger = data.pointerId;
			this.positionSecond = data.position;
			this.isZooming = true;
			if (this.handler != null)
			{
				this.handler.OnPinchStarted(this.positionFirst, this.positionSecond);
			}
			return;
		}
	}

	public void OnDrag(PointerEventData data)
	{
		if (this.currentFirstFinger == data.pointerId)
		{
			this.positionFirst = data.position;
		}
		if (this.currentSecondFinger == data.pointerId)
		{
			this.positionSecond = data.position;
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		this.kountFingersDown--;
		if (this.currentFirstFinger == data.pointerId)
		{
			this.currentFirstFinger = -1;
			if (this.isZooming)
			{
				this.isZooming = false;
				if (this.handler != null)
				{
					this.handler.OnPinchEnd();
				}
			}
		}
		if (this.currentSecondFinger == data.pointerId)
		{
			this.currentSecondFinger = -1;
			if (this.isZooming)
			{
				this.isZooming = false;
				if (this.handler != null)
				{
					this.handler.OnPinchEnd();
				}
			}
		}
	}

	private void Update()
	{
		if (this.isZooming && this.handler != null)
		{
			this.handler.OnPinch(this.positionFirst, this.positionSecond);
		}
	}

	private bool isZooming;

	private int kountFingersDown;

	private int currentFirstFinger = -1;

	private int currentSecondFinger = -1;

	private Vector2 positionFirst = Vector2.zero;

	private Vector2 positionSecond = Vector2.zero;

	private IPinchExtHandler handler;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleFingerInputModule : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler
{
	private void Awake()
	{
		this.needsUs = (base.GetComponent(typeof(ISingleFingerHandler)) as ISingleFingerHandler);
	}

	public void OnPointerDown(PointerEventData data)
	{
		this.kountFingersDown++;
		if (this.kountFingersDown > 1)
		{
			this.ignoreTouch = true;
			this.currentSingleFinger = -1;
			if (this.needsUs != null)
			{
				this.needsUs.OnSingleFingerUp(data.position, true);
			}
			return;
		}
		if (this.currentSingleFinger == -1 && this.kountFingersDown == 1)
		{
			this.currentSingleFinger = data.pointerId;
			if (this.needsUs != null)
			{
				this.needsUs.OnSingleFingerDown(data.position);
			}
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		this.kountFingersDown--;
		if (this.ignoreTouch)
		{
			if (this.kountFingersDown == 0)
			{
				this.ignoreTouch = false;
			}
			return;
		}
		if (this.currentSingleFinger == data.pointerId)
		{
			this.currentSingleFinger = -1;
			if (this.needsUs != null)
			{
				this.needsUs.OnSingleFingerUp(data.position, false);
			}
		}
	}

	public void OnDrag(PointerEventData data)
	{
		if (this.ignoreTouch)
		{
			return;
		}
		if (this.currentSingleFinger == data.pointerId && this.kountFingersDown == 1 && this.needsUs != null)
		{
			this.needsUs.OnSingleFingerDrag(data.position);
		}
	}

	private ISingleFingerHandler needsUs;

	private int currentSingleFinger = -1;

	private int kountFingersDown;

	private bool ignoreTouch;
}

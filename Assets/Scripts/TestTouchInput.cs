// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestTouchInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		UnityEngine.Debug.Log("OnPointerDown");
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		UnityEngine.Debug.Log("OnPointerUp");
	}

	public void OnDrag(PointerEventData eventData)
	{
		UnityEngine.Debug.Log("OnDrag");
	}
}

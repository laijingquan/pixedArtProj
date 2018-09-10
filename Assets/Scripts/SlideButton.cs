// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SlideButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	public Button.ButtonClickedEvent onClick
	{
		get
		{
			return this.m_OnClick;
		}
		set
		{
			this.m_OnClick = value;
		}
	}

	private void Click()
	{
		this.m_OnClick.Invoke();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		this.Click();
	}

	private Button btn;

	[FormerlySerializedAs("onClick")]
	[SerializeField]
	private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();

	[Serializable]
	public class ButtonClickedEvent : UnityEvent
	{
	}
}

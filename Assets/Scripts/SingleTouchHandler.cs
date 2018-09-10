// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SingleTouchHandler : MonoBehaviour, ISingleFingerHandler
{
	 
	public event Action TouchActionStarted;

	 
	public event Action<Vector2> Click;

	public void Init(RectTransform rt)
	{
		this.content = rt;
		this.minClickDrag = (int)((float)Screen.height * 0.05f);
		base.GetComponent<Graphic>().raycastTarget = true;
	}

	public void OnSingleFingerDown(Vector2 position)
	{
		if (this.TouchActionStarted != null)
		{
			this.TouchActionStarted();
		}
		this.pressPosition = position;
		this.offset = this.TranslateToCanvas(position) - this.content.anchoredPosition;
		this.clickTime = Time.realtimeSinceStartup;
	}

	public void OnSingleFingerDrag(Vector2 position)
	{
		Vector2 anchoredPosition = this.ClampToBounds(this.TranslateToCanvas(position) - this.offset);
		this.content.anchoredPosition = anchoredPosition;
	}

	public void OnSingleFingerUp(Vector2 position, bool ignore)
	{
		if (ignore)
		{
			return;
		}
		float magnitude = (position - this.pressPosition).magnitude;
		if (Time.realtimeSinceStartup - this.clickTime < 0.3f && magnitude < (float)this.minClickDrag && this.Click != null)
		{
			this.Click(this.TranslateToCanvas(this.content, position));
		}
	}

	private Vector2 ClampToBounds(Vector2 position)
	{
		if (position.x > this.bounds.x * this.content.localScale.x)
		{
			position.x = this.bounds.x * this.content.localScale.x;
		}
		if (position.x < -this.bounds.x * this.content.localScale.x)
		{
			position.x = -this.bounds.x * this.content.localScale.x;
		}
		if (position.y > this.bounds.y * this.content.localScale.x)
		{
			position.y = this.bounds.y * this.content.localScale.x;
		}
		if (position.y < -this.bounds.y * this.content.localScale.x)
		{
			position.y = -this.bounds.y * this.content.localScale.x;
		}
		return position;
	}

	private Vector2 TranslateToCanvas(RectTransform target, Vector2 position)
	{
		Vector2 result;
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(target, position, null, out result))
		{
			return Vector2.zero;
		}
		return result;
	}

	private Vector2 TranslateToCanvas(Vector2 position)
	{
		Vector2 result;
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)base.transform, position, null, out result))
		{
			return Vector2.zero;
		}
		return result;
	}

	public Vector2 bounds;

	private float clickTime;

	private Vector2 pressPosition;

	private int minClickDrag;

	private RectTransform content;

	private Vector2 offset;
}

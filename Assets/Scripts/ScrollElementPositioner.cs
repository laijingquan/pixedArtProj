// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollElementPositioner : MonoBehaviour, IBeginDragHandler, IEventSystemHandler
{
	public void Init(List<Vector2> pos, int padding, int spacing)
	{
		this.scrollWidth = ((RectTransform)base.transform).rect.width;
		this.content = base.GetComponent<ScrollRect>().content;
		this.elementSpacing = spacing;
		this.elementPositions = pos;
		this.elementSizes = new List<float>();
		if (this.elementPositions.Count > 1)
		{
			for (int i = 0; i < this.elementPositions.Count - 1; i++)
			{
				float item = this.elementPositions[i + 1].x - this.elementPositions[i].x - (float)spacing;
				this.elementSizes.Add(item);
			}
			float item2 = this.content.rect.width - this.elementPositions[this.elementPositions.Count - 1].x - (float)padding;
			this.elementSizes.Add(item2);
		}
		else
		{
			this.elementSizes.Add(1f);
		}
	}

	public void CheckFit(int index)
	{
		Vector2 vector = this.elementPositions[index];
		float num = Mathf.Abs(this.content.anchoredPosition.x) - vector.x;
		float num2 = Mathf.Abs(this.content.anchoredPosition.x) + this.scrollWidth - vector.x - this.elementSizes[index];
		if (num > 0f)
		{
			Vector2 vector2;
			if (index > 0)
			{
				vector2 = this.elementPositions[index - 1] + new Vector2(this.elementSizes[index - 1] * (1f - this.visabilityThreshold) - (float)this.elementSpacing, 0f);
			}
			else
			{
				vector2 = this.elementPositions[index];
			}
			this.offsetCoroutine = base.StartCoroutine(this.MoveCoroutine(this.content, this.content.anchoredPosition, new Vector2(-vector2.x, vector2.y), this.duration, this.delay));
		}
		else if (num2 < 0f)
		{
			Vector2 vector3;
			if (index < this.elementPositions.Count - 1)
			{
				vector3 = new Vector2(-this.scrollWidth, 0f) + this.elementPositions[index + 1] + new Vector2(this.elementSizes[index + 1] * this.visabilityThreshold + (float)this.elementSpacing, 0f);
			}
			else
			{
				vector3 = new Vector2(-this.scrollWidth, 0f) + this.elementPositions[index] + new Vector2(this.elementSizes[index], 0f);
			}
			this.offsetCoroutine = base.StartCoroutine(this.MoveCoroutine(this.content, this.content.anchoredPosition, new Vector2(-vector3.x, vector3.y), this.duration, this.delay));
		}
		else if (Mathf.Abs(num) <= Mathf.Abs(num2))
		{
			if (index > 0 && this.elementSizes[index - 1] * this.visabilityThreshold > Mathf.Abs(num))
			{
				Vector2 vector4 = this.elementPositions[index - 1] + new Vector2(this.elementSizes[index - 1] * (1f - this.visabilityThreshold) - (float)this.elementSpacing, 0f);
				this.offsetCoroutine = base.StartCoroutine(this.MoveCoroutine(this.content, this.content.anchoredPosition, new Vector2(-vector4.x, vector4.y), this.duration, this.delay));
			}
		}
		else if (index < this.elementSizes.Count - 1 && this.elementSizes[index + 1] * this.visabilityThreshold + (float)this.elementSpacing > Mathf.Abs(num2))
		{
			Vector2 vector5 = new Vector2(-this.scrollWidth, 0f) + this.elementPositions[index + 1] + new Vector2(this.elementSizes[index + 1] * this.visabilityThreshold + (float)this.elementSpacing, 0f);
			this.offsetCoroutine = base.StartCoroutine(this.MoveCoroutine(this.content, this.content.anchoredPosition, new Vector2(-vector5.x, vector5.y), this.duration, this.delay));
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (this.offsetCoroutine != null)
		{
			base.StopCoroutine(this.offsetCoroutine);
			this.offsetCoroutine = null;
		}
	}

	protected IEnumerator MoveCoroutine(RectTransform rt, Vector2 from, Vector2 to, float animDuration, float d = 0f)
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
			rt.anchoredPosition = Vector2.Lerp(from, to, this.curve.Evaluate(i));
			yield return 0;
		}
		rt.anchoredPosition = Vector2.Lerp(from, to, 1f);
		this.offsetCoroutine = null;
		yield break;
	}

	private RectTransform content;

	private float scrollWidth;

	[SerializeField]
	private AnimationCurve curve;

	[SerializeField]
	private float duration = 0.2f;

	[SerializeField]
	private float delay = 0.15f;

	[SerializeField]
	[Range(0f, 1f)]
	private float visabilityThreshold = 0.9f;

	private int elementSpacing;

	private List<Vector2> elementPositions;

	private List<float> elementSizes;

	private Coroutine offsetCoroutine;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
{
	public void Init(int s)
	{
		this.screens = s;
		this.scroll = base.gameObject.GetComponent<ScrollRect>();
		this.scroll.inertia = false;
		this.InitNormalized();
	}

	private void InitNormalized()
	{
		if (this.screens > 0)
		{
			try
			{
				this.CalcPositions();
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogError("fail scroll rect snap");
				this.points = new float[this.screens];
				this.stepSize = 1f / (float)(this.screens - 1);
				for (int i = 0; i < this.screens; i++)
				{
					this.points[i] = (float)i * this.stepSize;
				}
			}
		}
	}

	private void CalcPositions()
	{
		HorizontalLayoutGroup component = this.scroll.content.GetComponent<HorizontalLayoutGroup>();
		int num = (int)component.spacing;
		int num2 = (int)((RectTransform)this.scroll.content.GetChild(0)).rect.width;
		int num3 = (int)(((RectTransform)this.scroll.transform).rect.width / 2f);
		this.points = new float[this.screens];
		Vector2 anchoredPosition = this.scroll.content.anchoredPosition;
		for (int i = 1; i < this.screens; i++)
		{
			float num4 = 0f;
			num4 += (float)component.padding.left;
			num4 += (float)num2 / 2f;
			num4 += (float)(num2 * i);
			num4 += (float)(num * i);
			this.scroll.content.anchoredPosition = new Vector2(-num4 + (float)num3, anchoredPosition.y);
			this.points[i] = this.scroll.horizontalNormalizedPosition;
		}
		this.points[0] = 0f;
		if (this.screens > 1)
		{
			this.points[this.screens - 1] = 1f;
		}
		this.scroll.content.anchoredPosition = anchoredPosition;
	}

	private void CalcPositionsEx()
	{
		int num = (int)this.scroll.content.GetComponent<HorizontalLayoutGroup>().spacing;
		int num2 = (int)((RectTransform)this.scroll.content.GetChild(0)).rect.width;
		UnityEngine.Debug.Log("featured width " + num2);
		this.points = new float[this.screens];
		Vector2 anchoredPosition = this.scroll.content.anchoredPosition;
		for (int i = 0; i < this.screens; i++)
		{
			float num3 = 0f;
			if (i > 0)
			{
				num3 += (float)(num2 * i);
			}
			if (i > 1)
			{
				num3 += (float)(num * (i - 1));
			}
			this.scroll.content.anchoredPosition = new Vector2(-num3, anchoredPosition.y);
			this.points[i] = this.scroll.horizontalNormalizedPosition;
		}
		this.scroll.content.anchoredPosition = anchoredPosition;
		UnityEngine.Debug.Log("featured sp " + anchoredPosition);
	}

	private void Update()
	{
		this.NormalizedValLerp();
	}

	private void NormalizedValLerp()
	{
		if (this.LerpH)
		{
			this.scroll.horizontalNormalizedPosition = Mathf.Lerp(this.scroll.horizontalNormalizedPosition, this.targetH, 10f * Time.deltaTime);
			if (Mathf.Abs(this.scroll.horizontalNormalizedPosition - this.targetH) < 0.001f)
			{
				this.LerpH = false;
			}
		}
	}

	private int FindNearest(float f, float[] array)
	{
		float num = float.PositiveInfinity;
		if (f >= 1f)
		{
			return array.Length - 1;
		}
		if (f <= 0f)
		{
			return 0;
		}
		int result = 0;
		if (this.targetH - f > 0f)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (Mathf.Approximately(array[i], this.targetH))
				{
					return Mathf.Max(0, i - 1);
				}
			}
		}
		else
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (Mathf.Approximately(array[j], this.targetH))
				{
					return Mathf.Min(array.Length - 1, j + 1);
				}
			}
		}
		for (int k = 0; k < array.Length; k++)
		{
			int num2 = 1;
			if (Mathf.Approximately(array[k], this.targetH))
			{
				num2 = 20;
			}
			if (Mathf.Abs(array[k] - f) * (float)num2 < num)
			{
				num = Mathf.Abs(array[k] - f);
				result = k;
			}
		}
		return result;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		this.LerpH = false;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (this.points == null)
		{
			return;
		}
		this.targetH = this.points[this.FindNearest(this.scroll.horizontalNormalizedPosition, this.points)];
		this.LerpH = true;
	}

	private float[] points;

	private int screens = 1;

	private float stepSize;

	private ScrollRect scroll;

	private bool LerpH;

	private float targetH;
}

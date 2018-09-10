// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class CategoryBarLayout : MonoBehaviour
{
	public void Init(CategoryBar.ViewConfig config)
	{
		this.viewConfig = config;
		this.rt = (RectTransform)base.transform;
		this.rt.sizeDelta = new Vector2((float)(this.viewConfig.padding * 2), this.rt.sizeDelta.y);
		this.nexItemPos = new Vector2((float)this.viewConfig.padding, 0f);
		this.count = 0;
	}

	public void Add(CategoryFilterButton btn)
	{
		int num = btn.PrefferedWidth();
		int num2 = this.viewConfig.spacing + num;
		if (this.count > 0)
		{
			this.nexItemPos += new Vector2((float)this.viewConfig.spacing, 0f);
		}
		btn.transform.SetParent(base.transform);
		btn.transform.localScale = Vector3.one;
		RectTransform rectTransform = (RectTransform)btn.transform;
		rectTransform.anchoredPosition = this.nexItemPos;
		rectTransform.sizeDelta = new Vector2((float)num, rectTransform.sizeDelta.y);
		this.positions.Add(rectTransform.anchoredPosition);
		this.nexItemPos += new Vector2((float)num, 0f);
		this.rt.sizeDelta += new Vector2((float)num2, this.rt.sizeDelta.y);
		this.count++;
	}

	public void ScrollToIndex(int index)
	{
		if (index > 0 && index < this.positions.Count)
		{
			this.rt.anchoredPosition = new Vector2(-this.positions[index].x, this.rt.anchoredPosition.y);
		}
	}

	private RectTransform rt;

	private CategoryBar.ViewConfig viewConfig;

	private Vector2 nexItemPos;

	private int count;

	private List<Vector2> positions = new List<Vector2>();
}

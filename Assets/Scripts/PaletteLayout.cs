// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class PaletteLayout : MonoBehaviour
{
	public void Init(PaletteController.Config config)
	{
		this.spacing = config.spacing;
		this.side = config.size;
		this.padding = config.padding;
	}

	private RectTransform rt
	{
		get
		{
			return (RectTransform)base.transform;
		}
	}

	public void AddElement(RectTransform elementRt)
	{
		if (this.elements.Count == 0)
		{
			this.rt.sizeDelta = new Vector2((float)(this.padding * 2), (float)this.side);
			this.nextElementPos = new Vector2((float)this.padding + (float)this.side / 2f, 0f);
		}
		if (GeneralSettings.IsOldDesign)
		{
			elementRt.sizeDelta = new Vector2((float)this.side, (float)this.side);
		}
		elementRt.anchoredPosition = this.nextElementPos;
		this.nextElementPos += new Vector2((float)(this.side + this.spacing), 0f);
		this.elements.Add(elementRt);
		this.rt.sizeDelta = new Vector2(this.rt.sizeDelta.x + (float)this.side + (float)this.spacing, this.rt.sizeDelta.y);
	}

	private int padding;

	private int spacing;

	private int side;

	private List<RectTransform> elements = new List<RectTransform>();

	private Vector2 nextElementPos;
}

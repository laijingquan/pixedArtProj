// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class UINavBarButtonData
{
	public static UINavBarButtonData FromUIElement(UINavBarButton element)
	{
		return new UINavBarButtonData
		{
			btnPos = ((RectTransform)element.transform).anchoredPosition,
			bgPos = element.bg.anchoredPosition,
			bgSize = element.bg.sizeDelta,
			iconPos = element.icon.anchoredPosition,
			iconSize = element.icon.sizeDelta,
			textFontSize = element.label.fontSize,
			textPos = ((RectTransform)element.label.transform).anchoredPosition,
			textSize = ((RectTransform)element.label.transform).sizeDelta,
			selectBgEnabled = (element.bgSelected != null && element.bgSelected.gameObject.activeSelf),
			hasData = true
		};
	}

	public Vector2 btnPos;

	public Vector2 iconSize;

	public Vector2 iconPos;

	public Vector2 bgPos;

	public Vector2 bgSize;

	public int textFontSize;

	public Vector2 textSize;

	public Vector2 textPos;

	public bool selectBgEnabled;

	public bool hasData;
}

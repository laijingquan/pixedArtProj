// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

namespace FMUILayout
{
	[Serializable]
	public class UIRectPositionData
	{
		public static UIRectPositionData FromTransform(Transform t)
		{
			RectTransform rectTransform = (RectTransform)t;
			return new UIRectPositionData
			{
				pivot = rectTransform.pivot,
				anchorMin = rectTransform.anchorMin,
				anchorMax = rectTransform.anchorMax,
				offsetMin = rectTransform.offsetMin,
				offsetMax = rectTransform.offsetMax,
				anchoredPosition = rectTransform.anchoredPosition,
				hasData = true
			};
		}

		public Vector2 anchoredPosition;

		public Vector2 anchorMin;

		public Vector2 anchorMax;

		public Vector2 offsetMin;

		public Vector2 offsetMax;

		public Vector2 pivot;

		public bool hasData;
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FMUILayout
{
	[Serializable]
	public class UIFontData
	{
		public static UIFontData FromTransform(Transform t)
		{
			Text component = t.GetComponent<Text>();
			return new UIFontData
			{
				fontSize = component.fontSize,
				bestFit = component.resizeTextForBestFit,
				bestFitMinSize = component.resizeTextMinSize,
				bestFitMaxSize = component.resizeTextMaxSize,
				hasData = true
			};
		}

		public int fontSize;

		public bool bestFit;

		public int bestFitMinSize;

		public int bestFitMaxSize;

		public bool hasData;
	}
}

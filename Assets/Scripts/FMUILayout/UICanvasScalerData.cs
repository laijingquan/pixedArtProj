// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FMUILayout
{
	[Serializable]
	public class UICanvasScalerData
	{
		public static UICanvasScalerData FromCanvas(Transform t)
		{
			CanvasScaler component = t.GetComponent<CanvasScaler>();
			return new UICanvasScalerData
			{
				referenceResolution = component.referenceResolution,
				matchWidthOrHeight = component.matchWidthOrHeight,
				hasData = true
			};
		}

		public Vector2 referenceResolution;

		public float matchWidthOrHeight;

		public bool hasData;
	}
}

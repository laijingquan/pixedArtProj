// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragThresholdUtil : MonoBehaviour
{
	private void Start()
	{
		int pixelDragThreshold = EventSystem.current.pixelDragThreshold;
		EventSystem.current.pixelDragThreshold = Mathf.Max(pixelDragThreshold, (int)((float)pixelDragThreshold * Screen.dpi / 160f));
	}
}

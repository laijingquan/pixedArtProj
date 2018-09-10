// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private void OnGUI()
	{
		if (GUI.Button(new Rect(0f, 0f, (float)(Screen.width / 2), 100f), "Open Root Popup"))
		{
			this.popupManager.OpenRoot();
		}
		if (GUI.Button(new Rect((float)(Screen.width / 2), 0f, (float)(Screen.width / 2), 100f), "Open Forced Popup"))
		{
			this.popupManager.OpenForced();
		}
		if (GUI.Button(new Rect(0f, 100f, (float)(Screen.width / 2), 100f), "Open Delayed"))
		{
			this.popupManager.OpenDelayed();
		}
		if (GUI.Button(new Rect((float)(Screen.width / 2), 100f, (float)(Screen.width / 2), 100f), "Open Timeout popup"))
		{
			this.popupManager.OpenTimeoutPopup();
		}
	}

	public MyPopupManager popupManager;
}

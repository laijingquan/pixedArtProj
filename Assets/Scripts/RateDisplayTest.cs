// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RateDisplayTest : MonoBehaviour
{
	public bool WantToShow()
	{
		if (!Application.version.Equals(this.RateVersion))
		{
			this.RateVersion = Application.version;
			this.RateCount = this.RateStep - 1;
			this.RateVersionImpressions = 0;
		}
		this.RateDisplayCount++;
		if (this.RateVersionImpressions >= 3)
		{
			return false;
		}
		bool result = false;
		this.RateCount++;
		if (this.RateCount >= this.RateStep)
		{
			result = true;
			this.RateCount = 0;
			this.RateVersionImpressions++;
		}
		return result;
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(0f, 0f, 200f, 200f), "check"))
		{
			bool flag = this.WantToShow();
			UnityEngine.Debug.Log("WTS: " + flag);
		}
		if (GUI.Button(new Rect(200f, 0f, 200f, 200f), "upd"))
		{
			this.RateVersion += "1";
		}
	}

	public string RateVersion = string.Empty;

	public int RateCount = 4;

	public int RateStep = 5;

	public int RateVersionImpressions;

	public int RateDisplayCount;
}

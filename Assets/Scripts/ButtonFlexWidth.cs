// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFlexWidth : MonoBehaviour
{
	private void Start()
	{
		if (this.manualInit)
		{
			return;
		}
		base.StartCoroutine(this.Init());
	}

	private IEnumerator Init()
	{
		yield return 0;
		this.Compose();
		yield break;
	}

	public void Compose()
	{
		int num = Mathf.RoundToInt(this.btnText.rectTransform.offsetMin.x + -1f * this.btnText.rectTransform.offsetMax.x);
		UnityEngine.Debug.Log("btn padding " + num);
		float preferredWidth = this.btnText.preferredWidth;
		if (this.btnBgRt.sizeDelta.x - (float)(num * 2) < preferredWidth)
		{
			int num2 = Mathf.CeilToInt(preferredWidth + (float)(num * 2));
			if (this.maxWidth > 0)
			{
				num2 = Mathf.Min(num2, this.maxWidth);
			}
			this.btnBgRt.sizeDelta = new Vector2((float)num2, this.btnBgRt.sizeDelta.y);
		}
	}

	public Text btnText;

	public RectTransform btnBgRt;

	public int maxWidth;

	public bool manualInit;
}

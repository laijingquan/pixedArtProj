// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedPopupContent : MonoBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.Compose());
	}

	private IEnumerator Compose()
	{
		yield return 0;
		float bodyLabelHeight = this.bodyLagel.rectTransform.rect.height;
		float prefHeight = this.bodyLagel.preferredHeight;
		if (prefHeight > bodyLabelHeight)
		{
			int num = Mathf.RoundToInt(prefHeight - bodyLabelHeight);
			this.bodyLagel.rectTransform.sizeDelta = new Vector2(this.bodyLagel.rectTransform.sizeDelta.x, prefHeight);
			this.content.sizeDelta += new Vector2(0f, (float)num);
		}
		this.flexButton.Compose();
		yield break;
	}

	public ButtonFlexWidth flexButton;

	public RectTransform content;

	public Text bodyLagel;
}

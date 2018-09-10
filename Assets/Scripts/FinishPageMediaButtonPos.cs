// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class FinishPageMediaButtonPos : MonoBehaviour
{
	private void Awake()
	{
		base.StartCoroutine(this.Delay());
	}

	private IEnumerator Delay()
	{
		yield return null;
		this.Calc();
		yield break;
	}

	private void Calc()
	{
		RectTransform rectTransform = (RectTransform)base.transform;
		float height = this.rootRt.rect.height;
		float y = this.animRt.sizeDelta.y;
		float num = height / 2f;
		float num2 = num - this.animRt.anchoredPosition.y + y / 2f;
		rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -1f * num2);
	}

	[SerializeField]
	private RectTransform animRt;

	[SerializeField]
	private RectTransform rootRt;
}

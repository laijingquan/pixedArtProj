// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class LocalizedButtonLayout : MonoBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.DelayAction());
	}

	private IEnumerator DelayAction()
	{
		yield return 0;
		this.slaveButton.anchoredPosition = new Vector2(this.rootButton.anchoredPosition.x - this.refWidth.rect.width - (float)this.spacing, this.rootButton.anchoredPosition.y);
		yield break;
	}

	[SerializeField]
	private RectTransform refWidth;

	[SerializeField]
	private RectTransform rootButton;

	public int spacing;

	[SerializeField]
	private RectTransform slaveButton;
}

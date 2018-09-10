// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class DynamicHeightLayout : MonoBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.DelayAction());
	}

	private IEnumerator DelayAction()
	{
		yield return 0;
		this.root.sizeDelta = new Vector2(this.root.sizeDelta.x, this.root.sizeDelta.y + this.refRt.rect.height);
		yield break;
	}

	[SerializeField]
	private RectTransform root;

	[SerializeField]
	private RectTransform refRt;
}

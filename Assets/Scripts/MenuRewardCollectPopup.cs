// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuRewardCollectPopup : MonoBehaviour
{
	private void Update()
	{
		this.raysRt.Rotate(Vector3.forward, this.raySpeed * Time.deltaTime);
	}

	[SerializeField]
	private Text label;

	[SerializeField]
	private RectTransform raysRt;

	[SerializeField]
	private float raySpeed = 100f;
}

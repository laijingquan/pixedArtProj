// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class BonusPopup : MonoBehaviour
{
	private void OnEnable()
	{
		this.amountLabel.text = "+" + AdsManager.Instance.RewardConfig.dailyBonus;
	}

	private void Update()
	{
		this.raysRt.Rotate(Vector3.forward, this.raySpeed * Time.deltaTime);
	}

	[SerializeField]
	private Text amountLabel;

	[SerializeField]
	private RectTransform raysRt;

	private float raySpeed = 100f;
}

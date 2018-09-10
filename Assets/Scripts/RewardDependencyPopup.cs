// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class RewardDependencyPopup : MonoBehaviour
{
	public void StopRewardChecker()
	{
		base.StopAllCoroutines();
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.CheckRewardAvailability());
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	private IEnumerator CheckRewardAvailability()
	{
		bool popupCanceled = false;
		while (!popupCanceled)
		{
			if (!AdsManager.Instance.HasRewardedVideo())
			{
				popupCanceled = true;
				this.popupManager.CloseActive();
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield break;
	}

	[SerializeField]
	private GamePopupManager popupManager;
}

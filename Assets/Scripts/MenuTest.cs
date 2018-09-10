// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuTest : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnEnable()
	{
		AdsManager.Instance.RewardedVideoComplete += delegate()
		{
			UnityEngine.Debug.Log("reward complete");
			GeneralSettings.UpdateCoins(1);
			this.coinsLabel.text = GeneralSettings.HintsCount.ToString();
		};
		this.coinsLabel.text = GeneralSettings.HintsCount.ToString();
	}

	public void OnPictureClick(int id)
	{
		switch (id)
		{
		case 1:
			AdsManager.Instance.LoadRewardedVideo();
			UnityEngine.Debug.Log("req rev");
			break;
		case 2:
		{
			bool flag = AdsManager.Instance.HasRewardedVideo();
			UnityEngine.Debug.Log("rew st: " + flag);
			break;
		}
		case 3:
			AdsManager.Instance.ShowRewardedVideo();
			break;
		}
	}

	private void Update()
	{
		this.rewStatus.SetActive(AdsManager.Instance.HasRewardedVideo());
	}

	public GameObject rewStatus;

	public Text coinsLabel;
}

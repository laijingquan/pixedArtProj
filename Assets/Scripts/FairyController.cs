// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FairyController : MonoBehaviour
{
	public void StartTimer()
	{
		if (GeneralSettings.AdsDisabled)
		{
			return;
		}
		this.delay = (float)AdsManager.Instance.RewardConfig.timingDelay;
		this.interval = (float)AdsManager.Instance.RewardConfig.timingInterval;
		this.currentTime = this.delay;
		this.state = FairyController.State.Timer;
	}

	private void Update()
	{
		switch (this.state)
		{
		case FairyController.State.Timer:
			this.currentTime -= Time.deltaTime;
			if (this.currentTime <= 0f)
			{
				bool flag;
				try
				{
					flag = AdsManager.Instance.HasRewardedVideo();
				}
				catch (Exception)
				{
					FMLogger.vCore("hasRewarded ex.");
					flag = false;
				}
				if (flag)
				{
					if (GeneralSettings.AdsDisabled)
					{
						this.state = FairyController.State.Entry;
					}
					else
					{
						this.state = FairyController.State.Ready;
						AppState.TimingBonusReady = true;
					}
				}
				else
				{
					if (!this.rewardedRequested)
					{
						AdsManager.Instance.SingleRewardedRequest();
						this.rewardedRequested = true;
					}
					this.state = FairyController.State.WaitingForRewarded;
					this.currentTime = 5f;
				}
			}
			break;
		case FairyController.State.WaitingForRewarded:
			this.currentTime -= Time.deltaTime;
			if (this.currentTime <= 0f)
			{
				bool flag2;
				try
				{
					flag2 = AdsManager.Instance.HasRewardedVideo();
				}
				catch (Exception)
				{
					FMLogger.vCore("hasRewarded ex.");
					flag2 = false;
				}
				if (flag2)
				{
					if (GeneralSettings.AdsDisabled)
					{
						this.state = FairyController.State.Entry;
					}
					else
					{
						this.state = FairyController.State.Ready;
						AppState.TimingBonusReady = true;
					}
				}
				else
				{
					this.currentTime = 5f;
				}
			}
			break;
		case FairyController.State.Ready:
			if (!AppState.TimingBonusReady)
			{
				this.rewardedRequested = false;
				this.currentTime = this.interval;
				this.state = FairyController.State.Timer;
			}
			break;
		}
	}

	[SerializeField]
	private float currentTime;

	private float delay;

	private float interval;

	[SerializeField]
	private FairyController.State state = FairyController.State.Entry;

	private const float rewardCheckStep = 5f;

	private bool rewardedRequested;

	[Serializable]
	private enum State
	{
		Timer,
		WaitingForRewarded,
		Ready,
		Entry
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GamePopupManager : FMPopupManager
{
	public void OpenRate()
	{
		base.Open(this.rate, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenAdsBlock(bool intentionalPause = false)
	{
		this.adsBlock.SetBlockMode(!intentionalPause);
		this.adsBlock.Open();
	}

	public void OpenHintHelp()
	{
		base.Open(this.hintHelp, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenFairyHelp()
	{
		base.Open(this.fairyHelp, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenRewardCollect(int amount)
	{
		base.Open(this.rewardCollect, FMPopupManager.FMPopupPriority.Normal);
		if (GeneralSettings.IsOldDesign)
		{
			this.lagacyRewardCollect.Prepare(amount);
		}
		else
		{
			this.neoRewardCollect.Prepare(amount);
		}
	}

	public void OpenDailyBonus()
	{
		base.Open(this.dailyBonus, FMPopupManager.FMPopupPriority.Normal);
	}

	public void CloseAdsBlock()
	{
		this.adsBlock.Close();
	}

	public void OpenAdsRemoved()
	{
		base.Open(this.adsRemoved, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenDummy()
	{
		base.Open(this.dummy, FMPopupManager.FMPopupPriority.Normal);
	}

	public void CloseActive()
	{
		base.CloseActivePopup(true);
	}

	public bool IsRateOpened()
	{
		return base.ActivePopup == this.rate;
	}

	public bool AnyPopupOpen()
	{
		return base.ActivePopup != null;
	}

	public void ClearQueued()
	{
		base.ClearQueue();
	}

	[SerializeField]
	private FMPopup dummy;

	[SerializeField]
	private FMPopup adsRemoved;

	[SerializeField]
	private FMPopup rate;

	[SerializeField]
	private AdsBlockPopup adsBlock;

	[SerializeField]
	private FMPopup hintHelp;

	[SerializeField]
	private FMPopup fairyHelp;

	[SerializeField]
	private FMPopup dailyBonus;

	[SerializeField]
	private RewardCollectPopup lagacyRewardCollect;

	[SerializeField]
	private NeoRewardCollectPopup neoRewardCollect;

	[SerializeField]
	private FMPopup rewardCollect;
}

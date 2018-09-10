// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MenuPopupManager : FMPopupManager
{
	public void OpenCategoryFilter()
	{
		base.Open((!SafeLayout.IsTablet) ? this.catFilter : this.tabletCatFilter, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenSelectPic(bool solved, PicItem picItem)
	{
		if (GeneralSettings.IsOldDesign)
		{
			if (SafeLayout.IsTablet)
			{
				this.navBarTablet.SetButtons(!solved, true, false);
				this.navBarTablet.SetIcon(picItem);
				base.Open(this.navBarTablet, FMPopupManager.FMPopupPriority.Normal);
			}
			else
			{
				this.navBarPhone.SetButtons(!solved, true, false);
				base.Open(this.navBarPhone, FMPopupManager.FMPopupPriority.Normal);
			}
		}
		else if (SafeLayout.IsTablet)
		{
			this.startGameFadePopup.SetButtons(!solved, true, false);
			this.startGameFadePopup.SetIcon(picItem);
			base.Open(this.startGameFadePopup, FMPopupManager.FMPopupPriority.Normal);
		}
		else
		{
			this.startGameSlidePopup.SetButtons(!solved, true, false);
			this.startGameSlidePopup.SetIcon(picItem);
			base.Open(this.startGameSlidePopup, FMPopupManager.FMPopupPriority.Normal);
		}
	}

	public void OpenFeedPic(bool solved, PicItem picItem)
	{
		if (GeneralSettings.IsOldDesign)
		{
			if (SafeLayout.IsTablet)
			{
				this.navBarTablet.SetButtons(!solved, true, true);
				this.navBarTablet.SetIcon(picItem);
				base.Open(this.navBarTablet, FMPopupManager.FMPopupPriority.Normal);
			}
			else
			{
				this.navBarPhone.SetButtons(!solved, true, true);
				base.Open(this.navBarPhone, FMPopupManager.FMPopupPriority.Normal);
			}
		}
		else if (SafeLayout.IsTablet)
		{
			this.startGameFadePopup.SetButtons(!solved, true, true);
			this.startGameFadePopup.SetIcon(picItem);
			base.Open(this.startGameFadePopup, FMPopupManager.FMPopupPriority.Normal);
		}
		else
		{
			this.startGameSlidePopup.SetButtons(!solved, true, true);
			this.startGameSlidePopup.SetIcon(picItem);
			base.Open(this.startGameSlidePopup, FMPopupManager.FMPopupPriority.Normal);
		}
	}

	public void OpenSync()
	{
		base.Open(this.sync, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenDailyBonus()
	{
		base.Open(this.dailyBonus, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenBonusPicsClaim(string code)
	{
		this.bonusContentController.Init(code, delegate
		{
			this.CloseActive();
			this.OpenBonusPicsError();
		});
		base.Open(this.bonusContent, FMPopupManager.FMPopupPriority.Normal);
	}

	private void OpenBonusPicsError()
	{
		base.Open(this.bonusContentError, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenBonusReclaimError()
	{
		base.Open(this.bonusReclaimError, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenErrorPictureDownload()
	{
		base.Open(this.downloadError, FMPopupManager.FMPopupPriority.ForceOpen);
	}

	public void OpenSettings()
	{
		if (base.ActivePopup == this.settings)
		{
			this.CloseActive();
		}
		else
		{
			base.Open(this.settings, FMPopupManager.FMPopupPriority.Normal);
		}
	}

	public void OpenAdsBlock(bool intentionalPause = false)
	{
		this.adsBlock.SetBlockMode(!intentionalPause);
		this.adsBlock.Open();
	}

	public void CloseAdsBlock()
	{
		this.adsBlock.Close();
	}

	public void ShowDesignUpdDialog()
	{
		if (this.designUpd != null)
		{
			base.Open(this.designUpd, FMPopupManager.FMPopupPriority.Normal);
		}
	}

	public void OpenAdsRemoved()
	{
		base.Open(this.adsRemoved, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenAbout()
	{
		base.Open(this.about, FMPopupManager.FMPopupPriority.Normal);
	}

	public void OpenHelp()
	{
		base.Open(this.help, FMPopupManager.FMPopupPriority.Normal);
	}

	public void CloseActive()
	{
		base.CloseActivePopup(true);
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
	private NavBarPopup navBarPhone;

	[SerializeField]
	private StartGameFadePopup startGameFadePopup;

	[SerializeField]
	private StartGameSlidePopup startGameSlidePopup;

	[SerializeField]
	private StartContexPopup navBarTablet;

	[SerializeField]
	private FMPopup adsRemoved;

	[SerializeField]
	private FMPopup downloadError;

	[SerializeField]
	private FMPopup sync;

	[SerializeField]
	private FMPopup about;

	[SerializeField]
	private FMPopup help;

	[SerializeField]
	private FMPopup dailyBonus;

	[SerializeField]
	private AdsBlockPopup adsBlock;

	[SerializeField]
	private FMPopup designUpd;

	[SerializeField]
	private FMPopup catFilter;

	[SerializeField]
	private FMPopup tabletCatFilter;

	[SerializeField]
	private FMPopup bonusContent;

	[SerializeField]
	private FMPopup bonusContentError;

	[SerializeField]
	private FMPopup bonusReclaimError;

	[SerializeField]
	private FMPopup settings;

	[SerializeField]
	private BonusContentPopup bonusContentController;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
//using SA.Common.Models;
//using SA.Common.Pattern;
using UnityEngine;

public class GameScreen : MonoBehaviour
{
	private void OnDisable()
	{
		//InAppPurchases.Instance.AdsRemoved -= this.OnAdsRemoved;
		//IOSCamera.OnImageSaved -= this.OnImageSavedToLibrary;
		AdsManager.Instance.RewardedVideoComplete -= this.OnRewardedComplete;
		this.gameboard.Clean();
		this.solvedPage.Clean();
	}

	private void OnAdsRemoved()
	{
	}

	//private void OnImageSavedToLibrary(Result result)
	//{
	//}

	private void OnRewardedComplete()
	{
		if (this.rewardBonus != 0)
		{
			this.popupManager.OpenRewardCollect(this.rewardBonus);
			GeneralSettings.UpdateCoins(this.rewardBonus);
			this.rewardBonus = 0;
		}
	}

	private void CheckDailyBonus()
	{
		if (AppState.HasDailyBonus)
		{
			AppState.ConsumeDailyBonus();
			GeneralSettings.UpdateCoins(AdsManager.Instance.RewardConfig.dailyBonus);
			this.popupManager.OpenDailyBonus();
		}
	}

	private void Awake()
	{
		if (GeneralSettings.IsOldDesign)
		{
			this.solvedPage = this.classicSolvedPage;
			this.solvedPopup = this.classicSolvePopup;
			this.fairyButton = this.legacyFairyButton;
		}
		else
		{
			this.solvedPage = this.neoSolvedPage;
			this.solvedPopup = this.neoSolvePopup;
			this.fairyButton = this.neoFairyButton;
		}
		this.gameboard.Solved += this.OnDoneClick;
		this.gameboard.Loaded += delegate()
		{
			this.sceneLoader.Close();
		};
		this.gameboard.Error += this.OnError;
		this.gameboard.SolvedPage = this.solvedPage;
		//InAppPurchases.Instance.AdsRemoved += this.OnAdsRemoved;
		//IOSCamera.OnImageSaved += this.OnImageSavedToLibrary;
		AdsManager.Instance.RewardedVideoComplete += this.OnRewardedComplete;
	}

	private void Start()
	{
		Application.targetFrameRate = ((SystemUtils.GetDevicePerfomance() != SystemUtils.DevicePerfomance.High) ? 30 : 60);
		base.StartCoroutine(this.StartGame());
	}

	private IEnumerator StartGame()
	{
		yield return 0;
		this.gameboard.StartGame();
		if (AdsManager.Instance.HasBannerPlacement(BannerPlacement.Gameboard))
		{
			this.safeLayout.ShowBannerBackground();
			AdsManager.Instance.ShowBanner(BannerPlacement.Gameboard);
		}
		yield break;
	}

	private void OnError(Gameboard.LoadError error)
	{
		if (!AdsManager.Instance.HasBannerPlacement(BannerPlacement.Menu))
		{
			AdsManager.Instance.HideBanner(true);
		}
		if (error == Gameboard.LoadError.Unknown)
		{
			FMLogger.LogError("***Unknown error during gb loading");
		}
		else
		{
			FMLogger.LogError("*** " + error + " error during gb loading");
			SharedData.Instance.DeletePictureData(Gameboard.pictureData);
		}
		this.sceneLoader.LoadSceneImmediate(SceneName.menu);
	}

	private IEnumerator DelayAction(float delay, Action a)
	{
		yield return new WaitForSeconds(delay);
		a();
		yield break;
	}

	private IEnumerator AppResumeDelayed(Action a)
	{
		yield return new WaitForSeconds(0.25f);
		a();
		yield break;
	}

	private IEnumerator FrameDelay(Action a)
	{
		yield return null;
		a();
		yield break;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (this.popupManager.IsRateOpened())
			{
				this.OnRateCancel();
				this.popupManager.CloseActive();
			}
			else if (this.popupManager.AnyPopupOpen())
			{
				this.popupManager.CloseActive();
			}
			else if (this.solvedPage.IsOpened)
			{
				if (this.solvedPage.IsAnimating)
				{
					this.OnSkipAnimationClick();
				}
				else
				{
					this.OnSolvedExitClick();
				}
			}
			else
			{
				this.OnBackClick();
			}
		}
		if (AppState.TimingBonusReady && !this.popupManager.AnyPopupOpen() && !this.solved)
		{
			AppState.TimingBonusReady = false;
			this.fairyButton.Show();
		}
	}

	private void OnApplicationPause(bool isPause)
	{
		if (isPause)
		{
			this.gameboard.UpdateSave(true);
		}
		if (isPause)
		{
			this.popupManager.OpenAdsBlock(AppState.IntentionalPause);
			this.gameboard.UpdateSaveIcon();
		}
		else
		{
			base.StartCoroutine(this.AppResumeDelayed(delegate
			{
				this.popupManager.CloseAdsBlock();
				if (AppState.PushNotificationLaunch || AppState.IsGiftCodeLaunch)
				{
					if (!AdsManager.Instance.HasBannerPlacement(BannerPlacement.Menu))
					{
						AdsManager.Instance.HideBanner(true);
					}
					this.gameboard.UpdateSaveIcon();
					this.sceneLoader.LoadSceneImmediate(SceneName.menu);
				}
				else
				{
					this.CheckDailyBonus();
					this.gameboard.OnResume();
					if (!AppState.IntentionalPause && AdsManager.Instance.HasInterstitial(AdPlacement.AppResume))
					{
						AdsManager.Instance.ShowInterstitial(AdPlacement.AppResume);
					}
				}
			}));
		}
	}

	public void OnBackClick()
	{
		if (!AdsManager.Instance.HasBannerPlacement(BannerPlacement.Menu))
		{
			AdsManager.Instance.HideBanner(true);
		}
		this.gameboard.LevelExit();
		bool flag = AdsManager.Instance.HasInterstitial(AdPlacement.GameboardBack);
		if (flag)
		{
			this.sceneLoader.Open(false);
			base.StartCoroutine(this.DelayAction(this.sceneLoader.duration, delegate
			{
				base.StartCoroutine(this.DelayAction(0.5f, delegate
				{
					this.sceneLoader.LoadSceneImmediate(SceneName.menu);
				}));
				this.gameboard.UpdateSaveIcon();
				AdsManager.Instance.ShowInterstitial(AdPlacement.GameboardStart);
			}));
		}
		else
		{
			this.sceneLoader.LoadScene(SceneName.menu, delegate
			{
				this.gameboard.UpdateSaveIcon();
			}, false);
		}
	}

	public void OnSolvedExitClick()
	{
		if (!AdsManager.Instance.HasBannerPlacement(BannerPlacement.Menu))
		{
			AdsManager.Instance.HideBanner(true);
		}
		this.sceneLoader.LoadScene(SceneName.menu, delegate
		{
			this.gameboard.UpdateSaveIcon();
		}, false);
	}

	public void OnRateClick()
	{
		AppState.SystemUtilsPause();
		SystemUtils.OpenAppRate(true);
		AnalyticsManager.RateConfirm();
	}

	public void OnRateCancel()
	{
		AnalyticsManager.RateCancel();
	}

	public void OnDoneClick()
	{
		this.solved = true;
		if (!AdsManager.Instance.HasBannerPlacement(BannerPlacement.Solved))
		{
			AdsManager.Instance.HideBanner(true);
			this.safeLayout.HideBannerBackground();
		}
		else
		{
			this.safeLayout.ShowBannerBackground();
			AdsManager.Instance.ShowBanner(BannerPlacement.Solved);
		}
		this.gameboard.PictureSolved();
		base.StartCoroutine(this.FrameDelay(delegate
		{
			base.StartCoroutine(this.DelayAction(0.2f, new Action(this.gameboard.ResetPosition)));
			this.solvedPopup.Open();
		}));
	}

	public void OnHintClick()
	{
		if (GeneralSettings.HintsCount == 0 && !GeneralSettings.AdsDisabled)
		{
			if (AdsManager.Instance.HasRewardedVideo())
			{
				GeneralSettings.HintRewardUse++;
				this.rewardBonus = 1;
				AdsManager.Instance.rewardInfo.SetPlace(RewardEventData.Place.Hint);
				AdsManager.Instance.ShowRewardedVideo();
			}
			else
			{
				this.toastManager.ShowEmptyHints();
			}
		}
		else if (this.gameboard.FindEmptyZone())
		{
			GeneralSettings.HintUsed();
			this.hintButton.UpdateUI();
			this.hintButton.PlayUsedAnimation();
			AnalyticsManager.HintUse(this.gameboard.HintsUsed, GeneralSettings.HintsUsed, Gameboard.pictureData.Id, this.gameboard.currentGUID);
		}
	}

	public void OnHintConfirmRewardedClick()
	{
		if (AdsManager.Instance.HasRewardedVideo())
		{
			GeneralSettings.HintRewardUse++;
			AnalyticsManager.HintPopupConfirm();
			this.rewardBonus = 1;
			AdsManager.Instance.rewardInfo.SetPlace(RewardEventData.Place.Hint);
			AdsManager.Instance.ShowRewardedVideo();
		}
	}

	public void OnHintCancelClick()
	{
		AnalyticsManager.HintPopupCancel();
	}

	public void OnFairyClick()
	{
		AnalyticsManager.FairyButtonClick(this.fairyButton.GetShowingTime(), this.fairyButton.currentGUID, this.rewardBonus);
		if (AdsManager.Instance.HasRewardedVideo())
		{
			GeneralSettings.FairyUseCount++;
			this.rewardBonus = AdsManager.Instance.RewardConfig.timingBonus;
			AdsManager.Instance.rewardInfo.SetPlace(RewardEventData.Place.Magic);
			AdsManager.Instance.ShowRewardedVideo();
		}
	}

	public void OnFairyConfirmClick()
	{
		if (AdsManager.Instance.HasRewardedVideo())
		{
			GeneralSettings.FairyUseCount++;
			this.rewardBonus = AdsManager.Instance.RewardConfig.timingBonus;
			AdsManager.Instance.rewardInfo.SetPlace(RewardEventData.Place.Magic);
			AnalyticsManager.FairyPopupConfirm();
			AdsManager.Instance.ShowRewardedVideo();
		}
	}

	public void OnFairyCancel()
	{
		AnalyticsManager.FairyPopupCancel();
	}

	public void OnDownloadPic()
	{
		this.savePicCounter++;
		this.solvedPage.CutShareableTexture(delegate(Texture2D tex)
		{
			string name = "Coloring_" + Gameboard.pictureData.Id;
			RuntimePlatform platform = Application.platform;
			if (platform != RuntimePlatform.Android)
			{
				if (platform == RuntimePlatform.IPhonePlayer)
				{
					AppState.SystemUtilsPause();
					//Singleton<IOSCamera>.Instance.SaveTextureToCameraRoll(tex);
				}
			}
			else
			{
				if (this.savePicCounter > 1)
				{
					return;
				}
				//Singleton<AndroidCamera>.Instance.SaveImageToGallery(tex, name);
				this.toastManager.ShowPicSaved();
				AnalyticsManager.OnSharePic(Gameboard.pictureData.Id, "library");
			}
		});
	}

	public void OnFBSharePic()
	{
		this.solvedPage.CutShareableTexture(delegate(Texture2D tex)
		{
			AppState.SystemUtilsPause();
			//UM_ShareUtility.FacebookShare(string.Empty, tex);
			AnalyticsManager.OnSharePic(Gameboard.pictureData.Id, "fb");
		});
	}

	public void OnInstaSharePic()
	{
		this.solvedPage.CutShareableTexture(delegate(Texture2D tex)
		{
			AppState.SystemUtilsPause();
			AnalyticsManager.OnSharePic(Gameboard.pictureData.Id, "instagram");
			//UM_ShareUtility.InstagramShare(string.Empty, tex);
		});
	}

	public void OnNativeSharePic()
	{
		this.solvedPage.CutShareableTexture(delegate(Texture2D tex)
		{
			AppState.SystemUtilsPause();
			AnalyticsManager.OnSharePic(Gameboard.pictureData.Id, "native");
			//UM_ShareUtility.ShareMedia(string.Empty, string.Empty, tex);
		});
	}

	public void OnSkipAnimationClick()
	{
		if (this.solvedPage.IsAnimating)
		{
			this.solvedPage.ForceFinishAnimation();
			AnalyticsManager.AnimationSkip(Gameboard.pictureData.Id);
		}
	}

	[SerializeField]
	private SceneLoader sceneLoader;

	[SerializeField]
	private SolvedPage classicSolvedPage;

	[SerializeField]
	private ColoringResultPage neoSolvedPage;

	private ISolvePage solvedPage;

	[SerializeField]
	private FMPopup classicSolvePopup;

	[SerializeField]
	private FMPopup neoSolvePopup;

	private FMPopup solvedPopup;

	[SerializeField]
	private ToastManager toastManager;

	[SerializeField]
	private GamePopupManager popupManager;

	[SerializeField]
	private FairyRewardButton legacyFairyButton;

	[SerializeField]
	private MagnifierFairyRewardButton neoFairyButton;

	private IFairyButton fairyButton;

	[SerializeField]
	private HintBtn hintButton;

	[SerializeField]
	private GameSafeLayout safeLayout;

	[SerializeField]
	private Gameboard gameboard;

	private bool solved;

	private int savePicCounter;

	private int rewardBonus = 1;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using FMUILayout;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
	private void OnEnable()
	{
		Input.multiTouchEnabled = false;
		//InAppPurchases.Instance.AdsRemoved += this.OnAdsRemoved;
		//Application.lowMemory += this.OnLowMemory;
	}

	private void OnDisable()
	{
		Input.multiTouchEnabled = true;
		//InAppPurchases.Instance.AdsRemoved -= this.OnAdsRemoved;
		//Application.lowMemory -= this.OnLowMemory;
	}

	private void OnAdsRemoved()
	{
		if (!GeneralSettings.IsOldDesign)
		{
			this.pageController.Options.HideAdsButton();
		}
		if (AppState.TimingBonusReady)
		{
			AppState.TimingBonusReady = false;
		}
	}

	private void OnLowMemory()
	{
		this.pageController.HandleLowMemory();
	}

	private void Start()
	{
		Application.targetFrameRate = 60;
		base.StartCoroutine(this.ScreenInit());
		AnalyticsManager.UpdateDesignProperty(GeneralSettings.IsOldDesign);
	}

	private IEnumerator ScreenInit()
	{
		MenuScreen.LaunchCount++;
		yield return 0;
		if (this.uiLayout != null)
		{
			this.uiLayout.Init();
			yield return 0;
		}
		this.pageController.Init();
		base.StartCoroutine(this.DelayAction(0.05f, delegate
		{
			this.pageController.LoadPageContent();
		}));
		if (MenuScreen.LaunchCount == 1)
		{
			if (AdsManager.Instance.WantToShowAppLaunchInterstitial())
			{
				base.StartCoroutine(this.CheckAppLauchAds());
			}
			else
			{
				base.StartCoroutine(this.DelayAction(0.1f, delegate
				{
					this.SceneLoaded();
					this.sceneLoader.Close();
				}));
			}
		}
		else
		{
			base.StartCoroutine(this.DelayAction(0.1f, delegate
			{
				this.SceneLoaded();
				this.sceneLoader.Close();
			}));
		}
		this.pageController.Select.ContentLoaded += this.CheckBonusCode;
		yield break;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (this.downloadPictureTask != null && this.downloadPictureTask.IsRunning)
			{
				if (this.downloadPictureTask.IsRunningExtraLong)
				{
					this.OnCancelPictureDownload();
				}
			}
			else if (this.popupManager.AnyPopupOpen())
			{
				this.popupManager.CloseActive();
			}
			else
			{
				Application.Quit();
			}
		}
	}

	private void SceneLoaded()
	{
		if (AdsManager.Instance.HasBannerPlacement(BannerPlacement.Menu))
		{
			AdsManager.Instance.ShowBanner(BannerPlacement.Menu);
		}
		this.CheckDailyBonus();
		if (AppState.ShowDesignUpdDialog)
		{
			AppState.ShowDesignUpdDialog = false;
			this.popupManager.ShowDesignUpdDialog();
		}
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

	private IEnumerator CheckAppLauchAds()
	{
		double timeSinceStart = (DateTime.Now - AppState.LaunchTime).TotalSeconds;
		double waitTime = (double)AdsManager.Instance.AppLaunchPlacementDelay - timeSinceStart;
		bool hasAds = AdsManager.Instance.AppLaunchInterstitialReady();
		while (waitTime > 0.0 && !hasAds && WebLoader.Instance.HasInternetConnection)
		{
			waitTime -= (double)Time.deltaTime;
			hasAds = AdsManager.Instance.AppLaunchInterstitialReady();
			yield return 0;
			waitTime -= (double)Time.deltaTime;
			yield return 0;
		}
		if (hasAds)
		{
			base.StartCoroutine(this.DelayAction(0.5f, delegate
			{
				this.SceneLoaded();
				this.sceneLoader.Close();
			}));
			AdsManager.Instance.ShowInterstitial(AdPlacement.AppLaunch);
		}
		else
		{
			this.SceneLoaded();
			this.sceneLoader.Close();
		}
		yield break;
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

	private void CheckBonusCode()
	{
		if (AppState.IsGiftCodeLaunch)
		{
			BonusCodeData bonusCode = AppState.BonusCode;
			AppState.BonusCode = null;
			if (!bonusCode.alreadyActivated)
			{
				this.popupManager.OpenBonusPicsClaim(bonusCode.BonusCode);
			}
			else
			{
				this.popupManager.OpenBonusReclaimError();
			}
		}
	}

	private void AdsAndLoadScene(string sceneName, bool isAlreadyClosed = false)
	{
		bool flag = GeneralSettings.AppLaunchCounter > 1 && AdsManager.Instance.HasInterstitial(AdPlacement.GameboardStart);
		if (flag)
		{
			if (!isAlreadyClosed)
			{
				this.sceneLoader.Open(false);
				isAlreadyClosed = true;
				base.StartCoroutine(this.DelayAction(this.sceneLoader.duration, delegate
				{
					ImageManager.Instance.UnloadAllTextures();
					this.StartCoroutine(this.DelayAction(0.5f, delegate
					{
						this.LoadScene(sceneName, isAlreadyClosed, null);
					}));
					AdsManager.Instance.ShowInterstitial(AdPlacement.GameboardStart);
				}));
			}
			else
			{
				ImageManager.Instance.UnloadAllTextures();
				base.StartCoroutine(this.DelayAction(0.5f, delegate
				{
					this.LoadScene(sceneName, isAlreadyClosed, null);
				}));
				AdsManager.Instance.ShowInterstitial(AdPlacement.GameboardStart);
			}
		}
		else
		{
			if (isAlreadyClosed)
			{
				ImageManager.Instance.UnloadAllTextures();
			}
			this.LoadScene(sceneName, isAlreadyClosed, new Action(ImageManager.Instance.UnloadAllTextures));
		}
	}

	private void LoadScene(string sceneName, bool isAlreadyClosed = false, Action callbackOnLoaded = null)
	{
		if (!AdsManager.Instance.HasBannerPlacement(BannerPlacement.Gameboard))
		{
			AdsManager.Instance.HideBanner(true);
		}
		if (isAlreadyClosed)
		{
			this.sceneLoader.LoadSceneImmediate(sceneName);
		}
		else
		{
			this.sceneLoader.LoadScene(sceneName, callbackOnLoaded, false);
		}
	}

	private void OnApplicationPause(bool isPause)
	{
		if (isPause)
		{
			this.popupManager.OpenAdsBlock(AppState.IntentionalPause);
		}
		else
		{
			base.StartCoroutine(this.AppResumeDelayed(delegate
			{
				this.CheckDailyBonus();
				this.popupManager.CloseAdsBlock();
				if (AppState.IsServerContentExpired() || AppState.PushNotificationLaunch || AppState.IsGiftCodeLaunch)
				{
					if (!AdsManager.Instance.HasBannerPlacement(BannerPlacement.Menu))
					{
						AdsManager.Instance.HideBanner(true);
					}
					this.sceneLoader.LoadSceneImmediate(SceneName.menu);
				}
				else if (!AppState.IntentionalPause && AdsManager.Instance.HasInterstitial(AdPlacement.AppResume))
				{
					AdsManager.Instance.ShowInterstitial(AdPlacement.AppResume);
				}
			}));
		}
	}

	private void ProcessPicItem(PicItem item)
	{
		PictureType picType = item.PictureData.picType;
		if (picType != PictureType.Web)
		{
			if (picType != PictureType.System)
			{
				if (picType == PictureType.Local)
				{
					if (item.PictureData.HasSave)
					{
						this.selectedItem = item;
						if (this.pageController.Feed.IsOpened)
						{
							this.popupManager.OpenFeedPic(this.selectedItem.SaveData.progres == 100, this.selectedItem);
						}
						else
						{
							this.popupManager.OpenSelectPic(this.selectedItem.SaveData.progres == 100, this.selectedItem);
						}
					}
					else
					{
						if (!item.PictureData.Solved)
						{
							Gameboard.StartAnalyticEvent = new Gameboard.StartEvent
							{
								type = Gameboard.StartEventType.New,
								id = item.PictureData.Id
							};
						}
						Gameboard.pictureData = item.PictureData;
						this.AdsAndLoadScene(SceneName.game, false);
					}
				}
			}
		}
		else
		{
			this.selectedItem = item;
			this.sceneLoader.Open(true);
			this.DownloadPictureData(item);
		}
	}

	private void DownloadPictureData(PicItem item)
	{
		this.downloadPictureTask = new DownloadPictureTask(item.PictureData, new Action<bool, PictureData>(this.OnPictureDataDownloadResult), new Action(this.OnSlowPictureDataDownload), new Action(this.OnExtraSlowPictureDataDownload));
		SharedData.Instance.DownloadPicture(this.downloadPictureTask);
	}

	private void OnPictureDataDownloadResult(bool success, PictureData pd)
	{
		if (success)
		{
			Gameboard.StartAnalyticEvent = new Gameboard.StartEvent
			{
				type = Gameboard.StartEventType.New,
				id = pd.Id
			};
			Gameboard.pictureData = pd;
			this.AdsAndLoadScene(SceneName.game, true);
		}
		else
		{
			this.popupManager.OpenErrorPictureDownload();
			this.sceneLoader.Close();
		}
	}

	private void OnSlowPictureDataDownload()
	{
		FMLogger.vCore("OnSlowPictureDataDownload");
		this.sceneLoader.ShowSlowConnectionLabel();
	}

	private void OnExtraSlowPictureDataDownload()
	{
		FMLogger.vCore("OnExtraSlowPictureDataDownload. Can cancel");
		this.sceneLoader.ShowCloseButton(new Action(this.OnCancelPictureDownload));
	}

	public void OnPictureClick(PicItem item)
	{
		MenuState menuState = MenuScreen.MenuState;
		if (menuState != MenuState.Select)
		{
			if (menuState == MenuState.Feed)
			{
				MenuScreen.PaintStartSource = PaintStartSource.FeedPic;
			}
		}
		else
		{
			MenuScreen.PaintStartSource = PaintStartSource.LibPic;
			AnalyticsManager.SelectPicClick(item.Id, this.pageController.Select.GetItemPositionIndex(item), this.pageController.Select.ActiveCategory, this.pageController.Select.IsFilterCompletedOn);
		}
		this.ProcessPicItem(item);
	}

	public void OnRetryDownloadClick()
	{
		this.ProcessPicItem(this.selectedItem);
	}

	public void OnCancelPictureDownload()
	{
		FMLogger.vCore("OnCancelPictureDownload click");
		if (this.downloadPictureTask != null)
		{
			this.downloadPictureTask.Cancel();
		}
		this.sceneLoader.Close();
	}

	public void OnCategoryFilterClick()
	{
		this.popupManager.OpenCategoryFilter();
		AnalyticsManager.MenuCategoryFilterClick();
	}

	public void OnCategoryClick(CategoryFilterButton btn)
	{
		if (this.pageController.Select.OpenCategory(btn.CategoryId, false))
		{
			this.popupManager.CloseActive();
		}
	}

	public void OnCategoryFilterCompletedClick()
	{
		GeneralSettings.FilterCompleted = !GeneralSettings.FilterCompleted;
		this.pageController.Select.CategoryFilterCompleted();
		if (GeneralSettings.IsOldDesign)
		{
			this.popupManager.CloseActive();
		}
	}

	public void OnGetMoreFBPicsClick(ExternalLinkButton btn)
	{
		AppState.SystemUtilsPause();
		SystemUtils.OpenFbPage(btn.Scheme, btn.URL);
		AnalyticsManager.GetMoreBonusContentCatBar();
	}

	public void OnGetMoreBonusContentEmptyCatClick(ExternalLinkButton btn)
	{
		AppState.SystemUtilsPause();
		SystemUtils.OpenFbPage(btn.Scheme, btn.URL);
		AnalyticsManager.GetMoreBonusContentEmptyCat();
	}

	public void OnFeaturedItemClick(FeaturedItem featuredItem)
	{
		FeaturedItem.ItemType type = featuredItem.Type;
		if (type != FeaturedItem.ItemType.Daily)
		{
			if (type != FeaturedItem.ItemType.PromoPic)
			{
				if (type == FeaturedItem.ItemType.ExternalLink)
				{
					ExternalLinkItem externalLinkItem = (ExternalLinkItem)featuredItem;
					AnalyticsManager.FeaturedExternalLink(externalLinkItem.Id);
					SystemUtils.OpenUrl(externalLinkItem.TargetScheme, externalLinkItem.TargetUrl);
					FMLogger.vCore("External link click. " + externalLinkItem.TargetScheme + " url:" + externalLinkItem.TargetUrl);
				}
			}
			else
			{
				MenuScreen.PaintStartSource = PaintStartSource.LibFeaturedPromoPic;
				PromoPicItem promoPicItem = (PromoPicItem)featuredItem;
				PicItem picItem = promoPicItem.PicItem;
				AnalyticsManager.FeaturePromoPicClick(picItem.Id, promoPicItem.Order);
				this.ProcessPicItem(picItem);
			}
		}
		else
		{
			MenuScreen.PaintStartSource = PaintStartSource.LibFeaturedDailyPic;
			DailyPicItem dailyPicItem = (DailyPicItem)featuredItem;
			PicItem picItem = dailyPicItem.PicItem;
			AnalyticsManager.FeaturedDailyClick(picItem.Id, dailyPicItem.Order);
			this.ProcessPicItem(picItem);
		}
	}

	public void OnDailyTabPicClick(PicItem item)
	{
		MenuScreen.PaintStartSource = PaintStartSource.DailyOldPic;
		DailyEventInfo dailyDate = this.pageController.Daily.GetDailyDate(item.Id);
		AnalyticsManager.DailyTabPicClick(item.Id, dailyDate.row, dailyDate.day, dailyDate.month, dailyDate.year);
		this.ProcessPicItem(item);
	}

	public void OnDailyTabFeaturedClick(FeaturedItem featuredItem)
	{
		MenuScreen.PaintStartSource = PaintStartSource.DailyTodayPic;
		DailyPicItem dailyPicItem = (DailyPicItem)featuredItem;
		AnalyticsManager.DailyTabFeaturedClick(dailyPicItem.PicItem.Id);
		this.ProcessPicItem(dailyPicItem.PicItem);
	}

	public void OnBonusContentClaimClick()
	{
		this.popupManager.CloseActive();
		this.pageController.Select.OpenCategory(ContentFilterNavBar.FBCategoryId, false);
	}

	public void OnGalleryClick()
	{
		if (AdsManager.Instance.HasInterstitial(AdPlacement.LibNav))
		{
			AdsManager.Instance.ShowInterstitial(AdPlacement.LibNav);
		}
		this.pageController.OpenSelect();
		MenuScreen.MenuState = MenuState.Select;
	}

	public void OnFeedClick()
	{
		if (AdsManager.Instance.HasInterstitial(AdPlacement.FeedNav))
		{
			AdsManager.Instance.ShowInterstitial(AdPlacement.FeedNav);
		}
		this.pageController.OpenFeed();
		MenuScreen.MenuState = MenuState.Feed;
	}

	public void OnDailyClick()
	{
		this.pageController.OpenDaily();
		MenuScreen.MenuState = MenuState.Daily;
	}

	public void OnMenuClick()
	{
		if (AdsManager.Instance.HasInterstitial(AdPlacement.MenuNav))
		{
			AdsManager.Instance.ShowInterstitial(AdPlacement.MenuNav);
		}
		this.pageController.OpenMenu();
		MenuScreen.MenuState = MenuState.Menu;
	}

	public void OnNewsClick()
	{
		this.pageController.OpenNews();
		MenuScreen.MenuState = MenuState.News;
		AnalyticsManager.NewTabOpened();
	}

	public void OnSettingsClick()
	{
		this.popupManager.OpenSettings();
	}

	public void OnContinueClick()
	{
		MenuState menuState = MenuScreen.MenuState;
		if (menuState != MenuState.Select)
		{
			if (menuState != MenuState.Feed)
			{
				if (menuState == MenuState.Daily)
				{
					Gameboard.StartAnalyticEvent = new Gameboard.StartEvent
					{
						type = Gameboard.StartEventType.Continue,
						id = this.selectedItem.Id,
						progress = this.selectedItem.SaveData.ProgressRoundFive(),
						screenFrom = "daily"
					};
				}
			}
			else
			{
				Gameboard.StartAnalyticEvent = new Gameboard.StartEvent
				{
					type = Gameboard.StartEventType.Continue,
					id = this.selectedItem.Id,
					progress = this.selectedItem.SaveData.ProgressRoundFive(),
					screenFrom = "feed"
				};
			}
		}
		else
		{
			Gameboard.StartAnalyticEvent = new Gameboard.StartEvent
			{
				type = Gameboard.StartEventType.Continue,
				id = this.selectedItem.Id,
				progress = this.selectedItem.SaveData.ProgressRoundFive(),
				screenFrom = "library"
			};
		}
		Gameboard.pictureData = this.selectedItem.PictureData;
		this.AdsAndLoadScene(SceneName.game, false);
	}

	public void OnRestartClick()
	{
		MenuState menuState = MenuScreen.MenuState;
		if (menuState != MenuState.Select)
		{
			if (menuState != MenuState.Feed)
			{
				if (menuState == MenuState.Daily)
				{
					Gameboard.StartAnalyticEvent = new Gameboard.StartEvent
					{
						type = Gameboard.StartEventType.Restart,
						id = this.selectedItem.Id,
						progress = this.selectedItem.SaveData.ProgressRoundFive(),
						screenFrom = "daily"
					};
				}
			}
			else
			{
				Gameboard.StartAnalyticEvent = new Gameboard.StartEvent
				{
					type = Gameboard.StartEventType.Restart,
					id = this.selectedItem.Id,
					progress = this.selectedItem.SaveData.ProgressRoundFive(),
					screenFrom = "feed"
				};
			}
		}
		else
		{
			Gameboard.StartAnalyticEvent = new Gameboard.StartEvent
			{
				type = Gameboard.StartEventType.Restart,
				id = this.selectedItem.Id,
				progress = this.selectedItem.SaveData.ProgressRoundFive(),
				screenFrom = "library"
			};
		}
		SharedData.Instance.DeleteSave(this.selectedItem.PictureData);
		Gameboard.pictureData = this.selectedItem.PictureData;
		this.AdsAndLoadScene(SceneName.game, false);
	}

	public void OnSaveDeleteClick()
	{
		this.popupManager.CloseActive();
		SharedData.Instance.DeletePictureData(this.selectedItem.PictureData);
		this.selectedItem = null;
	}

	public void OnSaveShareClick()
	{
	}

	public void OnAppRateClick()
	{
		AppState.SystemUtilsPause();
		SystemUtils.OpenAppRate(false);
		AnalyticsManager.RateMenu();
	}

	public void OnAboutClick()
	{
		this.popupManager.OpenAbout();
	}

	public void OnRemoveAdsClick()
	{
		AppState.InAppPause();
		//InAppPurchases.Instance.BuyNoAds();
	}

	public void OnShareClick()
	{
		AppState.SystemUtilsPause();
		SystemUtils.OpenNativeShareDialog();
	}

	public void OnHelpClick()
	{
		this.popupManager.OpenHelp();
	}

	public void OnContactClick()
	{
		AppState.SystemUtilsPause();
		SystemUtils.SupportEmail();
	}

	public void OnSudokuPromoClick()
	{
		AppState.SystemUtilsPause();
		SystemUtils.OpenSudokuPage();
		AnalyticsManager.PromoSudoku();
	}

	public void OnSolitairePromoClick()
	{
		AppState.SystemUtilsPause();
		SystemUtils.OpenSolitairePage();
		AnalyticsManager.PromoSolitaire();
	}

	public void OnRestorePurchasesClick()
	{
		AppState.SystemUtilsPause();
		//InAppPurchases.Instance.RestorePurchases();
		AnalyticsManager.RestoreClick();
	}

	public void OnTermsClick()
	{
		AppState.SystemUtilsPause();
		SystemUtils.OpenBrowser("https://x-flow.app/terms-of-use.html");
	}

	public void OnPrivacyClick()
	{
		AppState.SystemUtilsPause();
		SystemUtils.OpenBrowser("https://x-flow.app/privacy-policy.html");
	}

	public void OnNumFontChange(float value)
	{
		int numSize = Mathf.RoundToInt(Mathf.Lerp(18f, 60f, value));
		GeneralSettings.NumSize = numSize;
	}

	public void OnDesignChangeClick()
	{
		if (GeneralSettings.IsOldDesign)
		{
			GeneralSettings.CurrentDesign = GeneralSettings.DesignType.Neo;
			AnalyticsManager.SettingsNewDesign();
		}
		else
		{
			GeneralSettings.CurrentDesign = GeneralSettings.DesignType.Legacy;
			AnalyticsManager.SettingsOldDesign();
		}
		this.LoadScene(SceneName.menu, false, null);
	}

	public static MenuState MenuState;

	public static PaintStartSource PaintStartSource;

	public static int RowItems = 2;

	[SerializeField]
	private UILayoutManager uiLayout;

	[SerializeField]
	private SceneLoader sceneLoader;

	[SerializeField]
	private MenuPopupManager popupManager;

	[SerializeField]
	private MenuPageController pageController;

	private float screenWidth;

	private PicItem selectedItem;

	private DownloadPictureTask downloadPictureTask;

	private static int LaunchCount;
}

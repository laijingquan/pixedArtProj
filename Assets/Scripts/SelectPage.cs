// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SelectPage : Page
{
	 
	public event Action ContentLoaded;

	public int ActiveCategory
	{
		get
		{
			return SelectPage.LastCategory;
		}
	}

	public bool IsFilterCompletedOn
	{
		get
		{
			return SelectPage.FilterCompleted;
		}
	}

	private bool isOldDesign
	{
		get
		{
			return this.categoryBar == null;
		}
	}

	public void LoadContent()
	{
		if (SafeLayout.IsTablet)
		{
			this.categoriesFilter = this.tabletCatFilterPopup;
		}
		else
		{
			this.categoriesFilter = this.phoneCatFilterNavBar;
		}
		if (!GeneralSettings.IsOldDesign)
		{
			SelectPage.FilterCompleted = GeneralSettings.FilterCompleted;
		}
		this.connectionResumeHelper = base.GetComponent<ConnectionResumeHelper>();
		this.waitingForPageResponse = true;
		this.pageContentTask = new PageContentTask(1, new Action<bool, PageContentInfo>(this.OnPageContentLoaded), new Action(this.OnSlowConnection));
		SharedData.Instance.LoadMainPage(this.pageContentTask);
	}

	public bool OpenCategory(int categoryId, bool initialLoad = false)
	{
		bool flag;
		if (!this.isOldDesign)
		{
			flag = this.categoryBar.SelectCategory(categoryId, initialLoad);
		}
		else
		{
			flag = this.categoriesFilter.SelectCategory(categoryId);
		}
		if (flag)
		{
			SelectPage.LastCategory = categoryId;
			if (!initialLoad)
			{
				this.scroll.FillterByCategoryId(categoryId, SelectPage.FilterCompleted, false, !base.IsOpened);
				AnalyticsManager.MenuCategoryFilterByCategory(SelectPage.LastCategory);
			}
			else
			{
				this.scroll.FillterByCategoryId(categoryId, SelectPage.FilterCompleted, true, !base.IsOpened);
			}
		}
		int displayPicsCount = this.scroll.DisplayPicsCount;
		if (this.isOldDesign)
		{
			string catName = string.Empty;
			catName = ((!(this.categoriesFilter.ActiveCategory != null)) ? string.Empty : this.categoriesFilter.ActiveCategory.CategoryName);
			if (categoryId == ContentFilterNavBar.ShowAllCategoryId)
			{
				catName = LocalizationService.Instance.GetTextByKey("filter_search");
			}
			int id = (!(this.categoriesFilter.ActiveCategory != null)) ? ContentFilterNavBar.ShowAllCategoryId : this.categoriesFilter.ActiveCategory.CategoryId;
			this.categoriesMenu.SetCategory(catName, id, displayPicsCount == 0);
		}
		else
		{
			this.fbPageBackground.gameObject.SetActive(categoryId == ContentFilterNavBar.FBCategoryId && displayPicsCount == 0);
		}
		if (categoryId == ContentFilterNavBar.FBCategoryId && !initialLoad)
		{
			AnalyticsManager.BonusCategorySelected(displayPicsCount);
		}
		return flag;
	}

	public void CategoryFilterCompleted()
	{
		if (GeneralSettings.IsOldDesign)
		{
			SelectPage.FilterCompleted = !SelectPage.FilterCompleted;
		}
		else
		{
			SelectPage.FilterCompleted = GeneralSettings.FilterCompleted;
		}
		if (this.isOldDesign)
		{
			this.categoriesFilter.FilterCompleted(SelectPage.FilterCompleted);
		}
		this.scroll.FillterByCategoryId(SelectPage.LastCategory, SelectPage.FilterCompleted, false, !base.IsOpened);
		AnalyticsManager.MenuCategoryHideCompletedClick(SelectPage.FilterCompleted);
	}

	public int GetItemPositionIndex(PicItem picItem)
	{
		return this.scroll.GetItemPositionIndex(picItem);
	}

	public void ResetLastCategory()
	{
		SelectPage.LastCategory = ContentFilterNavBar.ShowAllCategoryId;
	}

	public void ReloadTextures()
	{
		this.scroll.ReloadTextures();
	}

	public void UnloadTextures()
	{
		this.scroll.UnloadTextures();
	}

	private void OnPageContentLoaded(bool success, PageContentInfo pageContent)
	{
		if (success)
		{
			this.PrepareForContnentLoad();
			this.HideConnectionError();
			this.waitingForPageResponse = false;
			this.scrolledToBottom = true;
			bool flag = false;
			float num = 0f;
			int num2 = 0;
			if (pageContent.Featured != null && !pageContent.Featured.IsEmpty())
			{
				if (GeneralSettings.IsOldDesign)
				{
					this.featured.gameObject.SetActive(true);
					this.featured.LoadContent(pageContent.Featured);
					num2 = this.featured.SectionHeight;
				}
				else
				{
					this.featuredSection.gameObject.SetActive(true);
					this.featuredSection.LoadContent(pageContent.Featured);
					num2 = this.featuredSection.SectionHeight;
				}
				this.scroll.AddTopOffset(num2);
				num += (float)num2;
				flag = true;
			}
			if (pageContent.CategoryInfos != null && pageContent.CategoryInfos.Count > 0)
			{
				if (this.isOldDesign)
				{
					this.categoriesFilter.Compose(pageContent.CategoryInfos, SelectPage.FilterCompleted);
					this.categoriesMenu.gameObject.SetActive(true);
					if (flag)
					{
						this.categoriesMenu.AddTopOffset(num2);
					}
					this.scroll.AddTopOffset(this.categoriesMenu.SectionHeight);
					this.scroll.SetScrollTitleOffset(this.categoriesMenu.SectionHeight);
					num += (float)this.categoriesMenu.SectionHeight;
				}
				else
				{
					this.categoryBar.gameObject.SetActive(true);
					this.categoryBar.Compose(pageContent.CategoryInfos, SelectPage.FilterCompleted);
					if (flag)
					{
						this.categoryBar.AddTopOffset(num2);
					}
					this.scroll.AddTopOffset(this.categoryBar.SectionHeight);
					this.scroll.SetScrollTitleOffset(this.categoryBar.SectionHeight);
					num += (float)this.categoryBar.SectionHeight;
				}
			}
			List<PictureData> pics = pageContent.Pics;
			if (pics != null && pics.Count > 0)
			{
				this.scroll.LoadContent(pics);
				int num3 = SharedData.Instance.CalculateLibCompletePercent();
				if (num3 != -1)
				{
					AnalyticsManager.UpdateUserLibProgressProperty(num3);
				}
			}
			this.OpenCategory(SelectPage.LastCategory, true);
			if (pageContent.UpdateDialog != null && Gameboard.pictureData == null && (pageContent.UpdateDialog.ver == 0 || pageContent.UpdateDialog.ver > GeneralSettings.UpdateDialogVersion))
			{
				GeneralSettings.UpdateDialogVersion = pageContent.UpdateDialog.ver;
				AppState.SystemUtilsPause();
				SystemUtils.ShowUpdateDialog(pageContent.UpdateDialog);
			}
			RectTransform rectTransform = (RectTransform)base.transform;
			this.fbBackgroundLabel.anchoredPosition = new Vector2(0f, -rectTransform.rect.height / 2f - num / 2f);
			if (!GeneralSettings.IsOldDesign)
			{
				this.scroll.GetComponent<ScrollConstraints>().Init();
			}
			if (pageContent.BonusCategoryConfig != null && pageContent.BonusCategoryConfig.links != null)
			{
				for (int i = 0; i < pageContent.BonusCategoryConfig.links.Count; i++)
				{
					if (pageContent.BonusCategoryConfig.links[i].place != null)
					{
						if (pageContent.BonusCategoryConfig.links[i].place.Equals("background"))
						{
							this.fbPageBackground.Init(pageContent.BonusCategoryConfig.links[i].scheme, pageContent.BonusCategoryConfig.links[i].url);
						}
						else if (pageContent.BonusCategoryConfig.links[i].place.Equals("categoryBar"))
						{
							this.fbPageCatBar.Init(pageContent.BonusCategoryConfig.links[i].scheme, pageContent.BonusCategoryConfig.links[i].url);
						}
					}
				}
			}
			if (pageContent.News != null)
			{
				this.newsPage.LoadContent(pageContent.News);
			}
			base.StartCoroutine(base.DelayAction(5, 0f, new Action(this.ContentLoadComplete)));
		}
		else
		{
			this.ShowConnectionError();
		}
		this.pageContentTask = null;
		this.loadIndicator.SetActive(false);
		if (this.ContentLoaded != null)
		{
			this.ContentLoaded();
		}
	}

	public void OnRetryClick()
	{
		if (this.pageContentTask != null)
		{
			this.pageContentTask.Cancel();
			this.pageContentTask = null;
		}
		if (!WebLoader.Instance.HasInternetConnection)
		{
			WebLoader.Instance.CheckInternetConnection(null);
		}
		else
		{
			this.OnConnectionResume();
		}
	}

	private void OnEnable()
	{
		WebLoader.Instance.ConnectionResume += this.OnConnectionResume;
		WebLoader.Instance.ConnectionError += this.OnConnectionError;
		if (!GeneralSettings.IsOldDesign)
		{
			this.settingBtn.SetActive(!SafeLayout.IsTablet);
		}
	}

	private void OnDisable()
	{
		if (this.pageContentTask != null)
		{
			this.pageContentTask.Cancel();
			this.pageContentTask = null;
		}
		WebLoader.Instance.ConnectionResume -= this.OnConnectionResume;
		WebLoader.Instance.ConnectionError -= this.OnConnectionError;
	}

	private void ShowSlowConnection()
	{
		if (this.showingError)
		{
			this.noInternetLabel.SlowConnection();
			return;
		}
		this.showingError = true;
		RectTransform rectTransform = (RectTransform)this.scroll.transform;
		rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -this.noInternetLabel.RectTransform.sizeDelta.y);
		this.noInternetLabel.SlowConnection();
		this.noInternetLabel.gameObject.SetActive(true);
	}

	private void ShowConnectionError()
	{
		if (this.showingError)
		{
			this.noInternetLabel.NoConnection();
			return;
		}
		this.showingError = true;
		RectTransform rectTransform = (RectTransform)this.scroll.transform;
		rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -this.noInternetLabel.RectTransform.sizeDelta.y);
		this.noInternetLabel.NoConnection();
		this.noInternetLabel.gameObject.SetActive(true);
	}

	private void HideConnectionError()
	{
		if (!this.showingError)
		{
			return;
		}
		this.showingError = false;
		RectTransform rectTransform = (RectTransform)this.scroll.transform;
		rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0f);
		this.noInternetLabel.gameObject.SetActive(false);
		this.noInternetLabel.ResetStatus();
	}

	private void RetryLoadPageContent()
	{
		if (!this.waitingForPageResponse)
		{
			return;
		}
		if (this.scrolledToBottom)
		{
			return;
		}
		if (this.pageContentTask != null && this.pageContentTask.IsRunning)
		{
			return;
		}
		this.pageContentTask = new PageContentTask(1, new Action<bool, PageContentInfo>(this.OnPageContentLoaded), new Action(this.OnSlowConnection));
		this.loadIndicator.SetActive(true);
		this.waitingForPageResponse = true;
		SharedData.Instance.LoadMainPage(this.pageContentTask);
	}

	private void OnSlowConnection()
	{
		this.ShowSlowConnection();
	}

	private void OnConnectionError()
	{
		this.ShowConnectionError();
	}

	private void OnConnectionResume()
	{
		this.connectionResumeHelper.SafeResume(delegate
		{
			this.HideConnectionError();
			this.RetryLoadPageContent();
			this.scroll.ReloadFailedIcon();
			if (GeneralSettings.IsOldDesign)
			{
				this.featured.ReloadFailedIcon();
			}
			else
			{
				this.featuredSection.ReloadFailedIcon();
			}
		});
	}

	protected override void OnBeginOpenning()
	{
		this.scroll.ReloadTextures();
	}

	protected override void OnOpened()
	{
		if (GeneralSettings.IsOldDesign)
		{
			this.featured.WillBecomeVisable();
		}
		else
		{
			this.featuredSection.WillBecomeVisable();
		}
	}

	protected override void OnBeginClosing()
	{
	}

	protected override void OnClosed()
	{
		this.scroll.UnloadTextures();
	}

	[SerializeField]
	private NewsPage newsPage;

	[SerializeField]
	private GameObject settingBtn;

	[SerializeField]
	private ExternalLinkButton fbPageCatBar;

	[SerializeField]
	private ExternalLinkButton fbPageBackground;

	[SerializeField]
	private FeaturedMenuBar featured;

	[SerializeField]
	private FeaturedSection featuredSection;

	[SerializeField]
	private CategoryBar categoryBar;

	[SerializeField]
	private ContentFilterMenuBar categoriesMenu;

	[SerializeField]
	private ContentFilterNavBar phoneCatFilterNavBar;

	[SerializeField]
	private ContentFilterPopup tabletCatFilterPopup;

	[SerializeField]
	private ICategoryFilterView categoriesFilter;

	[SerializeField]
	private ConnectionStatusPanel noInternetLabel;

	[SerializeField]
	private GameObject loadIndicator;

	[SerializeField]
	private SelectScroll scroll;

	[SerializeField]
	private RectTransform fbBackgroundLabel;

	private ConnectionResumeHelper connectionResumeHelper;

	private bool waitingForPageResponse;

	private bool scrolledToBottom;

	private PageContentTask pageContentTask;

	private bool showingError;

	private static int LastCategory = ContentFilterNavBar.ShowAllCategoryId;

	private static bool FilterCompleted = false;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyPage : Page
{
	public void LoadContent()
	{
		this.connectionResumeHelper = base.GetComponent<ConnectionResumeHelper>();
		this.waitingForPageResponse = true;
		this.pageContentTask = new DailyPageContentTask(1, new Action<bool, DailyTabInfo>(this.OnPageContentLoaded), new Action(this.OnSlowConnection));
		SharedData.Instance.LoadDailyPage(this.pageContentTask);
	}

	public void UnloadTextures()
	{
		this.scroll.UnloadTextures();
	}

	public DailyEventInfo GetDailyDate(int id)
	{
		return this.scroll.GetDailyDate(id);
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
		if (this.pageContentTask != null)
		{
			this.pageContentTask.Cancel();
		}
		this.pageContentTask = new DailyPageContentTask(1, new Action<bool, DailyTabInfo>(this.OnPageContentLoaded), new Action(this.OnSlowConnection));
		this.loadIndicator.SetActive(true);
		this.waitingForPageResponse = true;
		SharedData.Instance.LoadDailyPage(this.pageContentTask);
	}

	private void OnPageContentLoaded(bool success, DailyTabInfo pageContent)
	{
		if (success)
		{
			this.PrepareForContnentLoad();
			this.HideConnectionError();
			this.waitingForPageResponse = false;
			this.scrolledToBottom = true;
			this.scroll.LoadContent(pageContent, !base.IsOpened);
			this.UpdateNewDailyIndicator();
			if (base.IsOpened)
			{
				this.MarkDailyAsSeen();
			}
			int num = SharedData.Instance.CalculateDailyCompletePercent();
			if (num != -1)
			{
				AnalyticsManager.UpdateUserDailyProgressProperty(num);
			}
			if (!GeneralSettings.IsOldDesign)
			{
				this.scroll.GetComponent<ScrollConstraints>().Init();
			}
			base.StartCoroutine(base.DelayAction(3, 0f, new Action(this.ContentLoadComplete)));
		}
		else
		{
			this.ShowConnectionError();
		}
		this.pageContentTask = null;
		this.loadIndicator.SetActive(false);
	}

	private void UpdateNewDailyIndicator()
	{
		PictureData currentDailyData = this.scroll.GetCurrentDailyData();
		if (currentDailyData != null && currentDailyData.Id != GeneralSettings.LastDailyItemId)
		{
			if (currentDailyData.HasSave || currentDailyData.Solved)
			{
				GeneralSettings.LastDailyItemId = currentDailyData.Id;
			}
			else
			{
				this.newItemsIndicator.SetActive(true);
			}
		}
	}

	private void MarkDailyAsSeen()
	{
		if (this.newItemsIndicator.activeSelf)
		{
			this.newItemsIndicator.SetActive(false);
			PictureData currentDailyData = this.scroll.GetCurrentDailyData();
			if (currentDailyData != null)
			{
				GeneralSettings.LastDailyItemId = currentDailyData.Id;
			}
		}
	}

	private void OnSlowConnection()
	{
		this.ShowSlowConnection();
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
		this.noInternetLabel.ResetStatus();
		this.noInternetLabel.gameObject.SetActive(false);
	}

	private IEnumerator DelayAction(float delay, Action a)
	{
		yield return new WaitForSeconds(delay);
		a();
		yield break;
	}

	private DailyPageResponse FakeResp()
	{
		DailyPageResponse dailyPageResponse = new DailyPageResponse();
		dailyPageResponse.months = new List<DailyMonthResp>();
		dailyPageResponse.months.Add(new DailyMonthResp
		{
			monthIndex = 6,
			year = 2018
		});
		dailyPageResponse.months.Add(new DailyMonthResp
		{
			monthIndex = 2,
			year = 2017
		});
		dailyPageResponse.months.Add(new DailyMonthResp
		{
			monthIndex = 4,
			year = 2018
		});
		dailyPageResponse.months.Add(new DailyMonthResp
		{
			monthIndex = 4,
			year = 2018
		});
		dailyPageResponse.months.Add(new DailyMonthResp
		{
			monthIndex = 4,
			year = 2017
		});
		dailyPageResponse.months.Add(new DailyMonthResp
		{
			monthIndex = 12,
			year = 2018
		});
		dailyPageResponse.months.Add(new DailyMonthResp
		{
			monthIndex = 8,
			year = 2018
		});
		dailyPageResponse.months.Add(new DailyMonthResp
		{
			monthIndex = 7,
			year = 2018
		});
		dailyPageResponse.Sort();
		return dailyPageResponse;
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
		});
	}

	protected override void OnBeginClosing()
	{
	}

	protected override void OnBeginOpenning()
	{
		this.scroll.ReloadTextures();
		this.MarkDailyAsSeen();
	}

	protected override void OnClosed()
	{
		this.scroll.UnloadTextures();
	}

	protected override void OnOpened()
	{
	}

	[SerializeField]
	private GameObject settingBtn;

	[SerializeField]
	private ConnectionStatusPanel noInternetLabel;

	[SerializeField]
	private GameObject newItemsIndicator;

	[SerializeField]
	private GameObject loadIndicator;

	public DailyScroll scroll;

	private bool waitingForPageResponse;

	private bool scrolledToBottom;

	private DailyPageContentTask pageContentTask;

	private bool showingError;

	private ConnectionResumeHelper connectionResumeHelper;
}

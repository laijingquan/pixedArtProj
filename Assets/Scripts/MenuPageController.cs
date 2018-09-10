// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MenuPageController : MonoBehaviour
{
	public NewsPage News
	{
		get
		{
			return this.news;
		}
	}

	public OptionsPage Options
	{
		get
		{
			return this.options;
		}
	}

	public FeedPage Feed
	{
		get
		{
			return this.feed;
		}
	}

	public DailyPage Daily
	{
		get
		{
			return this.daily;
		}
	}

	public SelectPage Select
	{
		get
		{
			return this.select;
		}
	}

	public bool LowMemoryMode { get; private set; }

	public void Init()
	{
		this.select.Init(this.slideCurve, this.time);
		this.feed.Init(this.slideCurve, this.time);
		this.daily.Init(this.slideCurve, this.time);
		if (!GeneralSettings.IsOldDesign)
		{
			this.options.Init(this.slideCurve, this.time);
		}
		if (this.news != null)
		{
			this.news.Init(this.slideCurve, this.time);
		}
		if (MenuScreen.MenuState == MenuState.Feed)
		{
			this.active = this.feed;
			MenuScreen.MenuState = MenuState.Feed;
			this.active.SetOpened();
			this.SetActiveButton(2, true);
		}
		else if (MenuScreen.MenuState == MenuState.Daily)
		{
			this.active = this.daily;
			MenuScreen.MenuState = MenuState.Daily;
			this.active.SetOpened();
			this.SetActiveButton(4, true);
		}
		else
		{
			this.active = this.select;
			MenuScreen.MenuState = MenuState.Select;
			this.active.SetOpened();
			this.SetActiveButton(0, true);
		}
	}

	public void LoadPageContent()
	{
		if (AppState.IsServerContentExpired() || AppState.PushNotificationLaunch || AppState.IsGiftCodeLaunch)
		{
			FMLogger.Log("content expited. upd");
			AppState.ContentReqTime = DateTime.Now;
			SharedData.Instance.ClearCache();
			Gameboard.pictureData = null;
			if (AppState.PushNotificationLaunch || AppState.IsGiftCodeLaunch)
			{
				MenuScreen.MenuState = MenuState.Select;
				this.Select.ResetLastCategory();
			}
			if (AppState.PushNotificationLaunch)
			{
				AppState.PushNotificationLaunch = false;
			}
		}
		switch (MenuScreen.MenuState)
		{
		case MenuState.Select:
			this.select.LoadContent();
			this.daily.LoadContent();
			this.feed.LoadContent();
			if (!GeneralSettings.IsOldDesign)
			{
				this.options.LoadContent();
			}
			break;
		case MenuState.Feed:
			this.feed.LoadContent();
			this.select.LoadContent();
			this.daily.LoadContent();
			if (!GeneralSettings.IsOldDesign)
			{
				this.options.LoadContent();
			}
			break;
		case MenuState.Daily:
			this.daily.LoadContent();
			this.select.LoadContent();
			this.feed.LoadContent();
			if (!GeneralSettings.IsOldDesign)
			{
				this.options.LoadContent();
			}
			break;
		default:
			this.select.LoadContent();
			this.daily.LoadContent();
			this.feed.LoadContent();
			if (!GeneralSettings.IsOldDesign)
			{
				this.options.LoadContent();
			}
			break;
		}
	}

	public void OpenSelect()
	{
		this.active.Close(AnimSlideDirection.Right);
		this.active = this.select;
		this.active.Open(AnimSlideDirection.Right);
		this.SetActiveButton(0, true);
	}

	public void OpenFeed()
	{
		AnimSlideDirection direction;
		if ((this.options != null && this.active == this.options) || (this.news != null && this.active == this.news))
		{
			direction = AnimSlideDirection.Right;
		}
		else
		{
			direction = AnimSlideDirection.Left;
		}
		this.active.Close(direction);
		this.active = this.feed;
		this.active.Open(direction);
		this.SetActiveButton(2, true);
	}

	public void OpenDaily()
	{
		AnimSlideDirection direction;
		if (this.active == this.select)
		{
			direction = AnimSlideDirection.Left;
		}
		else
		{
			direction = AnimSlideDirection.Right;
		}
		this.active.Close(direction);
		this.active = this.daily;
		this.active.Open(direction);
		this.SetActiveButton(4, true);
	}

	public void OpenNews()
	{
		AnimSlideDirection direction;
		if (this.options != null && this.active == this.options)
		{
			direction = AnimSlideDirection.Right;
		}
		else
		{
			direction = AnimSlideDirection.Left;
		}
		this.active.Close(direction);
		this.active = this.news;
		this.active.Open(direction);
		this.SetActiveButton(6, true);
	}

	public void OpenMenu()
	{
		this.active.Close(AnimSlideDirection.Left);
		this.active = this.options;
		this.active.Open(AnimSlideDirection.Left);
		this.SetActiveButton(8, true);
	}

	private void SetActiveButton(int index, bool animated = true)
	{
		if (GeneralSettings.IsOldDesign)
		{
			for (int i = 0; i < this.buttons.Length; i += 2)
			{
				if (i == index)
				{
					this.buttons[i].SetActive(true);
					this.buttons[i + 1].SetActive(false);
				}
				else
				{
					this.buttons[i].SetActive(false);
					this.buttons[i + 1].SetActive(true);
				}
			}
		}
		else
		{
			index /= 2;
			for (int j = 0; j < this.navBarTabButtons.Length; j++)
			{
				this.navBarTabButtons[j].SetState(j == index, true);
			}
		}
	}

	public void HandleLowMemory()
	{
		int num = ImageManager.Instance.UnloadUnusedTextures();
		FMLogger.vCore("LowMemory Warning " + num);
	}

	[SerializeField]
	private float time;

	[SerializeField]
	private AnimationCurve slideCurve;

	[SerializeField]
	private SelectPage select;

	[SerializeField]
	private FeedPage feed;

	[SerializeField]
	private DailyPage daily;

	[SerializeField]
	private OptionsPage options;

	[SerializeField]
	private NewsPage news;

	[SerializeField]
	private GameObject[] buttons;

	[SerializeField]
	private NavBarTabButton[] navBarTabButtons;

	public Page active;
}

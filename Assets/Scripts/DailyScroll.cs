// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class DailyScroll : MonoBehaviour, IDailyInfoProvider
{
	public void LoadContent(DailyTabInfo info, bool lazyIconLoad)
	{
		if (GeneralSettings.IsOldDesign)
		{
			this.dailyPic = this.dailyPicLegacy;
		}
		else
		{
			this.dailyPic = ((!SafeLayout.IsTablet) ? this.dailyPicPhone : this.dailyPicTablet);
		}
		this.tabData = info;
		if (GeneralSettings.IsOldDesign)
		{
			this.scroll.AddTopOffset(-1 * (int)((RectTransform)this.dailyPic.transform.parent).anchoredPosition.y);
		}
		if (this.tabData.dailyPic != null)
		{
			this.dailyPic.transform.parent.gameObject.SetActive(true);
			this.dailyPic.gameObject.SetActive(true);
			this.dailyPic.Init(this.tabData.dailyPic);
			int offset = (int)((RectTransform)this.dailyPic.transform.parent).rect.height;
			this.scroll.AddTopOffset(offset);
		}
		this.dailyContent = this.tabData.monthes;
		if (!GeneralSettings.IsOldDesign)
		{
			this.scroll.headerHeight = ((!SafeLayout.IsTablet) ? 153 : 114);
		}
		if (!GeneralSettings.IsOldDesign)
		{
			this.scroll.cellHeight = ((!SafeLayout.IsTablet) ? 542 : 500);
		}
		this.scroll.FillItem += delegate(int row, DailyRowItem item, bool lazyLoad)
		{
			item.OnDailyVisable(row, this, lazyLoad);
		};
		int num = 0;
		for (int i = 0; i < this.dailyContent.Count; i++)
		{
			int num2 = (this.dailyContent[i].pics.Count % MenuScreen.RowItems != 0) ? (this.dailyContent[i].pics.Count / MenuScreen.RowItems + 1) : (this.dailyContent[i].pics.Count / MenuScreen.RowItems);
			this.newMonthRows.Add(num);
			num++;
			num += num2;
		}
		if (this.dailyContent.Count > 0)
		{
			this.currentMonth = this.dailyContent[0];
			this.rowOffset = this.newMonthRows[0];
		}
		int scrollTo = 0;
		if (MenuScreen.MenuState == MenuState.Daily && MenuScreen.PaintStartSource == PaintStartSource.DailyOldPic && Gameboard.pictureData != null && Gameboard.pictureData.Id != 0)
		{
			scrollTo = this.GetItemRow(Gameboard.pictureData.Id, true);
		}
		this.scroll.InitData(num, scrollTo, this.newMonthRows, lazyIconLoad);
	}

	public DailyEventInfo GetDailyDate(int id)
	{
		DailyEventInfo result = default(DailyEventInfo);
		if (this.dailyPic != null && this.dailyPic.PicItem != null && this.dailyPic.PicItem.PictureData != null && this.dailyPic.PicItem.Id == id)
		{
			if (this.dailyContent != null && this.dailyContent.Count > 0)
			{
				result.day = this.dailyContent[0].pics.Count + 1;
				result.month = this.dailyContent[0].monthIndex;
				result.year = this.dailyContent[0].year;
				result.row = 0;
			}
			else
			{
				result.year = -1;
				result.month = -1;
				result.year = -1;
				result.row = -1;
			}
			return result;
		}
		if (this.dailyContent != null)
		{
			for (int i = 0; i < this.dailyContent.Count; i++)
			{
				for (int j = 0; j < this.dailyContent[i].pics.Count; j++)
				{
					if (this.dailyContent[i].pics[j].Id == id)
					{
						result.day = this.dailyContent[i].pics.Count - j;
						result.month = this.dailyContent[i].monthIndex;
						result.year = this.dailyContent[i].year;
						result.row = this.GetItemRow(id, false);
						return result;
					}
				}
			}
		}
		return result;
	}

	public PictureData GetCurrentDailyData()
	{
		if (this.dailyPic != null && this.dailyPic.PicItem != null && this.dailyPic.PicItem.PictureData != null)
		{
			return this.dailyPic.PicItem.PictureData;
		}
		return null;
	}

	public void ReloadFailedIcon()
	{
		DailyRowItem[] views = this.scroll.Views;
		if (views != null && views.Length > 0)
		{
			for (int i = 0; i < views.Length; i++)
			{
				views[i].ReloadFailedIcons();
			}
		}
		if (this.dailyPic != null && this.dailyPic.PicItem.IconLoadFailed())
		{
			this.dailyPic.PicItem.RetryLoadIcon();
		}
	}

	public bool IsHeader(int row)
	{
		bool result = false;
		bool flag = false;
		for (int i = 0; i < this.newMonthRows.Count; i++)
		{
			if (row < this.newMonthRows[i])
			{
				this.rowOffset = this.newMonthRows[i - 1];
				this.rowOffset++;
				this.currentMonth = this.dailyContent[i - 1];
				flag = true;
				break;
			}
			if (row == this.newMonthRows[i])
			{
				this.rowOffset = this.newMonthRows[i];
				this.rowOffset++;
				result = true;
				this.currentMonth = this.dailyContent[i];
				flag = true;
				break;
			}
		}
		if (!flag && row > this.newMonthRows[this.newMonthRows.Count - 1])
		{
			this.currentMonth = this.dailyContent[this.dailyContent.Count - 1];
			this.rowOffset = this.newMonthRows[this.newMonthRows.Count - 1];
			this.rowOffset++;
		}
		return result;
	}

	public string GetMonthName()
	{
		return this.currentMonth.monthName;
	}

	private int GetItemRow(int picId, bool includeTitle = true)
	{
		int num = 0;
		for (int i = 0; i < this.dailyContent.Count; i++)
		{
			num++;
			for (int j = 0; j < this.dailyContent[i].pics.Count; j++)
			{
				if (this.dailyContent[i].pics[j].Id == picId)
				{
					int num2 = j / MenuScreen.RowItems;
					num2 += num;
					if (!includeTitle)
					{
						num2 -= i + 1;
					}
					return num2;
				}
			}
			num += ((this.dailyContent[i].pics.Count % MenuScreen.RowItems != 0) ? (this.dailyContent[i].pics.Count / MenuScreen.RowItems + 1) : (this.dailyContent[i].pics.Count / MenuScreen.RowItems));
		}
		return 0;
	}

	public PictureData[] GetRowData(int row)
	{
		List<PictureData> pics = this.currentMonth.pics;
		row -= this.rowOffset;
		if (MenuScreen.RowItems == 2)
		{
			if (pics.Count - 1 >= row * 2 + 1)
			{
				return new PictureData[]
				{
					pics[row * 2],
					pics[row * 2 + 1]
				};
			}
			if (pics.Count - 1 == row * 2)
			{
				return new PictureData[]
				{
					pics[row * 2]
				};
			}
		}
		else if (MenuScreen.RowItems == 3)
		{
			if (pics.Count - 1 >= row * 3 + 2)
			{
				return new PictureData[]
				{
					pics[row * 3],
					pics[row * 3 + 1],
					pics[row * 3 + 2]
				};
			}
			if (pics.Count - 1 >= row * 3 + 1)
			{
				return new PictureData[]
				{
					pics[row * 3],
					pics[row * 3 + 1]
				};
			}
			if (pics.Count - 1 == row * 3)
			{
				return new PictureData[]
				{
					pics[row * 3]
				};
			}
		}
		FMLogger.Log("WTF EMPTY ROW: " + row);
		return new PictureData[0];
	}

	public PictureSaveData GetSave(PictureData picData)
	{
		if (!picData.HasSave)
		{
			return null;
		}
		return SharedData.Instance.GetSave(picData);
	}

	private void OnEnable()
	{
		SharedData.Instance.PictureDataDeleted += this.OnPicDeleted;
	}

	private void OnDisable()
	{
		SharedData.Instance.PictureDataDeleted -= this.OnPicDeleted;
	}

	private void OnPicDeleted(PictureData delPicData, PictureData replacePicData)
	{
		if (replacePicData == null)
		{
			return;
		}
		this.tabData.UpdatePicData(replacePicData);
		if (this.tabData.dailyPic != null && this.tabData.dailyPic.picData.Id == replacePicData.Id)
		{
			this.dailyPic.PicItem.Reset();
			this.dailyPic.PicItem.Init(replacePicData, false, false, true);
		}
		if (this.scroll != null && this.scroll.Count > 0)
		{
			DailyRowItem rowWithId = this.scroll.GetRowWithId(replacePicData.Id);
			if (rowWithId != null)
			{
				rowWithId.ReinitPicItem(replacePicData);
			}
		}
	}

	public void ReloadTextures()
	{
		for (int i = 0; i < this.scroll.Views.Length; i++)
		{
			this.scroll.Views[i].ReloadTextures();
		}
	}

	public void UnloadTextures()
	{
		for (int i = 0; i < this.scroll.Views.Length; i++)
		{
			this.scroll.Views[i].UnloadTextures();
		}
	}

	public DailyInfiniteScroll scroll;

	public DailyPicItem dailyPicPhone;

	public DailyPicItem dailyPicTablet;

	public DailyPicItem dailyPicLegacy;

	private DailyPicItem dailyPic;

	private DailyMonthInfo currentMonth;

	private int rowOffset;

	private List<int> newMonthRows = new List<int>();

	private DailyTabInfo tabData;

	private List<DailyMonthInfo> dailyContent;
}

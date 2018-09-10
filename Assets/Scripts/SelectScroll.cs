// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectScroll : MonoBehaviour, IPictureInfoProvider
{
	public int DisplayPicsCount
	{
		get
		{
			return (this.tabData == null) ? 0 : this.tabData.Count;
		}
	}

	public int FillterByCategoryId(int catId, bool filterSolved, bool scollToLast = false, bool lazyIconLoad = false)
	{
		if (this.tabData == null)
		{
			return 0;
		}
		this.tabData.Clear();
		if (catId == ContentFilterNavBar.ShowAllCategoryId)
		{
			if (filterSolved)
			{
				for (int i = 0; i < this.data.Count; i++)
				{
					if (!this.data[i].IsCompleted())
					{
						this.tabData.Add(this.data[i]);
					}
				}
			}
			else
			{
				this.tabData = new List<PictureData>(this.data);
			}
		}
		else
		{
			for (int j = 0; j < this.data.Count; j++)
			{
				if (this.data[j].IsInCategory(catId))
				{
					if (filterSolved)
					{
						if (!this.data[j].IsCompleted())
						{
							this.tabData.Add(this.data[j]);
						}
					}
					else
					{
						this.tabData.Add(this.data[j]);
					}
				}
			}
		}
		int count = (this.tabData.Count % MenuScreen.RowItems != 0) ? (this.tabData.Count / MenuScreen.RowItems + 1) : (this.tabData.Count / MenuScreen.RowItems);
		int scrollTo = 0;
		if (scollToLast && MenuScreen.MenuState == MenuState.Select && MenuScreen.PaintStartSource == PaintStartSource.LibPic && Gameboard.pictureData != null && Gameboard.pictureData.Id != 0 && (!filterSolved || !Gameboard.pictureData.IsCompleted()))
		{
			scrollTo = this.GetItemPositionIndex(Gameboard.pictureData);
		}
		this.scroll.InitData(count, scrollTo, lazyIconLoad);
		this.emptyLabel.gameObject.SetActive(this.tabData.Count == 0 && catId != ContentFilterNavBar.FBCategoryId);
		return this.tabData.Count;
	}

	public void LoadContent(List<PictureData> items)
	{
		if (!GeneralSettings.IsOldDesign)
		{
			this.scroll.height = ((!SafeLayout.IsTablet) ? 542 : 500);
		}
		this.scroll.FillItem += delegate(int row, ScrollRowItem item, bool lazyLoad)
		{
			item.OnBecomeVisable(row, this, lazyLoad, true);
		};
		this.data = new List<PictureData>(items);
		this.tabData = new List<PictureData>();
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

	public int GetItemPositionIndex(PicItem picItem)
	{
		return this.GetItemPositionIndex(picItem.PictureData);
	}

	public int GetItemPositionIndex(PictureData picData)
	{
		int num = this.tabData.IndexOf(picData);
		if (num == -1)
		{
			num = 0;
		}
		else
		{
			num /= MenuScreen.RowItems;
		}
		return num;
	}

	public void RemoveSave(int packId, int id)
	{
		ScrollRowItem rowWithId = this.scroll.GetRowWithId(id);
		if (rowWithId != null)
		{
			PicItem pictureItem = rowWithId.GetPictureItem(id);
			if (pictureItem != null)
			{
				pictureItem.RemoveSave();
			}
		}
	}

	public PictureData[] GetRowData(int row)
	{
		if (MenuScreen.RowItems == 2)
		{
			if (this.tabData.Count - 1 >= row * 2 + 1)
			{
				return new PictureData[]
				{
					this.tabData[row * 2],
					this.tabData[row * 2 + 1]
				};
			}
			if (this.tabData.Count - 1 == row * 2)
			{
				return new PictureData[]
				{
					this.tabData[row * 2]
				};
			}
		}
		else if (MenuScreen.RowItems == 3)
		{
			if (this.tabData.Count - 1 >= row * 3 + 2)
			{
				return new PictureData[]
				{
					this.tabData[row * 3],
					this.tabData[row * 3 + 1],
					this.tabData[row * 3 + 2]
				};
			}
			if (this.tabData.Count - 1 >= row * 3 + 1)
			{
				return new PictureData[]
				{
					this.tabData[row * 3],
					this.tabData[row * 3 + 1]
				};
			}
			if (this.tabData.Count - 1 == row * 3)
			{
				return new PictureData[]
				{
					this.tabData[row * 3]
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

	public void ReloadFailedIcon()
	{
		ScrollRowItem[] views = this.scroll.Views;
		if (views != null && views.Length > 0)
		{
			for (int i = 0; i < views.Length; i++)
			{
				views[i].ReloadFailedIcons();
			}
		}
	}

	private void OnPicDeleted(PictureData delPicData, PictureData replacePicData)
	{
		if (replacePicData == null)
		{
			return;
		}
		for (int i = 0; i < this.data.Count; i++)
		{
			if (this.data[i].Id == replacePicData.Id)
			{
				this.data[i] = replacePicData;
			}
		}
		if (this.scroll != null && this.scroll.Count > 0)
		{
			ScrollRowItem rowWithId = this.scroll.GetRowWithId(replacePicData.Id);
			if (rowWithId != null)
			{
				rowWithId.ReinitPicItem(replacePicData);
			}
		}
	}

	private void OnNewPicsAdded(List<PictureData> newData)
	{
		this.data.InsertRange(0, newData);
	}

	private void OnEnable()
	{
		SharedData.Instance.PictureDataDeleted += this.OnPicDeleted;
		SharedData.Instance.NewPicturesAdded += this.OnNewPicsAdded;
	}

	private void OnDisable()
	{
		SharedData.Instance.PictureDataDeleted -= this.OnPicDeleted;
		SharedData.Instance.NewPicturesAdded -= this.OnNewPicsAdded;
	}

	public void AddTopOffset(int offset)
	{
		this.emptyLabel.anchoredPosition += new Vector2(0f, (float)(-(float)offset));
		this.scroll.AddTopOffset(offset);
	}

	public void SetScrollTitleOffset(int offset)
	{
		this.scroll.SetScrollTitleOffset(offset);
	}

	[SerializeField]
	private RectTransform emptyLabel;

	[SerializeField]
	private InfiniteScroll scroll;

	private List<PictureData> data;

	private List<PictureData> tabData;
}

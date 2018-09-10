// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRowItem : MonoBehaviour
{
	public virtual void OnDailyVisable(int row, IDailyInfoProvider dataProvider, bool lazyIconLoad)
	{
		this.Clean();
		this.title.gameObject.SetActive(false);
		this.Row = row;
		for (int i = 0; i < this.pics.Count; i++)
		{
			this.pics[i].Reset();
		}
		if (dataProvider.IsHeader(row))
		{
			this.isHeader = true;
			this.title.gameObject.SetActive(true);
			this.title.text = dataProvider.GetMonthName();
			for (int j = 0; j < this.pics.Count; j++)
			{
				this.pics[j].gameObject.SetActive(false);
			}
		}
		else
		{
			PictureData[] rowData = dataProvider.GetRowData(row);
			for (int k = 0; k < rowData.Length; k++)
			{
				if (!this.pics[k].gameObject.activeSelf)
				{
					this.pics[k].gameObject.SetActive(true);
				}
				this.pics[k].Init(rowData[k], lazyIconLoad, false, true);
				if (rowData[k].HasSave)
				{
					this.pics[k].AddSave(dataProvider.GetSave(rowData[k]));
				}
			}
			for (int l = rowData.Length; l < this.pics.Count; l++)
			{
				this.pics[l].gameObject.SetActive(false);
			}
		}
	}

	public bool ContainsItem(int picId)
	{
		if (this.isHeader)
		{
			return false;
		}
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf && this.pics[i].PictureData != null && this.pics[i].PictureData.Id == picId)
			{
				return true;
			}
		}
		return false;
	}

	public PicItem GetPictureItem(int picId)
	{
		if (this.isHeader)
		{
			return null;
		}
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].PictureData.Id == picId)
			{
				return this.pics[i];
			}
		}
		return null;
	}

	public void ReloadFailedIcons()
	{
		if (this.isHeader)
		{
			return;
		}
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf && this.pics[i].IconLoadFailed())
			{
				this.pics[i].RetryLoadIcon();
			}
		}
	}

	public void ReinitPicItem(PictureData picData)
	{
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf && this.pics[i].Id == picData.Id)
			{
				FMLogger.Log("reset pic " + picData.Id);
				int dailyTabDate = this.pics[i].PictureData.Extras.dailyTabDate;
				picData.SetDailyTabDate(dailyTabDate);
				this.pics[i].Reset();
				this.pics[i].Init(picData, false, false, true);
				break;
			}
		}
	}

	private void Clean()
	{
		if (!this.isHeader)
		{
			for (int i = 0; i < this.pics.Count; i++)
			{
				this.pics[i].Reset();
			}
		}
		else
		{
			this.title.gameObject.SetActive(false);
			this.isHeader = false;
		}
	}

	public void ReloadTextures()
	{
		if (this.isHeader)
		{
			return;
		}
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf)
			{
				this.pics[i].ReloadTextures();
			}
		}
	}

	public void UnloadTextures()
	{
		if (this.isHeader)
		{
			return;
		}
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf)
			{
				this.pics[i].UnloadTextures();
			}
		}
	}

	public int Row;

	public List<PicItem> pics;

	public Text title;

	private bool isHeader;
}

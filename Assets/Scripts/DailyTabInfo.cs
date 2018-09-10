// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class DailyTabInfo
{
	public void UpdatePicData(PictureData newPd)
	{
		for (int i = 0; i < this.monthes.Count; i++)
		{
			if (this.monthes[i].pics != null)
			{
				for (int j = 0; j < this.monthes[i].pics.Count; j++)
				{
					if (this.monthes[i].pics[j].Id == newPd.Id)
					{
						this.monthes[i].pics[j] = newPd;
					}
				}
			}
		}
		if (this.dailyPic != null && this.dailyPic.picData != null && this.dailyPic.picData.Id == newPd.Id)
		{
			this.dailyPic.picData = newPd;
		}
	}

	public void UpdateSaveState(int picDataId, bool hasSave)
	{
		for (int i = 0; i < this.monthes.Count; i++)
		{
			if (this.monthes[i].pics != null)
			{
				for (int j = 0; j < this.monthes[i].pics.Count; j++)
				{
					if (this.monthes[i].pics[j].Id == picDataId)
					{
						this.monthes[i].pics[j].SetSaveState(hasSave);
					}
				}
			}
		}
		if (this.dailyPic != null && this.dailyPic.picData != null && this.dailyPic.picData.Id == picDataId)
		{
			this.dailyPic.picData.SetSaveState(hasSave);
		}
	}

	public List<DailyMonthInfo> monthes;

	public DailyPicInfo dailyPic;
}

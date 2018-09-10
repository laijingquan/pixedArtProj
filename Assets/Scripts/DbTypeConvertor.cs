// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public static class DbTypeConvertor
{
	public static PictureData ToPictureData(PictureDb p)
	{
		FillAlgorithm fillType = DbTypeConvertor.FillAlgFromDb(p.FillType);
		return new PictureData(PictureType.Local, p.PackId, p.PicId, fillType)
		{
			HasSave = (p.HasSave == 1),
			Solved = (p.Solved == 1),
			PicClass = DbTypeConvertor.PicClassFromDb(p.PicClass)
		};
	}

	public static PictureDb FromPictureData(PictureData pd)
	{
		return new PictureDb
		{
			PicId = pd.Id,
			PackId = pd.PackId,
			FillType = DbTypeConvertor.FillAlgToDb(pd.FillType),
			HasSave = ((!pd.HasSave) ? 0 : 1),
			Solved = ((!pd.Solved) ? 0 : 1),
			PicClass = DbTypeConvertor.PicClassToDb(pd.PicClass)
		};
	}

	public static PictureSaveData ToPictureSaveData(SaveDb s)
	{
		PictureSaveData pictureSaveData = new PictureSaveData();
		pictureSaveData.Id = s.PicId;
		pictureSaveData.PackId = s.PackId;
		pictureSaveData.iconMaskPath = s.IconPath;
		pictureSaveData.progres = s.Progress;
		pictureSaveData.timeSpent = s.TimeSpent;
		pictureSaveData.hintsUsed = s.HintsUsed;
		StepsDb stepsDb = JsonUtility.FromJson<StepsDb>(s.StepsJsonStr);
		if (stepsDb != null && stepsDb.steps != null)
		{
			pictureSaveData.steps = stepsDb.steps;
		}
		else
		{
			pictureSaveData.steps = null;
		}
		return pictureSaveData;
	}

	public static SaveDb FromPictureSaveData(PictureSaveData save)
	{
		SaveDb saveDb = new SaveDb();
		saveDb.PicId = save.Id;
		saveDb.PackId = save.PackId;
		saveDb.IconPath = save.iconMaskPath;
		saveDb.Progress = save.progres;
		saveDb.TimeSpent = save.timeSpent;
		saveDb.TimeStamp = DateTime.UtcNow.Ticks;
		saveDb.HintsUsed = save.hintsUsed;
		StepsDb obj = new StepsDb
		{
			steps = save.steps
		};
		saveDb.StepsJsonStr = JsonUtility.ToJson(obj);
		return saveDb;
	}

	public static BonusCodeData ToGiftData(GiftDb giftDb)
	{
		return new BonusCodeData
		{
			BonusCode = giftDb.GiftId,
			ClaimTime = DateTime.FromFileTimeUtc(giftDb.TimeStamp)
		};
	}

	public static GiftDb FromGiftData(BonusCodeData bonusCodeData)
	{
		return new GiftDb
		{
			GiftId = bonusCodeData.BonusCode,
			TimeStamp = bonusCodeData.ClaimTime.ToFileTimeUtc()
		};
	}

	private static FillAlgorithm FillAlgFromDb(int fill)
	{
		if (fill == 0)
		{
			return FillAlgorithm.Flood;
		}
		if (fill == 1)
		{
			return FillAlgorithm.Chop;
		}
		return FillAlgorithm.Chop;
	}

	private static int FillAlgToDb(FillAlgorithm fill)
	{
		if (fill == FillAlgorithm.Flood)
		{
			return 0;
		}
		if (fill == FillAlgorithm.Chop)
		{
			return 1;
		}
		return 1;
	}

	private static int PicClassToDb(PicClass picClass)
	{
		switch (picClass)
		{
		case PicClass.Common:
			return 0;
		case PicClass.Daily:
			return 1;
		case PicClass.FacebookGift:
			return 2;
		default:
			return 0;
		}
	}

	private static PicClass PicClassFromDb(int value)
	{
		if (value == 0)
		{
			return PicClass.Common;
		}
		if (value == 1)
		{
			return PicClass.Daily;
		}
		if (value == 2)
		{
			return PicClass.FacebookGift;
		}
		return PicClass.Common;
	}

	public static NewsDb FromNewsData(OrderedItemInfo item)
	{
		NewsDb newsDb = new NewsDb();
		newsDb.Id = item.id;
		int newsType = -1;
		OrderedItemType itemType = item.itemType;
		if (itemType != OrderedItemType.PromoPic)
		{
			if (itemType == OrderedItemType.ExternalLink)
			{
				newsDb.NewsType = 1;
			}
		}
		else
		{
			newsDb.NewsType = 0;
		}
		newsDb.NewsType = newsType;
		return newsDb;
	}
}

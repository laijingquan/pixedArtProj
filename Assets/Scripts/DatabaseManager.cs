// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using SimpleSQL;
using UnityEngine;

public static class DatabaseManager
{
	public static int Version
	{
		get
		{
			return PlayerPrefs.GetInt("dbver", 0);
		}
		private set
		{
			PlayerPrefs.SetInt("dbver", value);
			PlayerPrefs.Save();
		}
	}

	public static void Init(SimpleSQLManager database, PicturePack pack = null, List<PictureSaveData> saves = null)
	{
		DatabaseManager.db = database;
		bool flag = false;
		if (DatabaseManager.Version == 0)
		{
			int num = DatabaseManager.CreatePicDb();
			int num2 = DatabaseManager.CreateSaveDb();
			if (num != -1 && num2 != -1)
			{
				DatabaseManager.Version = 1;
				UnityEngine.Debug.Log("initial db creation successful");
			}
			else
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"db error picDb:",
					num,
					" saveDb:",
					num2
				}));
			}
			flag = true;
		}
		if (DatabaseManager.Version == 1)
		{
			DatabaseManager.DbUpdateOne();
			DatabaseManager.Version = 2;
		}
		if (DatabaseManager.Version == 2)
		{
			DatabaseManager.CreateGiftDb(false);
			DatabaseManager.Version = 3;
		}
		if (DatabaseManager.Version == 3)
		{
			DatabaseManager.CreateNewsDb(false);
			DatabaseManager.Version = 4;
		}
		DatabaseValidator.Validate(DatabaseManager.db);
		if (flag && pack != null && pack.Pictures != null)
		{
			DatabaseManager.InitialDbFill(pack, saves);
		}
	}

	private static void DbUpdateOne()
	{
		string text = string.Empty;
		try
		{
			string query = "ALTER TABLE \"SaveDb\" ADD COLUMN \"HintsUsed\" INTEGER DEFAULT 0";
			DatabaseManager.db.Execute(query, new object[0]);
			UnityEngine.Debug.Log("Added new row HintsUsed to SaveDb. Db update to success");
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("failed to add new column to saveDb. " + ex.Message);
			text = text + "upd1. saveDb Alter:" + ex.Message + "|";
		}
		try
		{
			string query2 = "ALTER TABLE \"PictureDb\" ADD COLUMN \"PicClass\" INTEGER DEFAULT 0";
			DatabaseManager.db.Execute(query2, new object[0]);
			UnityEngine.Debug.Log("Added new row PicClass to PictureDb. Db update to success");
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.Log("failed to add new column to PictureDb. " + ex2.Message);
			text = text + "picDb Alter:" + ex2.Message;
		}
		if (!string.IsNullOrEmpty(text))
		{
			AnalyticsManager.DbCreationError("updateOne", text);
		}
	}

	private static void InitialDbFill(PicturePack pack, List<PictureSaveData> saves)
	{
		for (int i = 0; i < pack.Pictures.Count; i++)
		{
			DatabaseManager.AddPicture(pack.Pictures[i]);
		}
		if (saves != null)
		{
			for (int j = 0; j < saves.Count; j++)
			{
				try
				{
					SaveDb saveDb = DbTypeConvertor.FromPictureSaveData(saves[j]);
					saveDb.TimeStamp -= (long)(j * 10000000);
					DatabaseManager.db.Insert(saveDb);
				}
				catch (Exception ex)
				{
					FMLogger.Log("error while inital save db fill. e:" + ex.Message);
				}
			}
		}
	}

	private static int CreatePicDb()
	{
		int result;
		try
		{
			string query = "CREATE TABLE IF NOT EXISTS \"PictureDb\" (\"PicId\" integer PRIMARY KEY, \"FillType\" integer DEFAULT 0 , \"PackId\" integer DEFAULT 0 , \"Solved\" integer DEFAULT 0 , \"HasSave\" integer DEFAULT 0 ) ";
			DatabaseManager.db.Execute(query, new object[0]);
			result = 1;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Failed to create pic db");
			AnalyticsManager.DbCreationError("pic", ex.Message);
			result = -1;
		}
		return result;
	}

	private static int CreateSaveDb()
	{
		int result;
		try
		{
			string query = "CREATE TABLE IF NOT EXISTS \"SaveDb\" (\"PicId\" integer PRIMARY KEY, \"PackId\" integer DEFAULT 0 , \"IconPath\" varchar(140) , \"TimeSpent\" float DEFAULT 0 , \"TimeStamp\" integer DEFAULT 0 , \"Progress\" integer DEFAULT 0 , \"StepsJsonStr\" varchar(140) )";
			DatabaseManager.db.Execute(query, new object[0]);
			result = 1;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Failed to create save db");
			AnalyticsManager.DbCreationError("save", ex.Message);
			result = -1;
		}
		return result;
	}

	public static int CreateGiftDb(bool isFallback = false)
	{
		int result;
		try
		{
			string query = "CREATE TABLE IF NOT EXISTS \"GiftDb\" (\"GiftId\" varchar(140) PRIMARY KEY, \"TimeStamp\" integer DEFAULT 0)";
			DatabaseManager.db.Execute(query, new object[0]);
			result = 1;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Failed to create gift db");
			if (isFallback)
			{
				AnalyticsManager.DbCreationError("gift", "fallback " + ex.Message);
			}
			else
			{
				AnalyticsManager.DbCreationError("gift", ex.Message);
			}
			result = -1;
		}
		return result;
	}

	public static int CreateNewsDb(bool isFallback = false)
	{
		int result;
		try
		{
			string query = "CREATE TABLE IF NOT EXISTS \"NewsDb\" (\"Id\" integer PRIMARY KEY, \"NewsType\" integer DEFAULT 0)";
			DatabaseManager.db.Execute(query, new object[0]);
			result = 1;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Failed to create news db");
			if (isFallback)
			{
				AnalyticsManager.DbCreationError("news", "fallback " + ex.Message);
			}
			else
			{
				AnalyticsManager.DbCreationError("news", ex.Message);
			}
			result = -1;
		}
		return result;
	}

	public static int FallbackCreatePicDb()
	{
		int result;
		try
		{
			string query = "CREATE TABLE IF NOT EXISTS \"PictureDb\" (\"PicId\" integer PRIMARY KEY, \"FillType\" integer DEFAULT 0 , \"PackId\" integer DEFAULT 0 , \"Solved\" integer DEFAULT 0 , \"HasSave\" integer DEFAULT 0 , \"PicClass\" integer DEFAULT 0 ) ";
			DatabaseManager.db.Execute(query, new object[0]);
			result = 1;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Failed to create pic db");
			AnalyticsManager.DbCreationError("pic", "fallback " + ex.Message);
			result = -1;
		}
		return result;
	}

	public static int FallbackCreateSaveDb()
	{
		int result;
		try
		{
			string query = "CREATE TABLE IF NOT EXISTS \"SaveDb\" (\"PicId\" integer PRIMARY KEY, \"PackId\" integer DEFAULT 0 , \"IconPath\" varchar(140) , \"TimeSpent\" float DEFAULT 0 , \"TimeStamp\" integer DEFAULT 0 , \"Progress\" integer DEFAULT 0 , \"StepsJsonStr\" varchar(140) ,\"HintsUsed\" integer DEFAULT 0 )";
			DatabaseManager.db.Execute(query, new object[0]);
			result = 1;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Failed to create save db");
			AnalyticsManager.DbCreationError("save", "fallback " + ex.Message);
			result = -1;
		}
		return result;
	}

	public static List<PictureData> GetLocalPictures()
	{
		List<PictureData> list = new List<PictureData>();
		try
		{
			string query = "SELECT * FROM PictureDb";
			List<PictureDb> list2 = DatabaseManager.db.Query<PictureDb>(query, new object[0]);
			if (list2 != null)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					list.Add(DbTypeConvertor.ToPictureData(list2[i]));
				}
			}
		}
		catch (Exception ex)
		{
			FMLogger.Log("queue all local pics error. " + ex.Message);
			AnalyticsManager.DbTransactionError("picDb", "GetLocalPictures: " + ex.Message);
		}
		return list;
	}

	public static PictureData GetPicture(int id)
	{
		PictureData result;
		try
		{
			PictureDb p = DatabaseManager.db.Get<PictureDb>(id);
			PictureData pictureData = DbTypeConvertor.ToPictureData(p);
			result = pictureData;
		}
		catch
		{
			result = null;
		}
		return result;
	}

	public static void AddPicture(PictureData pd)
	{
		try
		{
			PictureDb obj = DbTypeConvertor.FromPictureData(pd);
			DatabaseManager.db.Insert(obj);
		}
		catch (Exception ex)
		{
			FMLogger.Log("error adding picture. " + ex.Message);
			AnalyticsManager.DbTransactionError("picDb", "AddPicture: " + ex.Message);
		}
	}

	public static void UpdatePicture(PictureData pd)
	{
		try
		{
			PictureDb obj = DbTypeConvertor.FromPictureData(pd);
			DatabaseManager.db.UpdateTable(obj);
		}
		catch (Exception ex)
		{
			FMLogger.Log("upd pic error. " + ex.Message);
			AnalyticsManager.DbTransactionError("picDb", "UpdatePicture: " + ex.Message);
		}
	}

	public static void DeletePicture(PictureData pd)
	{
		if (pd.HasSave)
		{
			DatabaseManager.DeleteSave(pd.Id, false);
		}
		try
		{
			PictureDb obj = DbTypeConvertor.FromPictureData(pd);
			DatabaseManager.db.Delete<PictureDb>(obj);
		}
		catch (Exception ex)
		{
			FMLogger.Log("error del pic. " + ex.Message);
			AnalyticsManager.DbTransactionError("picDb", "DeletePicture: " + ex.Message);
		}
	}

	public static PictureSaveData GetSave(PictureData pd)
	{
		PictureSaveData result;
		try
		{
			SaveDb s = DatabaseManager.db.Get<SaveDb>(pd.Id);
			PictureSaveData pictureSaveData = DbTypeConvertor.ToPictureSaveData(s);
			result = pictureSaveData;
		}
		catch
		{
			result = null;
		}
		return result;
	}

	public static void AddSave(PictureSaveData pictureSaveData)
	{
		try
		{
			SaveDb obj = DbTypeConvertor.FromPictureSaveData(pictureSaveData);
			DatabaseManager.db.Insert(obj);
			string query = "SELECT * FROM PictureDb WHERE PicId=?";
			bool flag;
			PictureDb pictureDb = DatabaseManager.db.QueryFirstRecord<PictureDb>(out flag, query, new object[]
			{
				pictureSaveData.Id
			});
			if (flag && pictureDb.HasSave == 0)
			{
				pictureDb.HasSave = 1;
				DatabaseManager.db.UpdateTable(pictureDb);
			}
		}
		catch (Exception ex)
		{
			FMLogger.Log("failed to add save. " + ex.Message);
			AnalyticsManager.DbTransactionError("saveDb", "AddSave: " + ex.Message);
		}
	}

	public static void UpdateSave(PictureSaveData pictureSaveData)
	{
		try
		{
			SaveDb obj = DbTypeConvertor.FromPictureSaveData(pictureSaveData);
			DatabaseManager.db.UpdateTable(obj);
		}
		catch (Exception ex)
		{
			FMLogger.Log("upd saveData fail. " + ex.Message);
		}
	}

	public static void DeleteSave(int id, bool updPicData = true)
	{
		try
		{
			string query = "DELETE FROM SaveDb WHERE PicId = ?";
			DatabaseManager.db.Execute(query, new object[]
			{
				id
			});
			GeneralSettings.SavesCount--;
		}
		catch (Exception ex)
		{
			FMLogger.Log("error save del. " + ex.Message);
			AnalyticsManager.DbTransactionError("saveDb", "DeleteSave: " + ex.Message);
		}
		if (updPicData)
		{
			try
			{
				string query2 = "SELECT * FROM PictureDb WHERE PicId=?";
				bool flag;
				PictureDb pictureDb = DatabaseManager.db.QueryFirstRecord<PictureDb>(out flag, query2, new object[]
				{
					id
				});
				if (flag)
				{
					pictureDb.HasSave = 0;
					DatabaseManager.db.UpdateTable(pictureDb);
				}
			}
			catch (Exception ex2)
			{
				FMLogger.Log("error upd pd when save deleted. " + ex2.Message);
			}
		}
	}

	public static List<PictureSaveData> GetSaves()
	{
		List<PictureSaveData> list = new List<PictureSaveData>();
		try
		{
			string query = "SELECT * FROM SaveDb ORDER BY TimeStamp DESC";
			List<SaveDb> list2 = DatabaseManager.db.Query<SaveDb>(query, new object[0]);
			if (list2 != null)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					list.Add(DbTypeConvertor.ToPictureSaveData(list2[i]));
				}
			}
		}
		catch (Exception ex)
		{
			FMLogger.Log("failed to get saves. " + ex.Message);
			AnalyticsManager.DbTransactionError("saveDb", "GetSaves: " + ex.Message);
		}
		return list;
	}

	public static bool AddBonusCode(BonusCodeData bonusCode)
	{
		bool result;
		try
		{
			GiftDb obj = DbTypeConvertor.FromGiftData(bonusCode);
			DatabaseManager.db.Insert(obj);
			result = true;
		}
		catch (Exception ex)
		{
			FMLogger.Log("add gift fail. " + ex.Message);
			result = false;
		}
		return result;
	}

	public static List<BonusCodeData> GetClaimedGifts()
	{
		List<BonusCodeData> list = new List<BonusCodeData>();
		try
		{
			string query = "SELECT * FROM GiftDb ORDER BY TimeStamp DESC";
			List<GiftDb> list2 = DatabaseManager.db.Query<GiftDb>(query, new object[0]);
			if (list2 != null)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					list.Add(DbTypeConvertor.ToGiftData(list2[i]));
				}
			}
		}
		catch (Exception ex)
		{
			FMLogger.Log("failed to get saves. " + ex.Message);
			AnalyticsManager.DbTransactionError("giftDb", "GetClaimedGifts: " + ex.Message);
		}
		return list;
	}

	public static List<int> GetReadNewsIds()
	{
		List<int> list = new List<int>();
		try
		{
			string query = "SELECT * FROM NewsDb";
			List<NewsDb> list2 = DatabaseManager.db.Query<NewsDb>(query, new object[0]);
			if (list2 != null)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					list.Add(list2[i].Id);
				}
			}
		}
		catch (Exception ex)
		{
			FMLogger.Log("failed to get saves. " + ex.Message);
			AnalyticsManager.DbTransactionError("newsDb", "GetReadNewsIds: " + ex.Message);
		}
		return list;
	}

	public static void MarksNewsAsRead(List<OrderedItemInfo> newItems)
	{
		for (int i = 0; i < newItems.Count; i++)
		{
			NewsDb item = DbTypeConvertor.FromNewsData(newItems[i]);
			DatabaseManager.AddNewsItems(item);
		}
	}

	private static void AddNewsItems(NewsDb item)
	{
		try
		{
			DatabaseManager.db.Insert(item);
		}
		catch (Exception ex)
		{
			FMLogger.Log("add news fail. " + ex.Message);
		}
	}

	private static SimpleSQLManager db;
}

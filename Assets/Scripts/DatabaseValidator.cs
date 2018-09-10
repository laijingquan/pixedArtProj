// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using SimpleSQL;
using UnityEngine;

public static class DatabaseValidator
{
	public static void Validate(SimpleSQLManager db)
	{
		string text = string.Empty;
		if (DatabaseValidator.DbExists("PictureDb", db))
		{
			int num = DatabaseValidator.DbContainsCollumns("PictureDb", "PicClass", db);
			if (num == -1)
			{
				text += "picDbUpd:-1";
				UnityEngine.Debug.Log("picDb upd check error");
			}
			else if (num == 0)
			{
				text += "picDbUpd:0";
				UnityEngine.Debug.Log("picDb NOT up to date");
			}
		}
		else
		{
			text += "pic:NULL|";
			UnityEngine.Debug.Log("PictureDb doest not exist");
			int num2 = DatabaseManager.FallbackCreatePicDb();
			if (num2 == 1)
			{
				AnalyticsManager.DbRecovery("pic");
			}
		}
		if (DatabaseValidator.DbExists("SaveDb", db))
		{
			int num3 = DatabaseValidator.DbContainsCollumns("SaveDb", "HintsUsed", db);
			if (num3 == -1)
			{
				text += "saveDbUpd:-1";
				UnityEngine.Debug.Log("saveDb upd check error");
			}
			else if (num3 == 0)
			{
				text += "saveDbUpd:0";
				UnityEngine.Debug.Log("saveDb NOT up to date");
			}
		}
		else
		{
			text += "saves:NULL|";
			UnityEngine.Debug.Log("SaveDb doest not exist");
			int num4 = DatabaseManager.FallbackCreateSaveDb();
			if (num4 == 1)
			{
				AnalyticsManager.DbRecovery("save");
			}
		}
		if (!DatabaseValidator.DbExists("NewsDb", db))
		{
			UnityEngine.Debug.Log("NewsDb doest not exist");
			text += "news:NULL|";
			int num5 = DatabaseManager.CreateNewsDb(true);
			if (num5 == 1)
			{
				AnalyticsManager.DbRecovery("news");
			}
		}
		if (!DatabaseValidator.DbExists("GiftDb", db))
		{
			UnityEngine.Debug.Log("GiftDb doest not exist");
			text += "gifts:NULL|";
			int num6 = DatabaseManager.CreateGiftDb(true);
			if (num6 == 1)
			{
				AnalyticsManager.DbRecovery("gift");
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			AnalyticsManager.DbValidationError(text);
		}
	}

	private static bool DbExists(string dbName, SimpleSQLManager db)
	{
		bool result;
		try
		{
			//string query = "SELECT * FROM " + dbName + " LIMIT 1";
			//List<object> list = db.Query<object>(query, new object[0]);
			result = true;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(dbName + " DOES NOT exist. " + ex.Message);
			result = false;
		}
		return result;
	}

	private static int DbContainsCollumns(string dbName, string collumn, SimpleSQLManager db)
	{
		int result;
		try
		{
			bool flag = false;
			string query = "pragma table_info(\"" + dbName + "\")";
			List<DatabaseValidator.TableInfo> list = db.Query<DatabaseValidator.TableInfo>(query, new object[0]);
			foreach (DatabaseValidator.TableInfo tableInfo in list)
			{
				if (tableInfo.name.Equals(collumn))
				{
					flag = true;
					break;
				}
			}
			result = ((!flag) ? 0 : 1);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(" " + ex.Message);
			result = -1;
		}
		return result;
	}

	private class TableInfo
	{
		public string name { get; set; }
	}
}

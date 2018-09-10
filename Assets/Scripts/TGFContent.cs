// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class TGFContent
{
	public TGFContent(string advetrisingId, string altId)
	{
		this.advetrisingId = advetrisingId;
		this.altId = altId;
	}

	public void GetMainPage(int page, Action<LibraryPageResponce> callback)
	{
		if (this.mainPageTask != null && this.mainPageTask.Completed && this.mainPageTask.Success && this.libResponce != null)
		{
			callback(this.libResponce);
		}
		else if (this.mainPageTask != null && this.mainPageTask.IsRunning)
		{
			this.mainPageCallback = callback;
		}
		else
		{
			if (this.mainPageTask != null)
			{
				this.mainPageTask.Cancel();
			}
			string text = (!BuildConfig.TGF_DEBUG_URL) ? "https://coloring-gp.x-flow.app/api/v_2/content.php" : "https://coloring-gp-dev.x-flow.app/content.php";
			WWWForm wwwform = this.ContentRequestData();
			wwwform.AddField("page", page);
			List<BonusCodeData> claimedGifts = SharedData.Instance.GetClaimedGifts();
			if (claimedGifts != null && claimedGifts.Count > 0)
			{
				TGFContent.BonusCodesData obj = new TGFContent.BonusCodesData(claimedGifts);
				string text2 = JsonUtility.ToJson(obj);
				if (!string.IsNullOrEmpty(text2))
				{
					UnityEngine.Debug.Log("bd: " + text2);
					wwwform.AddField("bonus_content", text2);
				}
			}
			this.mainPageCallback = callback;
			this.mainPageTask = new WebPostTask(new string[]
			{
				text,
				text
			}, wwwform, new Action<bool, string>(this.OnMainPageLoaded));
			FMLogger.vCore(string.Concat(new object[]
			{
				"main page req: ",
				text,
				"d: ",
				wwwform
			}));
			WebLoader.Instance.LoadText(this.mainPageTask);
		}
	}

	public void GetDailyPage(Action<DailyPageResponse> callback)
	{
		if (this.dailyPageTask != null && this.dailyPageTask.Completed && this.dailyPageTask.Success && this.dailyResponce != null)
		{
			callback(this.dailyResponce);
		}
		else if (this.dailyPageTask != null && this.dailyPageTask.IsRunning)
		{
			this.dailyPageCallback = callback;
		}
		else
		{
			if (this.dailyPageTask != null)
			{
				this.dailyPageTask.Cancel();
			}
			string text = (!BuildConfig.TGF_DEBUG_URL) ? "https://coloring-gp.x-flow.app/api/v_2/content_daily.php" : "https://coloring-gp-dev.x-flow.app/content_daily.php";
			WWWForm wwwform = this.ContentRequestData();
			wwwform.AddField("page", 1);
			this.dailyPageCallback = callback;
			this.dailyPageTask = new WebPostTask(new string[]
			{
				text,
				text
			}, wwwform, new Action<bool, string>(this.OnDailyPageLoaded));
			FMLogger.vCore("daily page req: ");
			WebLoader.Instance.LoadText(this.dailyPageTask);
		}
	}

	public void GetBonusCodeContent(string giftCode, Action<BonusCodeResponse> callback)
	{
		string text = (!BuildConfig.TGF_DEBUG_URL) ? "https://coloring-gp.x-flow.app/api/v_2/bonus_content.php" : "https://coloring-gp-dev.x-flow.app/bonus_content.php";
		WWWForm wwwform = this.ContentRequestData();
		wwwform.AddField("code", giftCode);
		this.bonusCodeCallback = callback;
		this.bonusCodeTask = new WebPostTask(new string[]
		{
			text,
			text
		}, wwwform, new Action<bool, string>(this.OnBonusCodeLoaded));
		FMLogger.vCore("gift req: " + text + " c:" + giftCode);
		WebLoader.Instance.LoadText(this.bonusCodeTask);
	}

	public void ClearCache()
	{
		if (this.mainPageTask != null)
		{
			this.mainPageTask.Cancel();
			this.mainPageTask = null;
		}
		this.libResponce = null;
		this.mainPageCallback = null;
		if (this.dailyPageTask != null)
		{
			this.dailyPageTask.Cancel();
			this.dailyPageTask = null;
		}
		this.dailyPageCallback = null;
		this.dailyResponce = null;
		if (this.bonusCodeTask != null)
		{
			this.bonusCodeTask.Cancel();
			this.bonusCodeTask = null;
		}
		this.bonusCodeCallback = null;
	}

	private void OnMainPageLoaded(bool success, string text)
	{
		LibraryPageResponce libraryPageResponce = null;
		if (success && !string.IsNullOrEmpty(text))
		{
			try
			{
				libraryPageResponce = JsonUtility.FromJson<LibraryPageResponce>(text);
				FMLogger.vCore("resp: " + text);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("ex: " + ex.Message);
				UnityEngine.Debug.LogError("failed to parse page responce");
				UnityEngine.Debug.LogError(text);
				libraryPageResponce = null;
			}
			if (libraryPageResponce == null || !libraryPageResponce.IsValid())
			{
				libraryPageResponce = null;
				FMLogger.vCore("resp not valid: " + text);
			}
		}
		else
		{
			FMLogger.vCore("lib req fail. 404 or resp empty");
		}
		this.libResponce = libraryPageResponce;
		if (this.mainPageCallback != null)
		{
			this.mainPageCallback(this.libResponce);
		}
		this.mainPageCallback = null;
	}

	private void OnBonusCodeLoaded(bool success, string text)
	{
		if (this.bonusCodeCallback == null)
		{
			return;
		}
		if (success && !string.IsNullOrEmpty(text))
		{
			BonusCodeResponse bonusCodeResponse = null;
			try
			{
				bonusCodeResponse = JsonUtility.FromJson<BonusCodeResponse>(text);
				FMLogger.vCore("gift code resp: " + text);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("gift code ex: " + ex.Message);
				UnityEngine.Debug.LogError("gift code failed to parse page responce");
				UnityEngine.Debug.LogError(text);
				this.bonusCodeCallback(null);
			}
			if (bonusCodeResponse != null && bonusCodeResponse.IsValid())
			{
				this.bonusCodeCallback(bonusCodeResponse);
			}
			else
			{
				UnityEngine.Debug.LogError("gc resp not valid: " + text);
				this.bonusCodeCallback(null);
			}
		}
		else
		{
			this.bonusCodeCallback(null);
		}
		this.bonusCodeCallback = null;
	}

	private void OnDailyPageLoaded(bool success, string text)
	{
		DailyPageResponse dailyPageResponse = null;
		if (success && !string.IsNullOrEmpty(text))
		{
			try
			{
				dailyPageResponse = JsonUtility.FromJson<DailyPageResponse>(text);
				FMLogger.vCore("daily resp: " + text);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("daily ex: " + ex.Message);
				UnityEngine.Debug.LogError("daily failed to parse page responce");
				UnityEngine.Debug.LogError(text);
				dailyPageResponse = null;
			}
			if (dailyPageResponse == null || !dailyPageResponse.IsValid())
			{
				UnityEngine.Debug.LogError("daily resp not valid: " + text);
				dailyPageResponse = null;
			}
		}
		else
		{
			FMLogger.vCore("daily req fail. 404 or resp empty");
		}
		this.dailyResponce = dailyPageResponse;
		if (this.dailyPageCallback != null)
		{
			this.dailyPageCallback(this.dailyResponce);
		}
		this.dailyPageCallback = null;
	}

	private WWWForm ContentRequestData()
	{
		int i = Mathf.RoundToInt((float)(DateTime.Now - DateTime.UtcNow).TotalHours);
		string appVersion = SystemUtils.GetAppVersion();
		string language = SystemUtils.GetLanguage();
		SystemUtils.DevicePerfomance devicePerfomance = SystemUtils.GetDevicePerfomance();
		string value;
		if (devicePerfomance != SystemUtils.DevicePerfomance.High)
		{
			value = "low";
		}
		else
		{
			value = "high";
		}
		string value2;
		switch (SystemUtils.GetIconQuality())
		{
		case SystemUtils.IconQuality.Low:
			value2 = "low";
			break;
		case SystemUtils.IconQuality.Medium:
			value2 = "medium";
			break;
		case SystemUtils.IconQuality.High:
			value2 = "medium";
			break;
		default:
			value2 = "low";
			break;
		}
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("utc", i);
		wwwform.AddField("app_ver", appVersion);
		wwwform.AddField("quality", value);
		wwwform.AddField("package_name", SystemUtils.GetAppPackage());
		wwwform.AddField("lang", language);
		wwwform.AddField("icon_quality", value2);
		wwwform.AddField("advertising_id", this.advetrisingId);
		wwwform.AddField("alt_id", this.altId);
		return wwwform;
	}

	private string advetrisingId;

	private string altId;

	private WebPostTask mainPageTask;

	private Action<LibraryPageResponce> mainPageCallback;

	private LibraryPageResponce libResponce;

	private WebPostTask bonusCodeTask;

	private Action<BonusCodeResponse> bonusCodeCallback;

	private WebPostTask dailyPageTask;

	private Action<DailyPageResponse> dailyPageCallback;

	private DailyPageResponse dailyResponce;

	[Serializable]
	public class BonusCodesData
	{
		public BonusCodesData(List<BonusCodeData> data)
		{
			this.codes = new List<TGFContent.GiftDataWrap>();
			for (int i = 0; i < data.Count; i++)
			{
				TGFContent.GiftDataWrap giftDataWrap = new TGFContent.GiftDataWrap();
				giftDataWrap.name = data[i].BonusCode;
				giftDataWrap.date = data[i].ClaimTime.ToString("yyy-MM-dd HH:mm:ss");
				this.codes.Add(giftDataWrap);
			}
		}

		public List<TGFContent.GiftDataWrap> codes;
	}

	[Serializable]
	public class GiftDataWrap
	{
		public string name;

		public string date;
	}
}

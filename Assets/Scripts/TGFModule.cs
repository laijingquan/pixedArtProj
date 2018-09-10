// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class TGFModule : MonoBehaviour
{
	public static string TGFAnalyticsURL
	{
		get
		{
			return "https://coloring-gp.x-flow.app/api/v_2/index.php";
		}
	}

	public static TGFModule Instance
	{
		get
		{
			return TGFModule.instance;
		}
	}

	public string DeviceId
	{
		get
		{
			string text = (this.defParams == null) ? "unknown" : this.defParams.AdvetrisingId;
			return this.Encode(text);
		}
	}

	public string DeviceAltId
	{
		get
		{
			string text = (this.defParams == null) ? "unknown" : this.defParams.AltId;
			return this.Encode(text);
		}
	}

	private bool CanSend
	{
		get
		{
			return this.defParams != null;
		}
	}

	public void Init(string adId, int adsTrackingLimit, string countryCode, string countryLanguage)
	{
		this.defParams = new TGFReqParam();
		if (!string.IsNullOrEmpty(adId))
		{
			this.defParams.AdvetrisingId = adId;
		}
		else
		{
			this.defParams.AdvetrisingId = "unknown";
		}
		if (adsTrackingLimit != -1)
		{
			this.defParams.AdsTrackingLimit = adsTrackingLimit.ToString();
		}
		if (countryCode != null && countryLanguage != null)
		{
			this.defParams.Locale = countryCode + "_" + countryLanguage;
		}
		else
		{
			this.defParams.Locale = "unknown";
		}
		this.CheckInstallConfirmReportStatus(this.defParams.ToForm());
		this.StartSession();
		this.content = new TGFContent(this.defParams.AdvetrisingId, this.defParams.AltId);
	}

	public void PrecachePages()
	{
		this.content.GetMainPage(1, null);
		this.content.GetDailyPage(null);
	}

	private string Encode(string text)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		return Convert.ToBase64String(bytes);
	}

	private string Decode(string test)
	{
		byte[] bytes = Convert.FromBase64String(test);
		return Encoding.UTF8.GetString(bytes);
	}

	public void GameStarted(int picId, int size, FillAlgorithm fillType)
	{
		if (this.CanSend)
		{
			WWWForm wwwform = this.GameActionForm(picId);
			wwwform.AddField("sub_action", "start");
			wwwform.AddField("size", size.ToString());
			wwwform.AddField("filltype", fillType.ToString().ToLower());
			base.StartCoroutine(this.POST(wwwform));
		}
	}

	public void GameContinue(int picId, int size, FillAlgorithm fillType)
	{
		if (this.CanSend)
		{
			WWWForm wwwform = this.GameActionForm(picId);
			wwwform.AddField("sub_action", "continue");
			wwwform.AddField("size", size.ToString());
			wwwform.AddField("filltype", fillType.ToString().ToLower());
			base.StartCoroutine(this.POST(wwwform));
		}
	}

	public void GameFinish(int picId, int time, int size, FillAlgorithm fillType)
	{
		if (this.CanSend)
		{
			WWWForm wwwform = this.GameActionForm(picId);
			wwwform.AddField("sub_action", "finish");
			wwwform.AddField("time", time);
			wwwform.AddField("size", size.ToString());
			wwwform.AddField("filltype", fillType.ToString().ToLower());
			base.StartCoroutine(this.POST(wwwform));
		}
	}

	public void DbCreationError(string db, string msg)
	{
		if (this.CanSend)
		{
			WWWForm wwwform = this.defParams.ToForm();
			wwwform.AddField("action", "db_creation_error");
			wwwform.AddField("db", db);
			wwwform.AddField("msg", msg);
			base.StartCoroutine(this.POST(wwwform));
		}
	}

	public void GetDailyPage(Action<DailyPageResponse> callback)
	{
		this.content.GetDailyPage(callback);
	}

	public void GetMainPage(int page, Action<LibraryPageResponce> callback)
	{
		this.content.GetMainPage(page, callback);
	}

	public void GetBonusCodeContent(string giftCode, Action<BonusCodeResponse> callback)
	{
		this.content.GetBonusCodeContent(giftCode, callback);
	}

	public void ClearPageCache()
	{
		this.content.ClearCache();
	}

	private void CheckInstallConfirmReportStatus(WWWForm wwwForm)
	{
		if (!AppInstallReportService.ConfirmInstallGUIDSent)
		{
			AppInstallReportService.Instance.ReportConsentConfirm(wwwForm);
		}
	}

	private void StartSession()
	{
		if (this.CanSend)
		{
			WWWForm wwwform = this.defParams.ToForm();
			wwwform.AddField("action", "session_start");
			base.StartCoroutine(this.POST(wwwform));
		}
	}

	private void Awake()
	{
		TGFModule.instance = this;
	}

	private void OnApplicationPause(bool isPause)
	{
		if (isPause)
		{
			this.intentPause = AppState.IntentionalPause;
			this.pauseTime = DateTime.Now;
		}
		else
		{
			int num = (!this.intentPause) ? 10 : 60;
			double totalSeconds = (DateTime.Now - this.pauseTime).TotalSeconds;
			if (totalSeconds > (double)num)
			{
				this.StartSession();
			}
		}
	}

	private WWWForm GameActionForm(int picId)
	{
		WWWForm wwwform = this.defParams.ToForm();
		wwwform.AddField("action", "game");
		wwwform.AddField("level_id", picId.ToString());
		return wwwform;
	}

	private IEnumerator POST(WWWForm data)
	{
		string url = (!BuildConfig.TGF_DEBUG_URL) ? "https://coloring-gp.x-flow.app/api/v_2/index.php" : "https://coloring-gp-dev.x-flow.app/index.php";
		UnityWebRequest www = UnityWebRequest.Post(url, data);
		yield return www.Send();
		if (www.isNetworkError)
		{
			UnityEngine.Debug.Log(www.error);
		}
		yield break;
	}

	private bool DONT_SEND_REQ;

	public const string TGF_URL = "https://coloring-gp.x-flow.app/api/v_2/index.php";

	public const string TGF_CONTENT_URL = "https://coloring-gp.x-flow.app/api/v_2/content.php";

	public const string TGF_GIFTCODE_URL = "https://coloring-gp.x-flow.app/api/v_2/bonus_content.php";

	public const string TGF_DAILY_URL = "https://coloring-gp.x-flow.app/api/v_2/content_daily.php";

	public const string TGF_CHECK_URL = "https://coloring-gp.x-flow.app/api/v_2/checker.php";

	public const string TGF_URL_DEBUG = "https://coloring-gp-dev.x-flow.app/index.php";

	public const string TGF_DAILY_URL_DEBUG = "https://coloring-gp-dev.x-flow.app/content_daily.php";

	public const string TGF_GIFTCODE_URL_DEBUG = "https://coloring-gp-dev.x-flow.app/bonus_content.php";

	public const string TGF_CONTENT_URL_DEBUG = "https://coloring-gp-dev.x-flow.app/content.php";

	private static TGFModule instance;

	private TGFContent content;

	private bool intentPause;

	private DateTime pauseTime;

	private const int START_SESSION_INTERVAL = 10;

	private const int START_SESSION_ADS_INTERVAL = 60;

	private const string reqActionKey = "action";

	private TGFReqParam defParams;
}

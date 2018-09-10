// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

public class AppInstallReportService : MonoBehaviour
{
	 
	public static event Action<bool> GDPRStatusReceived;

	public static int Time
	{
		get
		{
			return PlayerPrefs.GetInt("c_time", 0);
		}
		set
		{
			PlayerPrefs.SetInt("c_time", value);
		}
	}

	public static bool ConsentReceived
	{
		get
		{
			return PlayerPrefs.GetInt("c_received", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("c_received", (!value) ? 0 : 1);
			PlayerPrefs.Save();
		}
	}

	public static string InstallGUID
	{
		get
		{
			return PlayerPrefs.GetString("c_guid", string.Empty);
		}
		set
		{
			PlayerPrefs.SetString("c_guid", value);
		}
	}

	public static GDPRStatus GDPRStatus
	{
		get
		{
			return (GDPRStatus)PlayerPrefs.GetInt("c_status", 0);
		}
		set
		{
			PlayerPrefs.SetInt("c_status", (int)value);
			PlayerPrefs.Save();
		}
	}

	public static GDPRStatus TGFR_GDPRStatus
	{
		get
		{
			return (GDPRStatus)PlayerPrefs.GetInt("tg_c_status", 0);
		}
		set
		{
			PlayerPrefs.SetInt("tg_c_status", (int)value);
			PlayerPrefs.Save();
		}
	}

	public static bool InstallGUIDSent
	{
		get
		{
			return PlayerPrefs.GetInt("c_install", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("c_install", (!value) ? 0 : 1);
		}
	}

	public static bool ConfirmInstallGUIDSent
	{
		get
		{
			return PlayerPrefs.GetInt("c_confirm", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("c_confirm", (!value) ? 0 : 1);
		}
	}

	public static AppInstallReportService Instance
	{
		get
		{
			if (AppInstallReportService.instance == null)
			{
				AppInstallReportService.instance = new GameObject(typeof(AppInstallReportService).Name).AddComponent<AppInstallReportService>();
				UnityEngine.Object.DontDestroyOnLoad(AppInstallReportService.instance);
			}
			return AppInstallReportService.instance;
		}
	}

	public bool IsReportingInstall { get; private set; }

	public bool IsReportingConfirm { get; private set; }

	public void ReportInstall()
	{
		if (this.IsReportingInstall)
		{
			return;
		}
		this.IsReportingInstall = true;
		this.url = TGFModule.TGFAnalyticsURL;
		this.wwwFormInstall = new WWWForm();
		this.wwwFormInstall.AddField("action", "install");
		this.wwwFormInstall.AddField("guid", AppInstallReportService.InstallGUID);
		this.SendInstallData();
		FMLogger.vCore("app install report");
	}

	public void ReportConsentConfirm(WWWForm wwwForm)
	{
		if (this.IsReportingConfirm)
		{
			return;
		}
		this.url = TGFModule.TGFAnalyticsURL;
		this.IsReportingConfirm = true;
		this.wwwFormConfirm = wwwForm;
		this.wwwFormConfirm.AddField("action", "confirm");
		this.wwwFormConfirm.AddField("guid", AppInstallReportService.InstallGUID);
		this.wwwFormConfirm.AddField("time", AppInstallReportService.Time.ToString());
		this.wwwFormConfirm.AddField("gdpr", (AppInstallReportService.GDPRStatus != GDPRStatus.Applied) ? "false" : "true");
		this.SendConfirmData();
		FMLogger.vCore("app confirm report");
	}

	private void SendInstallData()
	{
		base.StartCoroutine(this.POSTInstall(this.url, this.wwwFormInstall));
	}

	private void SendConfirmData()
	{
		base.StartCoroutine(this.POSTConfirm(this.url, this.wwwFormConfirm));
	}

	private IEnumerator POSTInstall(string host, WWWForm data)
	{
		UnityWebRequest www = UnityWebRequest.Post(host, data);
		yield return www.Send();
		if (!www.isNetworkError && www.responseCode == 200L)
		{
			string text = www.downloadHandler.text;
			AppInstallReportService.InstallRespone installRespone = JsonUtility.FromJson<AppInstallReportService.InstallRespone>(text);
			if (installRespone != null && installRespone.success == 1)
			{
				AppInstallReportService.InstallGUIDSent = true;
				if (AppInstallReportService.GDPRStatusReceived != null)
				{
					AppInstallReportService.GDPRStatusReceived(installRespone.gdpr == 1);
				}
				FMLogger.vCore("app install sent. status: " + (installRespone.gdpr == 1));
			}
		}
		if (!AppInstallReportService.InstallGUIDSent)
		{
			this.RescheduleInstallReport();
		}
		yield break;
	}

	private IEnumerator POSTConfirm(string host, WWWForm data)
	{
		UnityWebRequest www = UnityWebRequest.Post(host, data);
		yield return www.Send();
		if (!www.isNetworkError && www.responseCode == 200L)
		{
			string text = www.downloadHandler.text;
			AppInstallReportService.ConfirmInstallRespone confirmInstallRespone = JsonUtility.FromJson<AppInstallReportService.ConfirmInstallRespone>(text);
			if (confirmInstallRespone != null && confirmInstallRespone.success == 1)
			{
				AppInstallReportService.ConfirmInstallGUIDSent = true;
				FMLogger.vCore("app confirm sent");
			}
		}
		if (!AppInstallReportService.ConfirmInstallGUIDSent)
		{
			this.RescheduleConfirmReport();
			FMLogger.vCore("app confirm resp code:" + www.responseCode + string.Empty);
		}
		yield break;
	}

	private void RescheduleInstallReport()
	{
		FMLogger.vCore("app install report error. reschedule");
		base.StartCoroutine(this.DelayAction(1.5f, new Action(this.SendInstallData)));
	}

	private void RescheduleConfirmReport()
	{
		FMLogger.vCore("app confirm report error. reschedule");
		base.StartCoroutine(this.DelayAction(15f, new Action(this.SendConfirmData)));
	}

	public void ScheduleMopubGrantConsent(Action grantCallback)
	{
		base.StartCoroutine(this.DelayAction(60f, delegate
		{
			this.MopubGrantConsent(grantCallback);
		}));
	}

	private void MopubGrantConsent(Action grantCallback)
	{
		try
		{
			grantCallback();
		}
		catch (Exception ex)
		{
			FMLogger.vCore("failed to grant consent again. " + ex.Message);
		}
	}

	private IEnumerator DelayAction(float delay, Action a)
	{
		yield return new WaitForSeconds(delay);
		a();
		yield break;
	}

	public static void GenerateInstallGUID()
	{
		AppInstallReportService.InstallGUID = Guid.NewGuid().ToString();
		FMLogger.vCore("generated guid: " + AppInstallReportService.InstallGUID);
	}

	private const string consentReceivedKey = "c_received";

	private const string consentTimeKey = "c_time";

	private const string installGUIDKey = "c_guid";

	private const string gdprStatusKey = "c_status";

	private const string tg_gdprStatusKey = "tg_c_status";

	private const string installGUIDSentKey = "c_install";

	private const string confirmInstallGUIDSentKey = "c_confirm";

	private static AppInstallReportService instance;

	private bool reported;

	private WWWForm wwwFormInstall;

	private WWWForm wwwFormConfirm;

	private string url;

	[Serializable]
	public class InstallRespone
	{
		public int success;

		public int gdpr;
	}

	[Serializable]
	public class ConfirmInstallRespone
	{
		public int success;
	}
}

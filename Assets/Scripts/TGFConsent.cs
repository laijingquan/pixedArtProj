// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;

public class TGFConsent
{
	public TGFConsent()
	{
		AppInstallReportService.GDPRStatusReceived += this.OnGDPRStatusReceived;
	}

	 
	public event Action<bool> OnResult;

	public void Check()
	{
		this.time = DateTime.UtcNow;
		if (AppInstallReportService.TGFR_GDPRStatus == GDPRStatus.Unknown)
		{
			this.StartReportInstallService();
		}
		else
		{
			FMLogger.vCore(string.Concat(new object[]
			{
				"server gdpr status already known. ",
				AppInstallReportService.TGFR_GDPRStatus.ToString(),
				" cr: ",
				AppInstallReportService.ConsentReceived
			}));
			bool obj = AppInstallReportService.TGFR_GDPRStatus == GDPRStatus.Applied && !AppInstallReportService.ConsentReceived;
			if (this.OnResult != null)
			{
				this.OnResult(obj);
			}
		}
	}

	private void StartReportInstallService()
	{
		AppInstallReportService.Instance.ReportInstall();
	}

	private void OnGDPRStatusReceived(bool gdprApplied)
	{
		int num = (int)(DateTime.UtcNow - this.time).TotalMilliseconds;
		FMLogger.vCore(string.Concat(new object[]
		{
			"OnGDPRStatusReceived callback: ga",
			gdprApplied,
			" t:",
			num
		}));
		bool obj = false;
		if (AppInstallReportService.TGFR_GDPRStatus == GDPRStatus.Unknown)
		{
			AppInstallReportService.TGFR_GDPRStatus = ((!gdprApplied) ? GDPRStatus.NotApplied : GDPRStatus.Applied);
			obj = gdprApplied;
			FMLogger.vCore("changing server consent from unknown to " + ((!gdprApplied) ? "not applied" : "applied "));
		}
		else if (AppInstallReportService.TGFR_GDPRStatus == GDPRStatus.NotApplied && gdprApplied)
		{
			FMLogger.vCore("changing server consent from not applied to applied");
			AppInstallReportService.TGFR_GDPRStatus = GDPRStatus.Applied;
			obj = true;
		}
		if (this.OnResult != null)
		{
			this.OnResult(obj);
		}
	}

	public void Clean()
	{
		AppInstallReportService.GDPRStatusReceived -= this.OnGDPRStatusReceived;
	}

	private DateTime time;
}

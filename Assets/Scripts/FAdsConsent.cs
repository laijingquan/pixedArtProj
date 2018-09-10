// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;

public class FAdsConsent : IConsentManager
{
	 
	public event Action<bool> OnResult;

	public string PolicyUrl
	{
		get
		{
			return this.consentProvider.PolicyURL;
		}
	}

	public string VendorUrl
	{
		get
		{
			return this.consentProvider.VendorsURL;
		}
	}

	public void Check()
	{
		string adUnit = (!SafeLayout.IsTablet) ? "6bc3898062484e71a114d0ab59cb1c78" : "0543e571406140dd96252ac1351b99f5";
		this.time = DateTime.UtcNow;
		this.consentProvider.InitForConsent(adUnit);
		FMLogger.vCore("fads pre int consent status " + this.consentProvider.CurrentConsentStatus);
	}

	public void GrantConsent()
	{
		try
		{
			this.InternalGrantConsent();
		}
		catch (Exception ex)
		{
			FMLogger.vCore("fads failed to grant consent " + ex.Message);
			AppInstallReportService.Instance.ScheduleMopubGrantConsent(new Action(this.InternalGrantConsent));
		}
	}

	public void Clean()
	{
		this.consentProvider.ConsentInitialized -= this.OnSdkInited;
	}

	private void OnSdkInited()
	{
		int num = (int)(DateTime.UtcNow - this.time).TotalMilliseconds;
		FMLogger.vCore(string.Concat(new object[]
		{
			"fads inited. t:",
			num,
			" show?:",
			this.consentProvider.ShouldShowConsentDialog,
			". cs:",
			this.consentProvider.CurrentConsentStatus,
			" ga:",
			(this.consentProvider.IsGDPRApplicable == null) ? "null" : this.consentProvider.IsGDPRApplicable.ToString()
		}));
		bool obj = this.consentProvider.ShouldShowConsentDialog || (this.consentProvider.IsGDPRApplicable == null && this.consentProvider.CurrentConsentStatus == FAdsNetwork.ConsentStatus.Unknown);
		if (this.OnResult != null)
		{
			this.OnResult(obj);
		}
	}

	private void InternalGrantConsent()
	{
		this.consentProvider.GrantConsent();
	}

	private DateTime time;

	private IConsentProvider consentProvider;
}

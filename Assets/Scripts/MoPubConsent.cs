// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;

public class MoPubConsent : IConsentManager
{
	public MoPubConsent()
	{
		MoPubManager.OnSdkInitalizedEvent += this.OnSdkInited;
	}

	 
	public event Action<bool> OnResult;

	public string PolicyUrl
	{
		get
		{
			Uri currentConsentPrivacyPolicyUrl = MoPubAndroid.PartnerApi.CurrentConsentPrivacyPolicyUrl;
			return (!(currentConsentPrivacyPolicyUrl != null)) ? null : currentConsentPrivacyPolicyUrl.ToString();
		}
	}

	public string VendorUrl
	{
		get
		{
			Uri currentVendorListUrl = MoPubAndroid.PartnerApi.CurrentVendorListUrl;
			return (!(currentVendorListUrl != null)) ? null : currentVendorListUrl.ToString();
		}
	}

	public void Check()
	{
		string anyAdUnitId = (!SafeLayout.IsTablet) ? "6bc3898062484e71a114d0ab59cb1c78" : "0543e571406140dd96252ac1351b99f5";
		this.time = DateTime.UtcNow;
		MoPubAndroid.InitializeSdk(anyAdUnitId);
		FMLogger.vCore("mopub pre int consent status " + MoPubAndroid.CurrentConsentStatus);
	}

	private void OnSdkInited(string adUnit)
	{
		int num = (int)(DateTime.UtcNow - this.time).TotalMilliseconds;
		FMLogger.vCore(string.Concat(new object[]
		{
			"Mopub inited. t:",
			num,
			" show?:",
			MoPubAndroid.ShouldShowConsentDialog,
			". cs:",
			MoPubAndroid.CurrentConsentStatus,
			" ga:",
			(MoPubAndroid.IsGdprApplicable == null) ? "null" : MoPubAndroid.IsGdprApplicable.ToString()
		}));
		bool obj = MoPubAndroid.ShouldShowConsentDialog || (MoPubAndroid.IsGdprApplicable == null && MoPubAndroid.CurrentConsentStatus == MoPubBase.Consent.Status.Unknown);
		if (this.OnResult != null)
		{
			this.OnResult(obj);
		}
	}

	public void GrantConsent()
	{
		try
		{
			this.InternalGrantConsent();
		}
		catch (Exception ex)
		{
			FMLogger.vCore("failed to grant consent " + ex.Message);
			AppInstallReportService.Instance.ScheduleMopubGrantConsent(new Action(this.InternalGrantConsent));
		}
	}

	public void Clean()
	{
		MoPubManager.OnSdkInitalizedEvent -= this.OnSdkInited;
	}

	private void InternalGrantConsent()
	{
		MoPubAndroid.PartnerApi.GrantConsent();
	}

	private DateTime time;
}

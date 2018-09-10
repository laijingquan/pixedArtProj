// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConsentScreen : MonoBehaviour
{
	private void Awake()
	{
		SafeLayout.Init();
		if (SafeLayout.IsTablet)
		{
			CanvasScaler component = base.transform.parent.GetComponent<CanvasScaler>();
			component.referenceResolution = new Vector2(1654f, 2927f);
		}
	}

	private void Start()
	{
		if (string.IsNullOrEmpty(AppInstallReportService.InstallGUID))
		{
			AppInstallReportService.GenerateInstallGUID();
		}
		this.adsConsent = new MoPubConsent();
		this.adsConsent.OnResult += this.OnMopubResult;
		this.adsConsent.Check();
		base.StartCoroutine(this.TGFConsentTimeout((float)this.tgfResultTimeoutTime));
		this.tgfConsent = new TGFConsent();
		this.tgfConsent.OnResult += this.OnTGFResult;
		this.tgfConsent.Check();
		this.initTime = DateTime.UtcNow;
	}

	private void OnMopubResult(bool showDialog)
	{
		this.mopubResultReceived = true;
		this.mopubRequireConsent = showDialog;
		FMLogger.vCore("On mopub consent result. should show: " + showDialog);
	}

	private void OnTGFResult(bool showDialog)
	{
		this.tgfResultReceived = true;
		this.tgfRequireConsent = showDialog;
		FMLogger.vCore("On server consent result. should show: " + showDialog);
	}

	private void OnDisable()
	{
		if (this.tgfConsent != null)
		{
			this.tgfConsent.Clean();
		}
		if (this.adsConsent != null)
		{
			this.adsConsent.Clean();
		}
	}

	private void Update()
	{
		if (!this.consentHandled && this.mopubResultReceived && this.tgfResultReceived)
		{
			this.consentHandled = true;
			int num = (int)(DateTime.UtcNow - this.initTime).TotalMilliseconds;
			AppInstallReportService.Time = num;
			FMLogger.vCore(string.Concat(new object[]
			{
				"consent handle t: ",
				num,
				" s:",
				this.tgfRequireConsent,
				" m:",
				this.mopubRequireConsent
			}));
			this.HandleConsent();
		}
		if (!this.isLongLoad)
		{
			this.loadTime += Time.deltaTime;
			if (this.loadTime > 20f)
			{
				this.isLongLoad = true;
				if (!this.mopubResultReceived || !this.tgfResultReceived)
				{
					SplashCanvas.Instance.ShowError();
				}
			}
		}
	}

	private IEnumerator TGFConsentTimeout(float timeout)
	{
		yield return new WaitForSeconds(timeout);
		if (!this.tgfResultReceived)
		{
			this.tgfConsent.OnResult -= this.OnTGFResult;
			this.tgfRequireConsent = false;
			this.tgfResultReceived = true;
		}
		yield break;
	}

	private void HandleConsent()
	{
		if (this.mopubRequireConsent || this.tgfRequireConsent)
		{
			AppInstallReportService.ConsentReceived = false;
			AppInstallReportService.ConfirmInstallGUIDSent = false;
			FMLogger.vCore("showing consent dialog");
			this.ShowDialog();
		}
		else
		{
			FMLogger.vCore("consent not appliable. load next scene");
			this.LoadStartSceneNow();
		}
	}

	private void LoadStartSceneNow()
	{
		this.InitFabric();
		this.InitSmaato();
		base.StartCoroutine(this.DelayAction(0.1f, delegate
		{
			this.sceneLoader.LoadSceneImmediate("start");
		}));
	}

	private IEnumerator DelayAction(float delay, Action a)
	{
		yield return new WaitForSeconds(delay);
		a();
		yield break;
	}

	private void ShowDialog()
	{
		this.sceneLoader.Close();
		if (SafeLayout.IsTablet)
		{
			this.popupTablet.Open();
		}
		else
		{
			this.popupPhone.Open();
		}
	}

	private void InitFabric()
	{
		try
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("fmp.fabrichelper.Fabric"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
					androidJavaClass.CallStatic("init", new object[]
					{
						@static
					});
				}
			}
			FMLogger.vCore("fabric inited");
		}
		catch (Exception ex)
		{
			FMLogger.vCore("failed to init fabric. msg:" + ex.Message);
		}
	}

	private void InitSmaato()
	{
		try
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("fmp.smaatohelper.Smaato"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
					androidJavaClass.CallStatic("init", new object[]
					{
						@static
					});
				}
			}
			FMLogger.vCore("smaato inited");
		}
		catch (Exception ex)
		{
			FMLogger.vCore("failed to init smaato. msg:" + ex.Message);
		}
	}

	public void OnConfirmClick()
	{
		AppInstallReportService.ConsentReceived = true;
		AppInstallReportService.GDPRStatus = GDPRStatus.Applied;
		AppInstallReportService.TGFR_GDPRStatus = GDPRStatus.Applied;
		this.adsConsent.GrantConsent();
		FMLogger.vCore("consent OnConfirmClick. mopub cs: ");
		this.LoadStartSceneNow();
	}

	public void OnTermsClick()
	{
		FMLogger.vCore("consent OnTermsClick");
		SystemUtils.OpenBrowser("https://x-flow.app/terms-of-use.html");
	}

	public void OnPrivacyClick()
	{
		FMLogger.vCore("consent OnPrivacyClick");
		SystemUtils.OpenBrowser("https://x-flow.app/privacy-policy.html");
	}

	public void OnMopubPrivacyClick()
	{
		string text = "https://www.mopub.com/legal/privacy/";
		string url = string.IsNullOrEmpty(this.adsConsent.PolicyUrl) ? text : this.adsConsent.PolicyUrl;
		SystemUtils.OpenBrowser(url);
	}

	public void OnMopubPartnersClick()
	{
		string text = "https://www.mopub.com/legal/partners/";
		string url = string.IsNullOrEmpty(this.adsConsent.VendorUrl) ? text : this.adsConsent.VendorUrl;
		SystemUtils.OpenBrowser(url);
	}

	[SerializeField]
	private SceneLoader sceneLoader;

	[SerializeField]
	private FMPopup popupPhone;

	[SerializeField]
	private FMPopup popupTablet;

	private DateTime initTime;

	private bool isLongLoad;

	private float loadTime;

	private IConsentManager adsConsent;

	private TGFConsent tgfConsent;

	private bool mopubResultReceived;

	private bool tgfResultReceived;

	private bool mopubRequireConsent;

	private bool tgfRequireConsent;

	private bool consentHandled;

	private int tgfResultTimeoutTime = 5;
}

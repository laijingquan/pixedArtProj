// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoPubDemoGUI : MonoBehaviour
{
	private static bool IsAdUnitArrayNullOrEmpty(ICollection<string> adUnitArray)
	{
		return adUnitArray == null || adUnitArray.Count == 0;
	}

	private void AddAdUnitsToStateMaps(IEnumerable<string> adUnits)
	{
		foreach (string key in adUnits)
		{
			this._adUnitToLoadedMapping.Add(key, false);
			this._adUnitToShownMapping.Add(key, false);
		}
	}

	public void SdkInitialized()
	{
		this._canCollectPersonalInfo = MoPubAndroid.CanCollectPersonalInfo;
		this._currentConsentStatus = MoPubAndroid.CurrentConsentStatus;
		this._shouldShowConsentDialog = MoPubAndroid.ShouldShowConsentDialog;
		this._isGdprApplicable = MoPubAndroid.IsGdprApplicable;
	}

	public void ConsentStatusChanged(MoPubBase.Consent.Status newStatus, bool canCollectPersonalInfo)
	{
		this._canCollectPersonalInfo = canCollectPersonalInfo;
		this._currentConsentStatus = newStatus;
		this._shouldShowConsentDialog = MoPubAndroid.ShouldShowConsentDialog;
		this._status = "Consent status changed";
	}

	public void LoadAvailableRewards(string adUnitId, List<MoPubBase.Reward> availableRewards)
	{
		this._adUnitToRewardsMapping.Remove(adUnitId);
		if (availableRewards != null)
		{
			this._adUnitToRewardsMapping[adUnitId] = availableRewards;
		}
	}

	public void BannerLoaded(string adUnitId, float height)
	{
		this.AdLoaded(adUnitId);
		this._adUnitToShownMapping[adUnitId] = true;
	}

	public void AdLoaded(string adUnit)
	{
		this._adUnitToLoadedMapping[adUnit] = true;
		this._status = "Loaded " + adUnit;
	}

	public void AdFailed(string error)
	{
		this._status = "Error: " + error;
	}

	public void AdDismissed(string adUnit)
	{
		this._adUnitToLoadedMapping[adUnit] = false;
		this._status = string.Empty;
	}

	public bool ConsentDialogLoaded
	{
		private get
		{
			return this._consentDialogLoaded;
		}
		set
		{
			this._consentDialogLoaded = value;
			if (this._consentDialogLoaded)
			{
				this._status = "Consent dialog loaded";
			}
		}
	}

	private void Awake()
	{
		if (Screen.width < 960 && Screen.height < 960)
		{
			this._skin.button.fixedHeight = 50f;
		}
		this._smallerFont = new GUIStyle(this._skin.label)
		{
			fontSize = this._skin.button.fontSize
		};
		this._centeredStyle = new GUIStyle(this._skin.label)
		{
			alignment = TextAnchor.UpperCenter
		};
		this._sectionMarginSize = this._skin.label.fontSize;
		this.AddAdUnitsToStateMaps(this._bannerAdUnits);
		this.AddAdUnitsToStateMaps(this._interstitialAdUnits);
		this.AddAdUnitsToStateMaps(this._rewardedVideoAdUnits);
		this.AddAdUnitsToStateMaps(this._rewardedRichMediaAdUnits);
		this.ConsentDialogLoaded = false;
	}

	private void Start()
	{
		string anyAdUnitId = this._bannerAdUnits[0];
		MoPubAndroid.InitializeSdk(anyAdUnitId);
		MoPubAndroid.LoadBannerPluginsForAdUnits(this._bannerAdUnits);
		MoPubAndroid.LoadInterstitialPluginsForAdUnits(this._interstitialAdUnits);
		MoPubAndroid.LoadRewardedVideoPluginsForAdUnits(this._rewardedVideoAdUnits);
		MoPubAndroid.LoadRewardedVideoPluginsForAdUnits(this._rewardedRichMediaAdUnits);
		GameObject gameObject = GameObject.Find("MoPubNativeAds");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
	}

	private void OnGUI()
	{
		GUI.skin = this._skin;
		Rect screenRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		screenRect.x += 20f;
		screenRect.width -= 40f;
		GUILayout.BeginArea(screenRect);
		this.CreateTitleSection();
		this.CreateBannersSection();
		this.CreateInterstitialsSection();
		this.CreateRewardedVideosSection();
		this.CreateRewardedRichMediaSection();
		this.CreateUserConsentSection();
		this.CreateActionsSection();
		this.CreateStatusSection();
		GUILayout.EndArea();
	}

	private void CreateTitleSection()
	{
		int fontSize = this._centeredStyle.fontSize;
		this._centeredStyle.fontSize = 48;
		GUI.Label(new Rect(0f, 10f, (float)Screen.width, 60f), MoPubBase.PluginName, this._centeredStyle);
		this._centeredStyle.fontSize = fontSize;
		GUI.Label(new Rect(0f, 70f, (float)Screen.width, 60f), "with " + MoPub.SdkName, this._centeredStyle);
	}

	private void CreateBannersSection()
	{
		GUILayout.Space(102f);
		GUILayout.Label("Banners", new GUILayoutOption[0]);
		if (!MoPubDemoGUI.IsAdUnitArrayNullOrEmpty(this._bannerAdUnits))
		{
			foreach (string text in this._bannerAdUnits)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUI.enabled = !this._adUnitToLoadedMapping[text];
				if (GUILayout.Button(MoPubDemoGUI.CreateRequestButtonLabel(text), new GUILayoutOption[0]))
				{
					UnityEngine.Debug.Log("requesting banner with AdUnit: " + text);
					this._status = "Requesting " + text;
					MoPubAndroid.CreateBanner(text, MoPubBase.AdPosition.BottomCenter);
				}
				GUI.enabled = this._adUnitToLoadedMapping[text];
				if (GUILayout.Button("Destroy", new GUILayoutOption[0]))
				{
					this._status = string.Empty;
					MoPubAndroid.DestroyBanner(text);
					this._adUnitToLoadedMapping[text] = false;
					this._adUnitToShownMapping[text] = false;
				}
				GUI.enabled = (this._adUnitToLoadedMapping[text] && !this._adUnitToShownMapping[text]);
				if (GUILayout.Button("Show", new GUILayoutOption[0]))
				{
					this._status = string.Empty;
					MoPubAndroid.ShowBanner(text, true);
					this._adUnitToShownMapping[text] = true;
				}
				GUI.enabled = (this._adUnitToLoadedMapping[text] && this._adUnitToShownMapping[text]);
				if (GUILayout.Button("Hide", new GUILayoutOption[0]))
				{
					this._status = string.Empty;
					MoPubAndroid.ShowBanner(text, false);
					this._adUnitToShownMapping[text] = false;
				}
				GUI.enabled = true;
				GUILayout.EndHorizontal();
			}
		}
		else
		{
			GUILayout.Label("No banner AdUnits available", this._smallerFont, null);
		}
	}

	private void CreateInterstitialsSection()
	{
		GUILayout.Space((float)this._sectionMarginSize);
		GUILayout.Label("Interstitials", new GUILayoutOption[0]);
		if (!MoPubDemoGUI.IsAdUnitArrayNullOrEmpty(this._interstitialAdUnits))
		{
			foreach (string text in this._interstitialAdUnits)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUI.enabled = !this._adUnitToLoadedMapping[text];
				if (GUILayout.Button(MoPubDemoGUI.CreateRequestButtonLabel(text), new GUILayoutOption[0]))
				{
					UnityEngine.Debug.Log("requesting interstitial with AdUnit: " + text);
					this._status = "Requesting " + text;
					MoPubAndroid.RequestInterstitialAd(text, string.Empty, string.Empty);
				}
				GUI.enabled = this._adUnitToLoadedMapping[text];
				if (GUILayout.Button("Show", new GUILayoutOption[0]))
				{
					this._status = string.Empty;
					MoPubAndroid.ShowInterstitialAd(text);
				}
				GUI.enabled = true;
				GUILayout.EndHorizontal();
			}
		}
		else
		{
			GUILayout.Label("No interstitial AdUnits available", this._smallerFont, null);
		}
	}

	private void CreateRewardedVideosSection()
	{
		GUILayout.Space((float)this._sectionMarginSize);
		GUILayout.Label("Rewarded Videos", new GUILayoutOption[0]);
		if (!MoPubDemoGUI.IsAdUnitArrayNullOrEmpty(this._rewardedVideoAdUnits))
		{
			MoPubDemoGUI.CreateCustomDataField("rvCustomDataField", ref this._rvCustomData);
			foreach (string text in this._rewardedVideoAdUnits)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUI.enabled = !this._adUnitToLoadedMapping[text];
				if (GUILayout.Button(MoPubDemoGUI.CreateRequestButtonLabel(text), new GUILayoutOption[0]))
				{
					UnityEngine.Debug.Log("requesting rewarded video with AdUnit: " + text);
					this._status = "Requesting " + text;
					MoPubAndroid.RequestRewardedVideo(text, null, "rewarded, video, mopub", null, 37.7833, 122.4167, "customer101");
				}
				GUI.enabled = this._adUnitToLoadedMapping[text];
				if (GUILayout.Button("Show", new GUILayoutOption[0]))
				{
					this._status = string.Empty;
					MoPubAndroid.ShowRewardedVideo(text, MoPubDemoGUI.GetCustomData(this._rvCustomData));
				}
				GUI.enabled = true;
				GUILayout.EndHorizontal();
				if (MoPubAndroid.HasRewardedVideo(text) && this._adUnitToRewardsMapping.ContainsKey(text) && this._adUnitToRewardsMapping[text].Count > 1)
				{
					GUILayout.BeginVertical(new GUILayoutOption[0]);
					GUILayout.Space((float)this._sectionMarginSize);
					GUILayout.Label("Select a reward:", new GUILayoutOption[0]);
					foreach (MoPubBase.Reward selectedReward in this._adUnitToRewardsMapping[text])
					{
						if (GUILayout.Button(selectedReward.ToString(), new GUILayoutOption[0]))
						{
							MoPubAndroid.SelectReward(text, selectedReward);
						}
					}
					GUILayout.Space((float)this._sectionMarginSize);
					GUILayout.EndVertical();
				}
			}
		}
		else
		{
			GUILayout.Label("No rewarded video AdUnits available", this._smallerFont, null);
		}
	}

	private void CreateRewardedRichMediaSection()
	{
		GUILayout.Space((float)this._sectionMarginSize);
		GUILayout.Label("Rewarded Rich Media", new GUILayoutOption[0]);
		if (!MoPubDemoGUI.IsAdUnitArrayNullOrEmpty(this._rewardedRichMediaAdUnits))
		{
			MoPubDemoGUI.CreateCustomDataField("rrmCustomDataField", ref this._rrmCustomData);
			foreach (string text in this._rewardedRichMediaAdUnits)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUI.enabled = !this._adUnitToLoadedMapping[text];
				if (GUILayout.Button(MoPubDemoGUI.CreateRequestButtonLabel(text), new GUILayoutOption[0]))
				{
					UnityEngine.Debug.Log("requesting rewarded rich media with AdUnit: " + text);
					this._status = "Requesting " + text;
					MoPubAndroid.RequestRewardedVideo(text, null, "rewarded, video, mopub", null, 37.7833, 122.4167, "customer101");
				}
				GUI.enabled = this._adUnitToLoadedMapping[text];
				if (GUILayout.Button("Show", new GUILayoutOption[0]))
				{
					this._status = string.Empty;
					MoPubAndroid.ShowRewardedVideo(text, MoPubDemoGUI.GetCustomData(this._rrmCustomData));
				}
				GUI.enabled = true;
				GUILayout.EndHorizontal();
				if (MoPubAndroid.HasRewardedVideo(text) && this._adUnitToRewardsMapping.ContainsKey(text) && this._adUnitToRewardsMapping[text].Count > 1)
				{
					GUILayout.BeginVertical(new GUILayoutOption[0]);
					GUILayout.Space((float)this._sectionMarginSize);
					GUILayout.Label("Select a reward:", new GUILayoutOption[0]);
					foreach (MoPubBase.Reward selectedReward in this._adUnitToRewardsMapping[text])
					{
						if (GUILayout.Button(selectedReward.ToString(), new GUILayoutOption[0]))
						{
							MoPubAndroid.SelectReward(text, selectedReward);
						}
					}
					GUILayout.Space((float)this._sectionMarginSize);
					GUILayout.EndVertical();
				}
			}
		}
		else
		{
			GUILayout.Label("No rewarded rich media AdUnits available", this._smallerFont, null);
		}
	}

	private void CreateUserConsentSection()
	{
		GUILayout.Space((float)this._sectionMarginSize);
		GUILayout.Label("User Consent", new GUILayoutOption[0]);
		GUILayout.Label("Can collect personally identifiable information: " + this._canCollectPersonalInfo, this._smallerFont, new GUILayoutOption[0]);
		GUILayout.Label("Current consent status: " + this._currentConsentStatus, this._smallerFont, new GUILayoutOption[0]);
		GUILayout.Label("Should show consent dialog: " + this._shouldShowConsentDialog, this._smallerFont, new GUILayoutOption[0]);
		string str = "Is GDPR applicable: ";
		bool? isGdprApplicable = this._isGdprApplicable;
		GUILayout.Label(str + ((isGdprApplicable == null) ? "Unknown" : this._isGdprApplicable.ToString()), this._smallerFont, new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUI.enabled = !this.ConsentDialogLoaded;
		if (GUILayout.Button("Load Consent Dialog", new GUILayoutOption[0]))
		{
			this._status = "Loading consent dialog";
			MoPubAndroid.LoadConsentDialog();
		}
		GUI.enabled = this.ConsentDialogLoaded;
		if (GUILayout.Button("Show Consent Dialog", new GUILayoutOption[0]))
		{
			this._status = string.Empty;
			MoPubAndroid.ShowConsentDialog();
		}
		GUI.enabled = true;
		if (GUILayout.Button("Grant Consent", new GUILayoutOption[0]))
		{
			MoPubAndroid.PartnerApi.GrantConsent();
		}
		if (GUILayout.Button("Revoke Consent", new GUILayoutOption[0]))
		{
			MoPubAndroid.PartnerApi.RevokeConsent();
		}
		GUI.enabled = true;
		GUILayout.EndHorizontal();
	}

	private void CreateActionsSection()
	{
		GUILayout.Space((float)this._sectionMarginSize);
		GUILayout.Label("Actions", new GUILayoutOption[0]);
		if (GUILayout.Button("Report App Open", new GUILayoutOption[0]))
		{
			this._status = string.Empty;
			MoPubAndroid.ReportApplicationOpen(null);
		}
		if (!GUILayout.Button("Enable Location Support", new GUILayoutOption[0]))
		{
			return;
		}
		this._status = string.Empty;
		MoPubAndroid.EnableLocationSupport(true);
	}

	private static void CreateCustomDataField(string fieldName, ref string customDataValue)
	{
		GUI.SetNextControlName(fieldName);
		customDataValue = GUILayout.TextField(customDataValue, new GUILayoutOption[]
		{
			GUILayout.MinWidth(200f)
		});
		if (Event.current.type != EventType.Repaint)
		{
			return;
		}
		if (GUI.GetNameOfFocusedControl() == fieldName && customDataValue == MoPubDemoGUI._customDataDefaultText)
		{
			customDataValue = string.Empty;
		}
		else if (GUI.GetNameOfFocusedControl() != fieldName && string.IsNullOrEmpty(customDataValue))
		{
			customDataValue = MoPubDemoGUI._customDataDefaultText;
		}
	}

	private void CreateStatusSection()
	{
		GUILayout.Space(40f);
		GUILayout.Label(this._status, this._smallerFont, new GUILayoutOption[0]);
	}

	private static string GetCustomData(string customDataFieldValue)
	{
		return (!(customDataFieldValue != MoPubDemoGUI._customDataDefaultText)) ? null : customDataFieldValue;
	}

	private static string CreateRequestButtonLabel(string adUnit)
	{
		return (adUnit.Length <= 10) ? adUnit : ("Request " + adUnit.Substring(0, 10) + "...");
	}

	private readonly Dictionary<string, bool> _adUnitToLoadedMapping = new Dictionary<string, bool>();

	private readonly Dictionary<string, bool> _adUnitToShownMapping = new Dictionary<string, bool>();

	private readonly Dictionary<string, List<MoPubBase.Reward>> _adUnitToRewardsMapping = new Dictionary<string, List<MoPubBase.Reward>>();

	private bool _consentDialogLoaded;

	private readonly string[] _bannerAdUnits = new string[]
	{
		"b195f8dd8ded45fe847ad89ed1d016da"
	};

	private readonly string[] _interstitialAdUnits = new string[]
	{
		"24534e1901884e398f1253216226017e"
	};

	private readonly string[] _rewardedVideoAdUnits = new string[]
	{
		"920b6145fb1546cf8b5cf2ac34638bb7"
	};

	private readonly string[] _rewardedRichMediaAdUnits = new string[]
	{
		"15173ac6d3e54c9389b9a5ddca69b34b"
	};

	[SerializeField]
	private GUISkin _skin;

	private GUIStyle _smallerFont;

	private int _sectionMarginSize;

	private GUIStyle _centeredStyle;

	private static string _customDataDefaultText = "Optional custom data";

	private string _rvCustomData = MoPubDemoGUI._customDataDefaultText;

	private string _rrmCustomData = MoPubDemoGUI._customDataDefaultText;

	private bool _canCollectPersonalInfo;

	private MoPubBase.Consent.Status _currentConsentStatus;

	private bool _shouldShowConsentDialog;

	private bool? _isGdprApplicable = new bool?(false);

	private string _status = string.Empty;
}

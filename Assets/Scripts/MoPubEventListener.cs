// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoPubEventListener : MonoBehaviour
{
	private void Awake()
	{
		if (this._demoGUI == null)
		{
			this._demoGUI = base.GetComponent<MoPubDemoGUI>();
		}
		if (this._demoGUI == null)
		{
			UnityEngine.Debug.LogError("Missing reference to MoPubDemoGUI.  Please fix in the editor.");
			UnityEngine.Object.Destroy(this);
		}
	}

	private void OnEnable()
	{
		MoPubManager.OnSdkInitalizedEvent += this.OnSdkInitializedEvent;
		MoPubManager.OnConsentStatusChangedEvent += this.OnConsentStatusChangedEvent;
		MoPubManager.OnConsentDialogLoadedEvent += this.OnConsentDialogLoadedEvent;
		MoPubManager.OnConsentDialogFailedEvent += this.OnConsentDialogFailedEvent;
		MoPubManager.OnConsentDialogShownEvent += this.OnConsentDialogShownEvent;
		MoPubManager.OnAdLoadedEvent += this.OnAdLoadedEvent;
		MoPubManager.OnAdFailedEvent += this.OnAdFailedEvent;
		MoPubManager.OnAdClickedEvent += this.OnAdClickedEvent;
		MoPubManager.OnAdExpandedEvent += this.OnAdExpandedEvent;
		MoPubManager.OnAdCollapsedEvent += this.OnAdCollapsedEvent;
		MoPubManager.OnInterstitialLoadedEvent += this.OnInterstitialLoadedEvent;
		MoPubManager.OnInterstitialFailedEvent += this.OnInterstitialFailedEvent;
		MoPubManager.OnInterstitialShownEvent += this.OnInterstitialShownEvent;
		MoPubManager.OnInterstitialClickedEvent += this.OnInterstitialClickedEvent;
		MoPubManager.OnInterstitialDismissedEvent += this.OnInterstitialDismissedEvent;
		MoPubManager.OnInterstitialExpiredEvent += this.OnInterstitialExpiredEvent;
		MoPubManager.OnRewardedVideoLoadedEvent += this.OnRewardedVideoLoadedEvent;
		MoPubManager.OnRewardedVideoFailedEvent += this.OnRewardedVideoFailedEvent;
		MoPubManager.OnRewardedVideoExpiredEvent += this.OnRewardedVideoExpiredEvent;
		MoPubManager.OnRewardedVideoShownEvent += this.OnRewardedVideoShownEvent;
		MoPubManager.OnRewardedVideoClickedEvent += this.OnRewardedVideoClickedEvent;
		MoPubManager.OnRewardedVideoFailedToPlayEvent += this.OnRewardedVideoFailedToPlayEvent;
		MoPubManager.OnRewardedVideoReceivedRewardEvent += this.OnRewardedVideoReceivedRewardEvent;
		MoPubManager.OnRewardedVideoClosedEvent += this.OnRewardedVideoClosedEvent;
		MoPubManager.OnRewardedVideoLeavingApplicationEvent += this.OnRewardedVideoLeavingApplicationEvent;
	}

	private void OnDisable()
	{
		MoPubManager.OnSdkInitalizedEvent -= this.OnSdkInitializedEvent;
		MoPubManager.OnConsentStatusChangedEvent -= this.OnConsentStatusChangedEvent;
		MoPubManager.OnConsentDialogLoadedEvent -= this.OnConsentDialogLoadedEvent;
		MoPubManager.OnConsentDialogFailedEvent -= this.OnConsentDialogFailedEvent;
		MoPubManager.OnConsentDialogShownEvent -= this.OnConsentDialogShownEvent;
		MoPubManager.OnAdLoadedEvent -= this.OnAdLoadedEvent;
		MoPubManager.OnAdFailedEvent -= this.OnAdFailedEvent;
		MoPubManager.OnAdClickedEvent -= this.OnAdClickedEvent;
		MoPubManager.OnAdExpandedEvent -= this.OnAdExpandedEvent;
		MoPubManager.OnAdCollapsedEvent -= this.OnAdCollapsedEvent;
		MoPubManager.OnInterstitialLoadedEvent -= this.OnInterstitialLoadedEvent;
		MoPubManager.OnInterstitialFailedEvent -= this.OnInterstitialFailedEvent;
		MoPubManager.OnInterstitialShownEvent -= this.OnInterstitialShownEvent;
		MoPubManager.OnInterstitialClickedEvent -= this.OnInterstitialClickedEvent;
		MoPubManager.OnInterstitialDismissedEvent -= this.OnInterstitialDismissedEvent;
		MoPubManager.OnInterstitialExpiredEvent -= this.OnInterstitialExpiredEvent;
		MoPubManager.OnRewardedVideoLoadedEvent -= this.OnRewardedVideoLoadedEvent;
		MoPubManager.OnRewardedVideoFailedEvent -= this.OnRewardedVideoFailedEvent;
		MoPubManager.OnRewardedVideoExpiredEvent -= this.OnRewardedVideoExpiredEvent;
		MoPubManager.OnRewardedVideoShownEvent -= this.OnRewardedVideoShownEvent;
		MoPubManager.OnRewardedVideoClickedEvent -= this.OnRewardedVideoClickedEvent;
		MoPubManager.OnRewardedVideoFailedToPlayEvent -= this.OnRewardedVideoFailedToPlayEvent;
		MoPubManager.OnRewardedVideoReceivedRewardEvent -= this.OnRewardedVideoReceivedRewardEvent;
		MoPubManager.OnRewardedVideoClosedEvent -= this.OnRewardedVideoClosedEvent;
		MoPubManager.OnRewardedVideoLeavingApplicationEvent -= this.OnRewardedVideoLeavingApplicationEvent;
	}

	private void AdFailed(string adUnitId, string action, string error)
	{
		string text = "Failed to " + action + " ad unit " + adUnitId;
		if (!string.IsNullOrEmpty(error))
		{
			text = text + ": " + error;
		}
		UnityEngine.Debug.LogError(text);
		this._demoGUI.AdFailed(text);
	}

	private void OnSdkInitializedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnSdkInitializedEvent: " + adUnitId);
		this._demoGUI.SdkInitialized();
	}

	private void OnConsentStatusChangedEvent(MoPubBase.Consent.Status oldStatus, MoPubBase.Consent.Status newStatus, bool canCollectPersonalInfo)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"OnConsetStatusChangedEvent: old=",
			oldStatus,
			" new=",
			newStatus,
			" personalInfoOk=",
			canCollectPersonalInfo
		}));
		this._demoGUI.ConsentStatusChanged(newStatus, canCollectPersonalInfo);
	}

	private void OnConsentDialogLoadedEvent()
	{
		UnityEngine.Debug.Log("OnConsentDialogLoadedEvent: dialog loaded");
		this._demoGUI.ConsentDialogLoaded = true;
	}

	private void OnConsentDialogFailedEvent(string err)
	{
		UnityEngine.Debug.Log("OnConsentDialogFailedEvent: error = " + err);
	}

	private void OnConsentDialogShownEvent()
	{
		UnityEngine.Debug.Log("OnConsentDialogShownEvent: dialog shown");
		this._demoGUI.ConsentDialogLoaded = false;
	}

	private void OnAdLoadedEvent(string adUnitId, float height)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"OnAdLoadedEvent: ",
			adUnitId,
			" height: ",
			height
		}));
		this._demoGUI.BannerLoaded(adUnitId, height);
	}

	private void OnAdFailedEvent(string adUnitId, string error)
	{
		this.AdFailed(adUnitId, "load banner", error);
	}

	private void OnAdClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnAdClickedEvent: " + adUnitId);
	}

	private void OnAdExpandedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnAdExpandedEvent: " + adUnitId);
	}

	private void OnAdCollapsedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnAdCollapsedEvent: " + adUnitId);
	}

	private void OnInterstitialLoadedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialLoadedEvent: " + adUnitId);
		this._demoGUI.AdLoaded(adUnitId);
	}

	private void OnInterstitialFailedEvent(string adUnitId, string error)
	{
		this.AdFailed(adUnitId, "load interstitial", error);
	}

	private void OnInterstitialShownEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialShownEvent: " + adUnitId);
	}

	private void OnInterstitialClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialClickedEvent: " + adUnitId);
	}

	private void OnInterstitialDismissedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialDismissedEvent: " + adUnitId);
		this._demoGUI.AdDismissed(adUnitId);
	}

	private void OnInterstitialExpiredEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialExpiredEvent: " + adUnitId);
	}

	private void OnRewardedVideoLoadedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoLoadedEvent: " + adUnitId);
		List<MoPubBase.Reward> availableRewards = MoPubAndroid.GetAvailableRewards(adUnitId);
		this._demoGUI.AdLoaded(adUnitId);
		this._demoGUI.LoadAvailableRewards(adUnitId, availableRewards);
	}

	private void OnRewardedVideoFailedEvent(string adUnitId, string error)
	{
		this.AdFailed(adUnitId, "load rewarded video", error);
	}

	private void OnRewardedVideoExpiredEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoExpiredEvent: " + adUnitId);
	}

	private void OnRewardedVideoShownEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoShownEvent: " + adUnitId);
	}

	private void OnRewardedVideoClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoClickedEvent: " + adUnitId);
	}

	private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
	{
		this.AdFailed(adUnitId, "play rewarded video", error);
	}

	private void OnRewardedVideoReceivedRewardEvent(string adUnitId, string label, float amount)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"OnRewardedVideoReceivedRewardEvent for ad unit id ",
			adUnitId,
			" currency:",
			label,
			" amount:",
			amount
		}));
	}

	private void OnRewardedVideoClosedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoClosedEvent: " + adUnitId);
		this._demoGUI.AdDismissed(adUnitId);
	}

	private void OnRewardedVideoLeavingApplicationEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoLeavingApplicationEvent: " + adUnitId);
	}

	[SerializeField]
	private MoPubDemoGUI _demoGUI;
}

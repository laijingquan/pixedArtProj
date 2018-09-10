// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MoPubInternal.ThirdParty.MiniJSON;
using UnityEngine;

public class MoPubManager : MonoBehaviour
{
	public static MoPubManager Instance { get; private set; }

	 
	public static event Action<string> OnSdkInitalizedEvent;

	 
	public static event Action<string, float> OnAdLoadedEvent;

	 
	public static event Action<string, string> OnAdFailedEvent;

	 
	public static event Action<string> OnAdClickedEvent;

	 
	public static event Action<string> OnAdExpandedEvent;

	 
	public static event Action<string> OnAdCollapsedEvent;

	 
	public static event Action<string> OnInterstitialLoadedEvent;

	 
	public static event Action<string, string> OnInterstitialFailedEvent;

	 
	public static event Action<string> OnInterstitialDismissedEvent;

	 
	public static event Action<string> OnInterstitialExpiredEvent;

	 
	public static event Action<string> OnInterstitialShownEvent;

	 
	public static event Action<string> OnInterstitialClickedEvent;

	 
	public static event Action<string> OnRewardedVideoLoadedEvent;

	 
	public static event Action<string, string> OnRewardedVideoFailedEvent;

	 
	public static event Action<string> OnRewardedVideoExpiredEvent;

	 
	public static event Action<string> OnRewardedVideoShownEvent;

	 
	public static event Action<string> OnRewardedVideoClickedEvent;

	 
	public static event Action<string, string> OnRewardedVideoFailedToPlayEvent;

	 
	public static event Action<string, string, float> OnRewardedVideoReceivedRewardEvent;

	 
	public static event Action<string> OnRewardedVideoClosedEvent;

	 
	public static event Action<string> OnRewardedVideoLeavingApplicationEvent;

	 
	public static event Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> OnConsentStatusChangedEvent;

	 
	public static event Action OnConsentDialogLoadedEvent;

	 
	public static event Action<string> OnConsentDialogFailedEvent;

	 
	public static event Action OnConsentDialogShownEvent;

	private void Awake()
	{
		if (MoPubManager.Instance == null)
		{
			MoPubManager.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void OnDestroy()
	{
		if (MoPubManager.Instance == this)
		{
			MoPubManager.Instance = null;
		}
	}

	private string[] DecodeArgs(string argsJson, int min)
	{
		bool flag = false;
		List<object> list = Json.Deserialize(argsJson) as List<object>;
		if (list == null)
		{
			UnityEngine.Debug.LogError("Invalid JSON data: " + argsJson);
			list = new List<object>();
			flag = true;
		}
		if (list.Count < min)
		{
			if (!flag)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Missing one or more values: ",
					argsJson,
					" (expected ",
					min,
					")"
				}));
			}
			while (list.Count < min)
			{
				list.Add(string.Empty);
			}
		}
		return (from v in list
		select v.ToString()).ToArray<string>();
	}

	public void EmitSdkInitializedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onSdkInitalizedEvent = MoPubManager.OnSdkInitalizedEvent;
		if (onSdkInitalizedEvent != null)
		{
			onSdkInitalizedEvent(obj);
		}
	}

	public void EmitConsentStatusChangedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 3);
		MoPubBase.Consent.Status arg = MoPubBase.Consent.FromString(array[0]);
		MoPubBase.Consent.Status arg2 = MoPubBase.Consent.FromString(array[1]);
		bool arg3 = array[2].ToLower() == "true";
		Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool> onConsentStatusChangedEvent = MoPubManager.OnConsentStatusChangedEvent;
		if (onConsentStatusChangedEvent != null)
		{
			onConsentStatusChangedEvent(arg, arg2, arg3);
		}
	}

	public void EmitConsentDialogLoadedEvent()
	{
		Action onConsentDialogLoadedEvent = MoPubManager.OnConsentDialogLoadedEvent;
		if (onConsentDialogLoadedEvent != null)
		{
			onConsentDialogLoadedEvent();
		}
	}

	public void EmitConsentDialogFailedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onConsentDialogFailedEvent = MoPubManager.OnConsentDialogFailedEvent;
		if (onConsentDialogFailedEvent != null)
		{
			onConsentDialogFailedEvent(obj);
		}
	}

	public void EmitConsentDialogShownEvent()
	{
		Action onConsentDialogShownEvent = MoPubManager.OnConsentDialogShownEvent;
		if (onConsentDialogShownEvent != null)
		{
			onConsentDialogShownEvent();
		}
	}

	public void EmitAdLoadedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 2);
		string arg = array[0];
		string s = array[1];
		Action<string, float> onAdLoadedEvent = MoPubManager.OnAdLoadedEvent;
		if (onAdLoadedEvent != null)
		{
			onAdLoadedEvent(arg, float.Parse(s));
		}
	}

	public void EmitAdFailedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 2);
		string arg = array[0];
		string arg2 = array[1];
		Action<string, string> onAdFailedEvent = MoPubManager.OnAdFailedEvent;
		if (onAdFailedEvent != null)
		{
			onAdFailedEvent(arg, arg2);
		}
	}

	public void EmitAdClickedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onAdClickedEvent = MoPubManager.OnAdClickedEvent;
		if (onAdClickedEvent != null)
		{
			onAdClickedEvent(obj);
		}
	}

	public void EmitAdExpandedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onAdExpandedEvent = MoPubManager.OnAdExpandedEvent;
		if (onAdExpandedEvent != null)
		{
			onAdExpandedEvent(obj);
		}
	}

	public void EmitAdCollapsedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onAdCollapsedEvent = MoPubManager.OnAdCollapsedEvent;
		if (onAdCollapsedEvent != null)
		{
			onAdCollapsedEvent(obj);
		}
	}

	public void EmitInterstitialLoadedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onInterstitialLoadedEvent = MoPubManager.OnInterstitialLoadedEvent;
		if (onInterstitialLoadedEvent != null)
		{
			onInterstitialLoadedEvent(obj);
		}
	}

	public void EmitInterstitialFailedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 2);
		string arg = array[0];
		string arg2 = array[1];
		Action<string, string> onInterstitialFailedEvent = MoPubManager.OnInterstitialFailedEvent;
		if (onInterstitialFailedEvent != null)
		{
			onInterstitialFailedEvent(arg, arg2);
		}
	}

	public void EmitInterstitialDismissedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onInterstitialDismissedEvent = MoPubManager.OnInterstitialDismissedEvent;
		if (onInterstitialDismissedEvent != null)
		{
			onInterstitialDismissedEvent(obj);
		}
	}

	public void EmitInterstitialDidExpireEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onInterstitialExpiredEvent = MoPubManager.OnInterstitialExpiredEvent;
		if (onInterstitialExpiredEvent != null)
		{
			onInterstitialExpiredEvent(obj);
		}
	}

	public void EmitInterstitialShownEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onInterstitialShownEvent = MoPubManager.OnInterstitialShownEvent;
		if (onInterstitialShownEvent != null)
		{
			onInterstitialShownEvent(obj);
		}
	}

	public void EmitInterstitialClickedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onInterstitialClickedEvent = MoPubManager.OnInterstitialClickedEvent;
		if (onInterstitialClickedEvent != null)
		{
			onInterstitialClickedEvent(obj);
		}
	}

	public void EmitRewardedVideoLoadedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onRewardedVideoLoadedEvent = MoPubManager.OnRewardedVideoLoadedEvent;
		if (onRewardedVideoLoadedEvent != null)
		{
			onRewardedVideoLoadedEvent(obj);
		}
	}

	public void EmitRewardedVideoFailedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 2);
		string arg = array[0];
		string arg2 = array[1];
		Action<string, string> onRewardedVideoFailedEvent = MoPubManager.OnRewardedVideoFailedEvent;
		if (onRewardedVideoFailedEvent != null)
		{
			onRewardedVideoFailedEvent(arg, arg2);
		}
	}

	public void EmitRewardedVideoExpiredEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onRewardedVideoExpiredEvent = MoPubManager.OnRewardedVideoExpiredEvent;
		if (onRewardedVideoExpiredEvent != null)
		{
			onRewardedVideoExpiredEvent(obj);
		}
	}

	public void EmitRewardedVideoShownEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onRewardedVideoShownEvent = MoPubManager.OnRewardedVideoShownEvent;
		if (onRewardedVideoShownEvent != null)
		{
			onRewardedVideoShownEvent(obj);
		}
	}

	public void EmitRewardedVideoClickedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onRewardedVideoClickedEvent = MoPubManager.OnRewardedVideoClickedEvent;
		if (onRewardedVideoClickedEvent != null)
		{
			onRewardedVideoClickedEvent(obj);
		}
	}

	public void EmitRewardedVideoFailedToPlayEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 2);
		string arg = array[0];
		string arg2 = array[1];
		Action<string, string> onRewardedVideoFailedToPlayEvent = MoPubManager.OnRewardedVideoFailedToPlayEvent;
		if (onRewardedVideoFailedToPlayEvent != null)
		{
			onRewardedVideoFailedToPlayEvent(arg, arg2);
		}
	}

	public void EmitRewardedVideoReceivedRewardEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 3);
		string arg = array[0];
		string arg2 = array[1];
		string s = array[2];
		Action<string, string, float> onRewardedVideoReceivedRewardEvent = MoPubManager.OnRewardedVideoReceivedRewardEvent;
		if (onRewardedVideoReceivedRewardEvent != null)
		{
			onRewardedVideoReceivedRewardEvent(arg, arg2, float.Parse(s));
		}
	}

	public void EmitRewardedVideoClosedEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onRewardedVideoClosedEvent = MoPubManager.OnRewardedVideoClosedEvent;
		if (onRewardedVideoClosedEvent != null)
		{
			onRewardedVideoClosedEvent(obj);
		}
	}

	public void EmitRewardedVideoLeavingApplicationEvent(string argsJson)
	{
		string[] array = this.DecodeArgs(argsJson, 1);
		string obj = array[0];
		Action<string> onRewardedVideoLeavingApplicationEvent = MoPubManager.OnRewardedVideoLeavingApplicationEvent;
		if (onRewardedVideoLeavingApplicationEvent != null)
		{
			onRewardedVideoLeavingApplicationEvent(obj);
		}
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using com.adjust.sdk;
using UnityEngine;

public class ExampleGUI : MonoBehaviour
{
	private void OnGUI()
	{
		if (this.showPopUp)
		{
			GUI.Window(0, new Rect((float)(Screen.width / 2 - 150), (float)(Screen.height / 2 - 65), 300f, 130f), new GUI.WindowFunction(this.showGUI), "Is SDK enabled?");
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 0 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), this.txtManualLaunch) && !string.Equals(this.txtManualLaunch, "SDK Launched", StringComparison.OrdinalIgnoreCase))
		{
			AdjustConfig adjustConfig = new AdjustConfig("2fm9gkqubvpc", AdjustEnvironment.Sandbox);
			adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
			adjustConfig.setLogDelegate(delegate(string msg)
			{
				UnityEngine.Debug.Log(msg);
			});
			adjustConfig.setSendInBackground(true);
			adjustConfig.setLaunchDeferredDeeplink(true);
			adjustConfig.setEventSuccessDelegate(new Action<AdjustEventSuccess>(this.EventSuccessCallback), "Adjust");
			adjustConfig.setEventFailureDelegate(new Action<AdjustEventFailure>(this.EventFailureCallback), "Adjust");
			adjustConfig.setSessionSuccessDelegate(new Action<AdjustSessionSuccess>(this.SessionSuccessCallback), "Adjust");
			adjustConfig.setSessionFailureDelegate(new Action<AdjustSessionFailure>(this.SessionFailureCallback), "Adjust");
			adjustConfig.setDeferredDeeplinkDelegate(new Action<string>(this.DeferredDeeplinkCallback), "Adjust");
			adjustConfig.setAttributionChangedDelegate(new Action<AdjustAttribution>(this.AttributionChangedCallback), "Adjust");
			Adjust.start(adjustConfig);
			this.isEnabled = true;
			this.txtManualLaunch = "SDK Launched";
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Simple Event"))
		{
			AdjustEvent adjustEvent = new AdjustEvent("g3mfiw");
			Adjust.trackEvent(adjustEvent);
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 2 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Revenue Event"))
		{
			AdjustEvent adjustEvent2 = new AdjustEvent("a4fd35");
			adjustEvent2.setRevenue(0.25, "EUR");
			Adjust.trackEvent(adjustEvent2);
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 3 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Callback Event"))
		{
			AdjustEvent adjustEvent3 = new AdjustEvent("34vgg9");
			adjustEvent3.addCallbackParameter("key", "value");
			adjustEvent3.addCallbackParameter("foo", "bar");
			Adjust.trackEvent(adjustEvent3);
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 4 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Partner Event"))
		{
			AdjustEvent adjustEvent4 = new AdjustEvent("w788qs");
			adjustEvent4.addPartnerParameter("key", "value");
			adjustEvent4.addPartnerParameter("foo", "bar");
			Adjust.trackEvent(adjustEvent4);
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 5 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), this.txtSetOfflineMode))
		{
			if (string.Equals(this.txtSetOfflineMode, "Turn Offline Mode ON", StringComparison.OrdinalIgnoreCase))
			{
				Adjust.setOfflineMode(true);
				this.txtSetOfflineMode = "Turn Offline Mode OFF";
			}
			else
			{
				Adjust.setOfflineMode(false);
				this.txtSetOfflineMode = "Turn Offline Mode ON";
			}
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 6 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), this.txtSetEnabled))
		{
			if (string.Equals(this.txtSetEnabled, "Disable SDK", StringComparison.OrdinalIgnoreCase))
			{
				Adjust.setEnabled(false);
				this.txtSetEnabled = "Enable SDK";
			}
			else
			{
				Adjust.setEnabled(true);
				this.txtSetEnabled = "Disable SDK";
			}
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 7 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Is SDK Enabled?"))
		{
			this.isEnabled = Adjust.isEnabled();
			this.showPopUp = true;
		}
	}

	private void showGUI(int windowID)
	{
		if (this.isEnabled)
		{
			GUI.Label(new Rect(65f, 40f, 200f, 30f), "Adjust SDK is ENABLED!");
		}
		else
		{
			GUI.Label(new Rect(65f, 40f, 200f, 30f), "Adjust SDK is DISABLED!");
		}
		if (GUI.Button(new Rect(90f, 75f, 120f, 40f), "OK"))
		{
			this.showPopUp = false;
		}
	}

	public void handleGooglePlayId(string adId)
	{
		UnityEngine.Debug.Log("Google Play Ad ID = " + adId);
	}

	public void AttributionChangedCallback(AdjustAttribution attributionData)
	{
		UnityEngine.Debug.Log("Attribution changed!");
		if (attributionData.trackerName != null)
		{
			UnityEngine.Debug.Log("Tracker name: " + attributionData.trackerName);
		}
		if (attributionData.trackerToken != null)
		{
			UnityEngine.Debug.Log("Tracker token: " + attributionData.trackerToken);
		}
		if (attributionData.network != null)
		{
			UnityEngine.Debug.Log("Network: " + attributionData.network);
		}
		if (attributionData.campaign != null)
		{
			UnityEngine.Debug.Log("Campaign: " + attributionData.campaign);
		}
		if (attributionData.adgroup != null)
		{
			UnityEngine.Debug.Log("Adgroup: " + attributionData.adgroup);
		}
		if (attributionData.creative != null)
		{
			UnityEngine.Debug.Log("Creative: " + attributionData.creative);
		}
		if (attributionData.clickLabel != null)
		{
			UnityEngine.Debug.Log("Click label: " + attributionData.clickLabel);
		}
		if (attributionData.adid != null)
		{
			UnityEngine.Debug.Log("ADID: " + attributionData.adid);
		}
	}

	public void EventSuccessCallback(AdjustEventSuccess eventSuccessData)
	{
		UnityEngine.Debug.Log("Event tracked successfully!");
		if (eventSuccessData.Message != null)
		{
			UnityEngine.Debug.Log("Message: " + eventSuccessData.Message);
		}
		if (eventSuccessData.Timestamp != null)
		{
			UnityEngine.Debug.Log("Timestamp: " + eventSuccessData.Timestamp);
		}
		if (eventSuccessData.Adid != null)
		{
			UnityEngine.Debug.Log("Adid: " + eventSuccessData.Adid);
		}
		if (eventSuccessData.EventToken != null)
		{
			UnityEngine.Debug.Log("EventToken: " + eventSuccessData.EventToken);
		}
		if (eventSuccessData.JsonResponse != null)
		{
			UnityEngine.Debug.Log("JsonResponse: " + eventSuccessData.GetJsonResponse());
		}
	}

	public void EventFailureCallback(AdjustEventFailure eventFailureData)
	{
		UnityEngine.Debug.Log("Event tracking failed!");
		if (eventFailureData.Message != null)
		{
			UnityEngine.Debug.Log("Message: " + eventFailureData.Message);
		}
		if (eventFailureData.Timestamp != null)
		{
			UnityEngine.Debug.Log("Timestamp: " + eventFailureData.Timestamp);
		}
		if (eventFailureData.Adid != null)
		{
			UnityEngine.Debug.Log("Adid: " + eventFailureData.Adid);
		}
		if (eventFailureData.EventToken != null)
		{
			UnityEngine.Debug.Log("EventToken: " + eventFailureData.EventToken);
		}
		UnityEngine.Debug.Log("WillRetry: " + eventFailureData.WillRetry.ToString());
		if (eventFailureData.JsonResponse != null)
		{
			UnityEngine.Debug.Log("JsonResponse: " + eventFailureData.GetJsonResponse());
		}
	}

	public void SessionSuccessCallback(AdjustSessionSuccess sessionSuccessData)
	{
		UnityEngine.Debug.Log("Session tracked successfully!");
		if (sessionSuccessData.Message != null)
		{
			UnityEngine.Debug.Log("Message: " + sessionSuccessData.Message);
		}
		if (sessionSuccessData.Timestamp != null)
		{
			UnityEngine.Debug.Log("Timestamp: " + sessionSuccessData.Timestamp);
		}
		if (sessionSuccessData.Adid != null)
		{
			UnityEngine.Debug.Log("Adid: " + sessionSuccessData.Adid);
		}
		if (sessionSuccessData.JsonResponse != null)
		{
			UnityEngine.Debug.Log("JsonResponse: " + sessionSuccessData.GetJsonResponse());
		}
	}

	public void SessionFailureCallback(AdjustSessionFailure sessionFailureData)
	{
		UnityEngine.Debug.Log("Session tracking failed!");
		if (sessionFailureData.Message != null)
		{
			UnityEngine.Debug.Log("Message: " + sessionFailureData.Message);
		}
		if (sessionFailureData.Timestamp != null)
		{
			UnityEngine.Debug.Log("Timestamp: " + sessionFailureData.Timestamp);
		}
		if (sessionFailureData.Adid != null)
		{
			UnityEngine.Debug.Log("Adid: " + sessionFailureData.Adid);
		}
		UnityEngine.Debug.Log("WillRetry: " + sessionFailureData.WillRetry.ToString());
		if (sessionFailureData.JsonResponse != null)
		{
			UnityEngine.Debug.Log("JsonResponse: " + sessionFailureData.GetJsonResponse());
		}
	}

	private void DeferredDeeplinkCallback(string deeplinkURL)
	{
		UnityEngine.Debug.Log("Deferred deeplink reported!");
		if (deeplinkURL != null)
		{
			UnityEngine.Debug.Log("Deeplink URL: " + deeplinkURL);
		}
		else
		{
			UnityEngine.Debug.Log("Deeplink URL is null!");
		}
	}

	private int numberOfButtons = 8;

	private bool isEnabled;

	private bool showPopUp;

	private string txtSetEnabled = "Disable SDK";

	private string txtManualLaunch = "Manual Launch";

	private string txtSetOfflineMode = "Turn Offline Mode ON";
}

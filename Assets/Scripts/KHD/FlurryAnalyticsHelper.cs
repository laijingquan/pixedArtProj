// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

namespace KHD
{
	public class FlurryAnalyticsHelper : MonoBehaviour
	{
		private void Awake()
		{
			SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.SetDebugLogEnabled(this._enableDebugLog);
			SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.replicateDataToUnityAnalytics = this._replicateDataToUnityAnalytics;
			SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.StartSession(this._iOSApiKey, this._androidApiKey, this._sendCrashReports);
		}

		[SerializeField]
		private string _iOSApiKey;

		[SerializeField]
		private string _androidApiKey;

		[SerializeField]
		private bool _enableDebugLog;

		[SerializeField]
		private bool _sendCrashReports = true;

		[SerializeField]
		private bool _replicateDataToUnityAnalytics;

		[HideInInspector]
		[Space(10f)]
		[SerializeField]
		private bool _iOSIAPReportingEnabled;
	}
}

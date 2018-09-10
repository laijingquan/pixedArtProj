// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TGFReqParam
{
	public TGFReqParam()
	{
		this.AdvetrisingId = string.Empty;
		this.AltId = SystemInfo.deviceUniqueIdentifier;
		this.Locale = string.Empty;
		this.AdsTrackingLimit = "1";
		this.AppVer = Application.version;
		this.Bundle = SystemUtils.GetAppPackage();
	}

	public WWWForm ToForm()
	{
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("advertising_id", this.AdvetrisingId);
		wwwform.AddField("alt_id", this.AltId);
		wwwform.AddField("isLimitAdTrackingEnabled", this.AdsTrackingLimit);
		wwwform.AddField("locale", this.Locale);
		wwwform.AddField("app_ver", this.AppVer);
		wwwform.AddField("package_name", this.Bundle);
		return wwwform;
	}

	public const string advtIdKey = "advertising_id";

	public const string altIdIdKey = "alt_id";

	private const string localeKey = "locale";

	private const string adsTrackLimitKey = "isLimitAdTrackingEnabled";

	private const string appVerKey = "app_ver";

	private const string bundleKey = "package_name";

	public string AdvetrisingId;

	public string AltId;

	public string Locale;

	public string AdsTrackingLimit;

	public string AppVer;

	public string Bundle;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class SystemUtils
{
	public static void OpenAppRate(bool neveShowAgain = true)
	{
		if (neveShowAgain)
		{
			GeneralSettings.MarkRateAsClicked();
		}
		Application.OpenURL("market://details?id=" + Application.identifier);
	}

	public static void OpenFbPage(string scheme, string fallbackUrl)
	{
		if (scheme == null)
		{
			scheme = string.Empty;
		}
		if (fallbackUrl == null)
		{
			fallbackUrl = "http://facebook.com/bestcolorbynumber";
		}
		//FMNativeUtils.Instance.OpenUrl(scheme, fallbackUrl);
	}

	public static void OpenBrowser(string url)
	{
		Application.OpenURL(url);
	}

	public static void OpenUrl(string scheme, string fallbackUrl)
	{
		if (scheme == null && fallbackUrl == null)
		{
			return;
		}
		//FMNativeUtils.Instance.OpenUrl(scheme, fallbackUrl);
	}

	public static void SupportEmail()
	{
		SystemUtils.SendEmailFeedback();
	}

	public static void OpenNativeShareDialog()
	{
		//FMNativeUtils.Instance.OpenShareDialog(SystemUtils.appLink, string.Empty);
	}

	private static void SendEmailFeedback()
	{
		string text = string.Empty;
		string newValue = string.Empty;
		text = "Android";
		newValue = LocalizationService.Instance.GetTextByKey("appname");
		string text2 = LocalizationService.Instance.GetTextByKey("contactUs_body").Replace("%AppName%", newValue).Replace("%Platfom%", text).Replace("%AppVersion%", Application.version).Replace("\\n", "<br>");
		string text3 = text2;
		text2 = string.Concat(new string[]
		{
			text3,
			"id:",
			TGFModule.Instance.DeviceId,
			"<br>aId:",
			TGFModule.Instance.DeviceAltId
		});
		//UM_ShareUtility.SendMail(LocalizationService.Instance.GetTextByKey("appname") + ", " + text, text2, "support@x-flow.app");
	}

	public static string GetAppVersion()
	{
		return Application.version;
	}

	public static string GetAppPackage()
	{
		return Application.identifier;
	}

	public static void OpenSudokuPage()
	{
		Application.OpenURL("market://details?id=com.sudoku.brain.logic.puzzles.games");
	}

	public static void OpenSolitairePage()
	{
		Application.OpenURL("market://details?id=net.playwithworld.cards.solitaire.android");
	}

	public static string GetLanguage()
	{
		List<string> list = new List<string>
		{
			"English",
			"French",
			"ChineseTraditional",
			"ChineseSimplified",
			"German",
			"Hindi",
			"Italian",
			"Japanese",
			"Korean",
			"Portuguese",
			"Russian",
			"Thai",
			"Spanish",
			"Swedish",
			"Turkish",
			"Vietnamese"
		};
		string text = string.Empty;
		if (string.IsNullOrEmpty(text))
		{
			if (Application.systemLanguage != SystemLanguage.Unknown)
			{
				text = Application.systemLanguage.ToString();
			}
			else
			{
				string languageExtended = SystemUtils.GetLanguageExtended();
				if (languageExtended.StartsWith("hi", StringComparison.OrdinalIgnoreCase))
				{
					text = "Hindi";
				}
				else
				{
					text = "English";
				}
			}
		}
		if (!list.Contains(text))
		{
			text = "English";
		}
		return text;
	}

	public static string GetLanguageExtended()
	{
		string result;
		try
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.util.Locale");
			AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getDefault", new object[0]);
			string text = androidJavaObject.Call<string>("getLanguage", new object[0]);
			result = text;
		}
		catch (Exception)
		{
			result = string.Empty;
		}
		return result;
	}

	public static SystemUtils.IconQuality GetIconQuality()
	{
		return SystemUtils.IconQuality.Low;
	}

	public static SystemUtils.DevicePerfomance GetDevicePerfomance()
	{
		SystemUtils.DevicePerfomance devicePerfomance = GeneralSettings.DevicePerfomance;
		if (devicePerfomance == SystemUtils.DevicePerfomance.Unknown)
		{
			try
			{
				int num = 2013;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("fmt.deviceperf.DevicePower"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
					{
						AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
						int num2 = androidJavaClass.CallStatic<int>("GetDeviceYear", new object[]
						{
							@static
						});
						if (num2 >= num)
						{
							devicePerfomance = SystemUtils.DevicePerfomance.High;
						}
						else
						{
							devicePerfomance = SystemUtils.DevicePerfomance.Low;
						}
						FMLogger.Log(string.Concat(new object[]
						{
							"detect ",
							devicePerfomance,
							" y: ",
							num2
						}));
					}
				}
			}
			catch (Exception ex)
			{
				FMLogger.Log("device detect ex. fallback low");
				devicePerfomance = SystemUtils.DevicePerfomance.Low;
                Debug.Log(ex.ToString());
			}
			GeneralSettings.DevicePerfomance = devicePerfomance;
		}
		return devicePerfomance;
	}

	public static void ShowUpdateDialog(UpdateDialog dialogInfo)
	{
		//AndroidDialog androidDialog = AndroidDialog.Create(dialogInfo.title, dialogInfo.body, dialogInfo.yes, dialogInfo.no, AndroidDialogTheme.ThemeDeviceDefaultLight);
		//androidDialog.ActionComplete += delegate(AndroidDialogResult result)
		//{
		//	if (result == AndroidDialogResult.YES)
		//	{
		//		Application.OpenURL("market://details?id=" + Application.bundleIdentifier);
		//	}
		//};
	}

	public static void ShowBonusErrorDialog()
	{
		string title = "Uh-oh!";
		string message = "That was an invalid link. Find info about upcoming events on our Facebook page";
		//AndroidDialog.Create(title, message);
	}

	public const string TERMS_URL = "https://x-flow.app/terms-of-use.html";

	public const string PRIVACY_URL = "https://x-flow.app/privacy-policy.html";

	private const string supportEmail = "support@x-flow.app";

	private const string FB_URL = "http://facebook.com/bestcolorbynumber";

	private static readonly string appLink = "https://play.google.com/store/apps/details?id=" + Application.identifier;

	public enum IconQuality
	{
		Low,
		Medium,
		High
	}

	public enum DevicePerfomance
	{
		Low,
		Medium,
		High,
		Unknown = -1
	}
}

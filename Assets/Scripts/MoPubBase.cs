// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Linq;
using MoPubInternal.ThirdParty.MiniJSON;
using UnityEngine;

public class MoPubBase
{
	protected MoPubBase()
	{
	}

	public static string ConsentLanguageCode { get; set; }

	public static string PluginName
	{
		get
		{
			string result;
			if ((result = MoPubBase._pluginName) == null)
			{
				result = (MoPubBase._pluginName = "MoPub Unity Plugin v5.0.0");
			}
			return result;
		}
	}

	protected static void ValidateAdUnitForSdkInit(string adUnitId)
	{
		if (string.IsNullOrEmpty(adUnitId))
		{
			UnityEngine.Debug.LogError("A valid ad unit ID is needed to initialize the MoPub SDK.");
		}
	}

	protected static void ReportAdUnitNotFound(string adUnitId)
	{
		UnityEngine.Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
	}

	protected static Uri UrlFromString(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			return null;
		}
		Uri result;
		try
		{
			result = new Uri(url);
		}
		catch
		{
			UnityEngine.Debug.LogError("Invalid URL: " + url);
			result = null;
		}
		return result;
	}

	protected static void InitManager()
	{
		Type typeFromHandle = typeof(MoPubManager);
		MoPubManager component = new GameObject("MoPubManager", new Type[]
		{
			typeFromHandle
		}).GetComponent<MoPubManager>();
		if (MoPubManager.Instance != component)
		{
			UnityEngine.Debug.LogWarning("It looks like you have the " + typeFromHandle.Name + " on a GameObject in your scene. Please remove the script from your scene.");
		}
	}

	private const string PluginVersion = "5.0.0";

	public const double LatLongSentinel = 99999.0;

	private static string _pluginName;

	public enum AdPosition
	{
		TopLeft,
		TopCenter,
		TopRight,
		Centered,
		BottomLeft,
		BottomCenter,
		BottomRight
	}

	public static class Consent
	{
		public static MoPubBase.Consent.Status FromString(string status)
		{
			if (status != null)
			{
				if (status == "explicit_yes")
				{
					return MoPubBase.Consent.Status.Consented;
				}
				if (status == "explicit_no")
				{
					return MoPubBase.Consent.Status.Denied;
				}
				if (status == "dnt")
				{
					return MoPubBase.Consent.Status.DoNotTrack;
				}
				if (status == "potential_whitelist")
				{
					return MoPubBase.Consent.Status.PotentialWhitelist;
				}
				if (status == "unknown")
				{
					return MoPubBase.Consent.Status.Unknown;
				}
			}
			MoPubBase.Consent.Status result;
			try
			{
				result = (MoPubBase.Consent.Status)Enum.Parse(typeof(MoPubBase.Consent.Status), status);
			}
			catch
			{
				UnityEngine.Debug.LogError("Unknown consent status string: " + status);
				result = MoPubBase.Consent.Status.Unknown;
			}
			return result;
		}

		public enum Status
		{
			Unknown,
			Denied,
			DoNotTrack,
			PotentialWhitelist,
			Consented
		}

		private static class Strings
		{
			public const string ExplicitYes = "explicit_yes";

			public const string ExplicitNo = "explicit_no";

			public const string Unknown = "unknown";

			public const string PotentialWhitelist = "potential_whitelist";

			public const string Dnt = "dnt";
		}
	}

	public enum BannerType
	{
		Size320x50,
		Size300x250,
		Size728x90,
		Size160x600
	}

	public enum LogLevel
	{
		MPLogLevelAll,
		MPLogLevelTrace = 10,
		MPLogLevelDebug = 20,
		MPLogLevelInfo = 30,
		MPLogLevelWarn = 40,
		MPLogLevelError = 50,
		MPLogLevelFatal = 60,
		MPLogLevelOff = 70
	}

	public struct SdkConfiguration
	{
		public string AdvancedBiddersString
		{
			get
			{
				string result;
				if (this.AdvancedBidders != null)
				{
					result = string.Join(",", (from b in this.AdvancedBidders
					select b.ToString()).ToArray<string>());
				}
				else
				{
					result = string.Empty;
				}
				return result;
			}
		}

		public string MediationSettingsJson
		{
			get
			{
				return (this.MediationSettings == null) ? string.Empty : Json.Serialize(this.MediationSettings);
			}
		}

		public string NetworksToInitString
		{
			get
			{
				string result;
				if (this.NetworksToInit != null)
				{
					result = string.Join(",", (from b in this.NetworksToInit
					select b.ToString()).ToArray<string>());
				}
				else
				{
					result = string.Empty;
				}
				return result;
			}
		}

		public string AdUnitId;

		public MoPubBase.AdvancedBidder[] AdvancedBidders;

		public MoPubBase.MediationSetting[] MediationSettings;

		public MoPubBase.RewardedNetwork[] NetworksToInit;
	}

	public class MediationSetting : Dictionary<string, object>
	{
		public MediationSetting(string adVendor)
		{
			base.Add("adVendor", adVendor);
		}
	}

	public struct Reward
	{
		public override string ToString()
		{
			return string.Format("\"{0} {1}\"", this.Amount, this.Label);
		}

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(this.Label) && this.Amount > 0;
		}

		public string Label;

		public int Amount;
	}

	public abstract class ThirdPartyNetwork
	{
		protected ThirdPartyNetwork(string name)
		{
			this._name = "com.mopub.mobileads." + name;
		}

		public override string ToString()
		{
			return this._name;
		}

		private readonly string _name;
	}

	public class AdvancedBidder : MoPubBase.ThirdPartyNetwork
	{
		private AdvancedBidder(string name) : base(name)
		{
		}

		private static MoPubBase.AdvancedBidder GetBidder(string network)
		{
			network += "AdvancedBidder";
			return new MoPubBase.AdvancedBidder(network);
		}

		public static readonly MoPubBase.AdvancedBidder AdColony = MoPubBase.AdvancedBidder.GetBidder("AdColony");

		public static readonly MoPubBase.AdvancedBidder Facebook = MoPubBase.AdvancedBidder.GetBidder("Facebook");
	}

	public class RewardedNetwork : MoPubBase.ThirdPartyNetwork
	{
		private RewardedNetwork(string name) : base(name)
		{
		}

		private static MoPubBase.RewardedNetwork GetNetwork(string network)
		{
			network += "RewardedVideo";
			return new MoPubBase.RewardedNetwork(network);
		}

		private readonly string _name;

		public static readonly MoPubBase.RewardedNetwork AdColony = MoPubBase.RewardedNetwork.GetNetwork("AdColony");

		public static readonly MoPubBase.RewardedNetwork AdMob = MoPubBase.RewardedNetwork.GetNetwork("GooglePlayServices");

		public static readonly MoPubBase.RewardedNetwork Chartboost = MoPubBase.RewardedNetwork.GetNetwork("Chartboost");

		public static readonly MoPubBase.RewardedNetwork Facebook = MoPubBase.RewardedNetwork.GetNetwork("Facebook");

		public static readonly MoPubBase.RewardedNetwork Tapjoy = MoPubBase.RewardedNetwork.GetNetwork("Tapjoy");

		public static readonly MoPubBase.RewardedNetwork Unity = MoPubBase.RewardedNetwork.GetNetwork("Unity");

		public static readonly MoPubBase.RewardedNetwork Vungle = MoPubBase.RewardedNetwork.GetNetwork("Vungle");
	}
}

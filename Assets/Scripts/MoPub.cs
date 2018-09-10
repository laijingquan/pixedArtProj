// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class MoPub : MoPubAndroid
{
	public static string SdkName
	{
		get
		{
			string result;
			if ((result = MoPub._sdkName) == null)
			{
				result = (MoPub._sdkName = MoPubAndroid.GetSdkName().Replace("+unity", string.Empty));
			}
			return result;
		}
	}

	private static string _sdkName;
}

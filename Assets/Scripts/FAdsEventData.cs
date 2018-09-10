// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

[Serializable]
public class FAdsEventData
{
	public Dictionary<string, string> EventDataToDictionary()
	{
		if (this.eventKeys == null || this.eventKeys.Length == 0 || this.eventVals == null || this.eventVals.Length == 0 || this.eventKeys.Length != this.eventVals.Length)
		{
			return null;
		}
		Dictionary<string, string> result;
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			for (int i = 0; i < this.eventKeys.Length; i++)
			{
				dictionary.Add(this.eventKeys[i], this.eventVals[i]);
			}
			result = dictionary;
		}
		catch (Exception ex)
		{
			FMLogger.vCore("failed to convert event " + this.eventName + " msg:" + ex.Message);
			result = null;
		}
		return result;
	}

	public string eventName;

	public string[] eventKeys;

	public string[] eventVals;
}

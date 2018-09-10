// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	public class AdjustSessionFailure
	{
		public AdjustSessionFailure()
		{
		}

		public AdjustSessionFailure(Dictionary<string, string> sessionFailureDataMap)
		{
			if (sessionFailureDataMap == null)
			{
				return;
			}
			this.Adid = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyTimestamp);
			this.WillRetry = bool.Parse(AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyWillRetry));
			bool willRetry;
			if (bool.TryParse(AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyWillRetry), out willRetry))
			{
				this.WillRetry = willRetry;
			}
			string aJSON = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyJsonResponse);
			JSONNode jsonnode = JSON.Parse(aJSON);
			if (jsonnode != null && jsonnode.AsObject != null)
			{
				this.JsonResponse = new Dictionary<string, object>();
				AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
			}
		}

		public AdjustSessionFailure(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			if (jsonnode == null)
			{
				return;
			}
			this.Adid = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTimestamp);
			this.WillRetry = Convert.ToBoolean(AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyWillRetry));
			JSONNode jsonnode2 = jsonnode[AdjustUtils.KeyJsonResponse];
			if (jsonnode2 == null)
			{
				return;
			}
			if (jsonnode2.AsObject == null)
			{
				return;
			}
			this.JsonResponse = new Dictionary<string, object>();
			AdjustUtils.WriteJsonResponseDictionary(jsonnode2.AsObject, this.JsonResponse);
		}

		public string Adid { get; set; }

		public string Message { get; set; }

		public string Timestamp { get; set; }

		public bool WillRetry { get; set; }

		public Dictionary<string, object> JsonResponse { get; set; }

		public void BuildJsonResponseFromString(string jsonResponseString)
		{
			JSONNode jsonnode = JSON.Parse(jsonResponseString);
			if (jsonnode == null)
			{
				return;
			}
			this.JsonResponse = new Dictionary<string, object>();
			AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
		}

		public string GetJsonResponse()
		{
			return AdjustUtils.GetJsonResponseCompact(this.JsonResponse);
		}
	}
}

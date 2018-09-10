// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public class ConversionData
{
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			" pid:",
			(!string.IsNullOrEmpty(this.media_source)) ? this.media_source : "null",
			" c_id:",
			(!string.IsNullOrEmpty(this.af_c_id)) ? this.af_c_id : "null",
			" adset_id:",
			(!string.IsNullOrEmpty(this.af_adset_id)) ? this.af_adset_id : "null",
			" ad_id:",
			(!string.IsNullOrEmpty(this.af_ad_id)) ? this.af_ad_id : "null"
		});
	}

	public string media_source;

	public string af_c_id;

	public string af_adset_id;

	public string af_ad_id;
}

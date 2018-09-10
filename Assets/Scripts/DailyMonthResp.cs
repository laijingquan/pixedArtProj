// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

[Serializable]
public class DailyMonthResp
{
	public override string ToString()
	{
		return string.Format("{0}.{1}", this.monthIndex, this.year);
	}

	public string monthName;

	public int year;

	public int monthIndex;

	public List<WebPicData> pics;
}

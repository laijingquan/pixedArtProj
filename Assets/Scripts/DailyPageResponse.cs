// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

[Serializable]
public class DailyPageResponse
{
	public bool IsValid()
	{
		if (this.paths == null || this.paths.Length < 1)
		{
			return false;
		}
		bool flag = false;
		for (int i = 0; i < this.paths.Length; i++)
		{
			if (string.IsNullOrEmpty(this.paths[i]))
			{
				this.paths[i] = string.Empty;
			}
			else
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (this.daily != null && (this.daily.pic == null || !this.daily.pic.IsValid()))
		{
			this.daily = null;
		}
		if (this.months != null)
		{
			for (int j = 0; j < this.months.Count; j++)
			{
				if (this.months[j].pics == null)
				{
					return false;
				}
				for (int k = 0; k < this.months[j].pics.Count; k++)
				{
					if (!this.months[j].pics[k].IsValid())
					{
						return false;
					}
				}
			}
		}
		try
		{
			this.Sort();
		}
		catch (Exception ex)
		{
			FMLogger.vCore("failed to sort daily page response. msg: " + ex.Message);
		}
		return true;
	}

	public void Sort()
	{
		if (this.months != null)
		{
			this.months.Sort(new Comparison<DailyMonthResp>(this.ComparisonDesc));
		}
	}

	private int ComparisonAsc(DailyMonthResp x, DailyMonthResp y)
	{
		if (x.year > y.year)
		{
			return 1;
		}
		if (x.year < y.year)
		{
			return -1;
		}
		if (x.monthIndex > y.monthIndex)
		{
			return 1;
		}
		if (x.monthIndex < y.monthIndex)
		{
			return -1;
		}
		return 0;
	}

	private int ComparisonDesc(DailyMonthResp x, DailyMonthResp y)
	{
		if (x.year > y.year)
		{
			return -1;
		}
		if (x.year < y.year)
		{
			return 1;
		}
		if (x.monthIndex > y.monthIndex)
		{
			return -1;
		}
		if (x.monthIndex < y.monthIndex)
		{
			return 1;
		}
		return 0;
	}

	public string[] paths;

	public List<DailyMonthResp> months;

	public FeaturedDailyResp daily;
}

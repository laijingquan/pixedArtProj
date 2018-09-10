// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class FeaturedInfo
{
	public void UpdatePicData(PictureData newPd)
	{
		if (this.dailies != null)
		{
			for (int i = 0; i < this.dailies.Count; i++)
			{
				if (this.dailies[i].picData.Id == newPd.Id)
				{
					this.dailies[i].picData = newPd;
				}
			}
		}
		if (this.promoPics != null)
		{
			for (int j = 0; j < this.promoPics.Count; j++)
			{
				if (this.promoPics[j].picData.Id == newPd.Id)
				{
					this.promoPics[j].picData = newPd;
				}
			}
		}
	}

	public void UpdateSaveState(int picDataId, bool hasSave)
	{
		if (this.dailies != null)
		{
			for (int i = 0; i < this.dailies.Count; i++)
			{
				if (this.dailies[i].picData.Id == picDataId)
				{
					this.dailies[i].picData.SetSaveState(hasSave);
				}
			}
		}
		if (this.promoPics != null)
		{
			for (int j = 0; j < this.promoPics.Count; j++)
			{
				if (this.promoPics[j].picData.Id == picDataId)
				{
					this.promoPics[j].picData.SetSaveState(hasSave);
				}
			}
		}
	}

	public bool IsEmpty()
	{
		int num = 0;
		if (this.dailies != null)
		{
			num += this.dailies.Count;
		}
		if (this.promoPics != null)
		{
			num += this.promoPics.Count;
		}
		if (this.externalLinks != null)
		{
			num += this.externalLinks.Count;
		}
		return num == 0;
	}

	public void OrderItems()
	{
		this.orderedList = new List<OrderedItemInfo>();
		int num = 0;
		int num2 = this.dailies.Count + this.promoPics.Count + this.externalLinks.Count;
		int num3 = 0;
		int num4 = 100;
		int num5 = 0;
		while (num3 < num2 || num5 < num4)
		{
			for (int i = 0; i < this.dailies.Count; i++)
			{
				if (this.dailies[i].order == num)
				{
					num3++;
					this.orderedList.Add(this.dailies[i]);
					break;
				}
			}
			for (int j = 0; j < this.promoPics.Count; j++)
			{
				if (this.promoPics[j].order == num)
				{
					num3++;
					this.orderedList.Add(this.promoPics[j]);
					break;
				}
			}
			for (int k = 0; k < this.externalLinks.Count; k++)
			{
				if (this.externalLinks[k].order == num)
				{
					num3++;
					this.orderedList.Add(this.externalLinks[k]);
					break;
				}
			}
			num++;
			num5++;
		}
	}

	public List<OrderedItemInfo> orderedList;

	public List<DailyPicInfo> dailies;

	public List<PromoPicInfo> promoPics;

	public List<ExternalLinkInfo> externalLinks;
}

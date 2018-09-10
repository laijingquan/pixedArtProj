// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class NewsInfo
{
	public void UpdatePicData(PictureData newPd)
	{
		if (this.promoPics != null)
		{
			for (int i = 0; i < this.promoPics.Count; i++)
			{
				if (this.promoPics[i].picData.Id == newPd.Id)
				{
					this.promoPics[i].picData = newPd;
				}
			}
		}
	}

	public void UpdateSaveState(int picDataId, bool hasSave)
	{
		if (this.promoPics != null)
		{
			for (int i = 0; i < this.promoPics.Count; i++)
			{
				if (this.promoPics[i].picData.Id == picDataId)
				{
					this.promoPics[i].picData.SetSaveState(hasSave);
				}
			}
		}
	}

	public bool IsEmpty()
	{
		int num = 0;
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
		int num2 = this.promoPics.Count + this.externalLinks.Count;
		int num3 = 0;
		int num4 = 100;
		int num5 = 0;
		while (num3 < num2 || num5 < num4)
		{
			for (int i = 0; i < this.promoPics.Count; i++)
			{
				if (this.promoPics[i].order == num)
				{
					num3++;
					this.orderedList.Add(this.promoPics[i]);
					break;
				}
			}
			for (int j = 0; j < this.externalLinks.Count; j++)
			{
				if (this.externalLinks[j].order == num)
				{
					num3++;
					this.orderedList.Add(this.externalLinks[j]);
					break;
				}
			}
			num++;
			num5++;
		}
	}

	public List<OrderedItemInfo> orderedList;

	public List<PromoPicInfo> promoPics;

	public List<ExternalLinkInfo> externalLinks;
}

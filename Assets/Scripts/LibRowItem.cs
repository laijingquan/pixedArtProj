// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class LibRowItem : ScrollRowItem
{
	public override void OnBecomeVisable(int row, IPictureInfoProvider dataProvider, bool lazyLoad, bool showLabels = true)
	{
		base.Clean();
		this.Row = row;
		for (int i = 0; i < this.pics.Count; i++)
		{
			this.pics[i].Reset();
		}
		PictureData[] rowData = dataProvider.GetRowData(row);
		for (int j = 0; j < rowData.Length; j++)
		{
			if (!this.pics[j].gameObject.activeSelf)
			{
				this.pics[j].gameObject.SetActive(true);
			}
			this.pics[j].Init(rowData[j], lazyLoad, showLabels, false);
			if (rowData[j].HasSave)
			{
				this.pics[j].AddSave(dataProvider.GetSave(rowData[j]));
			}
		}
		for (int k = rowData.Length; k < this.pics.Count; k++)
		{
			this.pics[k].gameObject.SetActive(false);
		}
	}

	public override void ReinitPicItem(PictureData picData)
	{
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf && this.pics[i].PictureData != null && this.pics[i].Id == picData.Id)
			{
				FMLogger.Log("reset pic " + picData.Id);
				this.pics[i].Reset();
				this.pics[i].Init(picData, false, true, false);
				break;
			}
		}
	}
}

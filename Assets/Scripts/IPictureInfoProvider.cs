// dnSpy decompiler from Assembly-CSharp.dll
using System;

public interface IPictureInfoProvider
{
	PictureData[] GetRowData(int row);

	PictureSaveData GetSave(PictureData picData);
}

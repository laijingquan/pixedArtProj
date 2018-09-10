// dnSpy decompiler from Assembly-CSharp.dll
using System;

public interface IDailyInfoProvider
{
	bool IsHeader(int row);

	string GetMonthName();

	PictureData[] GetRowData(int row);

	PictureSaveData GetSave(PictureData picData);
}

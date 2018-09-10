// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

[Serializable]
public class PictureData
{
	public PictureData(PictureType type, int pId, int id, FillAlgorithm fillType)
	{
		this.picType = type;
		this.PackId = pId;
		this.Id = id;
		this.FillType = fillType;
		this.PicClass = PicClass.Common;
	}

	public PictureData(WebPicData webPic, string[] baseUrls)
	{
		this.picType = PictureType.Web;
		this.Id = webPic.id;
		this.PackId = webPic.packId;
		this.FillType = webPic.FillType;
		this.Extras = new PictureDataExtras();
		this.Extras.webPath = PicWebPath.FromWebPic(webPic, baseUrls);
		this.PicClass = PicClass.Common;
	}

	public PicWebPath WebPath
	{
		get
		{
			if (this.Extras != null && this.Extras.webPath != null)
			{
				return this.Extras.webPath;
			}
			return null;
		}
	}

	public virtual string Icon
	{
		get
		{
			switch (this.picType)
			{
			case PictureType.System:
				return string.Empty;
			case PictureType.Local:
				return string.Concat(new object[]
				{
					this.PackId,
					"/",
					this.Id,
					"i"
				});
			case PictureType.Web:
				return string.Empty;
			default:
				return string.Empty;
			}
		}
	}

	public virtual string LineArt
	{
		get
		{
			switch (this.picType)
			{
			case PictureType.System:
				return string.Empty;
			case PictureType.Local:
				return this.PackId + "/" + this.Id;
			case PictureType.Web:
				return string.Empty;
			default:
				return string.Empty;
			}
		}
	}

	public virtual string ColorMap
	{
		get
		{
			switch (this.picType)
			{
			case PictureType.System:
				return string.Empty;
			case PictureType.Local:
				return string.Concat(new object[]
				{
					this.PackId,
					"/",
					this.Id,
					"c"
				});
			case PictureType.Web:
				return string.Empty;
			default:
				return string.Empty;
			}
		}
	}

	public virtual string Palette
	{
		get
		{
			switch (this.picType)
			{
			case PictureType.System:
				return string.Empty;
			case PictureType.Local:
				return string.Concat(new object[]
				{
					this.PackId,
					"/",
					this.Id,
					"t"
				});
			case PictureType.Web:
				return string.Empty;
			default:
				return string.Empty;
			}
		}
	}

	public void AddExtras(List<PictureLabel> labels, int[] categories)
	{
		if (this.Extras == null)
		{
			this.Extras = new PictureDataExtras();
		}
		if (categories != null && categories.Length > 0)
		{
			this.Extras.categories = new List<int>(categories);
		}
		if (labels != null && labels.Count > 0)
		{
			this.Extras.labels = labels;
			if (this.Extras.labels.Contains(PictureLabel.Facebook))
			{
				this.SetPicClass(PicClass.FacebookGift);
			}
		}
	}

	public void CopyExtras(PictureDataExtras extras)
	{
		if (this.Extras == null)
		{
			this.Extras = new PictureDataExtras();
		}
		if (extras != null)
		{
			if (extras.labels != null && extras.labels.Count > 0)
			{
				this.Extras.labels = new List<PictureLabel>(extras.labels);
				if (this.Extras.labels.Contains(PictureLabel.Facebook))
				{
					this.SetPicClass(PicClass.FacebookGift);
				}
			}
			if (extras.categories != null && extras.categories.Count > 0)
			{
				this.Extras.categories = new List<int>(extras.categories);
			}
			this.Extras.dailyTabDate = extras.dailyTabDate;
		}
	}

	public void SetPicClass(PicClass picClass)
	{
		this.PicClass = picClass;
	}

	public void SetDailyTabDate(int date)
	{
		if (this.Extras == null)
		{
			this.Extras = new PictureDataExtras();
		}
		if (date > 0)
		{
			this.Extras.dailyTabDate = date;
		}
	}

	public bool IsInDailyTab()
	{
		return this.Extras != null && this.Extras.dailyTabDate > 0;
	}

	public bool IsInCategory(int categoryId)
	{
		return this.Extras != null && this.Extras.categories != null && this.Extras.categories.Contains(categoryId);
	}

	public void SetSaveState(bool hasSave)
	{
		this.HasSave = hasSave;
		if (!hasSave && this.Solved && this.Extras != null)
		{
			this.Extras.completed = false;
		}
	}

	public void SetCompleted()
	{
		if (this.Extras == null)
		{
			this.Extras = new PictureDataExtras();
		}
		this.Solved = true;
		this.HasSave = true;
		this.Extras.completed = true;
	}

	public bool IsCompleted()
	{
		return this.HasSave && this.Solved && this.Extras != null && this.Extras.completed;
	}

	public PictureDataExtras Extras;

	public FillAlgorithm FillType;

	public PictureType picType;

	public int PackId;

	public int Id;

	public bool HasSave;

	public bool Solved;

	public PicClass PicClass;
}

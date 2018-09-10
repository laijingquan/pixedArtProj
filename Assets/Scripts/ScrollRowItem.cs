// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScrollRowItem : MonoBehaviour
{
	public virtual void OnBecomeVisable(int row, IPictureInfoProvider dataProvider, bool lazyLoad, bool showLabels = true)
	{
	}

	public bool ContainsItem(int picId)
	{
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf && this.pics[i].PictureData != null && this.pics[i].PictureData.Id == picId)
			{
				return true;
			}
		}
		return false;
	}

	public PicItem GetPictureItem(int picId)
	{
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf && this.pics[i].PictureData != null && this.pics[i].PictureData.Id == picId)
			{
				return this.pics[i];
			}
		}
		return null;
	}

	public void ReloadFailedIcons()
	{
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf && this.pics[i].PictureData != null && this.pics[i].IconLoadFailed())
			{
				this.pics[i].RetryLoadIcon();
			}
		}
	}

	public abstract void ReinitPicItem(PictureData picData);

	protected void Clean()
	{
		for (int i = 0; i < this.pics.Count; i++)
		{
			this.pics[i].Reset();
		}
	}

	public void ReloadTextures()
	{
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf)
			{
				this.pics[i].ReloadTextures();
			}
		}
	}

	public void UnloadTextures()
	{
		for (int i = 0; i < this.pics.Count; i++)
		{
			if (this.pics[i].gameObject.activeSelf)
			{
				this.pics[i].UnloadTextures();
			}
		}
	}

	public int Row;

	public List<PicItem> pics;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class BasicIconSlot : MonoBehaviour
{
	public void Init(PictureData pictureData, bool useLoadIndicator)
	{
		this.pd = pictureData;
		this.useIndicator = useLoadIndicator;
		this.LoadIcon();
	}

	private void LoadIcon()
	{
		if (this.useIndicator)
		{
			this.loadIcon.SetActive(true);
		}
		FMLogger.Log("load web " + this.pd.Id);
		this.iconDownloadTask = new IconDownloadTask(this.pd.WebPath.baseUrls, this.pd.WebPath.icon, this.pd.WebPath.CacheKey, delegate(bool success, Texture2D tex)
		{
			if (success && tex != null)
			{
				if (this.useIndicator)
				{
					this.loadIcon.SetActive(false);
				}
				this.img.texture = tex;
			}
			else
			{
				FMLogger.Log("Error loading bonus pic: " + this.pd.Id);
			}
			this.iconDownloadTask = null;
		});
		ImageManager.Instance.DownloadIcon(this.iconDownloadTask);
	}

	public bool IconFailedToLoad()
	{
		return this.iconDownloadTask == null && this.img.texture == null;
	}

	public void ReloadFailedIcon()
	{
		if (this.pd == null)
		{
			return;
		}
		this.LoadIcon();
	}

	public void Reset()
	{
		this.loadIcon.SetActive(false);
		if (this.iconDownloadTask != null)
		{
			this.iconDownloadTask.Cancel();
			this.iconDownloadTask = null;
		}
		if (this.pd == null)
		{
			return;
		}
		if (this.img.texture != null)
		{
			ImageManager.Instance.RecycleWebIcon(this.pd.Extras.webPath, this.img.texture as Texture2D);
			this.img.texture = null;
		}
		this.pd = null;
	}

	[SerializeField]
	private GameObject loadIcon;

	[SerializeField]
	private RawImage img;

	private IconDownloadTask iconDownloadTask;

	private PictureData pd;

	private bool useIndicator;
}

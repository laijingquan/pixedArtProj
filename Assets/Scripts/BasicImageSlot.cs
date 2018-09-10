// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class BasicImageSlot : MonoBehaviour
{
	public void Init(ImageData imgData, bool isLazy = false)
	{
		this.imageData = imgData;
		this.lazyLoad = isLazy;
		this.LoadIcon();
	}

	private void LoadIcon()
	{
		if (this.lazyLoad)
		{
			return;
		}
		this.loadIcon.SetActive(true);
		this.iconDownloadTask = new IconDownloadTask(this.imageData.baseUrl, this.imageData.relativePath, this.imageData.CacheKey, delegate(bool success, Texture2D tex)
		{
			if (success && tex != null)
			{
				this.loadIcon.SetActive(false);
				this.img.texture = tex;
			}
			else
			{
				FMLogger.Log("Error loading external pic: " + this.imageData.relativePath);
			}
			this.iconDownloadTask = null;
		});
		ImageManager.Instance.DownloadIcon(this.iconDownloadTask);
	}

	public bool IconFailedToLoad()
	{
		return this.iconDownloadTask == null && this.loadIcon.activeSelf;
	}

	public void ReloadFailedIcon()
	{
		if (this.imageData == null)
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
		if (this.imageData == null)
		{
			return;
		}
		if (this.img.texture != null)
		{
			ImageManager.Instance.RecycleWebImage(this.imageData, this.img.texture as Texture2D);
			this.img.texture = null;
		}
		this.imageData = null;
	}

	public void ReloadTextures()
	{
		this.lazyLoad = false;
		this.LoadIcon();
	}

	public void UnloadTextures()
	{
		this.loadIcon.SetActive(false);
		if (this.iconDownloadTask != null)
		{
			this.iconDownloadTask.Cancel();
			this.iconDownloadTask = null;
		}
		if (this.imageData == null)
		{
			return;
		}
		if (this.img.texture != null)
		{
			ImageManager.Instance.RecycleWebImage(this.imageData, this.img.texture as Texture2D);
			this.img.texture = null;
		}
	}

	private void OnDestroy()
	{
		this.Reset();
	}

	[SerializeField]
	private GameObject loadIcon;

	[SerializeField]
	private RawImage img;

	private IconDownloadTask iconDownloadTask;

	private ImageData imageData;

	private bool lazyLoad;
}

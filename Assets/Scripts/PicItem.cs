// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class PicItem : MonoBehaviour
{
	public int Id
	{
		get
		{
			return this.pd.Id;
		}
	}

	public PictureData PictureData
	{
		get
		{
			return this.pd;
		}
	}

	public PictureSaveData SaveData
	{
		get
		{
			return this.saveData;
		}
	}

	public void Init(PictureData picData, bool lazyIconLoad = false, bool showLabels = true, bool isDailyTab = false)
	{
		this.pd = picData;
		this.lazyLoad = lazyIconLoad;
		this.InternalInit();
		PictureType picType = this.pd.picType;
		if (picType != PictureType.System)
		{
			if (picType != PictureType.Local)
			{
				if (picType == PictureType.Web)
				{
					this.WebInit();
				}
			}
			else
			{
				this.LocalInit();
			}
		}
		else
		{
			this.SysPicInit();
		}
		if (showLabels && this.pd.Extras != null && this.pd.Extras.labels != null)
		{
			this.labels.AddLabels(this.pd.Extras.labels);
		}
		if (isDailyTab && this.pd.Extras != null && this.pd.Extras.dailyTabDate > 0)
		{
			this.labels.AddDailyTab(this.pd.Extras.dailyTabDate);
		}
	}

	public void InitFromItem(PicItem copy)
	{
		this.labels.Clean();
		this.labels.RemoveComplete();
		this.pd = copy.pd;
		this.saveData = copy.saveData;
		if (!this.saveMask.gameObject.activeSelf && copy.saveMask.texture != null)
		{
			this.saveMask.gameObject.SetActive(true);
		}
		this.saveMask.texture = copy.saveMask.texture;
		if (this.saveData != null && this.saveData.progres == 100)
		{
			this.labels.AddComplete();
		}
		if (!this.webImg.gameObject.activeSelf)
		{
			this.webImg.gameObject.SetActive(true);
		}
		this.webImg.texture = copy.webImg.texture;
		copy.pd = null;
		copy.saveData = null;
		copy.webImg.texture = null;
		copy.saveMask.texture = null;
		copy.labels.Clean();
		copy.labels.RemoveComplete();
	}

	public bool IconLoadFailed()
	{
		if (this.pd == null)
		{
			return false;
		}
		PictureType picType = this.pd.picType;
		if (picType != PictureType.System)
		{
			if (picType != PictureType.Local)
			{
				if (picType == PictureType.Web)
				{
					return this.WebIconFailed();
				}
			}
		}
		return false;
	}

	public void RetryLoadIcon()
	{
		PictureType picType = this.pd.picType;
		if (picType != PictureType.System)
		{
			if (picType != PictureType.Local)
			{
				if (picType == PictureType.Web)
				{
					this.RetryLoadWebIcon();
				}
			}
		}
	}

	public void UnloadTextures()
	{
		if (this.pd == null)
		{
			return;
		}
		if (this.webImg.texture != null)
		{
			PictureType picType = this.pd.picType;
			if (picType != PictureType.Local)
			{
				if (picType == PictureType.Web)
				{
					this.WebReset();
				}
			}
			else
			{
				this.LocalReset();
			}
		}
		if (this.saveData != null)
		{
			if (this.saveIconTask != null)
			{
				this.saveIconTask.Cancel();
				this.saveIconTask = null;
			}
			if (this.saveMask.texture != null)
			{
				string path = (this.saveData == null) ? null : this.saveData.iconMaskPath;
				ImageManager.Instance.RecycleLocalIcon(path, this.saveMask.texture as Texture2D);
				this.saveMask.texture = null;
			}
			this.saveMask.gameObject.SetActive(false);
		}
		this.loadIcon.SetActive(false);
	}

	public void ReloadTextures()
	{
		if (this.pd == null)
		{
			return;
		}
		this.lazyLoad = false;
		if (this.webImg.texture == null)
		{
			PictureType picType = this.pd.picType;
			if (picType != PictureType.Local)
			{
				if (picType == PictureType.Web)
				{
					this.WebInit();
				}
			}
			else
			{
				this.LocalInit();
			}
		}
		if (this.saveData != null && this.saveMask.texture == null)
		{
			string iconMaskPath = this.saveData.iconMaskPath;
			if (string.IsNullOrEmpty(iconMaskPath))
			{
				return;
			}
			this.saveIconTask = new LocalIconTask(iconMaskPath, new Action<bool, Texture2D>(this.SaveIconLoaded));
			ImageManager.Instance.LoadLocalIcon(this.saveIconTask);
		}
	}

	public Texture GetIconTex()
	{
		return this.webImg.texture;
	}

	public Texture GetSaveTex()
	{
		return this.saveMask.texture;
	}

	private void WebInit()
	{
		if (this.lazyLoad)
		{
			return;
		}
		this.loadIcon.SetActive(true);
		this.iconDownloadTask = new IconDownloadTask(this.pd.WebPath.baseUrls, this.pd.WebPath.icon, this.pd.WebPath.CacheKey, new Action<bool, Texture2D>(this.LoadImageHandler));
		ImageManager.Instance.DownloadIcon(this.iconDownloadTask);
	}

	private void LoadImageHandler(bool result, Texture2D tex)
	{
		if (result && tex != null)
		{
			this.loadIcon.SetActive(false);
			this.webImg.texture = tex;
		}
		else
		{
			FMLogger.Log("Error loading pic: " + this.pd.Id);
		}
		this.iconDownloadTask = null;
	}

	private bool WebIconFailed()
	{
		return this.iconDownloadTask == null && this.loadIcon.activeSelf;
	}

	private void RetryLoadWebIcon()
	{
		this.iconDownloadTask = new IconDownloadTask(this.pd.WebPath.baseUrls, this.pd.WebPath.icon, this.pd.WebPath.CacheKey, new Action<bool, Texture2D>(this.LoadImageHandler));
		ImageManager.Instance.DownloadIcon(this.iconDownloadTask);
		FMLogger.Log("started reloading " + this.pd.Id);
	}

	private void WebReset()
	{
		this.loadIcon.SetActive(false);
		if (this.iconDownloadTask != null)
		{
			this.iconDownloadTask.Cancel();
			this.iconDownloadTask = null;
		}
		if (this.webImg.texture != null)
		{
			ImageManager.Instance.RecycleWebIcon(this.pd.Extras.webPath, this.webImg.texture as Texture2D);
			this.webImg.texture = null;
		}
	}

	private void LocalInit()
	{
		if (this.lazyLoad)
		{
			return;
		}
		this.loadIcon.SetActive(true);
		this.localIconTask = new LocalIconTask(this.pd.Icon, new Action<bool, Texture2D>(this.LocalIconLoaded));
		ImageManager.Instance.LoadLocalIcon(this.localIconTask);
	}

	private void LocalIconLoaded(bool success, Texture2D tex)
	{
		if (tex != null)
		{
			this.webImg.texture = tex;
			this.loadIcon.SetActive(false);
		}
		else
		{
			this.loadIcon.SetActive(true);
			if (this.pd != null)
			{
				SharedData.Instance.DeletePictureData(this.pd);
			}
		}
		this.localIconTask = null;
	}

	private void LocalReset()
	{
		this.loadIcon.SetActive(false);
		if (this.localIconTask != null)
		{
			this.localIconTask.Cancel();
			this.localIconTask = null;
		}
		if (this.webImg.texture != null)
		{
			ImageManager.Instance.RecycleLocalIcon(this.pd.Icon, this.webImg.texture as Texture2D);
			this.webImg.texture = null;
		}
	}

	private void SysPicInit()
	{
		this.loadIcon.SetActive(true);
	}

	private void SysPicReset()
	{
		this.loadIcon.SetActive(false);
	}

	private void InternalInit()
	{
	}

	public void AddSave(PictureSaveData save)
	{
		if (save == null)
		{
			return;
		}
		this.saveData = save;
		if (this.pd.picType == PictureType.System)
		{
			return;
		}
		string iconMaskPath = this.saveData.iconMaskPath;
		if (this.saveData.progres == 100)
		{
			this.labels.AddComplete();
		}
		if (string.IsNullOrEmpty(iconMaskPath))
		{
			return;
		}
		if (this.lazyLoad)
		{
			return;
		}
		this.saveIconTask = new LocalIconTask(iconMaskPath, new Action<bool, Texture2D>(this.SaveIconLoaded));
		ImageManager.Instance.LoadLocalIcon(this.saveIconTask);
	}

	public void RemoveSave()
	{
		this.labels.RemoveComplete();
		if (this.saveIconTask != null)
		{
			this.saveIconTask.Cancel();
			this.saveIconTask = null;
		}
		if (this.saveMask.texture != null)
		{
			string path = (this.saveData == null) ? null : this.saveData.iconMaskPath;
			ImageManager.Instance.RecycleLocalIcon(path, this.saveMask.texture as Texture2D);
			this.saveMask.texture = null;
		}
		this.saveMask.gameObject.SetActive(false);
	}

	private void CleanSave()
	{
		if (this.saveData == null)
		{
			return;
		}
		if (this.saveIconTask != null)
		{
			this.saveIconTask.Cancel();
			this.saveIconTask = null;
		}
		if (this.saveMask.texture != null)
		{
			string path = (this.saveData == null) ? null : this.saveData.iconMaskPath;
			ImageManager.Instance.RecycleLocalIcon(path, this.saveMask.texture as Texture2D);
			this.saveMask.texture = null;
		}
		this.saveMask.gameObject.SetActive(false);
		this.saveData = null;
		this.labels.RemoveComplete();
	}

	private void SaveIconLoaded(bool success, Texture2D tex)
	{
		if (success && tex != null)
		{
			this.saveMask.texture = tex;
			this.saveMask.gameObject.SetActive(true);
		}
		else
		{
			FMLogger.Log("Failed to load icon preview. file not found");
		}
		this.saveIconTask = null;
	}

	public void Reset()
	{
		if (this.pd == null)
		{
			return;
		}
		this.CleanSave();
		this.labels.Clean();
		PictureType picType = this.pd.picType;
		if (picType != PictureType.System)
		{
			if (picType != PictureType.Local)
			{
				if (picType == PictureType.Web)
				{
					this.WebReset();
				}
			}
			else
			{
				this.LocalReset();
			}
		}
		else
		{
			this.SysPicReset();
		}
		this.pd = null;
	}

	private void OnDestroy()
	{
		this.Reset();
	}

	[SerializeField]
	private PicLabel labels;

	[SerializeField]
	protected RawImage webImg;

	[SerializeField]
	protected RawImage saveMask;

	[SerializeField]
	private GameObject loadIcon;

	protected PictureData pd;

	protected PictureSaveData saveData;

	private bool lazyLoad;

	private IconDownloadTask iconDownloadTask;

	private LocalIconTask localIconTask;

	private LocalIconTask saveIconTask;
}

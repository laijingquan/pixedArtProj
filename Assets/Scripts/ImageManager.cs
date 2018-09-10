// dnSpy decompiler from Assembly-CSharp.dll
using System;
using FMImageManager;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
	public static ImageManager Instance
	{
		get
		{
			return ImageManager.instance;
		}
	}

	private void Awake()
	{
		if (ImageManager.instance != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.iconSize = ((SystemUtils.GetIconQuality() != SystemUtils.IconQuality.Low) ? 384 : 256);
		UnityEngine.Object.DontDestroyOnLoad(this);
		ImageManager.instance = this;
	}

	public void Init()
	{
		this.cacher = new ImageCacher(this.localRunner);
	}

	public void LoadLocalIcon(LocalIconTask task)
	{
		this.cacher.LoadLocal(task);
	}

	public void RecycleLocalIcon(string path, Texture2D tex)
	{
		this.cacher.Unload(path, tex);
	}

	public void DownloadIcon(IconDownloadTask task)
	{
		this.cacher.LoadWeb(task);
	}

	public void RecycleWebIcon(PicWebPath webPath, Texture2D tex)
	{
		this.cacher.Unload(webPath.CacheKey, tex);
	}

	public void RecycleWebImage(ImageData imageData, Texture2D tex)
	{
		this.cacher.Unload(imageData.CacheKey, tex);
	}

	public int UnloadUnusedTextures()
	{
		return this.cacher.UnloadUnused();
	}

	public void UnloadAllTextures()
	{
		this.cacher.UnloadAll();
	}

	public void LowMemoryMode()
	{
		this.cacher.SetLowMemoryMode();
	}

	public int CacheSize
	{
		get
		{
			return this.cacher.Size;
		}
	}

	private void Update()
	{
		if (this.cacher != null)
		{
			this.cacher.UpdateState();
		}
	}

	public void SaveIcon(string directory, string fileName, Texture2D source)
	{
		if (this.icon == null)
		{
			this.icon = new Texture2D(this.iconSize, this.iconSize, TextureFormat.RGBA32, false);
			this.icon.filterMode = FilterMode.Bilinear;
		}
		TextureScale.CopyIntoTexture(source, this.icon);
		FileHelper.SaveTextureToFile(this.icon, directory, fileName);
	}

	public void ShareTexture(Texture2D line, Texture2D drawTex, Texture2D target)
	{
		TextureScale.CopyArrayIntoTexture(new Texture2D[]
		{
			drawTex,
			line
		}, target);
	}

	private static ImageManager instance;

	[SerializeField]
	private LocalStorageRunner localRunner;

	private ImageCacher cacher;

	private const int ICON_SIZE_SMALL = 256;

	private const int ICON_SIZE_MEDIUM = 384;

	private int iconSize;

	private Texture2D icon;
}

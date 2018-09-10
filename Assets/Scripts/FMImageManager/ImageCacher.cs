// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FMImageManager
{
	public class ImageCacher
	{
		public ImageCacher(ILocalImageCache localRunner)
		{
			this.localRunner = localRunner;
			LocalStorageRunner.QueueSpeed queueSpeed = this.GetQueueSpeed();
			this.localRunner.Init(queueSpeed);
			ImageCacher.UnloaderType unloaderType = this.GetUnloaderType();
			FMLogger.vCore(string.Concat(new object[]
			{
				"buffer type: ",
				unloaderType,
				" queue speed: ",
				queueSpeed
			}));
			switch (unloaderType)
			{
			case ImageCacher.UnloaderType.LowEnd:
				this.unloader = new LowEndUnloadBehaviour(this.cached, localRunner);
				break;
			case ImageCacher.UnloaderType.MidEnd:
				this.unloader = new MidEndUnloadBehaviour(this.cached, localRunner);
				break;
			case ImageCacher.UnloaderType.HighEnd:
				this.unloader = new HighEndUnloadBehaviour(this.cached, localRunner);
				break;
			default:
				this.unloader = new LowEndUnloadBehaviour(this.cached, localRunner);
				break;
			}
		}

		public bool LowMemoryMode { get; private set; }

		public int Size
		{
			get
			{
				return this.cached.Count;
			}
		}

		public void LoadLocal(LocalIconTask task)
		{
			string path = task.Path;
			CacheImage cacheImageByKey = this.GetCacheImageByKey(path);
			if (cacheImageByKey != null)
			{
				cacheImageByKey.AddRef(1);
				task.Result(true, cacheImageByKey.Tex);
			}
			else
			{
				LocalCImageTask localCacheTaskByKey = this.GetLocalCacheTaskByKey(path);
				if (localCacheTaskByKey != null)
				{
					localCacheTaskByKey.JoinHandler(task);
				}
				else
				{
					LocalCImageTask cacheTask = new LocalCImageTask(task, path);
					this.localTasks.Add(cacheTask);
					cacheTask.Complete += delegate(CacheImage image)
					{
						if (image != null)
						{
							this.cached.Add(image);
						}
						this.localTasks.Remove(cacheTask);
					};
					cacheTask.Run(this.localRunner);
				}
			}
		}

		public void LoadWeb(IconDownloadTask task)
		{
			string cacheKey = task.CacheKey;
			CacheImage cacheImageByKey = this.GetCacheImageByKey(cacheKey);
			if (cacheImageByKey != null)
			{
				cacheImageByKey.AddRef(1);
				task.Result(true, cacheImageByKey.Tex);
			}
			else
			{
				WebCImageTask webCacheTaskByKey = this.GetWebCacheTaskByKey(cacheKey);
				if (webCacheTaskByKey != null)
				{
					webCacheTaskByKey.JoinHandler(task);
				}
				else
				{
					WebCImageTask cacheTask = new WebCImageTask(task, cacheKey);
					this.webTasks.Add(cacheTask);
					cacheTask.Complete += delegate(CacheImage image)
					{
						if (image != null)
						{
							this.cached.Add(image);
						}
						this.webTasks.Remove(cacheTask);
					};
					cacheTask.Run(this.localRunner);
				}
			}
		}

		public void Unload(string cacheKey, Texture2D tex)
		{
			CacheImage cacheImage = this.cached.Find((CacheImage img) => img.CacheKey == cacheKey);
			if (cacheImage != null)
			{
				cacheImage.RemoveRef();
				if (cacheImage.Tex != tex)
				{
					FMLogger.vCore("****Wtf RC tex not match");
					if (tex != null)
					{
						this.localRunner.DestroyTex(tex, false);
					}
				}
			}
			else if (tex != null)
			{
				FMLogger.vCore("***Wtf RC tex not found");
				this.localRunner.DestroyTex(tex, false);
			}
		}

		public int UnloadUnused()
		{
			return this.unloader.UnloadUnused();
		}

		public void UnloadAll()
		{
			for (int i = this.cached.Count - 1; i >= 0; i--)
			{
				this.localRunner.DestroyTex(this.cached[i].Tex, true);
				this.cached.RemoveAt(i);
			}
			this.cached.Clear();
		}

		public void SetLowMemoryMode()
		{
			this.LowMemoryMode = true;
			this.unloader.SetLowMemMode();
		}

		public void UpdateState()
		{
			for (int i = 0; i < this.webTasks.Count; i++)
			{
				this.webTasks[i].CheckHandlersState();
			}
			this.unloader.UpdateTick();
		}

		private CacheImage GetCacheImageByKey(string cacheKey)
		{
			for (int i = 0; i < this.cached.Count; i++)
			{
				if (this.cached[i].CacheKey == cacheKey)
				{
					return this.cached[i];
				}
			}
			return null;
		}

		private LocalCImageTask GetLocalCacheTaskByKey(string cacheKey)
		{
			for (int i = 0; i < this.localTasks.Count; i++)
			{
				if (this.localTasks[i].CacheKey == cacheKey)
				{
					return this.localTasks[i];
				}
			}
			return null;
		}

		private WebCImageTask GetWebCacheTaskByKey(string cacheKey)
		{
			for (int i = 0; i < this.webTasks.Count; i++)
			{
				if (this.webTasks[i].CacheKey == cacheKey)
				{
					return this.webTasks[i];
				}
			}
			return null;
		}

		private ImageCacher.UnloaderType GetUnloaderType()
		{
			int systemMemorySize = SystemInfo.systemMemorySize;
			if (systemMemorySize < 600)
			{
				return ImageCacher.UnloaderType.LowEnd;
			}
			if (systemMemorySize < 1100)
			{
				return ImageCacher.UnloaderType.MidEnd;
			}
			return ImageCacher.UnloaderType.HighEnd;
		}

		private LocalStorageRunner.QueueSpeed GetQueueSpeed()
		{
			int systemMemorySize = SystemInfo.systemMemorySize;
			if (systemMemorySize > 4000 || (SafeLayout.IsTablet && systemMemorySize > 3000))
			{
				return LocalStorageRunner.QueueSpeed.Medium;
			}
			return LocalStorageRunner.QueueSpeed.Slow;
		}

		private List<CacheImage> cached = new List<CacheImage>();

		private List<LocalCImageTask> localTasks = new List<LocalCImageTask>();

		private List<WebCImageTask> webTasks = new List<WebCImageTask>();

		private ILocalImageCache localRunner;

		private UnloadBehaviour unloader;

		private enum UnloaderType
		{
			LowEnd,
			MidEnd,
			HighEnd
		}
	}
}

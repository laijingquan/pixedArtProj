// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FMImageManager
{
	public class LocalStorageRunner : MonoBehaviour, ILocalImageCache
	{
		public void Init(LocalStorageRunner.QueueSpeed queueSpeed = LocalStorageRunner.QueueSpeed.Slow)
		{
			if (queueSpeed != LocalStorageRunner.QueueSpeed.Slow)
			{
				if (queueSpeed != LocalStorageRunner.QueueSpeed.Medium)
				{
					if (queueSpeed == LocalStorageRunner.QueueSpeed.Fast)
					{
						this.queue = new FastQueueRunner(new Func<IEnumerator, Coroutine>(base.StartCoroutine));
					}
				}
				else
				{
					this.queue = new MediumQueueRunner(new Func<IEnumerator, Coroutine>(base.StartCoroutine));
				}
			}
			else
			{
				this.queue = new SlowQueueRunner();
			}
			FMLogger.vCore("queue speed " + queueSpeed);
			this.processorCoroutine = base.StartCoroutine(this.queue.QueueProcessor());
		}

		private void OnDisable()
		{
			if (this.processorCoroutine != null)
			{
				base.StopCoroutine(this.queue.QueueProcessor());
			}
		}

		public void HasLocalCopy(string cacheKey, Action<Texture2D> callback)
		{
			GetCopyTask task = new GetCopyTask(cacheKey, callback);
			this.queue.Enqueue(task);
		}

		public void Recycle(string cacheKey, Texture2D tex)
		{
			if (LocalStorageRunner.__f__mg_cache0 == null)
			{
				LocalStorageRunner.__f__mg_cache0 = new Action<Texture2D>(UnityEngine.Object.Destroy);
			}
			RecycleTask task = new RecycleTask(cacheKey, tex, LocalStorageRunner.__f__mg_cache0);
			this.queue.Enqueue(task);
		}

		public void MakeLocalCopy(string cacheKey, Texture2D tex)
		{
			LocalCopyTask task = new LocalCopyTask(cacheKey, tex);
			this.queue.Enqueue(task);
		}

		public void LoadLocalIcon(string path, Action<Texture2D> callback)
		{
			LoadLocalTexTask task = new LoadLocalTexTask(path, callback);
			this.queue.Enqueue(task);
		}

		public void DestroyTex(Texture2D tex, bool forceImmediate = false)
		{
			if (forceImmediate)
			{
				UnityEngine.Object.Destroy(tex);
			}
			else
			{
				if (LocalStorageRunner.__f__mg_cache1 == null)
				{
					LocalStorageRunner.__f__mg_cache1 = new Action<Texture2D>(UnityEngine.Object.Destroy);
				}
				DestroyTexTask task = new DestroyTexTask(tex, LocalStorageRunner.__f__mg_cache1);
				this.queue.Enqueue(task);
			}
		}

		private Coroutine processorCoroutine;

		private IQueueRunner queue;

		[CompilerGenerated]
		private static Action<Texture2D> __f__mg_cache0;

		[CompilerGenerated]
		private static Action<Texture2D> __f__mg_cache1;

		public enum QueueSpeed
		{
			Slow,
			Medium,
			Fast
		}
	}
}

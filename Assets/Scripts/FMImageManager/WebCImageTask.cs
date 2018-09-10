// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FMImageManager
{
	public class WebCImageTask : CacheImageTask
	{
		public WebCImageTask(IconDownloadTask task, string cacheKey) : base(cacheKey)
		{
			this.initial = task;
		}

		public void Run(ILocalImageCache localRunner)
		{
			this.worker = new IconDownloadTask(this.initial.BaseUrls, this.initial.RelativeUrl, this.initial.CacheKey, delegate(bool success, Texture2D tex)
			{
				this.Tex = tex;
				if (this.Tex != null)
				{
					localRunner.MakeLocalCopy(this.initial.CacheKey, this.Tex);
				}
				this.ReportResults();
			});
			localRunner.HasLocalCopy(this.worker.CacheKey, delegate(Texture2D tex)
			{
				if (!this.worker.IsCanceled)
				{
					if (tex != null)
					{
						this.worker.Result(true, tex);
					}
					else
					{
						this.worker.AddHandlerOnCancelation(new Action<string, Texture2D>(localRunner.Recycle));
						WebLoader.Instance.DownloadIcon(this.worker);
					}
				}
				else if (tex != null)
				{
					localRunner.DestroyTex(tex, false);
				}
			});
		}

		public void JoinHandler(IconDownloadTask task)
		{
			if (this.joined == null)
			{
				this.joined = new List<IconDownloadTask>();
			}
			this.joined.Add(task);
		}

		public void CheckHandlersState()
		{
			int num = 0;
			if (!this.initial.IsCanceled)
			{
				num++;
			}
			if (this.joined != null)
			{
				for (int i = 0; i < this.joined.Count; i++)
				{
					if (!this.joined[i].IsCanceled)
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				this.worker.Cancel();
				this.ReportResults();
			}
		}

		private void ReportResults()
		{
			bool success = base.Tex != null;
			Texture2D tex = base.Tex;
			int num = 0;
			if (!this.initial.IsCanceled)
			{
				num++;
				this.initial.Result(success, tex);
			}
			if (this.joined != null)
			{
				for (int i = 0; i < this.joined.Count; i++)
				{
					if (!this.joined[i].IsCanceled)
					{
						num++;
						this.joined[i].Result(success, tex);
					}
				}
			}
			CacheImage cacheImage = null;
			if (base.Tex != null)
			{
				cacheImage = new CacheImage(this.initial.CacheKey);
				cacheImage.Tex = base.Tex;
				cacheImage.AddRef(num);
			}
			base.FireCompleted(cacheImage);
		}

		private IconDownloadTask worker;

		private IconDownloadTask initial;

		private List<IconDownloadTask> joined;
	}
}

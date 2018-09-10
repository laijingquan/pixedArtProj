// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FMImageManager
{
	public class LocalCImageTask : CacheImageTask
	{
		public LocalCImageTask(LocalIconTask task, string cacheKey) : base(cacheKey)
		{
			this.initial = task;
		}

		public void Run(ILocalImageCache localRunner)
		{
			localRunner.LoadLocalIcon(this.initial.Path, delegate(Texture2D tex)
			{
				base.Tex = tex;
				this.ReportResults();
			});
		}

		public void JoinHandler(LocalIconTask task)
		{
			if (this.joined == null)
			{
				this.joined = new List<LocalIconTask>();
			}
			this.joined.Add(task);
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
				cacheImage = new CacheImage(this.initial.Path);
				cacheImage.Tex = base.Tex;
				cacheImage.AddRef(num);
			}
			base.FireCompleted(cacheImage);
		}

		private LocalIconTask initial;

		private List<LocalIconTask> joined;
	}
}

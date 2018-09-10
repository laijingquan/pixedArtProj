// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;
using UnityEngine;

namespace FMImageManager
{
	public abstract class CacheImageTask
	{
		public CacheImageTask(string cacheKey)
		{
			this.IsCanceled = false;
			this.CacheKey = cacheKey;
		}

		public Texture2D Tex { get; protected set; }

		public string CacheKey { get; private set; }

		public bool IsCanceled { get; protected set; }

		public bool Completed { get; protected set; }

		 
		public event Action<CacheImage> Complete;

		public void Cancel()
		{
			this.IsCanceled = true;
		}

		public bool IsRunning
		{
			get
			{
				return !this.IsCanceled && !this.Completed;
			}
		}

		protected void FireCompleted(CacheImage ci)
		{
			if (this.Completed)
			{
				return;
			}
			this.Completed = true;
			if (this.Complete != null)
			{
				this.Complete(ci);
			}
		}
	}
}

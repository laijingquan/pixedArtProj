// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

namespace FMImageManager
{
	public abstract class UnloadBehaviour
	{
		 protected bool LowMemoryMode {  get; private set; }

		public void SetLowMemMode()
		{
			this.LowMemoryMode = true;
		}

		public abstract void UpdateTick();

		public abstract int UnloadUnused();

		protected int CacheSort(CacheImage x, CacheImage y)
		{
			if (x.LastUse > y.LastUse)
			{
				return -1;
			}
			if (x.LastUse < y.LastUse)
			{
				return 1;
			}
			return 0;
		}

		protected List<CacheImage> cached;

		protected ILocalImageCache localRunner;
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

namespace FMImageManager
{
	public class LowEndUnloadBehaviour : UnloadBehaviour
	{
		public LowEndUnloadBehaviour(List<CacheImage> cached, ILocalImageCache localRunner)
		{
			this.cached = cached;
			this.localRunner = localRunner;
		}

		public override void UpdateTick()
		{
			if (this.cached.Count < 1)
			{
				return;
			}
			for (int i = this.cached.Count - 1; i >= 0; i--)
			{
				if (this.cached[i].RefCount == 0)
				{
					this.localRunner.DestroyTex(this.cached[i].Tex, false);
					this.cached.RemoveAt(i);
				}
			}
		}

		public override int UnloadUnused()
		{
			int num = 0;
			if (this.cached.Count > 0)
			{
				this.cached.Sort(delegate(CacheImage x, CacheImage y)
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
				});
				for (int i = this.cached.Count - 1; i >= 0; i--)
				{
					if (this.cached[i].RefCount == 0)
					{
						num++;
						this.localRunner.DestroyTex(this.cached[i].Tex, true);
						this.cached.RemoveAt(i);
					}
				}
			}
			return num;
		}
	}
}

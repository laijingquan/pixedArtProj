// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

namespace FMImageManager
{
	public class MidEndUnloadBehaviour : UnloadBehaviour
	{
		public MidEndUnloadBehaviour(List<CacheImage> cached, ILocalImageCache localRunner)
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
			if (base.LowMemoryMode)
			{
				for (int i = this.cached.Count - 1; i >= 0; i--)
				{
					if (this.cached[i].RefCount == 0)
					{
						this.localRunner.DestroyTex(this.cached[i].Tex, false);
						this.cached.RemoveAt(i);
					}
				}
			}
			else if (this.cached.Count > this.TEX_STORED_LIMIT)
			{
				int num = 0;
				this.cached.Sort(new Comparison<CacheImage>(base.CacheSort));
				for (int j = this.cached.Count - 1; j >= 0; j--)
				{
					if (this.cached[j].RefCount == 0)
					{
						this.localRunner.DestroyTex(this.cached[j].Tex, false);
						this.cached.RemoveAt(j);
						num++;
						if (num == this.UNLOAD_STEP)
						{
							break;
						}
					}
				}
			}
		}

		public override int UnloadUnused()
		{
			int num = 0;
			if (this.cached.Count > 0)
			{
				this.cached.Sort(new Comparison<CacheImage>(base.CacheSort));
				for (int i = this.cached.Count - 1; i >= 0; i--)
				{
					if (this.cached[i].RefCount == 0)
					{
						num++;
						this.localRunner.DestroyTex(this.cached[i].Tex, false);
						this.cached.RemoveAt(i);
						if (num == this.UNLOAD_STEP)
						{
							break;
						}
					}
				}
			}
			return num;
		}

		private int UNLOAD_STEP = 20;

		private int TEX_STORED_LIMIT = 150;
	}
}

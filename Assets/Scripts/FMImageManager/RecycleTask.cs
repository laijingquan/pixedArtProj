// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace FMImageManager
{
	internal class RecycleTask : CacheTask
	{
		public RecycleTask(string cacheKey, Texture2D tex, Action<Texture2D> destroy)
		{
			this.tex = tex;
			this.cacheKey = cacheKey;
			this.destroy = destroy;
		}

		public override IEnumerator Run()
		{
			if (this.tex != null)
			{
				string path = Application.temporaryCachePath + "/" + this.cacheKey;
				if (!File.Exists(path))
				{
					FileHelper.SaveTextureToFile(this.tex, path);
				}
				yield return 0;
			}
			if (this.tex != null)
			{
				this.destroy(this.tex);
				this.tex = null;
			}
			yield return 0;
			yield break;
		}

		private Texture2D tex;

		private string cacheKey;

		private Action<Texture2D> destroy;
	}
}

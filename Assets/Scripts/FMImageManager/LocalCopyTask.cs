// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace FMImageManager
{
	internal class LocalCopyTask : CacheTask
	{
		public LocalCopyTask(string cacheKey, Texture2D tex)
		{
			this.tex = tex;
			this.cacheKey = cacheKey;
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
			}
			yield return 0;
			yield break;
		}

		private Texture2D tex;

		private string cacheKey;
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace FMImageManager
{
	internal class GetCopyTask : CacheTask
	{
		public GetCopyTask(string cacheKey, Action<Texture2D> callback)
		{
			this.cacheKey = cacheKey;
			this.callback = callback;
		}

		public override IEnumerator Run()
		{
			string path = Application.temporaryCachePath + "/" + this.cacheKey;
			Texture2D tex = null;
			if (File.Exists(path))
			{
				tex = FileHelper.LoadCacheTextureFromFile(path);
				tex.wrapMode = TextureWrapMode.Clamp;
			}
			try
			{
				if (this.callback != null)
				{
					this.callback(tex);
				}
			}
			catch (Exception)
			{
				FMLogger.LogError("never should happen");
			}
			yield return 0;
			yield break;
		}

		private string cacheKey;

		private Action<Texture2D> callback;
	}
}

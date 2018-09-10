// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

namespace FMImageManager
{
	internal class LoadLocalTexTask : CacheTask
	{
		public LoadLocalTexTask(string path, Action<Texture2D> callback)
		{
			this.path = path;
			this.callback = callback;
		}

		public override IEnumerator Run()
		{
			Texture2D tex = null;
			try
			{
				tex = FileHelper.LoadTextureFromFile(this.path);
				tex.wrapMode = TextureWrapMode.Clamp;
			}
			catch (Exception)
			{
				FMLogger.LogError("tex not found: " + this.path);
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

		private string path;

		private Action<Texture2D> callback;
	}
}

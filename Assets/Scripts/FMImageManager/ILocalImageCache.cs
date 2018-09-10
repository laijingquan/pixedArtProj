// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

namespace FMImageManager
{
	public interface ILocalImageCache
	{
		void Init(LocalStorageRunner.QueueSpeed queueSpeed = LocalStorageRunner.QueueSpeed.Slow);

		void LoadLocalIcon(string path, Action<Texture2D> callback);

		void HasLocalCopy(string cacheKey, Action<Texture2D> callback);

		void DestroyTex(Texture2D tex, bool forceImmediate = false);

		void Recycle(string cacheKey, Texture2D tex);

		void MakeLocalCopy(string cacheKey, Texture2D tex);
	}
}

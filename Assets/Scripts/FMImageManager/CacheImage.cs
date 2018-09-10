// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

namespace FMImageManager
{
	public class CacheImage
	{
		public CacheImage(string cacheKey)
		{
			this.CacheKey = cacheKey;
		}

		public string CacheKey { get; private set; }

		public int RefCount { get; private set; }

		public long LastUse { get; private set; }

		public void AddRef(int count = 1)
		{
			this.RefCount += count;
			this.LastUse = DateTime.UtcNow.Ticks;
		}

		public void RemoveRef()
		{
			this.RefCount--;
		}

		public Texture2D Tex;
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class RTImageCache
{
	public void RecycleLocal(string key, Texture2D tex)
	{
		RTImageCache.CacheTex2D cacheTex2D;
		if (this.cachedTex.TryGetValue(key, out cacheTex2D))
		{
			cacheTex2D.refCount--;
		}
		else
		{
			cacheTex2D = new RTImageCache.CacheTex2D(tex, RTImageCache.TexType.Local);
			this.cachedTex.Add(key, cacheTex2D);
		}
	}

	public void RecycleWeb(string key, Texture2D tex)
	{
		RTImageCache.CacheTex2D cacheTex2D;
		if (this.cachedTex.TryGetValue(key, out cacheTex2D))
		{
			cacheTex2D.refCount--;
		}
		else
		{
			cacheTex2D = new RTImageCache.CacheTex2D(tex, RTImageCache.TexType.Local);
			this.cachedTex.Add(key, cacheTex2D);
		}
	}

	public Texture2D GetCopy(string key)
	{
		RTImageCache.CacheTex2D cacheTex2D;
		if (this.cachedTex.TryGetValue(key, out cacheTex2D))
		{
			UnityEngine.Debug.Log("found copy " + key);
			cacheTex2D.refCount++;
			return cacheTex2D.tex;
		}
		return null;
	}

	public void AddLocal(string key, Texture2D tex)
	{
		RTImageCache.CacheTex2D cacheTex2D;
		if (this.cachedTex.TryGetValue(key, out cacheTex2D))
		{
			UnityEngine.Debug.Log("*********Duplicate tex");
			cacheTex2D.refCount++;
		}
		else
		{
			cacheTex2D = new RTImageCache.CacheTex2D(tex, RTImageCache.TexType.Local);
			this.cachedTex.Add(key, cacheTex2D);
		}
	}

	public void AddWeb(string key, Texture2D tex)
	{
		RTImageCache.CacheTex2D cacheTex2D;
		if (this.cachedTex.TryGetValue(key, out cacheTex2D))
		{
			cacheTex2D.refCount++;
		}
		else
		{
			cacheTex2D = new RTImageCache.CacheTex2D(tex, RTImageCache.TexType.Web);
			this.cachedTex.Add(key, cacheTex2D);
		}
	}

	public void Clean()
	{
		//foreach (KeyValuePair<string, RTImageCache.CacheTex2D> keyValuePair in this.cachedTex)
		//{
		//}
		this.cachedTex.Clear();
	}

	private Dictionary<string, RTImageCache.CacheTex2D> cachedTex = new Dictionary<string, RTImageCache.CacheTex2D>();

	private class CacheTex2D
	{
		public CacheTex2D(Texture2D tex, RTImageCache.TexType texType)
		{
			this.refCount = 1;
			this.tex = tex;
			this.TexType = texType;
		}

		public RTImageCache.TexType TexType { get; private set; }

		public int refCount;

		public Texture2D tex;
	}

	private enum TexType
	{
		Local,
		Web
	}
}

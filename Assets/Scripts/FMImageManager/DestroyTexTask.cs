// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

namespace FMImageManager
{
	internal class DestroyTexTask : CacheTask
	{
		public DestroyTexTask(Texture2D tex, Action<Texture2D> destroy)
		{
			this.tex = tex;
			this.destroy = destroy;
		}

		public override IEnumerator Run()
		{
			if (this.tex != null)
			{
				this.destroy(this.tex);
				this.tex = null;
			}
			yield return 0;
			yield break;
		}

		private Texture2D tex;

		private Action<Texture2D> destroy;
	}
}

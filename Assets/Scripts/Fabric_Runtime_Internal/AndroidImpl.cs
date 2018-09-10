// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

namespace Fabric.Runtime.Internal
{
	internal class AndroidImpl : Impl
	{
		public override string Initialize()
		{
			return AndroidImpl.FabricInitializer.CallStatic<string>("JNI_InitializeFabric", new object[0]);
		}

		private static readonly AndroidJavaClass FabricInitializer = new AndroidJavaClass("io.fabric.unity.android.FabricInitializer");
	}
}

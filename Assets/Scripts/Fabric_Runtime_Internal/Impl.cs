// dnSpy decompiler from Assembly-CSharp.dll
using System;
using Fabric.Internal.Runtime;

namespace Fabric.Runtime.Internal
{
	internal class Impl
	{
		public static Impl Make()
		{
			return new AndroidImpl();
		}

		public virtual string Initialize()
		{
			Utils.Log("Fabric", "Method Initialize () is unimplemented on this platform");
			return string.Empty;
		}

		protected const string Name = "Fabric";
	}
}

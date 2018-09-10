// dnSpy decompiler from Assembly-CSharp.dll
using System;

namespace FMUILayout
{
	[Serializable]
	public class UISpriteSwapnData
	{
		public static UISpriteSwapnData FromSpriteIndex(int index)
		{
			return new UISpriteSwapnData
			{
				spriteIndex = index,
				hasData = true
			};
		}

		public int spriteIndex;

		public bool hasData;
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class UIEmptyGraphic : Graphic
	{
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
		}
	}
}

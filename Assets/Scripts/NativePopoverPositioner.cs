// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class NativePopoverPositioner : MonoBehaviour
{
	public Vector2 GetPosNormilized()
	{
		Transform parent = this.refRt.parent;
		this.refRt.SetParent(this.root);
		Vector2 result = this.refRt.anchoredPosition + new Vector2(0f, this.refRt.rect.height / 2f);
		this.refRt.SetParent(parent);
		float height = this.root.rect.height;
		float width = this.root.rect.width;
		float y;
		if (result.y > 0f)
		{
			y = (height / 2f - result.y) / height;
		}
		else
		{
			y = (height / 2f + Mathf.Abs(result.y)) / height;
		}
		float x;
		if (result.x > 0f)
		{
			x = (width / 2f + result.x) / width;
		}
		else
		{
			x = (width / 2f - Mathf.Abs(result.x)) / width;
		}
		result = new Vector2(x, y);
		return result;
	}

	public RectTransform root;

	public RectTransform refRt;
}

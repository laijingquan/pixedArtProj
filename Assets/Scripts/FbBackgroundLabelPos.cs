// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FbBackgroundLabelPos : MonoBehaviour
{
	private void OnEnable()
	{
		float height = this.root.rect.height;
		float num = (float)(this.categoryBar.SectionHeight + this.categoryBar.SectionOffset);
		float num2 = height - num;
		float num3 = 1f;
		if (num2 < (float)(this.btnHeight + this.labelHeight + 100))
		{
			num3 = num2 / ((float)(this.btnHeight + this.labelHeight) + 100f);
			this.label.localScale = new Vector3(num3, num3, 1f);
		}
		else
		{
			this.label.localScale = Vector3.one;
		}
		Vector2 anchoredPosition = new Vector2(this.label.anchoredPosition.x, -1f * (num2 / 2f + num3 * ((float)(this.labelHeight + this.btnHeight) / 2f - (float)this.btnHeight / 2f)) - (float)this.categoryBar.SectionHeight);
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			height,
			"x",
			num,
			" empty space: ",
			num2,
			" pos",
			anchoredPosition.y
		}));
		this.label.anchoredPosition = anchoredPosition;
	}

	public int btnHeight;

	public int labelHeight;

	[SerializeField]
	private RectTransform label;

	[SerializeField]
	private CategoryBar categoryBar;

	[SerializeField]
	private RectTransform root;
}

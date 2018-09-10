// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class ContentFilterMenuBar : MonoBehaviour
{
	public int SectionHeight
	{
		get
		{
			return (!SafeLayout.IsTablet) ? 80 : 70;
		}
	}

	public void SetCategory(string catName, int id, bool isEmpty)
	{
		this.label.text = catName;
		this.fbMenuBarLabel.SetActive(id == ContentFilterNavBar.FBCategoryId && !isEmpty);
		this.fbBackgroundLabel.SetActive(id == ContentFilterNavBar.FBCategoryId && isEmpty);
	}

	public void AddTopOffset(int offset)
	{
		RectTransform rectTransform = (RectTransform)base.transform;
		rectTransform.anchoredPosition += new Vector2(0f, (float)(-(float)offset));
		base.GetComponent<ScrollTitleBar>().AddTopOffset(offset);
	}

	[SerializeField]
	private GameObject fbMenuBarLabel;

	[SerializeField]
	private GameObject fbBackgroundLabel;

	[SerializeField]
	private Text label;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class CategoryFilterButton : MonoBehaviour
{
	public int CategoryId { get; private set; }

	public string CategoryName { get; private set; }

	public bool IsSelected { get; private set; }

	public void Init(int catId, string title)
	{
		this.CategoryName = title;
		this.CategoryId = catId;
		this.label.text = this.CategoryName;
		this.checkmark.SetActive(this.IsSelected);
		if (!GeneralSettings.IsOldDesign)
		{
			this.label.color = ((!this.IsSelected) ? this.textDeselectColor : this.textSelectColor);
		}
	}

	public int PrefferedTextWidth()
	{
		return (int)this.label.preferredWidth;
	}

	public int PrefferedWidth()
	{
		return (int)this.label.preferredWidth + this.textPadding * 2 + 5;
	}

	public void Select()
	{
		if (this.IsSelected)
		{
			return;
		}
		this.IsSelected = true;
		this.checkmark.SetActive(true);
		if (!GeneralSettings.IsOldDesign)
		{
			this.label.font = this.selectFont;
			this.label.color = this.textSelectColor;
		}
	}

	public void Deselect()
	{
		if (!this.IsSelected)
		{
			return;
		}
		this.IsSelected = false;
		this.checkmark.SetActive(false);
		if (!GeneralSettings.IsOldDesign)
		{
			this.label.font = this.deselectFont;
			this.label.color = this.textDeselectColor;
		}
	}

	[SerializeField]
	private Font selectFont;

	[SerializeField]
	private Font deselectFont;

	[SerializeField]
	private Color textSelectColor;

	[SerializeField]
	private Color textDeselectColor;

	[SerializeField]
	private int textPadding;

	[SerializeField]
	private GameObject checkmark;

	[SerializeField]
	private Text label;
}

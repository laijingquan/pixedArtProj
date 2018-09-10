// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ContentFilterNavBar : BottomSheetPopup, ICategoryFilterView
{
	public CategoryFilterButton ActiveCategory { get; private set; }

	public void SetSafeLayoutOffset(int yOffset)
	{
		this.safeLayoutOffset = yOffset;
		this.closedPosition = new Vector2(0f, (float)this.safeLayoutOffset);
	}

	public void Compose(List<CategoryInfo> catInfos, bool filterCompleted)
	{
		int num = this.safeLayoutOffset + (int)this.openPosition.y;
		string textByKey = LocalizationService.Instance.GetTextByKey("filter_hideColored");
		string textByKey2 = LocalizationService.Instance.GetTextByKey("filter_showAllCategories");
		this.completeFilterButton.Init(-1, textByKey);
		if (filterCompleted)
		{
			this.completeFilterButton.Select();
		}
		else
		{
			this.completeFilterButton.Deselect();
		}
		CategoryFilterButton categoryFilterButton = this.CreateButton();
		categoryFilterButton.Init(ContentFilterNavBar.ShowAllCategoryId, textByKey2);
		this.categories.Add(categoryFilterButton);
		for (int i = 0; i < catInfos.Count; i++)
		{
			CategoryFilterButton categoryFilterButton2 = this.CreateButton();
			categoryFilterButton2.Init(catInfos[i].id, catInfos[i].name);
			this.categories.Add(categoryFilterButton2);
		}
		int num2 = Mathf.Min(this.maxButtonsCount, this.categories.Count);
		int num3 = 0;
		if (num2 < this.categories.Count)
		{
			num3 += this.buttonOffset / 2;
		}
		((RectTransform)this.content).sizeDelta = new Vector2(((RectTransform)this.content).sizeDelta.x, (float)(this.buttonOffset * this.categories.Count));
		((RectTransform)this.content.parent).sizeDelta = new Vector2(((RectTransform)this.content.parent).sizeDelta.x, (float)(this.buttonOffset * num2 + num3));
		this.openPosition = new Vector2(0f, (float)(num + this.buttonOffset * num2 + num3));
	}

	public bool SelectCategory(int catId)
	{
		if (this.ActiveCategory != null && this.ActiveCategory.CategoryId == catId)
		{
			return false;
		}
		for (int i = 0; i < this.categories.Count; i++)
		{
			if (this.categories[i].CategoryId == catId)
			{
				this.categories[i].Select();
				this.ActiveCategory = this.categories[i];
			}
			else
			{
				this.categories[i].Deselect();
			}
		}
		return true;
	}

	public void FilterCompleted(bool filter)
	{
		if (filter)
		{
			this.completeFilterButton.Select();
		}
		else
		{
			this.completeFilterButton.Deselect();
		}
	}

	private CategoryFilterButton CreateButton()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefabButton);
		gameObject.transform.SetParent(this.content);
		gameObject.transform.localScale = Vector3.one;
		return gameObject.GetComponent<CategoryFilterButton>();
	}

	public int safeLayoutOffset;

	public int buttonOffset = 160;

	private int maxButtonsCount = 6;

	public static readonly int ShowAllCategoryId;

	public static readonly int FBCategoryId = 1000;

	[SerializeField]
	private Transform content;

	[SerializeField]
	private GameObject prefabButton;

	[SerializeField]
	private CategoryFilterButton completeFilterButton;

	private List<CategoryFilterButton> categories = new List<CategoryFilterButton>();
}

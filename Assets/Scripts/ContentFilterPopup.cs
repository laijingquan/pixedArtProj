// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentFilterPopup : MonoBehaviour, ICategoryFilterView
{
	public CategoryFilterButton ActiveCategory { get; private set; }

	public void Compose(List<CategoryInfo> catInfos, bool filterCompleted)
	{
		Vector2 anchoredPosition = this.rootRt.anchoredPosition;
		this.rootRt.anchoredPosition = new Vector2(-7000f, 0f);
		this.rootRt.gameObject.SetActive(true);
		string textByKey = LocalizationService.Instance.GetTextByKey("filter_hideColored");
		string textByKey2 = LocalizationService.Instance.GetTextByKey("filter_showAllCategories");
		this.completeFilterButton.Init(-1, textByKey);
		int num = 0;
		VerticalLayoutGroup component = this.content.GetComponent<VerticalLayoutGroup>();
		num += component.padding.top;
		num += component.padding.bottom;
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
		if (num2 < this.categories.Count)
		{
			num += this.buttonOffset / 2;
		}
		int num3 = this.completeFilterButton.PrefferedTextWidth();
		for (int j = 0; j < this.categories.Count; j++)
		{
			int num4 = this.categories[j].PrefferedTextWidth();
			if (num4 > num3)
			{
				num3 = num4;
			}
		}
		((RectTransform)this.content).sizeDelta = new Vector2(((RectTransform)this.content).sizeDelta.x, (float)(this.buttonOffset * this.categories.Count));
		((RectTransform)this.content.parent).sizeDelta = new Vector2(((RectTransform)this.content.parent).sizeDelta.x, (float)(this.buttonOffset * num2 + num));
		this.bgRt.sizeDelta += new Vector2((float)(num3 + 80), ((RectTransform)this.content.parent).sizeDelta.y);
		this.rootRt.gameObject.SetActive(false);
		this.rootRt.anchoredPosition = anchoredPosition;
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

	public int buttonOffset = 160;

	private int maxButtonsCount = 6;

	[SerializeField]
	private RectTransform rootRt;

	[SerializeField]
	private RectTransform bgRt;

	[SerializeField]
	private Transform content;

	[SerializeField]
	private GameObject prefabButton;

	[SerializeField]
	private CategoryFilterButton completeFilterButton;

	private List<CategoryFilterButton> categories = new List<CategoryFilterButton>();
}

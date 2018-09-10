// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class CategoryBar : MonoBehaviour
{
	public int SectionHeight
	{
		get
		{
			return (int)((RectTransform)base.transform).sizeDelta.y;
		}
	}

	public int SectionOffset
	{
		get
		{
			int result = 0;
			ScrollTitleBar component = base.GetComponent<ScrollTitleBar>();
			if (component != null)
			{
				result = -1 * (int)component.InitialOffset;
			}
			return result;
		}
	}

	public CategoryFilterButton ActiveCategory { get; private set; }

	public bool SelectCategory(int catId, bool scrollToSelected = false)
	{
		if (this.ActiveCategory != null && this.ActiveCategory.CategoryId == catId)
		{
			return false;
		}
		int index = -1;
		for (int i = 0; i < this.categories.Count; i++)
		{
			if (this.categories[i].CategoryId == catId)
			{
				index = i;
				this.categories[i].Select();
				this.ActiveCategory = this.categories[i];
			}
			else
			{
				this.categories[i].Deselect();
			}
		}
		if (this.positioner != null)
		{
			this.positioner.CheckFit(index);
		}
		else if (scrollToSelected)
		{
			this.layout.ScrollToIndex(index);
		}
		return true;
	}

	public void Compose(List<CategoryInfo> catInfos, bool filterCompleted)
	{
		this.btnPrefab = ((!SafeLayout.IsTablet) ? this.phoneBtnPrefab : this.tabletBtnPrefab);
		CategoryBar.ViewConfig config = (!SafeLayout.IsTablet) ? this.phoneConfig : this.tabletConfig;
		this.layout.Init(config);
		string textByKey = LocalizationService.Instance.GetTextByKey("filter_showAllCategories");
		CategoryFilterButton categoryFilterButton = this.CreateButton();
		categoryFilterButton.Init(CategoryBar.ShowAllCategoryId, textByKey);
		this.layout.Add(categoryFilterButton);
		this.categories.Add(categoryFilterButton);
		for (int i = 0; i < catInfos.Count; i++)
		{
			CategoryFilterButton categoryFilterButton2 = this.CreateButton();
			categoryFilterButton2.Init(catInfos[i].id, catInfos[i].name);
			this.layout.Add(categoryFilterButton2);
			this.categories.Add(categoryFilterButton2);
		}
		this.positioner = this.layout.transform.parent.GetComponent<ScrollElementPositioner>();
		if (this.positioner != null)
		{
			List<Vector2> list = new List<Vector2>();
			for (int j = 0; j < this.categories.Count; j++)
			{
				list.Add(((RectTransform)this.categories[j].transform).anchoredPosition);
			}
			this.positioner.Init(list, config.padding, config.spacing);
		}
	}

	private CategoryFilterButton CreateButton()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.btnPrefab);
		return gameObject.GetComponent<CategoryFilterButton>();
	}

	public void AddTopOffset(int offset)
	{
		RectTransform rectTransform = (RectTransform)base.transform;
		rectTransform.anchoredPosition += new Vector2(0f, (float)(-(float)offset));
		base.GetComponent<ScrollTitleBar>().AddTopOffset(offset);
	}

	public static readonly int ShowAllCategoryId;

	public static readonly int FBCategoryId = 1000;

	[SerializeField]
	private GameObject tabletBtnPrefab;

	[SerializeField]
	private GameObject phoneBtnPrefab;

	private GameObject btnPrefab;

	[SerializeField]
	private CategoryBarLayout layout;

	private ScrollElementPositioner positioner;

	private CategoryBar.ViewConfig phoneConfig = new CategoryBar.ViewConfig
	{
		barHeight = 133,
		padding = 0,
		spacing = 1
	};

	private CategoryBar.ViewConfig tabletConfig = new CategoryBar.ViewConfig
	{
		barHeight = 115,
		padding = 0,
		spacing = 1
	};

	private List<CategoryFilterButton> categories = new List<CategoryFilterButton>();

	public struct ViewConfig
	{
		public int padding;

		public int spacing;

		public int barHeight;
	}
}

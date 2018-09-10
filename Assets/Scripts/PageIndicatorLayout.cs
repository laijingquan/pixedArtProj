// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class PageIndicatorLayout : MonoBehaviour
{
	public void Init(int itemsCount)
	{
		for (int i = 0; i < itemsCount; i++)
		{
			this.AddItem();
		}
	}

	private void AddItem()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.pageIndicatorPrefab);
		gameObject.transform.SetParent(this.content);
		gameObject.transform.localScale = Vector3.one;
		PageIndicatorItem component = gameObject.GetComponent<PageIndicatorItem>();
		this.pages.Add(component);
	}

	public void OnPageChanged(int page)
	{
		if (this.currentPage == page)
		{
			return;
		}
		if (this.currentPage >= 0 && this.currentPage < this.pages.Count)
		{
			this.pages[this.currentPage].Deselect();
		}
		this.currentPage = page;
		if (this.currentPage >= 0 && this.currentPage < this.pages.Count)
		{
			this.pages[this.currentPage].Select();
		}
	}

	[SerializeField]
	private RectTransform content;

	[SerializeField]
	private GameObject pageIndicatorPrefab;

	private int currentPage = -1;

	private List<PageIndicatorItem> pages = new List<PageIndicatorItem>();
}

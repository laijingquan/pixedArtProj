// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public interface ICategoryFilterView
{
	CategoryFilterButton ActiveCategory { get; }

	void FilterCompleted(bool filter);

	bool SelectCategory(int catId);

	void Compose(List<CategoryInfo> catInfos, bool filterCompleted);
}

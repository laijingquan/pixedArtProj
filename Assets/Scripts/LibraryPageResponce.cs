// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

[Serializable]
public class LibraryPageResponce
{
	public bool IsValid()
	{
		if (!this.IsValidPaths(this.paths))
		{
			return false;
		}
		if (this.content == null)
		{
			return false;
		}
		for (int i = this.content.Count - 1; i >= 0; i--)
		{
			if (!this.content[i].IsValid())
			{
				this.content.RemoveAt(i);
			}
		}
		if (this.content.Count < 1)
		{
			return false;
		}
		if (this.categories != null)
		{
			for (int j = this.categories.Count - 1; j >= 0; j--)
			{
				if (string.IsNullOrEmpty(this.categories[j].name))
				{
					FMLogger.vCore("filter cat.");
					this.categories.RemoveAt(j);
				}
			}
		}
		if (this.featured != null)
		{
			if (this.featured.daily_items != null)
			{
				for (int k = this.featured.daily_items.Count - 1; k >= 0; k--)
				{
					if (this.featured.daily_items[k].pic == null || !this.featured.daily_items[k].pic.IsValid())
					{
						FMLogger.vCore("filter daily.");
						this.featured.daily_items.RemoveAt(k);
					}
				}
			}
			if (this.featured.editor_choice != null)
			{
				for (int l = this.featured.editor_choice.Count - 1; l >= 0; l--)
				{
					if (this.featured.editor_choice[l].pic == null || !this.featured.editor_choice[l].pic.IsValid())
					{
						FMLogger.vCore("filter promo pic.");
						this.featured.editor_choice.RemoveAt(l);
					}
				}
			}
			if (this.featured.external_links != null)
			{
				for (int m = this.featured.external_links.Count - 1; m >= 0; m--)
				{
					bool flag = false;
					FeaturedExternalLink featuredExternalLink = this.featured.external_links[m];
					if (string.IsNullOrEmpty(featuredExternalLink.target_url))
					{
						flag = true;
					}
					if (!this.IsValidPaths(featuredExternalLink.paths))
					{
						flag = true;
					}
					if (string.IsNullOrEmpty(featuredExternalLink.img_url))
					{
						flag = true;
					}
					if (flag)
					{
						this.featured.external_links.RemoveAt(m);
					}
				}
			}
		}
		if (this.update_dialog != null && (string.IsNullOrEmpty(this.update_dialog.body) || string.IsNullOrEmpty(this.update_dialog.title) || string.IsNullOrEmpty(this.update_dialog.yes) || string.IsNullOrEmpty(this.update_dialog.no)))
		{
			this.update_dialog = null;
		}
		return true;
	}

	private bool IsValidPaths(string[] paths)
	{
		if (paths == null || paths.Length < 1)
		{
			return false;
		}
		bool result = false;
		for (int i = 0; i < paths.Length; i++)
		{
			if (string.IsNullOrEmpty(paths[i]))
			{
				paths[i] = string.Empty;
			}
			else
			{
				result = true;
			}
		}
		return result;
	}

	public string[] paths;

	public List<WebPicData> content;

	public List<CategoryInfo> categories;

	public FeaturedResponse featured;

	public NewsResponse news;

	public UpdateDialog update_dialog;

	public BonusCategoryConfig bonus_category_config;
}

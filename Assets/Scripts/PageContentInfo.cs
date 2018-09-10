// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class PageContentInfo
{
	public PageContentInfo(List<PictureData> pics, FeaturedInfo featured, NewsInfo news, List<CategoryInfo> categoryInfos, UpdateDialog updateDialog, BonusCategoryConfig bonusCategoryConfig)
	{
		this.Pics = pics;
		this.Featured = featured;
		this.CategoryInfos = categoryInfos;
		this.UpdateDialog = updateDialog;
		this.BonusCategoryConfig = bonusCategoryConfig;
		this.News = news;
	}

	public List<PictureData> Pics { get; private set; }

	public FeaturedInfo Featured { get; private set; }

	public List<CategoryInfo> CategoryInfos { get; private set; }

	public UpdateDialog UpdateDialog { get; private set; }

	public BonusCategoryConfig BonusCategoryConfig { get; private set; }

	public NewsInfo News { get; private set; }
}

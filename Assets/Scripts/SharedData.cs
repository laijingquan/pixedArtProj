// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SimpleSQL;
using UnityEngine;

public class SharedData : MonoBehaviour
{
	public static SharedData Instance
	{
		get
		{
			return SharedData.instance;
		}
	}

	public event Action<PictureData, PictureData> PictureDataDeleted;

	public event Action<List<PictureData>> NewPicturesAdded;

	public List<PictureData> GetMainPageData
	{
		get
		{
			return this.orderedMainPage;
		}
	}

	private void Awake()
	{
		if (SharedData.instance != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(this);
		SharedData.instance = this;
		this.picDataDownloader = new PictureDataDownloader(new Func<IEnumerator, Coroutine>(base.StartCoroutine), new Action<Coroutine>(base.StopCoroutine));
	}

	public void Init()
	{
		if (this.inited)
		{
			return;
		}
		if (!this.inited)
		{
			this.inited = true;
		}
		string @string = PlayerPrefs.GetString("packsdKey", string.Empty);
		PackCollectionIndex packCollectionIndex;
		if (string.IsNullOrEmpty(@string))
		{
			packCollectionIndex = new PackCollectionIndex();
			PlayerPrefs.SetString("packsdKey", JsonUtility.ToJson(packCollectionIndex));
		}
		else
		{
			packCollectionIndex = JsonUtility.FromJson<PackCollectionIndex>(@string);
		}
		Dictionary<int, PicturePack> dictionary = new Dictionary<int, PicturePack>();
		for (int i = packCollectionIndex.packIds.Count - 1; i >= 0; i--)
		{
			int num = packCollectionIndex.packIds[i];
			string string2 = PlayerPrefs.GetString(PackCollectionIndex.IndexToKey(num), string.Empty);
			if (!string.IsNullOrEmpty(string2))
			{
				PicturePack value = JsonUtility.FromJson<PicturePack>(string2);
				dictionary.Add(num, value);
			}
			else
			{
				packCollectionIndex.packIds.RemoveAt(i);
			}
		}
		string string3 = PlayerPrefs.GetString("feedKey", string.Empty);
		FeedData feedData;
		if (string.IsNullOrEmpty(string3))
		{
			feedData = new FeedData();
			feedData.saves = new List<PictureSaveData>();
			PlayerPrefs.SetString("feedKey", JsonUtility.ToJson(feedData));
			PlayerPrefs.Save();
		}
		else
		{
			feedData = JsonUtility.FromJson<FeedData>(string3);
		}
		this.CleanSysPack(feedData);
		PicturePack pack = null;
		if (dictionary.ContainsKey(0))
		{
			pack = dictionary[0];
		}
		DatabaseManager.Init(this.simpleSqlManager, pack, (feedData == null) ? null : feedData.saves);
	}

	private void CleanSysPack(FeedData feed)
	{
		List<PictureSaveData> saves = feed.saves;
		string @string = PlayerPrefs.GetString("sysPackKey", string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return;
		}
		int num = 0;
		for (int i = saves.Count - 1; i >= 0; i--)
		{
			if (saves[i].PackId == -1)
			{
				num++;
				saves.RemoveAt(i);
			}
		}
		PlayerPrefs.SetString("sysPackKey", string.Empty);
		PlayerPrefs.SetString("feedKey", JsonUtility.ToJson(feed));
		PlayerPrefs.Save();
		AnalyticsManager.SysCleanup(num);
	}

	public void LoadDailyPage(DailyPageContentTask dailyPageTask)
	{
		if (this.dailyPageResponse != null && this.dailyTabInfo != null)
		{
			dailyPageTask.Result(true, this.dailyTabInfo);
		}
		else
		{
			if (this.dailyPageTaskTimeoutCoroutine != null)
			{
				FMLogger.Log("daily Multi page load invocation");
				base.StopCoroutine(this.dailyPageTaskTimeoutCoroutine);
				FMLogger.Log("terminate prev daily page task handler");
			}
			this.dailyPageTaskTimeoutCoroutine = base.StartCoroutine(this.DailyPageTaskTimeout(dailyPageTask, 5f));
			TGFModule.Instance.GetDailyPage(delegate(DailyPageResponse response)
			{
				if (dailyPageTask == null || dailyPageTask.IsCanceled)
				{
					FMLogger.Log("daily task canceled " + dailyPageTask.Page);
				}
				else if (dailyPageTask != null && dailyPageTask.Completed)
				{
					FMLogger.Log("daily task already completed " + dailyPageTask.Page);
				}
				else
				{
					if (this.dailyPageTaskTimeoutCoroutine != null)
					{
						this.StopCoroutine(this.dailyPageTaskTimeoutCoroutine);
						this.dailyPageTaskTimeoutCoroutine = null;
					}
					if (response != null)
					{
						this.ParseDailyPageContent(response);
						this.dailyPageResponse = response;
						dailyPageTask.Result(true, this.dailyTabInfo);
					}
					else
					{
						dailyPageTask.Result(false, null);
					}
				}
			});
		}
	}

	public void LoadGiftCode(BonusCodeContentTask task)
	{
		TGFModule.Instance.GetBonusCodeContent(task.GiftCode, delegate(BonusCodeResponse response)
		{
			if (response != null)
			{
				List<PictureData> list = this.ParseGiftCodeContent(response);
				this.JoinGiftPicsToResp(list);
				task.Result(true, list);
			}
			else
			{
				task.Result(false, null);
			}
		});
	}

	public void LoadMainPage(PageContentTask pageTask)
	{
		if (pageTask.Page == 1 && this.orderedMainPage != null && this.orderedMainPage.Count > 0 && this.libResponse != null)
		{
			pageTask.Result(true, new PageContentInfo(this.orderedMainPage, this.featuredSection, this.newsTabInfo, this.libResponse.categories, this.libResponse.update_dialog, this.libResponse.bonus_category_config));
		}
		else if (pageTask.Page == 1)
		{
			if (this.mainPageTaskTimeoutCoroutine != null)
			{
				FMLogger.Log("Multi page load invocation");
				base.StopCoroutine(this.mainPageTaskTimeoutCoroutine);
				FMLogger.Log("terminate prev main page task handler");
			}
			this.mainPageTaskTimeoutCoroutine = base.StartCoroutine(this.MainPageTaskTimeout(pageTask, 5f));
			TGFModule.Instance.GetMainPage(pageTask.Page, delegate(LibraryPageResponce response)
			{
				if (pageTask == null || pageTask.IsCanceled)
				{
					FMLogger.Log("task canceled " + pageTask.Page);
				}
				else if (pageTask != null && pageTask.Completed)
				{
					FMLogger.Log("task already completed " + pageTask.Page);
				}
				else
				{
					if (this.mainPageTaskTimeoutCoroutine != null)
					{
						this.StopCoroutine(this.mainPageTaskTimeoutCoroutine);
						this.mainPageTaskTimeoutCoroutine = null;
					}
					if (response != null)
					{
						this.ParseMainPageContent(response);
						this.libResponse = response;
						pageTask.Result(true, new PageContentInfo(this.orderedMainPage, this.featuredSection, this.newsTabInfo, this.libResponse.categories, this.libResponse.update_dialog, this.libResponse.bonus_category_config));
					}
					else
					{
						pageTask.Result(false, null);
					}
				}
			});
		}
	}

	public void DownloadPicture(DownloadPictureTask task)
	{
		this.picDataDownloader.Run(task);
	}

	public void ClearCache()
	{
		this.orderedMainPage.Clear();
		this.libResponse = null;
		this.dailyPageResponse = null;
		this.featuredSection = new FeaturedInfo();
		this.newsTabInfo = new NewsInfo();
		this.dailyTabInfo = new DailyTabInfo();
		TGFModule.Instance.ClearPageCache();
	}

	private List<PictureData> ParseGiftCodeContent(BonusCodeResponse response)
	{
		List<PictureData> list = new List<PictureData>();
		if (response.content != null)
		{
			for (int i = 0; i < response.content.Length; i++)
			{
				WebPicData webPic = response.content[i];
				PictureData pictureData = this.ParseWebPic(webPic, response.paths, null, true);
				if (pictureData != null)
				{
					list.Add(pictureData);
				}
			}
		}
		return list;
	}

	private void JoinGiftPicsToResp(List<PictureData> data)
	{
		//SharedData.<JoinGiftPicsToResp>c__AnonStorey5 <JoinGiftPicsToResp>c__AnonStorey = new SharedData.<JoinGiftPicsToResp>c__AnonStorey5();
		//<JoinGiftPicsToResp>c__AnonStorey.data = data;
		//List<PictureData> list = new List<PictureData>();
		//if (this.orderedMainPage == null)
		//{
		//	return;
		//}
		//int i;
		//for (i = 0; i < <JoinGiftPicsToResp>c__AnonStorey.data.Count; i++)
		//{
		//	if (this.orderedMainPage.Find((PictureData p) => p.Id == <JoinGiftPicsToResp>c__AnonStorey.data[i].Id) == null)
		//	{
		//		list.Add(<JoinGiftPicsToResp>c__AnonStorey.data[i]);
		//	}
		//}
		//if (list.Count > 0)
		//{
		//	this.orderedMainPage.InsertRange(0, list);
		//	FMLogger.vCore("added " + list.Count + " bones pics");
		//	if (this.NewPicturesAdded != null)
		//	{
		//		this.NewPicturesAdded(list);
		//	}
		//}
	}

	private void ParseDailyPageContent(DailyPageResponse response)
	{
		List<PictureData> localPictures = DatabaseManager.GetLocalPictures();
		this.dailyTabInfo = new DailyTabInfo();
		this.dailyTabInfo.monthes = new List<DailyMonthInfo>();
		if (response.months != null)
		{
			for (int i = 0; i < response.months.Count; i++)
			{
				DailyMonthInfo dailyMonthInfo = new DailyMonthInfo();
				dailyMonthInfo.monthName = ((!string.IsNullOrEmpty(response.months[i].monthName)) ? response.months[i].monthName : string.Empty);
				dailyMonthInfo.monthIndex = response.months[i].monthIndex;
				dailyMonthInfo.year = response.months[i].year;
				dailyMonthInfo.pics = new List<PictureData>();
				for (int j = 0; j < response.months[i].pics.Count; j++)
				{
					WebPicData webPic = response.months[i].pics[j];
					PictureData pictureData = this.ParseWebPic(webPic, response.paths, localPictures, false);
					if (pictureData != null)
					{
						pictureData.SetDailyTabDate(response.months[i].pics.Count - j);
						pictureData.SetPicClass(PicClass.Daily);
						dailyMonthInfo.pics.Add(pictureData);
					}
				}
				this.dailyTabInfo.monthes.Add(dailyMonthInfo);
			}
		}
		if (response.daily != null)
		{
			this.dailyTabInfo.dailyPic = new DailyPicInfo();
			this.dailyTabInfo.dailyPic.day = ((!string.IsNullOrEmpty(response.daily.day)) ? response.daily.day : string.Empty);
			this.dailyTabInfo.dailyPic.desc = ((!string.IsNullOrEmpty(response.daily.description)) ? response.daily.description : string.Empty);
			this.dailyTabInfo.dailyPic.month = ((!string.IsNullOrEmpty(response.daily.month)) ? response.daily.month : string.Empty);
			this.dailyTabInfo.dailyPic.btnLabel = (response.daily.cta ?? string.Empty);
			PictureData pictureData2 = this.ParseWebPic(response.daily.pic, response.paths, localPictures, false);
			if (pictureData2 != null)
			{
				pictureData2.SetPicClass(PicClass.Daily);
				this.dailyTabInfo.dailyPic.picData = pictureData2;
			}
			else
			{
				this.dailyTabInfo.dailyPic = null;
			}
		}
	}

	private void ParseMainPageContent(LibraryPageResponce responce)
	{
		List<PictureData> localPictures = DatabaseManager.GetLocalPictures();
		List<PictureData> list = new List<PictureData>();
		for (int i = 0; i < responce.content.Count; i++)
		{
			WebPicData webPic = responce.content[i];
			PictureData pictureData = this.ParseWebPic(webPic, responce.paths, localPictures, false);
			if (pictureData != null)
			{
				list.Add(pictureData);
			}
		}
		this.orderedMainPage = list;
		this.featuredSection = new FeaturedInfo();
		if (responce.featured != null)
		{
			this.featuredSection.dailies = new List<DailyPicInfo>();
			this.featuredSection.promoPics = new List<PromoPicInfo>();
			this.featuredSection.externalLinks = new List<ExternalLinkInfo>();
			if (responce.featured.daily_items != null)
			{
				for (int j = 0; j < responce.featured.daily_items.Count; j++)
				{
					FeaturedDailyResp tmp = responce.featured.daily_items[j];
					DailyPicInfo dailyPicInfo = new DailyPicInfo();
					dailyPicInfo.order = tmp.order;
					dailyPicInfo.id = tmp.id;
					dailyPicInfo.desc = (tmp.description ?? string.Empty);
					dailyPicInfo.day = (tmp.day ?? string.Empty);
					dailyPicInfo.month = (tmp.month ?? string.Empty);
					dailyPicInfo.btnLabel = (tmp.cta ?? string.Empty);
					PictureData pictureData2 = this.orderedMainPage.Find((PictureData p) => p.Id == tmp.pic.id);
					if (pictureData2 != null)
					{
						dailyPicInfo.picData = pictureData2;
					}
					else
					{
						dailyPicInfo.picData = this.ParseWebPic(tmp.pic, responce.paths, localPictures, false);
					}
					dailyPicInfo.picData.SetPicClass(PicClass.Daily);
					this.featuredSection.dailies.Add(dailyPicInfo);
				}
			}
			if (responce.featured.editor_choice != null)
			{
				for (int k = 0; k < responce.featured.editor_choice.Count; k++)
				{
					FeaturedEditorChoiceResp tmp = responce.featured.editor_choice[k];
					PromoPicInfo promoPicInfo = new PromoPicInfo();
					promoPicInfo.order = tmp.order;
					promoPicInfo.id = tmp.id;
					promoPicInfo.desc = (tmp.description ?? string.Empty);
					promoPicInfo.title = (tmp.title ?? string.Empty);
					promoPicInfo.btnLabel = (tmp.cta ?? string.Empty);
					PictureData pictureData3 = this.orderedMainPage.Find((PictureData p) => p.Id == tmp.pic.id);
					if (pictureData3 != null)
					{
						promoPicInfo.picData = pictureData3;
					}
					else
					{
						promoPicInfo.picData = this.ParseWebPic(tmp.pic, responce.paths, localPictures, false);
					}
					this.featuredSection.promoPics.Add(promoPicInfo);
				}
			}
			if (responce.featured.external_links != null)
			{
				for (int l = 0; l < responce.featured.external_links.Count; l++)
				{
					FeaturedExternalLink featuredExternalLink = responce.featured.external_links[l];
					ExternalLinkInfo externalLinkInfo = new ExternalLinkInfo();
					externalLinkInfo.id = featuredExternalLink.id;
					externalLinkInfo.order = featuredExternalLink.order;
					externalLinkInfo.desc = (featuredExternalLink.description ?? string.Empty);
					externalLinkInfo.title = (featuredExternalLink.title ?? string.Empty);
					externalLinkInfo.btnLabel = (featuredExternalLink.cta ?? string.Empty);
					externalLinkInfo.targetUrl = featuredExternalLink.target_url;
					externalLinkInfo.targetScheme = (featuredExternalLink.target_scheme ?? string.Empty);
					externalLinkInfo.imageData = new ImageData(featuredExternalLink.paths, featuredExternalLink.img_url);
					this.featuredSection.externalLinks.Add(externalLinkInfo);
				}
			}
		}
		this.featuredSection.OrderItems();
		this.newsTabInfo = new NewsInfo();
		if (responce.news != null)
		{
			this.newsTabInfo.promoPics = new List<PromoPicInfo>();
			this.newsTabInfo.externalLinks = new List<ExternalLinkInfo>();
			if (responce.news.editor_choice != null)
			{
				for (int m = 0; m < responce.news.editor_choice.Count; m++)
				{
					FeaturedEditorChoiceResp tmp = responce.news.editor_choice[m];
					PromoPicInfo promoPicInfo2 = new PromoPicInfo();
					promoPicInfo2.id = tmp.id;
					promoPicInfo2.order = tmp.order;
					promoPicInfo2.desc = (tmp.description ?? string.Empty);
					promoPicInfo2.title = (tmp.title ?? string.Empty);
					promoPicInfo2.btnLabel = (tmp.cta ?? string.Empty);
					PictureData pictureData4 = this.orderedMainPage.Find((PictureData p) => p.Id == tmp.pic.id);
					if (pictureData4 != null)
					{
						promoPicInfo2.picData = pictureData4;
					}
					else
					{
						promoPicInfo2.picData = this.ParseWebPic(tmp.pic, responce.paths, localPictures, false);
					}
					this.newsTabInfo.promoPics.Add(promoPicInfo2);
				}
			}
			if (responce.news.external_links != null)
			{
				for (int n = 0; n < responce.news.external_links.Count; n++)
				{
					FeaturedExternalLink featuredExternalLink2 = responce.news.external_links[n];
					ExternalLinkInfo externalLinkInfo2 = new ExternalLinkInfo();
					externalLinkInfo2.id = featuredExternalLink2.id;
					externalLinkInfo2.order = featuredExternalLink2.order;
					externalLinkInfo2.desc = (featuredExternalLink2.description ?? string.Empty);
					externalLinkInfo2.title = (featuredExternalLink2.title ?? string.Empty);
					externalLinkInfo2.btnLabel = (featuredExternalLink2.cta ?? string.Empty);
					externalLinkInfo2.targetUrl = featuredExternalLink2.target_url;
					externalLinkInfo2.targetScheme = (featuredExternalLink2.target_scheme ?? string.Empty);
					externalLinkInfo2.imageData = new ImageData(featuredExternalLink2.paths, featuredExternalLink2.img_url);
					this.newsTabInfo.externalLinks.Add(externalLinkInfo2);
				}
			}
		}
		this.newsTabInfo.OrderItems();
	}

	private PictureData ParseWebPic(WebPicData webPic, string[] serverPaths, List<PictureData> pics, bool forceManualSearch = false)
	{
		if (pics != null)
		{
			PictureData pictureData = pics.Find((PictureData pd) => pd.Id == webPic.id);
			if (pictureData != null)
			{
				pictureData.AddExtras(WebPicData.ParseLabels(webPic.labels), webPic.categories);
				if (pictureData.Solved)
				{
					PictureSaveData save = this.GetSave(pictureData);
					if (save != null && save.progres == 100)
					{
						pictureData.SetCompleted();
					}
				}
				return pictureData;
			}
		}
		else if (forceManualSearch)
		{
			PictureData pictureData2 = this.GetPictureData(webPic.id);
			if (pictureData2 != null)
			{
				pictureData2.AddExtras(WebPicData.ParseLabels(webPic.labels), webPic.categories);
				if (pictureData2.Solved)
				{
					PictureSaveData save2 = this.GetSave(pictureData2);
					if (save2 != null && save2.progres == 100)
					{
						pictureData2.SetCompleted();
					}
				}
				return pictureData2;
			}
		}
		PictureData pictureData3 = new PictureData(webPic, serverPaths);
		pictureData3.AddExtras(WebPicData.ParseLabels(webPic.labels), webPic.categories);
		return pictureData3;
	}

	private IEnumerator MainPageTaskTimeout(PageContentTask pageTask, float warningTime)
	{
		yield return new WaitForSeconds(warningTime);
		if (pageTask != null && !pageTask.Completed)
		{
			FMLogger.Log("main page long load report");
			pageTask.TriggerLongLoading();
		}
		else
		{
			FMLogger.Log("Wtf task is completed");
		}
		this.mainPageTaskTimeoutCoroutine = null;
		yield break;
	}

	private IEnumerator DailyPageTaskTimeout(DailyPageContentTask pageTask, float warningTime)
	{
		yield return new WaitForSeconds(warningTime);
		if (pageTask != null && !pageTask.Completed)
		{
			FMLogger.Log("daily page long load report");
			pageTask.TriggerWarning();
		}
		else
		{
			FMLogger.Log("daily Wtf task is completed");
		}
		this.dailyPageTaskTimeoutCoroutine = null;
		yield break;
	}

	public PictureSaveData GetSave(PictureData picData)
	{
		return DatabaseManager.GetSave(picData);
	}

	public void DeleteSave(PictureData picData)
	{
		DatabaseManager.DeleteSave(picData.Id, true);
		if (this.orderedMainPage != null)
		{
			for (int i = 0; i < this.orderedMainPage.Count; i++)
			{
				if (this.orderedMainPage[i].Id == picData.Id)
				{
					this.orderedMainPage[i].SetSaveState(false);
				}
			}
		}
		if (this.featuredSection != null)
		{
			this.featuredSection.UpdateSaveState(picData.Id, false);
		}
		if (this.newsTabInfo != null)
		{
			this.newsTabInfo.UpdateSaveState(picData.Id, false);
		}
		if (this.dailyTabInfo != null)
		{
			this.dailyTabInfo.UpdateSaveState(picData.Id, false);
		}
		picData.SetSaveState(false);
	}

	public List<PictureSaveData> GetSaves()
	{
		List<PictureSaveData> saves = DatabaseManager.GetSaves();
		if (GeneralSettings.SavesCount > 0 && saves.Count == 0)
		{
			AnalyticsManager.PossibleProgressLost(GeneralSettings.SavesCount);
		}
		GeneralSettings.SavesCount = saves.Count;
		return saves;
	}

	public PictureData GetPictureData(int id)
	{
		return DatabaseManager.GetPicture(id);
	}

	public void UpdatePicuteData(PictureData pd)
	{
		DatabaseManager.UpdatePicture(pd);
		for (int i = 0; i < this.orderedMainPage.Count; i++)
		{
			if (this.orderedMainPage[i].Id == pd.Id)
			{
				if (pd.IsCompleted())
				{
					this.orderedMainPage[i].SetCompleted();
				}
				break;
			}
		}
	}

	public void DeletePictureData(PictureData pd)
	{
		FMLogger.Log("del pd invoke. " + pd.Id);
		DatabaseManager.DeletePicture(pd);
		FileHelper.DeleteFile(pd.Icon);
		FileHelper.DeleteFile(pd.LineArt);
		if (pd.FillType == FillAlgorithm.Flood)
		{
			FileHelper.DeleteFile(pd.ColorMap);
		}
		FileHelper.DeleteFile(pd.Palette);
		PictureData pictureData = null;
		if (this.libResponse != null)
		{
			int num = -1;
			for (int i = 0; i < this.libResponse.content.Count; i++)
			{
				if (this.libResponse.content[i].id == pd.Id)
				{
					num = i;
					FMLogger.Log("found respone match " + i);
					break;
				}
			}
			if (num != -1)
			{
				pictureData = this.ParseWebPic(this.libResponse.content[num], this.libResponse.paths, null, false);
				pictureData.Solved = pd.Solved;
				if (pd.PicClass == PicClass.Daily)
				{
					pictureData.SetPicClass(PicClass.Daily);
				}
				else if (pd.PicClass == PicClass.FacebookGift)
				{
					pictureData.SetPicClass(PicClass.FacebookGift);
				}
				for (int j = 0; j < this.orderedMainPage.Count; j++)
				{
					if (this.orderedMainPage[j].Id == pictureData.Id)
					{
						this.orderedMainPage[j] = pictureData;
						FMLogger.Log("found and replaced local pd with web pd in main page pics");
						break;
					}
				}
				if (this.featuredSection != null)
				{
					this.featuredSection.UpdatePicData(pictureData);
				}
				if (this.newsTabInfo != null)
				{
					this.newsTabInfo.UpdatePicData(pictureData);
				}
				if (this.dailyTabInfo != null)
				{
					this.dailyTabInfo.UpdatePicData(pictureData);
				}
			}
			else
			{
				WebPicData webPicData = null;
				if (this.libResponse.featured != null)
				{
					if (this.libResponse.featured.daily_items != null)
					{
						for (int k = 0; k < this.libResponse.featured.daily_items.Count; k++)
						{
							if (this.libResponse.featured.daily_items[k].pic != null && this.libResponse.featured.daily_items[k].pic.id == pd.Id)
							{
								webPicData = this.libResponse.featured.daily_items[k].pic;
								break;
							}
						}
					}
					if (webPicData == null && this.dailyPageResponse != null)
					{
						bool flag = false;
						if (this.dailyPageResponse.months != null)
						{
							for (int l = 0; l < this.dailyPageResponse.months.Count; l++)
							{
								if (this.dailyPageResponse.months[l].pics != null)
								{
									for (int m = 0; m < this.dailyPageResponse.months[l].pics.Count; m++)
									{
										if (this.dailyPageResponse.months[l].pics[m].id == pd.Id)
										{
											webPicData = this.dailyPageResponse.months[l].pics[m];
											flag = true;
											break;
										}
									}
								}
								if (flag)
								{
									break;
								}
							}
						}
						if (!flag && this.dailyPageResponse.daily != null && this.dailyPageResponse.daily.pic != null && this.dailyPageResponse.daily.pic.id == pd.Id)
						{
							webPicData = this.dailyPageResponse.daily.pic;
						}
					}
				}
				if (webPicData != null)
				{
					pictureData = this.ParseWebPic(webPicData, this.libResponse.paths, null, false);
					pictureData.Solved = pd.Solved;
					if (pd.PicClass == PicClass.Daily)
					{
						pictureData.SetPicClass(PicClass.Daily);
					}
					if (this.featuredSection != null)
					{
						this.featuredSection.UpdatePicData(pictureData);
					}
					if (this.newsTabInfo != null)
					{
						this.newsTabInfo.UpdatePicData(pictureData);
					}
					if (this.dailyTabInfo != null)
					{
						this.dailyTabInfo.UpdatePicData(pictureData);
					}
				}
			}
		}
		if (this.PictureDataDeleted != null)
		{
			this.PictureDataDeleted(pd, pictureData);
		}
	}

	public void AddSave(PictureSaveData pictureSaveData)
	{
		DatabaseManager.AddSave(pictureSaveData);
		if (this.orderedMainPage != null)
		{
			for (int i = 0; i < this.orderedMainPage.Count; i++)
			{
				if (this.orderedMainPage[i].Id == pictureSaveData.Id)
				{
					this.orderedMainPage[i].SetSaveState(true);
				}
			}
		}
		if (this.featuredSection != null)
		{
			this.featuredSection.UpdateSaveState(pictureSaveData.Id, true);
		}
		if (this.newsTabInfo != null)
		{
			this.newsTabInfo.UpdateSaveState(pictureSaveData.Id, true);
		}
		if (this.dailyTabInfo != null)
		{
			this.dailyTabInfo.UpdateSaveState(pictureSaveData.Id, true);
		}
	}

	public void UpdateSave(PictureSaveData pictureSaveData)
	{
		DatabaseManager.UpdateSave(pictureSaveData);
	}

	public PictureData AddPictureToPack(PictureData pd)
	{
		PictureData pictureData = new PictureData(PictureType.Local, pd.PackId, pd.Id, pd.FillType);
		pictureData.CopyExtras(pd.Extras);
		if (pd.PicClass == PicClass.Daily)
		{
			pictureData.SetPicClass(PicClass.Daily);
		}
		DatabaseManager.AddPicture(pictureData);
		for (int i = 0; i < this.orderedMainPage.Count; i++)
		{
			if (this.orderedMainPage[i].Id == pictureData.Id)
			{
				this.orderedMainPage[i] = pictureData;
			}
		}
		if (this.featuredSection != null)
		{
			this.featuredSection.UpdatePicData(pictureData);
		}
		if (this.newsTabInfo != null)
		{
			this.newsTabInfo.UpdatePicData(pictureData);
		}
		if (this.dailyTabInfo != null)
		{
			this.dailyTabInfo.UpdatePicData(pictureData);
		}
		return pictureData;
	}

	public List<BonusCodeData> GetClaimedGifts()
	{
		return DatabaseManager.GetClaimedGifts();
	}

	public bool AddBonusCode(BonusCodeData bonusCodeData)
	{
		return DatabaseManager.AddBonusCode(bonusCodeData);
	}

	public List<int> HasUnreadNews(List<int> currentNews)
	{
		if (currentNews == null || currentNews.Count < 1)
		{
			return null;
		}
		List<int> readNewsIds = DatabaseManager.GetReadNewsIds();
		List<int> list = currentNews.Except(readNewsIds).ToList<int>();
		if (list.Count > 0)
		{
			return list;
		}
		return null;
	}

	public void MarkNewsAsRead(List<OrderedItemInfo> unreadNews)
	{
		if (unreadNews == null || unreadNews.Count == 0)
		{
			return;
		}
		DatabaseManager.MarksNewsAsRead(unreadNews);
	}

	public int CalculateLibCompletePercent()
	{
		if (this.orderedMainPage == null || this.orderedMainPage.Count == 0)
		{
			return -1;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.orderedMainPage.Count; i++)
		{
			num++;
			if (this.orderedMainPage[i].Solved)
			{
				num2++;
			}
		}
		if (num == 0)
		{
			return -1;
		}
		int num3 = Mathf.RoundToInt((float)num2 / (float)num * 100f);
		if (num3 == 0 && num2 > 0)
		{
			num3 = 1;
		}
		FMLogger.vCore(string.Concat(new object[]
		{
			"lib progress:",
			num3,
			" tc:",
			num,
			" sp:",
			num2
		}));
		return num3;
	}

	public int CalculateDailyCompletePercent()
	{
		if (this.dailyTabInfo == null || this.dailyTabInfo.monthes == null)
		{
			return -1;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.dailyTabInfo.monthes.Count; i++)
		{
			DailyMonthInfo dailyMonthInfo = this.dailyTabInfo.monthes[i];
			if (dailyMonthInfo.pics != null)
			{
				for (int j = 0; j < dailyMonthInfo.pics.Count; j++)
				{
					num++;
					if (dailyMonthInfo.pics[j].Solved)
					{
						num2++;
					}
				}
			}
		}
		if (num == 0)
		{
			return -1;
		}
		int num3 = Mathf.RoundToInt((float)num2 / (float)num * 100f);
		if (num3 == 0 && num2 > 0)
		{
			num3 = 1;
		}
		FMLogger.vCore(string.Concat(new object[]
		{
			"daily progress:",
			num3,
			" tc:",
			num,
			" sp:",
			num2
		}));
		return num3;
	}

	private static SharedData instance;

	[SerializeField]
	private SimpleSQLManager simpleSqlManager;

	private List<PictureData> orderedMainPage = new List<PictureData>();

	private FeaturedInfo featuredSection;

	private LibraryPageResponce libResponse;

	private NewsInfo newsTabInfo;

	private DailyTabInfo dailyTabInfo;

	private DailyPageResponse dailyPageResponse;

	private const string packsIndexKey = "packsdKey";

	private const string feedKey = "feedKey";

	private const string sysPackKey = "sysPackKey";

	private bool inited;

	private PictureDataDownloader picDataDownloader;

	private Coroutine mainPageTaskTimeoutCoroutine;

	private Coroutine dailyPageTaskTimeoutCoroutine;

	private const float MAIN_PAGE_RESP_WARNING = 5f;
}

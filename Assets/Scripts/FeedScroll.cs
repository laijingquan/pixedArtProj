// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedScroll : MonoBehaviour, IPictureInfoProvider
{
	public void LoadContent(bool lazyIconLoad)
	{
		this.saves = new List<PictureSaveData>(SharedData.Instance.GetSaves());
		if (this.saves == null || this.saves.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.saves.Count; i++)
		{
			PictureData pictureData = SharedData.Instance.GetPictureData(this.saves[i].Id);
			if (pictureData != null)
			{
				this.data.Add(pictureData);
			}
		}
		if (this.data.Count == 0)
		{
			return;
		}
		if (!GeneralSettings.IsOldDesign)
		{
			this.scroll.height = ((!SafeLayout.IsTablet) ? 542 : 500);
		}
		if (!GeneralSettings.IsOldDesign)
		{
			this.scroll.AddTopOffset((!SafeLayout.IsTablet) ? 40 : 30);
		}
		this.scroll.FillItem += delegate(int row, ScrollRowItem item, bool lazyLoad)
		{
			item.OnBecomeVisable(row, this, lazyLoad, false);
		};
		int count = (this.data.Count % MenuScreen.RowItems != 0) ? (this.data.Count / MenuScreen.RowItems + 1) : (this.data.Count / MenuScreen.RowItems);
		int scrollTo = 0;
		this.scroll.InitData(count, scrollTo, lazyIconLoad);
	}

	public void RemoveItemFromFeed(int id)
	{
		ScrollRowItem rowWithId = this.scroll.GetRowWithId(id);
		List<ScrollRowItem> list = new List<ScrollRowItem>(this.scroll.Views);
		if (rowWithId != null)
		{
			List<PicItem> pics = new List<PicItem>();
			list.Sort((ScrollRowItem x, ScrollRowItem y) => x.Row.CompareTo(y.Row));
			int num = list.IndexOf(rowWithId);
			if (MenuScreen.RowItems == 2)
			{
				if (rowWithId.pics[0].Id == id)
				{
					rowWithId.pics[0].Reset();
					if (rowWithId.pics[1].gameObject.activeSelf)
					{
						pics.AddRange(rowWithId.pics);
					}
					else
					{
						pics.Add(rowWithId.pics[0]);
					}
				}
				else
				{
					rowWithId.pics[1].Reset();
					pics.Add(rowWithId.pics[1]);
				}
			}
			else if (MenuScreen.RowItems == 3)
			{
				if (rowWithId.pics[0].Id == id)
				{
					rowWithId.pics[0].Reset();
					pics.Add(rowWithId.pics[0]);
					if (rowWithId.pics[1].gameObject.activeSelf)
					{
						pics.Add(rowWithId.pics[1]);
					}
					if (rowWithId.pics[2].gameObject.activeSelf)
					{
						pics.Add(rowWithId.pics[2]);
					}
				}
				else if (rowWithId.pics[1].gameObject.activeSelf && rowWithId.pics[1].Id == id)
				{
					rowWithId.pics[1].Reset();
					pics.Add(rowWithId.pics[1]);
					if (rowWithId.pics[2].gameObject.activeSelf)
					{
						pics.Add(rowWithId.pics[2]);
					}
				}
				else
				{
					rowWithId.pics[2].Reset();
					pics.Add(rowWithId.pics[2]);
				}
			}
			for (int i = num + 1; i < list.Count; i++)
			{
				for (int j = 0; j < list[i].pics.Count; j++)
				{
					if (list[i].pics[j].gameObject.activeSelf)
					{
						pics.Add(list[i].pics[j]);
					}
				}
			}
			for (int k = 0; k < pics.Count - 1; k++)
			{
				pics[k].InitFromItem(pics[k + 1]);
			}
			if (pics.Count == 1)
			{
				pics[pics.Count - 1].Reset();
				pics[pics.Count - 1].gameObject.SetActive(false);
			}
			else
			{
				PictureData pictureData = this.data.Find((PictureData pd) => pd.Id == pics[pics.Count - 2].Id);
				if (pictureData != null)
				{
					int num2 = this.data.IndexOf(pictureData);
					if (num2 == this.data.Count - 1)
					{
						pics[pics.Count - 1].gameObject.SetActive(false);
					}
					else if (num2 != -1 && num2 < this.data.Count - 1)
					{
						pics[pics.Count - 1].Init(this.data[num2 + 1], false, true, false);
						for (int l = 0; l < this.saves.Count; l++)
						{
							if (this.saves[l].Id == pics[pics.Count - 1].Id)
							{
								pics[pics.Count - 1].AddSave(this.saves[l]);
								break;
							}
						}
					}
				}
			}
		}
		for (int m = 0; m < this.data.Count; m++)
		{
			PictureData pictureData2 = this.data[m];
			if (pictureData2.Id == id)
			{
				this.data.RemoveAt(m);
				break;
			}
		}
		for (int n = 0; n < this.saves.Count; n++)
		{
			PictureSaveData pictureSaveData = this.saves[n];
			if (pictureSaveData.Id == id)
			{
				this.saves.RemoveAt(n);
				break;
			}
		}
	}

	private void OnPicureDataDeleted(PictureData delPicData, PictureData replacePicData)
	{
		base.StartCoroutine(this.DelayAction(delegate
		{
			this.RemoveItemFromFeed(delPicData.Id);
		}));
	}

	private IEnumerator DelayAction(Action a)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		a();
		yield break;
	}

	private void OnEnable()
	{
		SharedData.Instance.PictureDataDeleted += this.OnPicureDataDeleted;
	}

	private void OnDisable()
	{
		SharedData.Instance.PictureDataDeleted -= this.OnPicureDataDeleted;
	}

	public PictureData[] GetRowData(int row)
	{
		if (MenuScreen.RowItems == 2)
		{
			if (this.data.Count - 1 >= row * 2 + 1)
			{
				return new PictureData[]
				{
					this.data[row * 2],
					this.data[row * 2 + 1]
				};
			}
			if (this.data.Count - 1 == row * 2)
			{
				return new PictureData[]
				{
					this.data[row * 2]
				};
			}
		}
		else if (MenuScreen.RowItems == 3)
		{
			if (this.data.Count - 1 >= row * 3 + 2)
			{
				return new PictureData[]
				{
					this.data[row * 3],
					this.data[row * 3 + 1],
					this.data[row * 3 + 2]
				};
			}
			if (this.data.Count - 1 >= row * 3 + 1)
			{
				return new PictureData[]
				{
					this.data[row * 3],
					this.data[row * 3 + 1]
				};
			}
			if (this.data.Count - 1 == row * 3)
			{
				return new PictureData[]
				{
					this.data[row * 3]
				};
			}
		}
		return new PictureData[0];
	}

	public PictureSaveData GetSave(PictureData picData)
	{
		if (!picData.HasSave)
		{
			return null;
		}
		for (int i = 0; i < this.saves.Count; i++)
		{
			if (this.saves[i].Id == picData.Id)
			{
				return this.saves[i];
			}
		}
		return null;
	}

	public void ReloadTextures()
	{
		for (int i = 0; i < this.scroll.Views.Length; i++)
		{
			this.scroll.Views[i].ReloadTextures();
		}
	}

	public void UnloadTextures()
	{
		for (int i = 0; i < this.scroll.Views.Length; i++)
		{
			this.scroll.Views[i].UnloadTextures();
		}
	}

	[SerializeField]
	private InfiniteScroll scroll;

	private List<PictureData> data = new List<PictureData>();

	private List<PictureSaveData> saves = new List<PictureSaveData>();
}

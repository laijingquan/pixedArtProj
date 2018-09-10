// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class StartGameFadePopup : FadeScaleView
{
	public void OnShareClick()
	{
		this.preview.CutShareableTexture(delegate(Texture2D tex)
		{
			AppState.SystemUtilsPause();
			AnalyticsManager.OnSharePic(this.picItem.Id, "native");
			//UM_ShareUtility.ShareMedia(string.Empty, string.Empty, tex);
		});
	}

	public void OnPicClick()
	{
		if (this.preview.Inited)
		{
			if (this.preview.IsAnimating)
			{
				this.preview.FinishAnim();
			}
			else
			{
				this.preview.ReplayAnim();
			}
		}
	}

	public void SetIcon(PicItem item)
	{
		this.picItem = item;
		this.shareIsOn = (item.SaveData != null && item.SaveData.progres == 100);
	}

	public void SetButtons(bool cont, bool rest, bool del)
	{
		this.continueIsOn = cont;
		this.restartIsOn = rest;
		this.deleteIsOn = del;
		this.shareIsOn = true;
	}

	protected override void WillBecomeVisible()
	{
		if (!this.inited)
		{
			this.defaultPopupSize = this.content.sizeDelta;
			this.inited = true;
		}
		Vector2 vector = this.defaultPopupSize;
		if (!this.continueIsOn)
		{
			this.continueBtn.SetActive(false);
			vector -= new Vector2(0f, (float)this.btnHeight);
		}
		else
		{
			this.continueBtn.SetActive(true);
		}
		if (!this.restartIsOn)
		{
			this.restartBtn.SetActive(false);
			vector -= new Vector2(0f, (float)this.btnHeight);
		}
		else
		{
			this.restartBtn.SetActive(true);
		}
		if (!this.deleteIsOn)
		{
			this.deleteBtn.SetActive(false);
			vector -= new Vector2(0f, (float)this.btnHeight);
		}
		else
		{
			this.deleteBtn.SetActive(true);
		}
		if (!this.shareIsOn)
		{
			this.shareBtn.SetActive(false);
			vector -= new Vector2(0f, (float)this.btnHeight);
		}
		else
		{
			this.shareBtn.SetActive(true);
		}
		this.content.sizeDelta = vector;
		this.preview.Init(this.picItem);
	}

	protected override void BecomeVisable()
	{
		this.preview.Play();
	}

	protected override void WillBecomeInvisable()
	{
		this.preview.StopAnimation();
	}

	protected override void BecomeInvisable()
	{
		this.preview.Clean();
		GC.Collect();
		Resources.UnloadUnusedAssets();
	}

	public PicturePreview preview;

	private PicItem picItem;

	public GameObject continueBtn;

	public GameObject restartBtn;

	public GameObject deleteBtn;

	public GameObject shareBtn;

	public RectTransform content;

	private Vector2 defaultPopupSize;

	private Vector2 popupSize;

	private int btnHeight = 92;

	private bool inited;

	private bool continueIsOn;

	private bool restartIsOn;

	private bool deleteIsOn;

	private bool shareIsOn;
}

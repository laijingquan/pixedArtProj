// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LegacySettingsPopup : FadeScaleView
{
	protected override void WillBecomeVisible()
	{
		if (!this.inited)
		{
			this.defaultPopupSize = this.content.sizeDelta;
			this.inited = true;
		}
		Transform parent = this.refOpenAnchor.parent;
		this.refOpenAnchor.SetParent(this.content.parent);
		this.content.anchoredPosition = this.refOpenAnchor.anchoredPosition;
		this.refOpenAnchor.SetParent(parent);
		Vector2 vector = this.defaultPopupSize;
		if (GeneralSettings.AdsDisabled)
		{
			this.removeAds.SetActive(false);
			vector -= new Vector2(0f, (float)this.btnHeight);
		}
		if (!GeneralSettings.CanUseLegacyDesign)
		{
			this.design.SetActive(false);
			vector -= new Vector2(0f, (float)this.btnHeight);
		}
		this.restore.SetActive(false);
		vector -= new Vector2(0f, (float)this.btnHeight);
		this.content.sizeDelta = vector;
	}

	protected override void BecomeVisable()
	{
	}

	protected override void WillBecomeInvisable()
	{
	}

	protected override void BecomeInvisable()
	{
	}

	[SerializeField]
	private RectTransform refOpenAnchor;

	[SerializeField]
	private RectTransform content;

	[SerializeField]
	private GameObject restore;

	[SerializeField]
	private GameObject removeAds;

	[SerializeField]
	private GameObject design;

	private int btnHeight = 135;

	private bool inited;

	private Vector2 defaultPopupSize;
}

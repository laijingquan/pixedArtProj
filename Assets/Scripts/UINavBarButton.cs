// dnSpy decompiler from Assembly-CSharp.dll
using System;
using FMUILayout;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UINavBarButton : UIElement
{
	protected override void OnDeviceTypeChanged(UIDeviceType dt)
	{
		base.OnDeviceTypeChanged(dt);
		if (this.deviceType == dt)
		{
			return;
		}
		this.deviceType = dt;
		this.UpdateUI();
	}

	protected override void OnOrientationChanged(UIDeviceOrientation orientation)
	{
		base.OnOrientationChanged(orientation);
		if (!base.IsNewOrientation(orientation))
		{
			return;
		}
		this.deviceOrientation = orientation;
		this.UpdateUI();
	}

	protected override void UpdateUI()
	{
		base.UpdateUI();
		if (this.deviceType == UIDeviceType.Phone)
		{
			switch (this.deviceOrientation)
			{
			case UIDeviceOrientation.Portrait:
			case UIDeviceOrientation.PortraitUpsideDown:
				this.ApplyRect(this.phonePortrait);
				break;
			case UIDeviceOrientation.LandscapeRight:
			case UIDeviceOrientation.LandscapeLeft:
				this.ApplyRect(this.phoneLandscape);
				break;
			}
		}
		else
		{
			UIDeviceOrientation deviceOrientation = this.deviceOrientation;
			if (deviceOrientation != UIDeviceOrientation.Portrait)
			{
				if (deviceOrientation == UIDeviceOrientation.LandscapeRight || deviceOrientation == UIDeviceOrientation.LandscapeLeft)
				{
					this.ApplyRect(this.tabletLandscape);
				}
			}
			else
			{
				this.ApplyRect(this.tabletPortrait);
			}
		}
	}

	private void ApplyRect(UINavBarButtonData data)
	{
		if (data == null || !data.hasData)
		{
			return;
		}
		this.bg.anchoredPosition = data.bgPos;
		this.bg.sizeDelta = data.bgSize;
		this.label.fontSize = data.textFontSize;
		((RectTransform)this.label.transform).anchoredPosition = data.textPos;
		((RectTransform)this.label.transform).sizeDelta = data.textSize;
		this.icon.anchoredPosition = data.iconPos;
		this.icon.sizeDelta = data.iconSize;
		if (this.bgSelected != null)
		{
			this.bgSelected.gameObject.SetActive(data.selectBgEnabled);
		}
	}

	public override void EditorSave()
	{
		base.EditorSave();
		UIDeviceType deviceType = UILayoutManager.DeviceType;
		if (deviceType != UIDeviceType.Phone)
		{
			if (deviceType == UIDeviceType.Tablet)
			{
				switch (UILayoutManager.Orientation)
				{
				case UIDeviceOrientation.Portrait:
				case UIDeviceOrientation.PortraitUpsideDown:
					if (this.tabletPortrait == null)
					{
						this.tabletPortrait = new UINavBarButtonData();
					}
					this.tabletPortrait = UINavBarButtonData.FromUIElement(this);
					break;
				case UIDeviceOrientation.LandscapeRight:
				case UIDeviceOrientation.LandscapeLeft:
					if (this.tabletLandscape == null)
					{
						this.tabletLandscape = new UINavBarButtonData();
					}
					this.tabletLandscape = UINavBarButtonData.FromUIElement(this);
					break;
				}
			}
		}
		else
		{
			switch (UILayoutManager.Orientation)
			{
			case UIDeviceOrientation.Portrait:
			case UIDeviceOrientation.PortraitUpsideDown:
				if (this.phonePortrait == null)
				{
					this.phonePortrait = new UINavBarButtonData();
				}
				this.phonePortrait = UINavBarButtonData.FromUIElement(this);
				break;
			case UIDeviceOrientation.LandscapeRight:
			case UIDeviceOrientation.LandscapeLeft:
				if (this.phoneLandscape == null)
				{
					this.phoneLandscape = new UINavBarButtonData();
				}
				this.phoneLandscape = UINavBarButtonData.FromUIElement(this);
				break;
			}
		}
	}

	[HideInInspector]
	[SerializeField]
	public UINavBarButtonData phonePortrait;

	[HideInInspector]
	[SerializeField]
	public UINavBarButtonData phoneLandscape;

	[HideInInspector]
	[SerializeField]
	public UINavBarButtonData tabletPortrait;

	[HideInInspector]
	[SerializeField]
	public UINavBarButtonData tabletLandscape;

	public RectTransform bg;

	public Text label;

	public RectTransform icon;

	public RectTransform bgSelected;
}

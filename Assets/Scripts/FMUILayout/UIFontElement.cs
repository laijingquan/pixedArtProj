// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FMUILayout
{
	[ExecuteInEditMode]
	public class UIFontElement : UIElement
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
				UIDeviceOrientation deviceOrientation = this.deviceOrientation;
				if (deviceOrientation != UIDeviceOrientation.Portrait)
				{
					if (deviceOrientation == UIDeviceOrientation.LandscapeRight || deviceOrientation == UIDeviceOrientation.LandscapeLeft)
					{
						this.ApplyFont(this.phoneLandscape);
					}
				}
				else
				{
					this.ApplyFont(this.phonePortrait);
				}
			}
			else
			{
				UIDeviceOrientation deviceOrientation2 = this.deviceOrientation;
				if (deviceOrientation2 != UIDeviceOrientation.Portrait)
				{
					if (deviceOrientation2 == UIDeviceOrientation.LandscapeRight || deviceOrientation2 == UIDeviceOrientation.LandscapeLeft)
					{
						this.ApplyFont(this.tabletLandscape);
					}
				}
				else
				{
					this.ApplyFont(this.tabletPortrait);
				}
			}
		}

		private void ApplyFont(UIFontData fontData)
		{
			if (fontData == null || !fontData.hasData)
			{
				return;
			}
			Text component = base.GetComponent<Text>();
			if (fontData.bestFit)
			{
				component.resizeTextForBestFit = true;
				component.resizeTextMinSize = fontData.bestFitMinSize;
				component.resizeTextMaxSize = fontData.bestFitMaxSize;
			}
			else
			{
				component.fontSize = fontData.fontSize;
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
							this.tabletPortrait = new UIFontData();
						}
						this.tabletPortrait = UIFontData.FromTransform(base.transform);
						break;
					case UIDeviceOrientation.LandscapeRight:
					case UIDeviceOrientation.LandscapeLeft:
						if (this.tabletLandscape == null)
						{
							this.tabletLandscape = new UIFontData();
						}
						this.tabletLandscape = UIFontData.FromTransform(base.transform);
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
						this.phonePortrait = new UIFontData();
					}
					this.phonePortrait = UIFontData.FromTransform(base.transform);
					break;
				case UIDeviceOrientation.LandscapeRight:
				case UIDeviceOrientation.LandscapeLeft:
					if (this.phoneLandscape == null)
					{
						this.phoneLandscape = new UIFontData();
					}
					this.phoneLandscape = UIFontData.FromTransform(base.transform);
					break;
				}
			}
		}

		[HideInInspector]
		[SerializeField]
		public UIFontData phonePortrait;

		[HideInInspector]
		[SerializeField]
		public UIFontData phoneLandscape;

		[HideInInspector]
		[SerializeField]
		public UIFontData tabletPortrait;

		[HideInInspector]
		[SerializeField]
		public UIFontData tabletLandscape;
	}
}

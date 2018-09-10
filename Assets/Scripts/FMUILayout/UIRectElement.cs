// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

namespace FMUILayout
{
	[ExecuteInEditMode]
	public class UIRectElement : UIElement
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
						this.ApplyRect(this.phoneLandscape);
					}
				}
				else
				{
					this.ApplyRect(this.phonePortrait);
				}
			}
			else
			{
				UIDeviceOrientation deviceOrientation2 = this.deviceOrientation;
				if (deviceOrientation2 != UIDeviceOrientation.Portrait)
				{
					if (deviceOrientation2 == UIDeviceOrientation.LandscapeRight || deviceOrientation2 == UIDeviceOrientation.LandscapeLeft)
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

		private void ApplyRect(UIRectPositionData rectPosition)
		{
			if (rectPosition == null || !rectPosition.hasData)
			{
				return;
			}
			RectTransform rectTransform = (RectTransform)base.transform;
			rectTransform.anchoredPosition = rectPosition.anchoredPosition;
			rectTransform.anchorMin = rectPosition.anchorMin;
			rectTransform.anchorMax = rectPosition.anchorMax;
			rectTransform.offsetMin = rectPosition.offsetMin;
			rectTransform.offsetMax = rectPosition.offsetMax;
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
							this.tabletPortrait = new UIRectPositionData();
						}
						this.tabletPortrait = UIRectPositionData.FromTransform(base.transform);
						break;
					case UIDeviceOrientation.LandscapeRight:
					case UIDeviceOrientation.LandscapeLeft:
						if (this.tabletLandscape == null)
						{
							this.tabletLandscape = new UIRectPositionData();
						}
						this.tabletLandscape = UIRectPositionData.FromTransform(base.transform);
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
						this.phonePortrait = new UIRectPositionData();
					}
					this.phonePortrait = UIRectPositionData.FromTransform(base.transform);
					break;
				case UIDeviceOrientation.LandscapeRight:
				case UIDeviceOrientation.LandscapeLeft:
					if (this.phoneLandscape == null)
					{
						this.phoneLandscape = new UIRectPositionData();
					}
					this.phoneLandscape = UIRectPositionData.FromTransform(base.transform);
					break;
				}
			}
		}

		[HideInInspector]
		[SerializeField]
		public UIRectPositionData phonePortrait;

		[HideInInspector]
		[SerializeField]
		public UIRectPositionData phoneLandscape;

		[HideInInspector]
		[SerializeField]
		public UIRectPositionData tabletPortrait;

		[HideInInspector]
		[SerializeField]
		public UIRectPositionData tabletLandscape;
	}
}

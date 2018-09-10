// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FMUILayout
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasScaler))]
	public class UICanvasScalerElement : UIElement
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
			if (this.deviceType == UIDeviceType.Phone)
			{
				UIDeviceOrientation deviceOrientation = this.deviceOrientation;
				if (deviceOrientation != UIDeviceOrientation.Portrait)
				{
					if (deviceOrientation == UIDeviceOrientation.LandscapeRight || deviceOrientation == UIDeviceOrientation.LandscapeLeft)
					{
						this.ApplyScaler(this.phoneLandscape);
					}
				}
				else
				{
					this.ApplyScaler(this.phonePortrait);
				}
			}
			else
			{
				UIDeviceOrientation deviceOrientation2 = this.deviceOrientation;
				if (deviceOrientation2 != UIDeviceOrientation.Portrait)
				{
					if (deviceOrientation2 == UIDeviceOrientation.LandscapeRight || deviceOrientation2 == UIDeviceOrientation.LandscapeLeft)
					{
						this.ApplyScaler(this.tabletLandscape);
					}
				}
				else
				{
					this.ApplyScaler(this.tabletPortrait);
				}
			}
		}

		private void ApplyScaler(UICanvasScalerData scaler)
		{
			if (scaler == null || !scaler.hasData)
			{
				return;
			}
			CanvasScaler component = base.GetComponent<CanvasScaler>();
			component.referenceResolution = scaler.referenceResolution;
			component.matchWidthOrHeight = scaler.matchWidthOrHeight;
		}

		public override void EditorSave()
		{
			base.EditorSave();
			UIDeviceType deviceType = UILayoutManager.DeviceType;
			if (deviceType != UIDeviceType.Phone)
			{
				if (deviceType == UIDeviceType.Tablet)
				{
					UIDeviceOrientation orientation = UILayoutManager.Orientation;
					if (orientation != UIDeviceOrientation.Portrait)
					{
						if (orientation == UIDeviceOrientation.LandscapeRight || orientation == UIDeviceOrientation.LandscapeLeft)
						{
							if (this.tabletLandscape == null)
							{
								this.tabletLandscape = new UICanvasScalerData();
							}
							this.tabletLandscape = UICanvasScalerData.FromCanvas(this.transform);
						}
					}
					else
					{
						if (this.tabletPortrait == null)
						{
							this.tabletPortrait = new UICanvasScalerData();
						}
						this.tabletPortrait = UICanvasScalerData.FromCanvas(this.transform);
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
						this.phonePortrait = new UICanvasScalerData();
					}
					this.phonePortrait = UICanvasScalerData.FromCanvas(this.transform);
					break;
				case UIDeviceOrientation.LandscapeRight:
				case UIDeviceOrientation.LandscapeLeft:
					if (this.phoneLandscape == null)
					{
						this.phoneLandscape = new UICanvasScalerData();
					}
					this.phoneLandscape = UICanvasScalerData.FromCanvas(this.transform);
					break;
				}
			}
		}

		[HideInInspector]
		[SerializeField]
		public UICanvasScalerData phonePortrait;

		[HideInInspector]
		[SerializeField]
		public UICanvasScalerData phoneLandscape;

		[HideInInspector]
		[SerializeField]
		public UICanvasScalerData tabletPortrait;

		[HideInInspector]
		[SerializeField]
		public UICanvasScalerData tabletLandscape;
	}
}

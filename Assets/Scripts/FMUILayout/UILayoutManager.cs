// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;
using UnityEngine;

namespace FMUILayout
{
	[ExecuteInEditMode]
	public class UILayoutManager : MonoBehaviour
	{
		 
		public static event Action<UIDeviceType> OnDeviceTypeChanged;

		 
		public static event Action<UIDeviceOrientation> OnOrientationChanged;

		 
		public static event Action<UIDeviceType> DebugDeviceTypeChanged;

		 
		public static event Action<UIDeviceOrientation> DebugOrientationChanged;

		public void Init()
		{
			UILayoutManager.DeviceType = this.GetDeviceType();
			if (this.SupportOrientationChange)
			{
				UISysOrientationChangeWatcher.OrientationChanged += this.OnSystemOrientationChanged;
				this.supportedOrientations = ((UILayoutManager.DeviceType != UIDeviceType.Phone) ? this.tabletOrientations : this.phoneOrientations);
			}
			this.SendNewDeviceType();
		}

		public void SendNewDeviceType()
		{
			if (UILayoutManager.OnDeviceTypeChanged != null)
			{
				UILayoutManager.OnDeviceTypeChanged(UILayoutManager.DeviceType);
			}
		}

		public void SendOrientation()
		{
			if (UILayoutManager.OnOrientationChanged != null)
			{
				UILayoutManager.OnOrientationChanged(UILayoutManager.Orientation);
			}
		}

		public void SendDebugDeviceType()
		{
			if (UILayoutManager.DebugDeviceTypeChanged != null)
			{
				UILayoutManager.DebugDeviceTypeChanged(UILayoutManager.DeviceType);
			}
		}

		public void SendDebugOrientation()
		{
			if (UILayoutManager.DebugOrientationChanged != null)
			{
				UILayoutManager.DebugOrientationChanged(UILayoutManager.Orientation);
			}
		}

		private UIDeviceType GetDeviceType()
		{
			if (SafeLayout.IsTablet)
			{
				return UIDeviceType.Tablet;
			}
			return UIDeviceType.Phone;
		}

		private void OnDisable()
		{
			UISysOrientationChangeWatcher.OrientationChanged -= this.OnSystemOrientationChanged;
		}

		private void OnSystemOrientationChanged(DeviceOrientation newOrientation)
		{
			if (!this.SupportOrientationChange)
			{
				return;
			}
			UIDeviceOrientation uideviceOrientation = this.CovertDeviceOrientation(newOrientation);
			if ((uideviceOrientation & this.supportedOrientations) == (UIDeviceOrientation)0)
			{
				return;
			}
			switch (uideviceOrientation)
			{
			case UIDeviceOrientation.Portrait:
				Screen.orientation = ScreenOrientation.Portrait;
				break;
			case UIDeviceOrientation.PortraitUpsideDown:
				Screen.orientation = ScreenOrientation.PortraitUpsideDown;
				break;
			case UIDeviceOrientation.LandscapeRight:
				Screen.orientation = ScreenOrientation.LandscapeRight;
				break;
			case UIDeviceOrientation.LandscapeLeft:
				Screen.orientation = ScreenOrientation.LandscapeRight;
				break;
			}
			UILayoutManager.Orientation = uideviceOrientation;
			this.SendOrientation();
		}

		private UIDeviceOrientation CovertDeviceOrientation(DeviceOrientation newOrientation)
		{
			UIDeviceOrientation result = UIDeviceOrientation.Portrait;
			switch (newOrientation)
			{
			case DeviceOrientation.Portrait:
			case DeviceOrientation.FaceUp:
				result = UIDeviceOrientation.Portrait;
				break;
			case DeviceOrientation.PortraitUpsideDown:
			case DeviceOrientation.FaceDown:
				result = UIDeviceOrientation.PortraitUpsideDown;
				break;
			case DeviceOrientation.LandscapeLeft:
				result = UIDeviceOrientation.LandscapeLeft;
				break;
			case DeviceOrientation.LandscapeRight:
				result = UIDeviceOrientation.LandscapeRight;
				break;
			}
			return result;
		}

		public bool SupportOrientationChange;

		public UIDeviceOrientation phoneOrientations;

		public UIDeviceOrientation tabletOrientations;

		public static UIDeviceType DeviceType;

		public static UIDeviceOrientation Orientation = UIDeviceOrientation.Portrait;

		private UIDeviceOrientation supportedOrientations;
	}
}

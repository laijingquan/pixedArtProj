// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

namespace FMUILayout
{
	[ExecuteInEditMode]
	public class UIElement : MonoBehaviour
	{
		public virtual void EditorSave()
		{
		}

		protected virtual void Awake()
		{
			this.deviceType = UILayoutManager.DeviceType;
			this.deviceOrientation = UILayoutManager.Orientation;
			this.UpdateUI();
		}

		protected virtual void OnEnable()
		{
			UILayoutManager.OnDeviceTypeChanged += this.OnDeviceTypeChanged;
			UILayoutManager.DebugDeviceTypeChanged += this.DebugDeviceTypeChanged;
			UILayoutManager.OnOrientationChanged += this.OnOrientationChanged;
			UILayoutManager.DebugOrientationChanged += this.DebugOrientationChanged;
		}

		protected virtual void OnDisable()
		{
			UILayoutManager.OnDeviceTypeChanged -= this.OnDeviceTypeChanged;
			UILayoutManager.DebugDeviceTypeChanged -= this.DebugDeviceTypeChanged;
			UILayoutManager.OnOrientationChanged -= this.OnOrientationChanged;
			UILayoutManager.DebugOrientationChanged -= this.DebugOrientationChanged;
		}

		private void DebugDeviceTypeChanged(UIDeviceType dt)
		{
			this.deviceType = dt;
		}

		protected virtual void OnDeviceTypeChanged(UIDeviceType dt)
		{
		}

		protected virtual void OnOrientationChanged(UIDeviceOrientation orientation)
		{
		}

		private void DebugOrientationChanged(UIDeviceOrientation orientation)
		{
			this.deviceOrientation = orientation;
		}

		protected virtual void UpdateUI()
		{
		}

		protected bool IsNewOrientation(UIDeviceOrientation orientation)
		{
			return this.deviceOrientation != orientation && ((this.deviceOrientation != UIDeviceOrientation.Portrait && this.deviceOrientation != UIDeviceOrientation.PortraitUpsideDown) || (orientation != UIDeviceOrientation.Portrait && orientation != UIDeviceOrientation.PortraitUpsideDown)) && ((this.deviceOrientation != UIDeviceOrientation.LandscapeLeft && this.deviceOrientation != UIDeviceOrientation.LandscapeRight) || (orientation != UIDeviceOrientation.LandscapeLeft && orientation != UIDeviceOrientation.LandscapeRight));
		}

		[HideInInspector]
		[SerializeField]
		public UIDeviceOrientation deviceOrientation;

		[HideInInspector]
		[SerializeField]
		public UIDeviceType deviceType;
	}
}

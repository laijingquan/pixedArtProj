// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FMUILayout
{
	[ExecuteInEditMode]
	public class UISpriteSwapElement : UIElement
	{
		private Image Image
		{
			get
			{
				if (this.image == null)
				{
					this.image = base.GetComponent<Image>();
				}
				return this.image;
			}
		}

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
						this.ApplySprite(this.phoneLandscape);
					}
				}
				else
				{
					this.ApplySprite(this.phonePortrait);
				}
			}
			else
			{
				UIDeviceOrientation deviceOrientation2 = this.deviceOrientation;
				if (deviceOrientation2 != UIDeviceOrientation.Portrait)
				{
					if (deviceOrientation2 == UIDeviceOrientation.LandscapeRight || deviceOrientation2 == UIDeviceOrientation.LandscapeLeft)
					{
						this.ApplySprite(this.tabletLandscape);
					}
				}
				else
				{
					this.ApplySprite(this.tabletPortrait);
				}
			}
		}

		private void ApplySprite(UISpriteSwapnData spriteData)
		{
			if (spriteData == null || !spriteData.hasData)
			{
				return;
			}
			if (spriteData.spriteIndex < this.sprites.Length && spriteData.spriteIndex >= 0)
			{
				this.Image.sprite = this.sprites[spriteData.spriteIndex];
			}
			else
			{
				UnityEngine.Debug.LogError("UISpriteSwapError. " + base.gameObject.name);
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
							this.tabletPortrait = new UISpriteSwapnData();
						}
						this.tabletPortrait = UISpriteSwapnData.FromSpriteIndex(this.GetSpriteIndex());
						break;
					case UIDeviceOrientation.LandscapeRight:
					case UIDeviceOrientation.LandscapeLeft:
						if (this.tabletLandscape == null)
						{
							this.tabletLandscape = new UISpriteSwapnData();
						}
						this.tabletLandscape = UISpriteSwapnData.FromSpriteIndex(this.GetSpriteIndex());
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
						this.phonePortrait = new UISpriteSwapnData();
					}
					this.phonePortrait = UISpriteSwapnData.FromSpriteIndex(this.GetSpriteIndex());
					break;
				case UIDeviceOrientation.LandscapeRight:
				case UIDeviceOrientation.LandscapeLeft:
					if (this.phoneLandscape == null)
					{
						this.phoneLandscape = new UISpriteSwapnData();
					}
					this.phoneLandscape = UISpriteSwapnData.FromSpriteIndex(this.GetSpriteIndex());
					break;
				}
			}
		}

		private int GetSpriteIndex()
		{
			if (this.sprites.Length > 0)
			{
				for (int i = 0; i < this.sprites.Length; i++)
				{
					if (this.Image.sprite == this.sprites[i])
					{
						return i;
					}
				}
			}
			UnityEngine.Debug.LogError("UISpriteSwap error. Cant fint sprite");
			return -1;
		}

		[HideInInspector]
		[SerializeField]
		public UISpriteSwapnData phonePortrait;

		[HideInInspector]
		[SerializeField]
		public UISpriteSwapnData phoneLandscape;

		[HideInInspector]
		[SerializeField]
		public UISpriteSwapnData tabletPortrait;

		[HideInInspector]
		[SerializeField]
		public UISpriteSwapnData tabletLandscape;

		public Sprite[] sprites;

		private Image image;
	}
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PaletteController : MonoBehaviour
{
	 
	public event Action<int> NewColor;

	public bool HasColor
	{
		get
		{
			return this.activeButton != null;
		}
	}

	public Color Current
	{
		get
		{
			if (this.activeButton != null)
			{
				return this.activeButton.Color;
			}
			return new Color(0f, 0f, 0f, 0f);
		}
	}

	public int CurrentId
	{
		get
		{
			return (!(this.activeButton == null)) ? this.activeButton.Id : -1;
		}
	}

	public void Create(List<Color> colors)
	{
		this.Init();
		this.layout.Init(this.layoutConfig);
		this.buttons = new List<PaletterButton>();
		for (int i = 0; i < colors.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.btnPrefab);
			gameObject.transform.SetParent(this.layout.transform);
			gameObject.transform.localScale = Vector3.one;
			PaletterButton component = gameObject.GetComponent<PaletterButton>();
			component.Init(i, colors[i]);
			this.layout.AddElement((RectTransform)component.transform);
			this.buttons.Add(component);
		}
		this.positioner = this.layout.transform.parent.GetComponent<ScrollElementPositioner>();
		if (this.positioner != null)
		{
			List<Vector2> list = new List<Vector2>();
			Vector2 b = new Vector2((float)this.layoutConfig.size / 2f, 0f);
			Vector2 b2 = new Vector2((float)this.layoutConfig.padding, 0f);
			for (int j = 0; j < this.buttons.Count; j++)
			{
				if (j == 0)
				{
					list.Add(((RectTransform)this.buttons[j].transform).anchoredPosition - b2 - b);
				}
				else
				{
					list.Add(((RectTransform)this.buttons[j].transform).anchoredPosition - b);
				}
			}
			this.positioner.Init(list, this.layoutConfig.padding, this.layoutConfig.spacing);
		}
	}

	public void MarkCurrentUsed(int i = -1)
	{
		if (i == -1)
		{
			this.activeButton.MarkComplete();
		}
		else
		{
			this.buttons[i].MarkComplete();
		}
	}

	public Color IdToColor(int id)
	{
		return this.buttons[id].Color;
	}

	public void OnColorClick(PaletterButton btn)
	{
		if (this.activeButton == btn)
		{
			return;
		}
		if (btn.Completed && !PaletteController.ALLOW_SELECT_COMPLETED)
		{
			this.toastManager.ShowPaletteError(btn.Id + 1);
			return;
		}
		if (this.activeButton != null)
		{
			this.activeButton.Deselect();
		}
		this.activeButton = btn;
		this.activeButton.Select();
		if (this.positioner != null)
		{
			this.positioner.CheckFit(btn.Id);
		}
		if (this.NewColor != null)
		{
			this.NewColor(btn.Id);
		}
	}

	private void Init()
	{
		if (GeneralSettings.IsOldDesign)
		{
			this.layoutConfig = new PaletteController.Config();
			this.layoutConfig.size = ((!SafeLayout.IsTablet) ? 137 : 125);
			this.layoutConfig.padding = 40;
			this.layoutConfig.spacing = ((!SafeLayout.IsTablet) ? 27 : 30);
			this.btnPrefab = this.legacyBtnPrefab;
		}
		else
		{
			this.layoutConfig = ((!SafeLayout.IsTablet) ? this.phoneConfig : this.tabletConfig);
			this.btnPrefab = ((!SafeLayout.IsTablet) ? this.phoneBtnPreafab : this.tabletBtnPrefab);
		}
	}

	private static readonly bool ALLOW_SELECT_COMPLETED;

	private List<PaletterButton> buttons;

	private PaletterButton activeButton;

	[SerializeField]
	private ToastManager toastManager;

	[SerializeField]
	private PaletteLayout layout;

	[SerializeField]
	private GameObject legacyBtnPrefab;

	[SerializeField]
	private GameObject phoneBtnPreafab;

	[SerializeField]
	private GameObject tabletBtnPrefab;

	private ScrollElementPositioner positioner;

	private PaletteController.Config phoneConfig = new PaletteController.Config
	{
		padding = 25,
		size = 135,
		spacing = 37
	};

	private PaletteController.Config tabletConfig = new PaletteController.Config
	{
		padding = 20,
		size = 114,
		spacing = 33
	};

	private GameObject btnPrefab;

	private PaletteController.Config layoutConfig;

	private Coroutine pointerMoveCoroutine;

	public class Config
	{
		public int spacing;

		public int size;

		public int padding;
	}
}

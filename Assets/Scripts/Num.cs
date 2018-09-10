// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class Num : MonoBehaviour
{
	public void Init(int value, float size, int fontSize, float initSize)
	{
		this.Id = value;
		this.label.fontSize = fontSize;
		this.label.text = (value + 1).ToString();
		this.rt = (RectTransform)base.transform;
		this.requiredSize = size;
		this.initialSize = initSize;
		this.disabled = false;
		base.gameObject.SetActive(false);
	}

	public void MarkFilled()
	{
		if (this.disabled)
		{
			return;
		}
		this.disabled = true;
		base.gameObject.SetActive(false);
	}

	public void Hide()
	{
		if (this.disabled)
		{
			return;
		}
		if (!this.isOn)
		{
			return;
		}
		this.isOn = false;
		base.gameObject.SetActive(false);
	}

	public void UpdateFixedSizeState(float scaleFactor)
	{
		if (this.disabled)
		{
			return;
		}
		if (this.isOn)
		{
			this.rt.localScale = new Vector3(1f / scaleFactor, 1f / scaleFactor, 1f);
		}
		if (!this.isOn && this.initialSize < this.requiredSize * scaleFactor)
		{
			this.isOn = true;
			this.rt.localScale = new Vector3(1f / scaleFactor, 1f / scaleFactor, 1f);
			base.gameObject.SetActive(true);
		}
		else if (this.isOn && this.initialSize > this.requiredSize * scaleFactor)
		{
			this.isOn = false;
			base.gameObject.SetActive(false);
		}
	}

	public void UpdateZoomableState(float scaleFactor)
	{
		if (this.disabled)
		{
			return;
		}
		if (!this.isOn && this.initialSize < this.requiredSize * scaleFactor)
		{
			this.isOn = true;
			this.rt.localScale = new Vector3(1f / scaleFactor, 1f / scaleFactor, 1f);
			base.gameObject.SetActive(true);
		}
		else if (this.isOn && this.initialSize > this.requiredSize * scaleFactor)
		{
			this.isOn = false;
			base.gameObject.SetActive(false);
		}
	}

	public int Id;

	[SerializeField]
	private Text label;

	private RectTransform rt;

	[SerializeField]
	private float requiredSize;

	private float initialSize;

	private bool isOn;

	private bool disabled;
}

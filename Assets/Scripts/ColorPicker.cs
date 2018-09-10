// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
	private void Start()
	{
		this.UpdateColor();
	}

	public void OnSliderRedChange(float value)
	{
		this.red = Mathf.Clamp01(value);
		this.UpdateColor();
	}

	public void OnSliderBlueChange(float value)
	{
		this.blue = Mathf.Clamp01(value);
		this.UpdateColor();
	}

	public void OnSliderGreenChange(float value)
	{
		this.green = Mathf.Clamp01(value);
		this.UpdateColor();
	}

	public void UpdateColor()
	{
		this.pickedColor = new Color32((byte)(this.red * 255f), (byte)(this.green * 255f), (byte)(this.blue * 255f), byte.MaxValue);
		this.img.color = this.pickedColor;
		this.flood.drawColor = this.pickedColor;
	}

	public FloodTest flood;

	public Color pickedColor;

	public Image img;

	private float red = 0.1f;

	private float green = 0.1f;

	private float blue = 0.1f;
}

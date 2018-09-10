// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class LineMaterialTester : MonoBehaviour
{
	public void OnDistanceChanged(float value)
	{
		float value2 = Mathf.Clamp01(value);
		this.lineMaterial.SetFloat("_Dist", value2);
	}

	public void SwitchMaterialClick()
	{
		if (this.img.material == this.lineMaterial)
		{
			this.img.material = this.defaultMaterial;
		}
		else
		{
			this.img.material = this.lineMaterial;
		}
	}

	public Material lineMaterial;

	public Material defaultMaterial;

	public RawImage img;
}

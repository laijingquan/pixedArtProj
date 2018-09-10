// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class UILayout : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Debug.LogError("yi" + base.name);
	}

	private void Start()
	{
		int num = (int)this.refRt.sizeDelta.y;
		int num2 = AdsManager.Instance.CalcBannerHeight(num);
		FMLogger.Log(string.Concat(new object[]
		{
			"banner height ",
			num2,
			" canvas height ",
			num
		}));
		this.topControls.anchoredPosition += new Vector2(0f, (float)num2);
	}

	[SerializeField]
	private RectTransform refRt;

	[SerializeField]
	private RectTransform topControls;
}

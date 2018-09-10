// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUILayout : MonoBehaviour
{
	private void Awake()
	{
		if (GeneralSettings.IsOldDesign && SafeLayout.IsTablet)
		{
			CanvasScaler component = base.GetComponent<CanvasScaler>();
			component.referenceResolution = new Vector2(1654f, 2927f);
			this.ApplyTabletLayout();
		}
	}

	private void ApplyTabletLayout()
	{
	}

	private void Start()
	{
		base.StartCoroutine(this.FrameDelay(delegate
		{
			base.GetComponent<GameSafeLayout>().ApplySafeArea();
		}));
	}

	private IEnumerator FrameDelay(Action a)
	{
		yield return new WaitForEndOfFrame();
		a();
		yield break;
	}
}

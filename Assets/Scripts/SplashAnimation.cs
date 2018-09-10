// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashAnimation : MonoBehaviour
{
	private void Awake()
	{
		base.StartCoroutine(this.DelayInit());
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.DotStepAnim());
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	private IEnumerator DotStepAnim()
	{
		yield return new WaitForSeconds(this.delay);
		for (;;)
		{
			this.label.text = string.Empty;
			yield return new WaitForSeconds(this.stepTime);
			this.label.text = ".";
			yield return new WaitForSeconds(this.stepTime);
			this.label.text = "..";
			yield return new WaitForSeconds(this.postStepTime);
			this.label.text = "...";
			yield return new WaitForSeconds(this.postStepTime);
		}

	}

	private IEnumerator DelayInit()
	{
		yield return null;
		Text text = base.GetComponent<Text>();
		float width = text.preferredWidth;
		RectTransform rt = (RectTransform)base.transform;
		rt.sizeDelta = new Vector2(width + 3f, rt.sizeDelta.y);
		yield break;
	}

	[SerializeField]
	private Text label;

	private int dotStep;

	private float delay = 0.3f;

	private float stepTime = 0.5f;

	private float postStepTime = 0.5f;
}

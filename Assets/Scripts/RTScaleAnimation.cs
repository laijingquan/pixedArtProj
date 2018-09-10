// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class RTScaleAnimation : TransformAnimationBase
{
	protected override IEnumerator DoStaff()
	{
		float upTime = this.animDuration * this.timeSplit;
		float downTime = this.animDuration - upTime;
		yield return this.ScaleCoroutine(this.downScale, this.upScale, upTime, this.upCurve);
		yield return this.ScaleCoroutine(this.upScale, this.downScale, downTime, this.downCurve);
		yield break;
	}

	private IEnumerator ScaleCoroutine(float from, float to, float duration, AnimationCurve curve)
	{
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / duration;
			float s = Mathf.LerpUnclamped(from, to, curve.Evaluate(i));
			this.rt.localScale = new Vector3(s, s, 1f);
			yield return 0;
		}
		this.rt.localScale = new Vector3(to, to, 1f);
		yield break;
	}

	[SerializeField]
	[Range(0f, 1f)]
	private float timeSplit;

	[SerializeField]
	private AnimationCurve upCurve;

	[SerializeField]
	private AnimationCurve downCurve;

	[SerializeField]
	private float upScale;

	[SerializeField]
	private float downScale;

	[SerializeField]
	private RectTransform rt;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class RTMoveAnimation : TransformAnimationBase
{
	protected override IEnumerator DoStaff()
	{
		float upTime = this.animDuration * this.timeSplit;
		float downTime = this.animDuration - upTime;
		yield return this.MoveCoroutine(this.fromPos, this.toPos, upTime, this.upCurve);
		yield return this.MoveCoroutine(this.toPos, this.fromPos, downTime, this.downCurve);
		yield break;
	}

	private IEnumerator MoveCoroutine(Vector2 from, Vector2 to, float duration, AnimationCurve curve)
	{
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / duration;
			this.rt.anchoredPosition = Vector2.LerpUnclamped(from, to, curve.Evaluate(i));
			yield return 0;
		}
		this.rt.anchoredPosition = Vector2.Lerp(from, to, 1f);
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
	private Vector2 fromPos;

	[SerializeField]
	private Vector2 toPos;

	[SerializeField]
	private RectTransform rt;
}

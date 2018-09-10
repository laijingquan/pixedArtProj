// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnimation : MonoBehaviour
{
	private void Awake()
	{
		this.rt = (base.transform as RectTransform);
		this.image = base.GetComponent<Image>();
	}

	private void OnEnable()
	{
		this.image.transform.localRotation = Quaternion.identity;
		this.image.fillAmount = this.minGap;
		this.image.fillClockwise = false;
		base.StartCoroutine(this.AninCoroutine());
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	private void Update()
	{
		this.rt.Rotate(new Vector3(0f, 0f, this.speed * Time.deltaTime));
	}

	private IEnumerator AninCoroutine()
	{
		float offsetAngle = (1f - this.maxGap) * 360f;
		float offsetAngleTwo = this.minGap * 360f;
		for (;;)
		{
			yield return this.Radial(this.minGap, this.maxGap, this.ringSpeed);
			this.rt.Rotate(new Vector3(0f, 0f, -offsetAngle));
			this.image.fillClockwise = !this.image.fillClockwise;
			yield return new WaitForSeconds(this.ringDelay);
			yield return this.Radial(this.maxGap, this.minGap, this.ringSpeed);
			this.rt.Rotate(new Vector3(0f, 0f, -offsetAngleTwo));
			this.image.fillClockwise = !this.image.fillClockwise;
			yield return new WaitForSeconds(this.ringDelay);
		}
	}

	private IEnumerator Radial(float from, float to, float animDuration)
	{
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			this.image.fillAmount = LoadingAnimation.EaseInOutSine(from, to, i);
			yield return 0;
		}
		this.image.fillAmount = to;
		yield return null;
		yield break;
	}

	public static float EaseInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end * 0.5f * (Mathf.Cos(3.14159274f * value) - 1f) + start;
	}

	private float speed = 200f;

	private float minGap = 0.1f;

	private float maxGap = 0.8f;

	private float ringSpeed = 0.5f;

	private float ringDelay = 0.2f;

	private RectTransform rt;

	private Image image;
}

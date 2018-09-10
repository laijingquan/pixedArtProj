// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StripeBackground : MonoBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.Delay());
	}

	private void Compose()
	{
		this.layout = base.GetComponent<HorizontalLayoutGroup>();
		int num = (!SafeLayout.IsTablet) ? 132 : 124;
		float width = this.refWidth.rect.width;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"ref width ",
			width,
			"  sd ",
			this.refWidth.sizeDelta
		}));
		int num2 = Mathf.CeilToInt(width / (float)num);
		for (int i = 0; i < num2; i++)
		{
			this.AddStripe(num);
		}
	}

	private IEnumerator Delay()
	{
		yield return 0;
		yield return 0;
		this.Compose();
		yield break;
	}

	private void AddStripe(int stripeWidth)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.stripePrefab);
		gameObject.transform.SetParent(this.layout.transform);
		gameObject.transform.localScale = Vector3.one;
		((RectTransform)gameObject.transform).sizeDelta = new Vector2((float)stripeWidth, 1f);
	}

	private HorizontalLayoutGroup layout;

	[SerializeField]
	private RectTransform refWidth;

	[SerializeField]
	private GameObject stripePrefab;
}

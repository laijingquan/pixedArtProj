// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class FPSDisplay : MonoBehaviour
	{
		private void Start()
		{
			this.text = base.GetComponent<Text>();
		}

		private void Update()
		{
			this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
			float num = this.deltaTime * 1000f;
			float num2 = 1f / this.deltaTime;
			if (num2 > 30f)
			{
				this.text.color = Color.blue;
			}
			else if (num2 < 30f)
			{
				this.text.color = Color.red;
			}
			this.text.text = string.Format("{0:0.0} ms ({1:0.} fps)", num, num2);
		}

		private float deltaTime;

		private Text text;
	}
}

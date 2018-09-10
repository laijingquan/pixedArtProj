// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	private void Awake()
	{
		string @string = PlayerPrefs.GetString("foobar", string.Empty);
		Foo obj;
		if (string.IsNullOrEmpty(@string))
		{
			obj = new Foo();
		}
		else
		{
			obj = JsonUtility.FromJson<Foo>(@string);
		}
		string text = JsonUtility.ToJson(obj);
		PlayerPrefs.SetString("foobar", text);
		UnityEngine.Debug.Log(text);
	}
}

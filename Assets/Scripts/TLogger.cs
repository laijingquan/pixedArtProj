// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class TLogger : MonoBehaviour
{
	public static TLogger Instance
	{
		get
		{
			return TLogger.instance;
		}
	}

	private void Awake()
	{
		TLogger.instance = this;
	}

	public void Log(float msg)
	{
		this.Log(msg.ToString());
	}

	public void Log(int msg)
	{
		this.Log(msg.ToString());
	}

	public void Log(string msg)
	{
	}

	private void UpdateText()
	{
		this.texLabel.text = this.log;
	}

	private static TLogger instance;

	private string log;

	public int cap = 400;

	public Text texLabel;
}

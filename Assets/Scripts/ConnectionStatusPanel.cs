// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ConnectionStatusPanel : MonoBehaviour
{
	public RectTransform RectTransform
	{
		get
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = (RectTransform)base.transform;
			}
			return this.rectTransform;
		}
	}

	public void SlowConnection()
	{
		this.disconnectLabel.SetActive(false);
		this.recconectButton.SetActive(false);
		this.slowLabel.SetActive(true);
	}

	public void NoConnection()
	{
		this.slowLabel.SetActive(false);
		this.disconnectLabel.SetActive(true);
		this.recconectButton.SetActive(true);
	}

	public void ResetStatus()
	{
		this.disconnectLabel.SetActive(false);
		this.recconectButton.SetActive(false);
		this.slowLabel.SetActive(false);
	}

	[SerializeField]
	private GameObject slowLabel;

	[SerializeField]
	private GameObject disconnectLabel;

	[SerializeField]
	private GameObject recconectButton;

	private RectTransform rectTransform;
}

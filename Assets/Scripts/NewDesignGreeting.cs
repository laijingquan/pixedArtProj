// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class NewDesignGreeting : MonoBehaviour
{
	private void OnEnable()
	{
		try
		{
			Sprite sprite = Resources.Load<Sprite>("hello_screen");
			if (sprite != null)
			{
				this.image.sprite = sprite;
			}
			else
			{
				FMLogger.vCore("failed to load designUpd sprite");
			}
		}
		catch (Exception ex)
		{
			FMLogger.vCore("ex failed to load designUpd sprite. " + ex.Message);
		}
	}

	private void OnDisable()
	{
		this.image.sprite = null;
		Resources.UnloadUnusedAssets();
	}

	[SerializeField]
	private Image image;
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
	private void Awake()
	{
		this.appManager.Loaded += this.AppManagerOnLoaded;
	}

	private void AppManagerOnLoaded()
	{
		this.appManager.Loaded -= this.AppManagerOnLoaded;
		Screen.fullScreen = false;
		SceneManager.LoadSceneAsync(SceneName.menu);
	}

	public AppManager appManager;
}

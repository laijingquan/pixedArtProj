// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FeedPage : Page
{
	public void LoadContent()
	{
		this.PrepareForContnentLoad();
		this.scroll.LoadContent(!base.IsOpened);
		base.StartCoroutine(base.DelayAction(3, 0f, new Action(this.ContentLoadComplete)));
	}

	public void UnloadTextures()
	{
		this.scroll.UnloadTextures();
	}

	protected override void OnBeginClosing()
	{
	}

	protected override void OnBeginOpenning()
	{
		this.scroll.ReloadTextures();
	}

	protected override void OnClosed()
	{
		this.scroll.UnloadTextures();
	}

	protected override void OnOpened()
	{
	}

	[SerializeField]
	private FeedScroll scroll;
}

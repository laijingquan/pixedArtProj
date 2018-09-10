// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class NavBarPopup : BottomSheetPopup
{
	public void SetSafeLayoutOffset(int yOffset)
	{
		this.safeLayoutOffset = yOffset;
		this.closedPosition = new Vector2(0f, (float)this.safeLayoutOffset);
	}

	public void SetButtons(bool cont, bool rest, bool del)
	{
		this.buttons[0].SetActive(cont);
		this.buttons[1].SetActive(rest);
		this.buttons[2].SetActive(del);
		int num = this.maxOpenedOffset + this.safeLayoutOffset;
		if (!cont)
		{
			num -= this.buttonOffset;
		}
		if (!rest)
		{
			num -= this.buttonOffset;
		}
		if (!del)
		{
			num -= this.buttonOffset;
		}
		this.openPosition = new Vector2(0f, (float)num);
	}

	public GameObject[] buttons;

	public int maxOpenedOffset = 525;

	public int safeLayoutOffset;

	public int buttonOffset = 140;
}

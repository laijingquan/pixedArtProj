// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;

public class ToastManager : FMPopupManager
{
	public void ShowWrongColor()
	{
		base.Open(this.wrongColor, FMPopupManager.FMPopupPriority.ForceOpen);
		TapticEngine.Trigger(TapticEngine.FeedbackType.Light);
	}

	public void ShowHintPickColor()
	{
		base.Open(this.hintSelectColor, FMPopupManager.FMPopupPriority.ForceOpen);
		TapticEngine.Trigger(TapticEngine.FeedbackType.Light);
	}

	public void ShowHintAlreadyPainted()
	{
		base.Open(this.hintAlreadyPainted, FMPopupManager.FMPopupPriority.ForceOpen);
		TapticEngine.Trigger(TapticEngine.FeedbackType.Light);
	}

	public void ShowPaletteError(int id)
	{
		this.paletterLabel.text = LocalizationService.Instance.GetTextByKey("game_wrongPalette").Replace("1", id.ToString());
		base.Open(this.palette, FMPopupManager.FMPopupPriority.ForceOpen);
		TapticEngine.Trigger(TapticEngine.FeedbackType.Light);
	}

	public void ShowPicSaved()
	{
		base.Open(this.picSaved, FMPopupManager.FMPopupPriority.Normal);
		TapticEngine.Trigger(TapticEngine.FeedbackType.Light);
	}

	public void ShowEmptyHints()
	{
		base.Open(this.hintsEmpty, FMPopupManager.FMPopupPriority.Normal);
		TapticEngine.Trigger(TapticEngine.FeedbackType.Light);
	}

	[SerializeField]
	private FMPopup wrongColor;

	[SerializeField]
	private Text paletterLabel;

	[SerializeField]
	private FMPopup palette;

	[SerializeField]
	private FMPopup picSaved;

	[SerializeField]
	private FMPopup hintSelectColor;

	[SerializeField]
	private FMPopup hintAlreadyPainted;

	[SerializeField]
	private FMPopup hintsEmpty;
}

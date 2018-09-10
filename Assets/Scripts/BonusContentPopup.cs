// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BonusContentPopup : MonoBehaviour
{
	public void Init(string code, Action cc)
	{
		this.closePopupCallback = cc;
		this.waitingForPageResponse = true;
		this.bonusCode = code;
		this.currentReconnectAttemp = 0;
	}

	public void OnRetryClick()
	{
		this.retryClicks++;
		if (this.retryClicks > 10 || (this.retryClicks > 3 && this.openedTime > 20f))
		{
			this.closePopupCallback.SafeInvoke();
		}
		if (!WebLoader.Instance.HasInternetConnection)
		{
			WebLoader.Instance.CheckInternetConnection(null);
		}
		else
		{
			this.OnConnectionResume();
		}
	}

	private void Update()
	{
		this.openedTime += Time.deltaTime;
	}

	private void OnEnable()
	{
		WebLoader.Instance.ConnectionResume += this.OnConnectionResume;
		WebLoader.Instance.ConnectionError += this.OnConnectionError;
		this.LoadGiftCode();
	}

	private void OnDisable()
	{
		this.Reset();
		WebLoader.Instance.ConnectionResume -= this.OnConnectionResume;
		WebLoader.Instance.ConnectionError -= this.OnConnectionError;
	}

	private void OnConnectionError()
	{
		this.ShowConnectionError();
	}

	private void OnConnectionResume()
	{
		this.connectionResumeHelper.SafeResume(delegate
		{
			this.HideConnectionError();
			this.RetryLoadPageContent();
			this.ReloadFailedIcons();
		});
	}

	private void ShowConnectionError()
	{
		if (this.showingError)
		{
			return;
		}
		this.showingError = true;
		this.errorLabel.SetActive(true);
		this.errorBtn.SetActive(true);
	}

	private void HideConnectionError()
	{
		if (!this.showingError)
		{
			return;
		}
		this.showingError = false;
		this.errorLabel.SetActive(false);
		this.errorBtn.SetActive(false);
	}

	private void RetryLoadPageContent()
	{
		this.LoadGiftCode();
	}

	private void ReloadFailedIcons()
	{
		if (this.contentReceivedCount < 1)
		{
			return;
		}
		if (this.contentReceivedCount < 3)
		{
			if (this.centerSlot.IconFailedToLoad())
			{
				this.centerSlot.ReloadFailedIcon();
			}
		}
		else
		{
			for (int i = 0; i < this.multiSlots.Length; i++)
			{
				if (this.multiSlots[i].IconFailedToLoad())
				{
					this.multiSlots[i].ReloadFailedIcon();
				}
			}
		}
	}

	private void LoadGiftCode()
	{
		if (!this.waitingForPageResponse)
		{
			return;
		}
		if (this.bonusCodeTask != null)
		{
			this.bonusCodeTask.Cancel();
			this.bonusCodeTask = null;
		}
		this.waitingForPageResponse = true;
		this.loadingIndicator.SetActive(true);
		this.bonusCodeTask = new BonusCodeContentTask(this.bonusCode, new Action<bool, List<PictureData>>(this.OnBonusCodeLoaded));
		SharedData.Instance.LoadGiftCode(this.bonusCodeTask);
	}

	private void OnBonusCodeLoaded(bool success, List<PictureData> data)
	{
		this.currentReconnectAttemp++;
		if (success)
		{
			this.HideConnectionError();
			this.waitingForPageResponse = false;
			if (data != null && data.Count > 0)
			{
				if (GeneralSettings.IsOldDesign)
				{
					this.contentReceivedCount = data.Count;
				}
				else
				{
					this.contentReceivedCount = 1;
				}
				this.LoadPicContent(data);
				this.claimBtn.SetActive(true);
			}
			else if (this.closePopupCallback != null)
			{
				this.closePopupCallback();
			}
		}
		else
		{
			this.ShowConnectionError();
			if (this.currentReconnectAttemp >= 10 && this.closePopupCallback != null)
			{
				this.closePopupCallback();
			}
		}
		this.bonusCodeTask = null;
		this.loadingIndicator.SetActive(false);
	}

	private void LoadPicContent(List<PictureData> data)
	{
		if (data.Count < 3 || !GeneralSettings.IsOldDesign)
		{
			this.singlePicRoot.SetActive(true);
			this.centerSlot.Init(data[0], true);
		}
		else
		{
			this.singlePicRoot.SetActive(false);
			this.multiPicRoot.SetActive(true);
			this.multiSlots[0].Init(data[0], true);
			this.multiSlots[1].Init(data[1], false);
			this.multiSlots[2].Init(data[2], false);
		}
	}

	private void Reset()
	{
		if (this.bonusCodeTask != null)
		{
			this.bonusCodeTask.Cancel();
			this.bonusCodeTask = null;
		}
		if (this.contentReceivedCount > 0)
		{
			if (this.contentReceivedCount < 3)
			{
				this.centerSlot.Reset();
			}
			else
			{
				for (int i = 0; i < this.multiSlots.Length; i++)
				{
					this.multiSlots[i].Reset();
				}
			}
		}
		this.multiPicRoot.SetActive(false);
		this.singlePicRoot.SetActive(true);
		this.HideConnectionError();
		this.contentReceivedCount = 0;
		this.waitingForPageResponse = false;
		this.claimBtn.SetActive(false);
		this.retryClicks = 0;
		this.openedTime = 0f;
	}

	[SerializeField]
	private GameObject claimBtn;

	[SerializeField]
	private GameObject errorBtn;

	[SerializeField]
	private GameObject errorLabel;

	[SerializeField]
	private GameObject loadingIndicator;

	[SerializeField]
	private ConnectionResumeHelper connectionResumeHelper;

	[SerializeField]
	private GameObject multiPicRoot;

	[SerializeField]
	private GameObject singlePicRoot;

	[SerializeField]
	private BasicIconSlot centerSlot;

	[SerializeField]
	private BasicIconSlot[] multiSlots;

	private int contentReceivedCount;

	private bool waitingForPageResponse;

	private bool showingError;

	private BonusCodeContentTask bonusCodeTask;

	private string bonusCode;

	private Action closePopupCallback;

	private float openedTime;

	private int retryClicks;

	private const int reconnectMaxAttempts = 10;

	private int currentReconnectAttemp;
}

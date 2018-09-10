// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class PictureDataDownloader
{
	public PictureDataDownloader(Func<IEnumerator, Coroutine> startCoroutine, Action<Coroutine> stopCoroutine)
	{
		this.startCoroutine = startCoroutine;
		this.stopCoroutine = stopCoroutine;
	}

	private string directory
	{
		get
		{
			if (this.task != null && this.task.PictureData != null)
			{
				return this.task.PictureData.PackId.ToString();
			}
			return "-1";
		}
	}

	private string iconPath
	{
		get
		{
			if (this.task != null && this.task.PictureData != null)
			{
				return this.task.PictureData.Id + "i";
			}
			return "-1i";
		}
	}

	private string linePath
	{
		get
		{
			if (this.task != null && this.task.PictureData != null)
			{
				return this.task.PictureData.Id.ToString();
			}
			return "-1";
		}
	}

	private string coloredPath
	{
		get
		{
			if (this.task != null && this.task.PictureData != null)
			{
				return this.task.PictureData.Id + "c";
			}
			return "-1c";
		}
	}

	private string jsonPath
	{
		get
		{
			if (this.task != null && this.task.PictureData != null)
			{
				return this.task.PictureData.Id + "t";
			}
			return "-1t";
		}
	}

	public void Run(DownloadPictureTask picTask)
	{
		this.Clean();
		this.task = picTask;
		this.task.AddCancelationHandler(new Action(this.OnTaskCanceled));
		this.task.SetTimeouts(90f, 5f, 15f);
		this.proccessor = this.startCoroutine(this.ProccessDownload());
	}

	private IEnumerator ProccessDownload()
	{
		DownloadPictureTask currentTask = this.task;
		PictureData pd = currentTask.PictureData;
		this.CheckPrecacheResources();
		this.CreateTasks();
		bool wait = true;
		bool allFilesOk = false;
		float waitTime = 0f;
		int triggerIndex = 0;
		bool hasDownloadedResource = false;
		int maxDownloadAttempts = 4;
		int maxFirstStageAttempts = 2;
		int iconAttempts = 0;
		int lineAttempts = 0;
		int jsonAttempts = 0;
		int coloredAttempts = 0;
		int currentFirstStageResets = 0;
		int maxFirstStageResets = 3;
		float currentFirstStageTime = 0f;
		int currentSecondStageResets = 0;
		int maxSecondStageResets = 5;
		float currentSecondStageTime = 0f;
		float resetTimeStep = 0.5f;
		while (wait)
		{
			if (!currentTask.IsRunning)
			{
				yield break;
			}
			waitTime += Time.deltaTime;
			if (triggerIndex == 0)
			{
				if (waitTime > this.task.FirstSlowNotifTimeout)
				{
					triggerIndex = 1;
					this.task.TriggerLongLoad();
				}
			}
			else if (triggerIndex == 1)
			{
				if (waitTime > this.task.SecondSlowNotifTimeout)
				{
					triggerIndex = 2;
					this.task.TriggerExtraLongLoad();
				}
			}
			else if (waitTime > this.task.Timeout)
			{
				FMLogger.vCore("pic dl global timeout " + this.task.Timeout);
				wait = false;
			}
			if (!hasDownloadedResource)
			{
				if (this.iconState == PictureDataDownloader.DownloadState.Downloaded)
				{
					hasDownloadedResource = true;
				}
				if (this.lineState == PictureDataDownloader.DownloadState.Downloaded)
				{
					hasDownloadedResource = true;
				}
				if (this.jsonState == PictureDataDownloader.DownloadState.Downloaded)
				{
					hasDownloadedResource = true;
				}
				if (this.colorState == PictureDataDownloader.DownloadState.Downloaded)
				{
					hasDownloadedResource = true;
				}
				if (hasDownloadedResource)
				{
					FMLogger.vCore("pic dl. one of resources was downloaded. dl next stage");
				}
				if (this.iconState == PictureDataDownloader.DownloadState.Failed && iconAttempts < maxFirstStageAttempts)
				{
					iconAttempts++;
					this.StartIconTask(pd);
				}
				if (this.lineState == PictureDataDownloader.DownloadState.Failed && lineAttempts < maxFirstStageAttempts)
				{
					lineAttempts++;
					this.StartLineTask(pd);
				}
				if (this.jsonState == PictureDataDownloader.DownloadState.Failed && jsonAttempts < maxFirstStageAttempts)
				{
					jsonAttempts++;
					this.StartJsonTask(pd);
				}
				if (this.colorState == PictureDataDownloader.DownloadState.Failed && coloredAttempts < maxFirstStageAttempts)
				{
					coloredAttempts++;
					this.StartColoredTask(pd);
				}
				int num = 0;
				int num2 = 0;
				if (this.iconState == PictureDataDownloader.DownloadState.Failed)
				{
					if (iconAttempts == maxFirstStageAttempts)
					{
						num2++;
					}
				}
				else if (this.iconState == PictureDataDownloader.DownloadState.Cached)
				{
					num++;
				}
				if (this.lineState == PictureDataDownloader.DownloadState.Failed)
				{
					if (lineAttempts == maxFirstStageAttempts)
					{
						num2++;
					}
				}
				else if (this.lineState == PictureDataDownloader.DownloadState.Cached)
				{
					num++;
				}
				if (this.jsonState == PictureDataDownloader.DownloadState.Failed)
				{
					if (jsonAttempts == maxFirstStageAttempts)
					{
						num2++;
					}
				}
				else if (this.jsonState == PictureDataDownloader.DownloadState.Cached)
				{
					num++;
				}
				if (this.colorState == PictureDataDownloader.DownloadState.Failed)
				{
					if (coloredAttempts == maxFirstStageAttempts)
					{
						num2++;
					}
				}
				else if (this.colorState == PictureDataDownloader.DownloadState.Cached)
				{
					num++;
				}
				bool flag = num2 + num == 4;
				if (flag)
				{
					if (currentFirstStageResets < maxFirstStageResets)
					{
						currentFirstStageTime += Time.deltaTime;
						if (currentFirstStageTime >= resetTimeStep)
						{
							FMLogger.vCore("pic dl. initial reset attemps " + currentFirstStageResets);
							currentFirstStageResets++;
							currentFirstStageTime = 0f;
							iconAttempts = 0;
							lineAttempts = 0;
							jsonAttempts = 0;
							coloredAttempts = 0;
						}
					}
					else
					{
						FMLogger.vCore("pic dl. failed after " + currentFirstStageResets + " attemps");
						wait = false;
					}
				}
				else if ((this.iconState == PictureDataDownloader.DownloadState.Downloaded || this.iconState == PictureDataDownloader.DownloadState.Cached) && (this.lineState == PictureDataDownloader.DownloadState.Downloaded || this.lineState == PictureDataDownloader.DownloadState.Cached) && (this.jsonState == PictureDataDownloader.DownloadState.Downloaded || this.jsonState == PictureDataDownloader.DownloadState.Cached) && (this.colorState == PictureDataDownloader.DownloadState.Downloaded || this.colorState == PictureDataDownloader.DownloadState.Cached))
				{
					wait = false;
					allFilesOk = true;
				}
			}
			else
			{
				if (this.iconState == PictureDataDownloader.DownloadState.Failed && iconAttempts < maxDownloadAttempts)
				{
					iconAttempts++;
					this.StartIconTask(pd);
				}
				else if (this.lineState == PictureDataDownloader.DownloadState.Failed && lineAttempts < maxDownloadAttempts)
				{
					lineAttempts++;
					this.StartLineTask(pd);
				}
				else if (this.jsonState == PictureDataDownloader.DownloadState.Failed && jsonAttempts < maxDownloadAttempts)
				{
					jsonAttempts++;
					this.StartJsonTask(pd);
				}
				else if (this.colorState == PictureDataDownloader.DownloadState.Failed && coloredAttempts < maxDownloadAttempts)
				{
					coloredAttempts++;
					this.StartColoredTask(pd);
				}
				if ((this.iconState == PictureDataDownloader.DownloadState.Failed && iconAttempts >= maxDownloadAttempts) || (this.lineState == PictureDataDownloader.DownloadState.Failed && lineAttempts >= maxDownloadAttempts) || (this.jsonState == PictureDataDownloader.DownloadState.Failed && jsonAttempts >= maxDownloadAttempts) || (this.colorState == PictureDataDownloader.DownloadState.Failed && coloredAttempts >= maxDownloadAttempts))
				{
					if (currentSecondStageResets < maxSecondStageResets)
					{
						currentSecondStageTime += Time.deltaTime;
						if (currentSecondStageTime >= resetTimeStep)
						{
							FMLogger.vCore("pic dl. 2nd stage reset attemps " + currentSecondStageResets);
							currentSecondStageResets++;
							currentSecondStageTime = (float)(maxDownloadAttempts - maxFirstStageAttempts);
							iconAttempts = maxDownloadAttempts - maxFirstStageAttempts;
							lineAttempts = maxDownloadAttempts - maxFirstStageAttempts;
							jsonAttempts = maxDownloadAttempts - maxFirstStageAttempts;
							coloredAttempts = maxDownloadAttempts - maxFirstStageAttempts;
						}
					}
					else
					{
						FMLogger.vCore("pic dl. 2nd stage failed after " + currentSecondStageResets + " attemps");
						wait = false;
					}
				}
				if ((this.iconState == PictureDataDownloader.DownloadState.Downloaded || this.iconState == PictureDataDownloader.DownloadState.Cached) && (this.lineState == PictureDataDownloader.DownloadState.Downloaded || this.lineState == PictureDataDownloader.DownloadState.Cached) && (this.jsonState == PictureDataDownloader.DownloadState.Downloaded || this.jsonState == PictureDataDownloader.DownloadState.Cached) && (this.colorState == PictureDataDownloader.DownloadState.Downloaded || this.colorState == PictureDataDownloader.DownloadState.Cached))
				{
					wait = false;
					allFilesOk = true;
				}
			}
			yield return null;
			this.proccessor = null;
		}
		yield return null;
		if (allFilesOk && currentTask == this.task)
		{
			FMLogger.Log("pic dl complete " + pd.Id);
			PictureData result = SharedData.Instance.AddPictureToPack(pd);
			this.task.Result(true, result);
		}
		else
		{
			FMLogger.Log("pic dl error " + pd.Id);
			this.task.Result(false, null);
		}
		yield break;
	}

	private void CheckPrecacheResources()
	{
		PictureData pictureData = this.task.PictureData;
		if (FileHelper.FileExist(this.directory, this.iconPath))
		{
			this.iconState = PictureDataDownloader.DownloadState.Cached;
			FMLogger.Log("skip dl icon, already stored");
		}
		if (FileHelper.FileExist(this.directory, this.linePath))
		{
			this.lineState = PictureDataDownloader.DownloadState.Cached;
			FMLogger.Log("skip dl line, already stored");
		}
		if (pictureData.FillType == FillAlgorithm.Flood)
		{
			if (FileHelper.FileExist(this.directory, this.coloredPath))
			{
				this.colorState = PictureDataDownloader.DownloadState.Cached;
				FMLogger.Log("skip dl colormap, already stored");
			}
		}
		else
		{
			this.colorState = PictureDataDownloader.DownloadState.Cached;
			FMLogger.Log("skip dl colormap, not needed anymore");
		}
		if (FileHelper.FileExist(this.directory, this.jsonPath))
		{
			this.jsonState = PictureDataDownloader.DownloadState.Cached;
			FMLogger.Log("skip dl json, already stored");
		}
	}

	private void CreateTasks()
	{
		PictureData pictureData = this.task.PictureData;
		if (this.iconState != PictureDataDownloader.DownloadState.Cached)
		{
			this.StartIconTask(pictureData);
		}
		if (this.lineState != PictureDataDownloader.DownloadState.Cached)
		{
			this.StartLineTask(pictureData);
		}
		if (this.jsonState != PictureDataDownloader.DownloadState.Cached)
		{
			this.StartJsonTask(pictureData);
		}
		if (this.colorState != PictureDataDownloader.DownloadState.Cached)
		{
			this.StartColoredTask(pictureData);
		}
	}

	private void StartColoredTask(PictureData pd)
	{
		this.colorState = PictureDataDownloader.DownloadState.Pending;
		this.coloredTask = new RawBytesDownloadTask(pd.WebPath.baseUrls, pd.WebPath.colored, delegate(bool success, byte[] bytes)
		{
			if (success)
			{
				FileHelper.SaveRawBytesToFile(bytes, this.directory, this.coloredPath);
				this.colorState = PictureDataDownloader.DownloadState.Downloaded;
			}
			else
			{
				this.colorState = PictureDataDownloader.DownloadState.Failed;
			}
		});
		WebLoader.Instance.LoadRawBytes(this.coloredTask);
	}

	private void StartJsonTask(PictureData pd)
	{
		this.jsonState = PictureDataDownloader.DownloadState.Pending;
		this.jsonTask = new WebGetTask(pd.WebPath.baseUrls, pd.WebPath.json, delegate(bool success, string tex)
		{
			if (success && tex != null)
			{
				FileHelper.SaveStringToFile(tex, this.directory, this.jsonPath);
				this.jsonState = PictureDataDownloader.DownloadState.Downloaded;
			}
			else
			{
				this.jsonState = PictureDataDownloader.DownloadState.Failed;
			}
		});
		WebLoader.Instance.LoadText(this.jsonTask);
	}

	private void StartLineTask(PictureData pd)
	{
		if (this.lineTask != null)
		{
			this.lineTask.Cancel();
		}
		this.lineState = PictureDataDownloader.DownloadState.Pending;
		this.lineTask = new RawBytesDownloadTask(pd.WebPath.baseUrls, pd.WebPath.lineart, delegate(bool success, byte[] bytes)
		{
			if (success)
			{
				FileHelper.SaveRawBytesToFile(bytes, this.directory, this.linePath);
				this.lineState = PictureDataDownloader.DownloadState.Downloaded;
			}
			else
			{
				this.lineState = PictureDataDownloader.DownloadState.Failed;
			}
		});
		WebLoader.Instance.LoadRawBytes(this.lineTask);
	}

	private void StartIconTask(PictureData pd)
	{
		if (this.iconTask != null)
		{
			this.iconTask.Cancel();
		}
		this.iconState = PictureDataDownloader.DownloadState.Pending;
		this.iconTask = new RawBytesDownloadTask(pd.WebPath.baseUrls, pd.WebPath.icon, delegate(bool success, byte[] bytes)
		{
			if (success)
			{
				FileHelper.SaveRawBytesToFile(bytes, this.directory, this.iconPath);
				this.iconState = PictureDataDownloader.DownloadState.Downloaded;
			}
			else
			{
				this.iconState = PictureDataDownloader.DownloadState.Failed;
			}
		});
		WebLoader.Instance.LoadRawBytes(this.iconTask);
	}

	private void OnTaskCanceled()
	{
		this.Clean();
	}

	private void Clean()
	{
		if (this.proccessor != null)
		{
			this.stopCoroutine(this.proccessor);
		}
		this.iconState = PictureDataDownloader.DownloadState.Pending;
		this.lineState = PictureDataDownloader.DownloadState.Pending;
		this.jsonState = PictureDataDownloader.DownloadState.Pending;
		this.colorState = PictureDataDownloader.DownloadState.Pending;
		if (this.iconTask != null)
		{
			this.iconTask.Cancel();
		}
		if (this.lineTask != null)
		{
			this.lineTask.Cancel();
		}
		if (this.jsonTask != null)
		{
			this.jsonTask.Cancel();
		}
		if (this.coloredTask != null)
		{
			this.coloredTask.Cancel();
		}
		this.task = null;
	}

	private const float DOWNLOAD_TIMEOUT = 90f;

	private const float DOWNLOAD_WARNING = 5f;

	private const float DOWNLOAD_CAN_CANCEL = 15f;

	private DownloadPictureTask task;

	private Func<IEnumerator, Coroutine> startCoroutine;

	private Action<Coroutine> stopCoroutine;

	private PictureDataDownloader.DownloadState iconState;

	private PictureDataDownloader.DownloadState lineState;

	private PictureDataDownloader.DownloadState colorState;

	private PictureDataDownloader.DownloadState jsonState;

	private RawBytesDownloadTask iconTask;

	private RawBytesDownloadTask lineTask;

	private RawBytesDownloadTask coloredTask;

	private WebGetTask jsonTask;

	private Coroutine proccessor;

	private enum DownloadState
	{
		Pending,
		Failed = -1,
		Downloaded = 1,
		Cached
	}
}

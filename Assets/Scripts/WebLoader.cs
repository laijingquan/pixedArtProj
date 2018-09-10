// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

public class WebLoader : MonoBehaviour
{
	 
	public event Action ConnectionError;

	 
	public event Action ConnectionResume;

	public bool HasInternetConnection
	{
		get
		{
			return !this.noInternet;
		}
	}

	public static WebLoader Instance
	{
		get
		{
			return WebLoader.instance;
		}
	}

	private void Awake()
	{
		WebLoader.instance = this;
	}

	private void Update()
	{
		this.UpdateIconConnectionsQueue();
	}

	private void UpdateIconConnectionsQueue()
	{
		for (int i = this.iconRequestQueue.Count - 1; i >= 0; i--)
		{
			if (this.iconRequestQueue[i] == null || this.iconRequestQueue[i].IsCanceled)
			{
				this.iconRequestQueue.RemoveAt(i);
			}
		}
		if (this.iconRequestQueue.Count > 0 && this.currentIconConnections < 4)
		{
			IconDownloadTask task = this.iconRequestQueue[0];
			base.StartCoroutine(this.LoadIconAsync(task, true));
			this.currentIconConnections++;
			this.iconRequestQueue.RemoveAt(0);
		}
	}

	public void LoadText(WebGetTask task)
	{
		base.StartCoroutine(this.LoadTextAsync(task));
	}

	public void LoadText(WebPostTask task)
	{
		base.StartCoroutine(this.LoadTextAsync(task));
	}

	public void LoadRawBytes(RawBytesDownloadTask task)
	{
		base.StartCoroutine(this.LoadRawBytesAsync(task));
	}

	public void DownloadIcon(IconDownloadTask task)
	{
		if (this.iconRequestQueue.Find((IconDownloadTask t) => t.RelativeUrl.Equals(task.RelativeUrl)) == null)
		{
			this.iconRequestQueue.Add(task);
		}
		else
		{
			UnityEngine.Debug.LogError("*******task duplicate");
		}
	}

	public void CheckInternetConnection(Action<bool> result = null)
	{
		this.InternetCheck(result);
	}

	private void OnApplicationPause(bool isPause)
	{
		if (!isPause && this.noInternet && this.ConnectionResume != null)
		{
			if (this.internetCheckerCoroutine != null)
			{
				base.StopCoroutine(this.internetCheckerCoroutine);
			}
			FMLogger.Log("launch IC from pause");
			this.internetCheckerCoroutine = base.StartCoroutine(this.InternetChecker());
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus && this.noInternet && this.ConnectionResume != null)
		{
			if (this.internetCheckerCoroutine != null)
			{
				base.StopCoroutine(this.internetCheckerCoroutine);
			}
			FMLogger.Log("launch IC from focus");
			this.internetCheckerCoroutine = base.StartCoroutine(this.InternetChecker());
		}
	}

	private void StepFailedPicture()
	{
		this.currentFailedPicStreak++;
		if (this.currentFailedPicStreak >= 5)
		{
			this.SendConnectionError();
		}
	}

	private void SendConnectionError()
	{
		if (this.noInternet)
		{
			return;
		}
		this.noInternet = true;
		this.ConnectionError.SafeInvoke();
		FMLogger.Log("launch IC from error");
		this.internetCheckerCoroutine = base.StartCoroutine(this.InternetChecker());
	}

	private void SendConnectionResume()
	{
		this.currentFailedPicStreak = 0;
		if (!this.noInternet)
		{
			return;
		}
		this.noInternet = false;
		this.ConnectionResume.SafeInvoke();
		if (this.internetCheckerCoroutine != null)
		{
			base.StopCoroutine(this.internetCheckerCoroutine);
		}
	}

	private IEnumerator InternetChecker()
	{
		int fastPacedAttemps = 5;
		float shortTimeStep = 3f;
		float longTimeStep = 60f;
		while (fastPacedAttemps > 0 && this.noInternet)
		{
			fastPacedAttemps--;
			FMLogger.Log("short step internet check");
			this.InternetCheck(null);
			yield return new WaitForSeconds(shortTimeStep);
		}
		while (this.noInternet)
		{
			FMLogger.Log("short step internet check");
			this.InternetCheck(null);
			yield return new WaitForSeconds(longTimeStep);
		}
		yield break;
	}

	private void InternetCheck(Action<bool> result = null)
	{
		WebGetTask task = new WebGetTask("https://coloring-gp.x-flow.app/api/v_2/checker.php", delegate(bool success, string text)
		{
			if (success)
			{
				FMLogger.Log("intrnet resume");
				this.StopCoroutine(this.internetCheckerCoroutine);
				this.SendConnectionResume();
			}
			else
			{
				FMLogger.Log("no internet");
			}
			if (result != null)
			{
				result(success);
			}
		});
		base.StartCoroutine(this.LoadTextAsync(task));
	}

	private IEnumerator DownloadPictureWaiter(DownloadPictureTask task)
	{
		PictureData pd = task.PictureData;
		int id = pd.Id;
		int packId = pd.PackId;
		int iconState = 0;
		int lineState = 0;
		int colorState = 0;
		int jsonState = 0;
		string directory = packId.ToString();
		string iconPath = id + "i";
		string linePath = id.ToString();
		string coloredPath = id + "c";
		string jsonPath = id + "t";
		RawBytesDownloadTask iconTask = null;
		RawBytesDownloadTask lineTask = null;
		RawBytesDownloadTask coloredTask = null;
		WebGetTask jsonTask = null;
		if (FileHelper.FileExist(directory, iconPath))
		{
			iconState = 1;
			FMLogger.Log("skip dl icon, already stored");
		}
		else
		{
			iconTask = new RawBytesDownloadTask(pd.WebPath.baseUrls, pd.WebPath.icon, delegate(bool success, byte[] bytes)
			{
				if (success)
				{
					FileHelper.SaveRawBytesToFile(bytes, directory, iconPath);
					iconState = 1;
				}
				else
				{
					iconState = -1;
				}
			});
		}
		if (FileHelper.FileExist(directory, linePath))
		{
			lineState = 1;
			FMLogger.Log("skip dl line, already stored");
		}
		else
		{
			lineTask = new RawBytesDownloadTask(pd.WebPath.baseUrls, pd.WebPath.lineart, delegate(bool success, byte[] bytes)
			{
				if (success)
				{
					FileHelper.SaveRawBytesToFile(bytes, directory, linePath);
					lineState = 1;
				}
				else
				{
					lineState = -1;
				}
			});
		}
		if (pd.FillType == FillAlgorithm.Flood)
		{
			if (FileHelper.FileExist(directory, coloredPath))
			{
				colorState = 1;
				FMLogger.Log("skip dl colormap, already stored");
			}
			else
			{
				coloredTask = new RawBytesDownloadTask(pd.WebPath.baseUrls, pd.WebPath.colored, delegate(bool success, byte[] bytes)
				{
					if (success)
					{
						FileHelper.SaveRawBytesToFile(bytes, directory, coloredPath);
						colorState = 1;
					}
					else
					{
						colorState = -1;
					}
				});
			}
		}
		else
		{
			colorState = 1;
			FMLogger.Log("skip dl colormap, not needed anymore");
		}
		if (FileHelper.FileExist(directory, jsonPath))
		{
			jsonState = 1;
			FMLogger.Log("skip dl json, already stored");
		}
		else
		{
			jsonTask = new WebGetTask(pd.WebPath.baseUrls, pd.WebPath.json, delegate(bool success, string tex)
			{
				if (success && tex != null)
				{
					FileHelper.SaveStringToFile(tex, directory, jsonPath);
					jsonState = 1;
				}
				else
				{
					jsonState = -1;
				}
			});
		}
		if (iconTask != null)
		{
			base.StartCoroutine(this.LoadRawBytesAsync(iconTask));
		}
		if (lineTask != null)
		{
			base.StartCoroutine(this.LoadRawBytesAsync(lineTask));
		}
		if (coloredTask != null)
		{
			base.StartCoroutine(this.LoadRawBytesAsync(coloredTask));
		}
		if (jsonTask != null)
		{
			base.StartCoroutine(this.LoadTextAsync(jsonTask));
		}
		yield return 0;
		bool wait = true;
		bool allFilesOk = false;
		float waitTime = 0f;
		while (wait)
		{
			waitTime += Time.deltaTime;
			if (waitTime > task.Timeout)
			{
				if (colorState == 0 && coloredTask != null)
				{
					coloredTask.Cancel();
				}
				if (lineState == 0 && lineTask != null)
				{
					lineTask.Cancel();
				}
				if (iconState == 0 && iconTask != null)
				{
					iconTask.Cancel();
				}
				if (jsonState == 0 && jsonTask != null)
				{
					jsonTask.Cancel();
				}
				wait = false;
			}
			else if (iconState == -1 || lineState == -1 || colorState == -1 || jsonState == -1)
			{
				allFilesOk = false;
				wait = false;
			}
			else if (iconState == 1 && lineState == 1 && colorState == 1 && jsonState == 1)
			{
				allFilesOk = true;
				wait = false;
			}
			yield return 0;
		}
		if (allFilesOk)
		{
			FMLogger.Log("pic dl complete " + pd.Id);
			PictureData result = SharedData.Instance.AddPictureToPack(pd);
			task.Result(true, result);
		}
		else
		{
			FMLogger.Log("pic dl error " + pd.Id);
			task.Result(false, null);
		}
		yield return 0;
		yield break;
	}

	private IEnumerator LoadIconAsync(IconDownloadTask task, bool queued = true)
	{
		for (int i = 0; i < task.BaseUrls.Length; i++)
		{
			using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(task.BaseUrls[i] + task.RelativeUrl))
			{
				//www.timeout = 13;
				yield return www.Send();
				if (!www.isNetworkError && www.responseCode == 200L)
				{
					if (task.IsCanceled)
					{
						Texture2D content = DownloadHandlerTexture.GetContent(www);
						if (content != null && content.width > 10 && content.height > 10)
						{
							task.FallbackCancelation(content);
						}
					}
					else
					{
						Texture2D content2 = DownloadHandlerTexture.GetContent(www);
						if (content2 != null && content2.width > 10 && content2.height > 10)
						{
							content2.wrapMode = TextureWrapMode.Clamp;
							task.Result(true, content2);
						}
						else
						{
							task.Result(false, null);
						}
						this.SendConnectionResume();
					}
					yield return 0;
					this.currentIconConnections--;
					yield break;
				}
				FMLogger.Log(string.Concat(new object[]
				{
					"icon dl error.",
					i,
					". ",
					task.BaseUrls[i],
					string.Empty,
					task.RelativeUrl,
					" msg:",
					www.error
				}));
			}
		}
		this.currentIconConnections--;
		this.StepFailedPicture();
		if (!task.IsCanceled)
		{
			FMLogger.Log("*************error task:" + task.RelativeUrl);
			task.Result(false, null);
		}
		yield break;
	}

	private IEnumerator LoadTextAsync(WebGetTask task)
	{
		int timeout = 60;
		TimeoutTask timeoutTask = new TimeoutTask((float)timeout, delegate()
		{
			if (task != null && task.IsRunning)
			{
				FMLogger.vCore("LoadTextAsync timeout");
				task.Result(false, null);
			}
		});
		timeoutTask.Run();
		for (int i = 0; i < task.BaseUrls.Length; i++)
		{
			using (UnityWebRequest www = UnityWebRequest.Get(task.BaseUrls[i] + task.RelativeUrl))
			{
				yield return www.Send();
				if (task == null || !task.IsRunning)
				{
					yield break;
				}
				if (!www.isNetworkError && www.responseCode == 200L)
				{
					string text = www.downloadHandler.text;
					if (!string.IsNullOrEmpty(text))
					{
						timeoutTask.Cancel();
						task.Result(true, text);
					}
					else
					{
						timeoutTask.Cancel();
						task.Result(false, null);
					}
					this.SendConnectionResume();
					yield break;
				}
			}
		}
		if (task.IsRunning)
		{
			timeoutTask.Cancel();
			task.Result(false, null);
		}
		this.SendConnectionError();
		yield break;
	}

	private IEnumerator LoadTextAsync(WebPostTask task)
	{
		int timeout = 60;
		TimeoutTask timeoutTask = new TimeoutTask((float)timeout, delegate()
		{
			if (task != null && task.IsRunning)
			{
				FMLogger.vCore("LoadTextAsync timeout");
				task.Result(false, null);
			}
		});
		timeoutTask.Run();
		for (int i = 0; i < task.BaseUrls.Length; i++)
		{
			using (UnityWebRequest www = UnityWebRequest.Post(task.BaseUrls[i] + task.RelativeUrl, task.Data))
			{
				yield return www.Send();
				if (task == null || !task.IsRunning)
				{
					yield break;
				}
				if (!www.isNetworkError && www.responseCode == 200L)
				{
					string text = www.downloadHandler.text;
					if (!string.IsNullOrEmpty(text))
					{
						timeoutTask.Cancel();
						task.Result(true, text);
					}
					else
					{
						timeoutTask.Cancel();
						task.Result(false, null);
					}
					this.SendConnectionResume();
					yield break;
				}
			}
		}
		if (task.IsRunning)
		{
			timeoutTask.Cancel();
			task.Result(false, null);
		}
		this.SendConnectionError();
		yield break;
	}

	private IEnumerator LoadRawBytesAsync(RawBytesDownloadTask task)
	{
		for (int i = 0; i < task.BaseUrls.Length; i++)
		{
			using (WWW www = new WWW(task.BaseUrls[i] + task.RelativeUrl))
			{
				yield return www;
				if (task.IsCanceled)
				{
					yield break;
				}
				if (string.IsNullOrEmpty(www.error))
				{
					byte[] bytes = www.bytes;
					if (bytes != null && bytes.Length > 0)
					{
						task.Result(true, bytes);
					}
					else
					{
						FMLogger.Log("*************raw bytes array error. never should happen");
						task.Result(false, null);
					}
					this.SendConnectionResume();
					yield break;
				}
			}
		}
		if (!task.IsCanceled)
		{
			this.StepFailedPicture();
			FMLogger.Log("*************raw bytes dl error .task:" + task.RelativeUrl);
			task.Result(false, null);
		}
		yield break;
	}

	private bool noInternet;

	private static WebLoader instance;

	private const int REQUEST_TIMEOUT = 60;

	private const int PIC_FAILED_LIMIT_FOR_ERROR = 5;

	private int currentFailedPicStreak;

	private List<IconDownloadTask> iconRequestQueue = new List<IconDownloadTask>();

	private int currentIconConnections;

	private const int MAX_ICON_CONNECTIONS = 4;

	private Coroutine internetCheckerCoroutine;
}

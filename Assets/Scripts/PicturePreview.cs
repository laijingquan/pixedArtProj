// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicturePreview : MonoBehaviour
{
	public bool Inited { get; private set; }

	public bool IsAnimating
	{
		get
		{
			return this.coloringAnimation.IsAnimating;
		}
	}

	public void ReplayAnim()
	{
		this.coloringAnimation.Replay();
	}

	public void FinishAnim()
	{
		if (this.animRepeatCoroutine != null)
		{
			base.StopCoroutine(this.animRepeatCoroutine);
		}
		this.coloringAnimation.ForceFinishAnimation();
	}

	public void Init(PicItem item)
	{
		ImageManager.Instance.UnloadUnusedTextures();
		this.Inited = false;
		this.picItem = item;
		this.iconUpdate = base.StartCoroutine(this.IconUpdate(item));
		this.coloringAnimation.Completed += this.OnAnimationCompleted;
	}

	public void Play()
	{
		if (this.iconUpdate != null)
		{
			base.StopCoroutine(this.iconUpdate);
		}
		this.animInit = base.StartCoroutine(this.LazyInit());
	}

	public void StopAnimation()
	{
		if (this.Inited)
		{
			if (this.coloringAnimation.IsAnimating)
			{
				this.coloringAnimation.StopAnimation();
			}
		}
		else if (this.animInit != null)
		{
			base.StopCoroutine(this.animInit);
			this.animInit = null;
		}
	}

	public void Clean()
	{
		FMLogger.vCore("preview clean");
		if (this.animInit != null)
		{
			base.StopCoroutine(this.animInit);
			this.animInit = null;
		}
		this.coloringAnimation.Completed -= this.OnAnimationCompleted;
		this.lowResCanvas.gameObject.SetActive(true);
		this.lowResCanvas.alpha = 1f;
		this.highResCanvas.gameObject.SetActive(false);
		this.highResCanvas.alpha = 0f;
		this.iconLine.texture = null;
		this.iconSave.texture = null;
		this.drawImg.texture = null;
		this.lineImg.texture = null;
		this.coloringAnimation.Clean();
		if (this.chopFill != null)
		{
			List<Texture2D> list = this.chopFill.Clean();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null)
				{
					UnityEngine.Object.Destroy(list[i]);
					FMLogger.vCore("preview chopFill destroy tex");
				}
			}
			this.chopFill = null;
		}
		if (this.initWaitCoroutine != null)
		{
			base.StopCoroutine(this.initWaitCoroutine);
		}
		if (this.texForShare != null)
		{
			UnityEngine.Object.Destroy(this.texForShare);
			this.texForShare = null;
		}
	}

	public void CutShareableTexture(Action<Texture2D> callback)
	{
		if (this.Inited)
		{
			this.InternalCutShareableTex(callback);
		}
		else
		{
			this.initWaitCoroutine = base.StartCoroutine(this.WaitForInit(delegate
			{
				this.InternalCutShareableTex(callback);
			}));
		}
	}

	private IEnumerator WaitForInit(Action a)
	{
		while (!this.Inited)
		{
			yield return 0;
		}
		yield return 0;
		this.initWaitCoroutine = null;
		a();
		yield break;
	}

	private void InternalCutShareableTex(Action<Texture2D> callback)
	{
		if (this.texForShare == null)
		{
			this.coloringAnimation.ForceFinishAnimation();
			Texture2D texture2D = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.wrapMode = TextureWrapMode.Clamp;
			ImageManager.Instance.ShareTexture(this.coloringAnimation.DrawTex, this.coloringAnimation.LineTex, texture2D);
			this.texForShare = texture2D;
		}
		callback(this.texForShare);
	}

	private void LoadResources(PictureData picData)
	{
		this.line = FileHelper.LoadTextureFromFile(picData.LineArt);
		this.line.wrapMode = TextureWrapMode.Clamp;
		string json = FileHelper.LoadTextFromFile(picData.Palette);
		this.palleteData = JsonUtility.FromJson<PaletteData>(json);
	}

	private Color IdToColor(int id)
	{
		return this.colors[id];
	}

	private IEnumerator LazyInit()
	{
		yield return 0;
		yield return 0;
		yield return 0;
		this.LoadResources(this.picItem.PictureData);
		this.lineImg.texture = this.line;
		this.colors = new List<Color>();
		for (int i = 0; i < this.palleteData.entities.Length; i++)
		{
			this.palleteData.entities[i].color.a = byte.MaxValue;
			this.colors.Add(this.palleteData.entities[i].color);
		}
		yield return 0;
		this.chopFill = new ChopFill();
		this.chopFill.Create(this.line, this.palleteData, true);
		this.drawImg.texture = this.chopFill.DrawTex;
		yield return this.CrossFade(0.7f);
		this.coloringAnimation.InitAsPreview(this.chopFill.DrawTex, this.chopFill.Pixels, this.picItem.SaveData.steps, this.chopFill, new Func<int, Color>(this.IdToColor));
		this.coloringAnimation.StartAnimation(0.5f, 0f);
		this.animInit = null;
		this.Inited = true;
		FMLogger.vCore("preview inited");
		yield break;
	}

	private void OnAnimationCompleted()
	{
		this.animRepeatCoroutine = base.StartCoroutine(this.DelayAction(3f, new Action(this.ReplayAnim)));
	}

	private IEnumerator DelayAction(float delay, Action a)
	{
		yield return new WaitForSeconds(delay);
		a();
		yield break;
	}

	private IEnumerator CrossFade(float animDuration)
	{
		float i = 0f;
		float currentTime = 0f;
		float min = 0f;
		float max = 1f;
		this.highResCanvas.gameObject.SetActive(true);
		this.highResCanvas.alpha = min;
		this.lowResCanvas.alpha = max;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			this.lowResCanvas.alpha = Mathf.Lerp(max, min, i);
			this.highResCanvas.alpha = Mathf.Lerp(min, max, i);
			yield return 0;
		}
		this.highResCanvas.alpha = max;
		this.lowResCanvas.alpha = min;
		this.lowResCanvas.gameObject.SetActive(false);
		yield return 0;
		yield break;
	}

	private IEnumerator IconUpdate(PicItem item)
	{
		bool hasIcon = this.iconLine.texture != null;
		bool hasSave = this.iconSave.texture != null;
		while (!hasIcon || !hasSave)
		{
			if (!hasIcon)
			{
				Texture iconTex = item.GetIconTex();
				if (iconTex != null)
				{
					this.iconLine.texture = iconTex;
					hasIcon = true;
				}
			}
			if (!hasSave)
			{
				Texture saveTex = item.GetSaveTex();
				if (saveTex != null)
				{
					this.iconSave.texture = saveTex;
					hasSave = true;
				}
			}
			this.iconUpdate = null;
			yield return 0;
		}
		yield break;
	}

	public CanvasGroup highResCanvas;

	public CanvasGroup lowResCanvas;

	public RawImage iconLine;

	public RawImage iconSave;

	private Coroutine iconUpdate;

	private Coroutine animRepeatCoroutine;

	private Coroutine initWaitCoroutine;

	[SerializeField]
	private RawImage drawImg;

	[SerializeField]
	private RawImage lineImg;

	[SerializeField]
	private ColoringAnimation coloringAnimation;

	private Coroutine animInit;

	private PicItem picItem;

	private ChopFill chopFill;

	private Texture2D line;

	private PaletteData palleteData;

	private List<Color> colors;

	private Texture2D texForShare;
}

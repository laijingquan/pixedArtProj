// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Gameboard : MonoBehaviour
{
	 
	public event Action Solved;

	 
	public event Action Loaded;

	 
	public event Action<Gameboard.LoadError> Error;

	public static int LaunchCount { get; private set; }

	public ISolvePage SolvedPage { get; set; }

	public int HintsUsed
	{
		get
		{
			if (this.pictureSaveData != null)
			{
				return this.pictureSaveData.hintsUsed;
			}
			return 1;
		}
	}

	public void StartGame()
	{
		if (Gameboard.pictureData == null)
		{
			if (this.Error != null)
			{
				this.Error(Gameboard.LoadError.Unknown);
			}
			Gameboard.pictureData = null;
			return;
		}
		this.currentGUID = AnalyticsUtils.GenerateGUID();
		if (this.Create(Gameboard.pictureData))
		{
			Gameboard.LaunchCount++;
			if (this.Loaded != null)
			{
				this.Loaded();
			}
		}
		if (Gameboard.StartAnalyticEvent != null)
		{
			Gameboard.StartEventType type = Gameboard.StartAnalyticEvent.type;
			if (type != Gameboard.StartEventType.New)
			{
				if (type != Gameboard.StartEventType.Restart)
				{
					if (type == Gameboard.StartEventType.Continue)
					{
						AnalyticsManager.ContinuePic(Gameboard.StartAnalyticEvent.id, Gameboard.StartAnalyticEvent.progress, Gameboard.StartAnalyticEvent.screenFrom, this.paintFill.TexWidth, Gameboard.pictureData.FillType);
					}
				}
				else
				{
					AnalyticsManager.RestartPic(Gameboard.StartAnalyticEvent.id, Gameboard.StartAnalyticEvent.progress, Gameboard.StartAnalyticEvent.screenFrom, this.paintFill.TexWidth, Gameboard.pictureData.FillType);
				}
			}
			else
			{
				AnalyticsManager.StartNewColoring(Gameboard.StartAnalyticEvent.id, this.paintFill.TexWidth, Gameboard.pictureData.FillType);
				if (Gameboard.pictureData.PicClass == PicClass.FacebookGift)
				{
					AnalyticsManager.BonusPictureStartColoring(Gameboard.pictureData.Id);
				}
				else if (Gameboard.pictureData.PicClass == PicClass.Daily)
				{
					AnalyticsManager.DailyStartColoring(Gameboard.pictureData.Id);
				}
			}
			Gameboard.StartAnalyticEvent = null;
		}
		AnalyticsManager.LevelTryStarted(Gameboard.pictureData.Id, this.currentGUID);
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			if (this.pictureSaveData != null && this.pictureSaveData.progres < 100)
			{
				this.pictureSaveData.AddTimeSpent((float)(DateTime.Now - this.timeFromLastAction).TotalSeconds);
				this.timeFromLastAction = DateTime.Now;
			}
		}
		else
		{
			this.timeFromLastAction = DateTime.Now;
		}
	}

	public bool Create(PictureData picData)
	{
		this.timeFromLastAction = DateTime.Now;
		Texture2D texture2D = null;
		Texture2D c = null;
		PaletteData p = default(PaletteData);
		this.fillType = picData.FillType;
		if (picData.picType == PictureType.System)
		{
			if (this.Error != null)
			{
				this.Error(Gameboard.LoadError.Unknown);
			}
			return false;
		}
		if (picData.picType == PictureType.Local)
		{
			try
			{
				texture2D = FileHelper.LoadTextureFromFile(picData.LineArt);
				texture2D.wrapMode = TextureWrapMode.Clamp;
				if (this.fillType == FillAlgorithm.Flood)
				{
					c = FileHelper.LoadTextureFromFile(picData.ColorMap);
				}
				string json = FileHelper.LoadTextFromFile(picData.Palette);
				p = JsonUtility.FromJson<PaletteData>(json);
			}
			catch (Exception ex)
			{
				if (this.Error != null)
				{
					this.Error(Gameboard.LoadError.FileMissing);
                    UnityEngine.Debug.Log(ex.ToString());
                }
				return false;
			}
		}
		this.Create(texture2D, c, p);
		if (picData.HasSave)
		{
			PictureSaveData pictureSaveData = SharedData.Instance.GetSave(picData);
			if (pictureSaveData != null && pictureSaveData.steps != null)
			{
				this.LoadSave(pictureSaveData.steps);
				if (this.pictureSaveData != null)
				{
					this.pictureSaveData.AddTimeSpent(pictureSaveData.timeSpent);
					this.pictureSaveData.hintsUsed = pictureSaveData.hintsUsed;
				}
			}
		}
		return true;
	}

	public void PictureSolved()
	{
		Gameboard.pictureData.Solved = true;
		Gameboard.pictureData.SetCompleted();
		SharedData.Instance.UpdatePicuteData(Gameboard.pictureData);
		this.SolvedPage.SetData(this.save.Steps, this.paintFill, new Func<int, Color>(this.palette.IdToColor));
		AnalyticsManager.CompletePic(Gameboard.pictureData.Id, this.pictureSaveData.TimeSpentRoundFive(), this.paintFill.TexWidth, Gameboard.pictureData.FillType, this.pictureSaveData.hintsUsed);
		if (Gameboard.pictureData.PicClass == PicClass.Daily)
		{
			AnalyticsManager.CompleteDailyPic(Gameboard.pictureData.Id, this.pictureSaveData.TimeSpentRoundFive(), this.paintFill.TexWidth, Gameboard.pictureData.FillType, this.pictureSaveData.hintsUsed);
		}
		if (Gameboard.pictureData.PicClass == PicClass.Daily)
		{
			int num = SharedData.Instance.CalculateDailyCompletePercent();
			if (num != -1)
			{
				AnalyticsManager.UpdateUserDailyProgressProperty(num);
			}
		}
		else
		{
			int num2 = SharedData.Instance.CalculateLibCompletePercent();
			if (num2 != -1)
			{
				AnalyticsManager.UpdateUserLibProgressProperty(num2);
			}
		}
		PlayTimeEventTracker.PictureSolved();
		AnalyticsManager.LevelTrySucceed(Gameboard.pictureData.Id, this.currentGUID);
	}

	public void LevelExit()
	{
		if (Gameboard.pictureData != null)
		{
			AnalyticsManager.LevelTryExit(Gameboard.pictureData.Id, this.currentGUID);
		}
	}

	public void ResetPosition()
	{
		this.boardController.ResetZoom(0.85f);
	}

	private void Create(Texture2D l, Texture2D c, PaletteData p)
	{
		FillAlgorithm fillAlgorithm = this.fillType;
		if (fillAlgorithm != FillAlgorithm.Flood)
		{
			if (fillAlgorithm == FillAlgorithm.Chop)
			{
				this.chopFill = new ChopFill();
				this.chopFill.Create(l, p, true);
				this.sourceImg.texture = l;
				this.drawImg.texture = this.chopFill.DrawTex;
				this.imageRect = this.sourceImg.rectTransform.rect;
				this.paintFill = this.chopFill;
			}
		}
		else
		{
			this.floodFill = new FloodFill();
			this.floodFill.Create(l, c, p);
			this.sourceImg.texture = l;
			this.drawImg.texture = this.floodFill.DrawTex;
			this.imageRect = this.sourceImg.rectTransform.rect;
			this.paintFill = this.floodFill;
		}
		this.paletteData = p;
		PaletteData paletteData = p;
		List<Color> list = new List<Color>();
		for (int i = 0; i < paletteData.entities.Length; i++)
		{
			paletteData.entities[i].color.a = byte.MaxValue;
			PaletteEntity paletteEntity = paletteData.entities[i];
			list.Add(paletteEntity.color);
		}
		this.numController.Init(new Func<int, Vector2>(this.TextureIndexToCanvas), this.imageRect.width / (float)this.paintFill.TexWidth);
		this.numController.Create(paletteData);
		this.palette.Create(list);
		this.palette.NewColor += delegate(int cId)
		{
			this.highlighter.HighlightColor(cId);
		};
		FillAlgorithm fillAlgorithm2 = this.fillType;
		if (fillAlgorithm2 != FillAlgorithm.Flood)
		{
			if (fillAlgorithm2 == FillAlgorithm.Chop)
			{
				ChopHighlightController component = this.highlightGameObject.GetComponent<ChopHighlightController>();
				component.Init(this.chopFill, paletteData, this.numController);
				this.highlighter = component;
			}
		}
		else
		{
			FloodHighlightController component2 = this.highlightGameObject.GetComponent<FloodHighlightController>();
			component2.Init(this.floodFill.Paint, paletteData, this.paintFill.DrawTex);
			this.highlighter = component2;
		}
		this.validator = new LevelValidator();
		this.validator.Init(paletteData);
		this.save = new BoardSave();
		this.boardController.Init(this.numController);
		this.boardController.Click += this.OnClick;
	}

	private void LoadSave(List<SaveStep> steps)
	{
		for (int i = 0; i < steps.Count; i++)
		{
			this.FillPoint(steps[i].point, steps[i].colorId, false);
		}
		this.paintFill.UpdateDrawTex();
	}

	private Vector2 TextureIndexToCanvas(int index)
	{
		Point p = default(Point);
		p.i = Mathf.FloorToInt((float)index / (float)this.paintFill.TexWidth);
		p.j = index - p.i * this.paintFill.TexWidth;
		return this.TextureToCanvas(p);
	}

	private Vector2 TextureToCanvas(Point p)
	{
		int num = Mathf.FloorToInt((float)p.j / (float)this.paintFill.TexWidth * this.imageRect.width);
		int num2 = Mathf.FloorToInt((float)p.i / (float)this.paintFill.TexHeight * this.imageRect.height);
		Vector2 result = new Vector2((float)num, (float)num2);
		return result;
	}

	private Point CanvasToTextureCoord(Vector2 position, Texture2D texture)
	{
		Vector2 a = new Vector2(this.imageRect.width / 2f, this.imageRect.height / 2f);
		Vector2 vector = a + position;
		Point point = new Point((int)vector.y, (int)vector.x);
		int x = Mathf.FloorToInt((float)point.i / this.imageRect.width * (float)texture.width);
		int y = Mathf.FloorToInt((float)point.j / this.imageRect.height * (float)texture.height);
		Point result = new Point(x, y);
		return result;
	}

	public void OnClick(Vector2 position)
	{
		Point point = this.CanvasToTextureCoord(position, this.paintFill.DrawTex);
		TLogger.Instance.Log(point.ToString());
		//float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (point.j < 0 || point.j >= this.paintFill.TexWidth || point.i < 0 || point.i >= this.paintFill.TexHeight)
		{
			TLogger.Instance.Log("out of bounds");
			return;
		}
		if (!this.palette.HasColor)
		{
			this.toastManager.ShowHintPickColor();
			TLogger.Instance.Log("Pick a color to draw");
			return;
		}
		if (this.paintFill.IsLineInPixel(point))
		{
			TLogger.Instance.Log("Hit line");
			return;
		}
		Color color = this.palette.Current;
		Color color2 = this.paintFill.ExpectedColorInPixel(point);
		Color32 a = this.paintFill.CurrentColorInPixel(point);
		if (!ColorUtils.IsSameColors(color, color2))
		{
			if (a.a != 255)
			{
				this.toastManager.ShowWrongColor();
			}
			TLogger.Instance.Log(string.Concat(new object[]
			{
				"Wrong palette color.  brush",
				color,
				" expected: ",
				color2
			}));
			return;
		}
		if (ColorUtils.IsSameColors(a, color))
		{
			TLogger.Instance.Log("repaint " + color);
			return;
		}
		FMLogger.Log(string.Concat(new object[]
		{
			"color.  brush",
			color,
			" tex pixel: ",
			color2
		}));
		if (this.debugSolve)
		{
			this.DebugSolveColor(this.palette.CurrentId, false);
		}
		else
		{
			this.FillPoint(point, this.palette.CurrentId, true);
		}
	}

	private void FillPoint(Point pColor, int colorId, bool writeData = true)
	{
		int zoneId = -1;
		FillAlgorithm fillAlgorithm = this.fillType;
		if (fillAlgorithm != FillAlgorithm.Flood)
		{
			if (fillAlgorithm == FillAlgorithm.Chop)
			{
				zoneId = this.chopFill.FillPoint(pColor, this.palette.IdToColor(colorId), writeData);
				this.numController.FilledZone(zoneId);
			}
		}
		else
		{
			this.floodFill.FillPoint(pColor, this.palette.IdToColor(colorId), writeData);
		}
		this.save.AddStep(colorId, pColor);
		if (writeData)
		{
			AnalyticsManager.LevelProgress(Gameboard.pictureData.Id, this.currentGUID, zoneId, this.save.StepCount);
		}
		this.UpdateSave(writeData);
		bool flag = this.validator.Filled(colorId);
		if (flag)
		{
			this.palette.MarkCurrentUsed(colorId);
		}
		if (this.validator.Solved)
		{
			this.boardController.Click -= this.OnClick;
			if (this.Solved != null)
			{
				this.Solved();
			}
		}
	}

	private void DebugSolveColor(int colorId, bool checkPainted = false)
	{
		int[] indexes = this.paletteData.entities[colorId].indexes;
		for (int i = 0; i < indexes.Length; i++)
		{
			Point point = default(Point);
			point.i = Mathf.FloorToInt((float)indexes[i] / (float)this.paintFill.TexWidth);
			point.j = indexes[i] - point.i * this.paintFill.TexWidth;
			bool flag = true;
			if (checkPainted)
			{
				//int num = this.paintFill.TexWidth * point.i + point.j;
				if (this.paintFill.CurrentColorInPixel(point).a > 0)
				{
					flag = false;
					TLogger.Instance.Log("skip paint ColorId: " + colorId + ". Already painted");
				}
			}
			if (flag)
			{
				this.FillPoint(point, colorId, false);
			}
		}
		this.paintFill.UpdateDrawTex();
	}

	public void UpdateSave(bool forceSave = true)
	{
		if (this.save == null || this.save.StepCount < 1)
		{
			return;
		}
		if (this.save.StepCount == 1)
		{
			bool hasSave = Gameboard.pictureData.HasSave;
			this.pictureSaveData = new PictureSaveData();
			Gameboard.pictureData.SetSaveState(true);
			this.pictureSaveData.Id = Gameboard.pictureData.Id;
			this.pictureSaveData.PackId = Gameboard.pictureData.PackId;
			this.pictureSaveData.SetTotalStepsCount(this.validator.TotalPieces);
			if (!hasSave)
			{
				SharedData.Instance.AddSave(this.pictureSaveData);
			}
			else
			{
				SharedData.Instance.UpdateSave(this.pictureSaveData);
			}
		}
		this.pictureSaveData.SetSteps(this.save.Steps);
		this.pictureSaveData.AddTimeSpent((float)(DateTime.Now - this.timeFromLastAction).TotalSeconds);
		this.timeFromLastAction = DateTime.Now;
		if (forceSave)
		{
			SharedData.Instance.UpdateSave(this.pictureSaveData);
		}
	}

	public void Clean()
	{
		if (this.paintFill != null)
		{
			List<Texture2D> list = this.paintFill.Clean();
			for (int i = 0; i < list.Count; i++)
			{
				UnityEngine.Object.Destroy(list[i]);
			}
		}
		if (this.highlighter != null)
		{
			this.highlighter.Clean();
		}
	}

	public void OnResume()
	{
		if (this.highlighter != null)
		{
			this.highlighter.HighlightLast();
		}
	}

	public void UpdateSaveIcon()
	{
		if (this.save == null || this.save.StepCount < 1)
		{
			return;
		}
		this.highlighter.DehighlightForIcon();
		string text = Gameboard.pictureData.PackId.ToString();
		string text2 = Gameboard.pictureData.Id + "pr.png";
		ImageManager.Instance.SaveIcon(text, text2, this.paintFill.DrawTex);
		this.pictureSaveData.iconMaskPath = text + "/" + text2;
		SharedData.Instance.UpdateSave(this.pictureSaveData);
	}

	public bool FindEmptyZone()
	{
		if (this.palette.CurrentId == -1)
		{
			this.toastManager.ShowHintPickColor();
			return false;
		}
		bool flag = this.validator.IsFilled(this.palette.CurrentId);
		if (flag)
		{
			FMLogger.Log("used fully. " + this.palette.CurrentId);
			this.toastManager.ShowHintAlreadyPainted();
			return false;
		}
		int currentId = this.palette.CurrentId;
		int[] indexes = this.paletteData.entities[currentId].indexes;
		int num = -1;
		for (int i = 0; i < indexes.Length; i++)
		{
			Point p = default(Point);
			p.i = Mathf.FloorToInt((float)indexes[i] / (float)this.paintFill.TexWidth);
			p.j = indexes[i] - p.i * this.paintFill.TexWidth;
			Color32 a = this.paintFill.CurrentColorInPixel(p);
			Color32 c = this.paintFill.ExpectedColorInPixel(p);
			if (!ColorUtils.IsSameColors(a, c))
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			FMLogger.Log("element not found. never should happen");
			return false;
		}
		int num2 = this.paletteData.entities[currentId].indexes[num];
		Point p2 = default(Point);
		p2.i = Mathf.FloorToInt((float)num2 / (float)this.paintFill.TexWidth);
		p2.j = num2 - p2.i * this.paintFill.TexWidth;
		Vector2 b = this.TextureToCanvas(p2);
		this.boardController.ZoomToPosition(new Vector2(this.imageRect.width / 2f, this.imageRect.height / 2f) - b);
		if (this.hintRevealedIndexes.Contains(num2))
		{
			FMLogger.Log("already hint revealed");
			return false;
		}
		this.hintRevealedIndexes.Add(num2);
		if (this.pictureSaveData != null)
		{
			this.pictureSaveData.hintsUsed++;
			FMLogger.Log("hints used " + this.pictureSaveData.hintsUsed);
		}
		return true;
	}

	public void OnLineToggle(bool isOn)
	{
		this.sourceImg.gameObject.SetActive(isOn);
	}

	public void OnFastSolveClick(bool isOn)
	{
		this.debugSolve = isOn;
	}

	[SerializeField]
	private ToastManager toastManager;

	[SerializeField]
	private NumberController numController;

	[SerializeField]
	private BoardController boardController;

	[SerializeField]
	private GameObject highlightGameObject;

	private IHighlighter highlighter;

	[SerializeField]
	private PaletteController palette;

	[SerializeField]
	private RawImage sourceImg;

	[SerializeField]
	private RawImage drawImg;

	private Rect imageRect;

	private FillAlgorithm fillType;

	private IPaintFill paintFill;

	private FloodFill floodFill;

	private ChopFill chopFill;

	private BoardSave save;

	private LevelValidator validator;

	public static Gameboard.StartEvent StartAnalyticEvent;

	public static PictureData pictureData;

	private PictureSaveData pictureSaveData;

	private PaletteData paletteData;

	private DateTime timeFromLastAction;

	private bool debugSolve;

	private List<int> hintRevealedIndexes = new List<int>();

	public string currentGUID = string.Empty;

	public enum LoadError
	{
		FileMissing,
		Unknown
	}

	public class StartEvent
	{
		public Gameboard.StartEventType type;

		public int id;

		public int progress;

		public string screenFrom;
	}

	public enum StartEventType
	{
		New,
		Restart,
		Continue
	}
}

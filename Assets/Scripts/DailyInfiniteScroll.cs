// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DailyInfiniteScroll : MonoBehaviour, IDropHandler, IEventSystemHandler
{
	public DailyInfiniteScroll.Range DisplayRange
	{
		get
		{
			return new DailyInfiniteScroll.Range
			{
				From = this.position,
				To = this.position + this._views.Length - 1
			};
		}
	}

	public int Count
	{
		get
		{
			return this._count;
		}
	}

	public DailyRowItem[] Views
	{
		get
		{
			return this._views;
		}
	}

	 
	public event Action<int> RequestNewContent;

	 
	public event Action<int, DailyRowItem, bool> FillItem;




	private void Awake()
	{
		this._scroll = base.GetComponent<ScrollRect>();
		this._scroll.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScrollChange));
		this._content = this._scroll.viewport.transform.GetChild(0).GetComponent<RectTransform>();
		this.CreateViews();
		this.CreateLabels();
	}

	private void Update()
	{
		if (this._count == 0)
		{
			return;
		}
		float y = this._content.anchoredPosition.y;
		if (Mathf.Abs(y - this.contentLastUpdPos) < 10f)
		{
			return;
		}
		this.contentLastUpdPos = this._content.anchoredPosition.y;
		this.position = this.GetTopRowNumber();
		if (this._previousPosition == this.position)
		{
			return;
		}
		if (this.position > this._previousPosition)
		{
			if (this.position - this._previousPosition > 1)
			{
				this.position = this._previousPosition + 1;
			}
			int num = this.position % this._views.Length;
			num--;
			if (num < 0)
			{
				num = this._views.Length - 1;
			}
			int num2 = this.position + this._views.Length - 1;
			if (num2 < this._count)
			{
				this.FillItem(num2, this._views[num], false);
				Vector2 anchoredPosition = this._rects[num].anchoredPosition;
				anchoredPosition.y = this.viewPositions[num2].y;
				this._rects[num].anchoredPosition = anchoredPosition;
			}
			else
			{
				this.RequestNewContent(this._count);
			}
		}
		else
		{
			if (this._previousPosition - this.position > 1)
			{
				this.position = this._previousPosition - 1;
			}
			int num3 = this.position % this._views.Length;
			this.FillItem(this.position, this._views[num3], false);
			Vector2 anchoredPosition2 = this._rects[num3].anchoredPosition;
			anchoredPosition2.y = this.viewPositions[this.position].y;
			this._rects[num3].anchoredPosition = anchoredPosition2;
		}
		this._previousPosition = this.position;
	}

	private void OnScrollChange(Vector2 vector)
	{
		float num = (float)(this._count / this._views.Length);
		float num2 = 0f;
		this._isCanLoadUp = false;
		this._isCanLoadDown = false;
		if (vector.y > 1f)
		{
			num2 = (vector.y - 1f) * num;
		}
		else if (vector.y < 0f)
		{
			num2 = vector.y * num;
		}
		if (num2 > this.pullValue && this.isPullTop)
		{
			this.topLabel.gameObject.SetActive(true);
			this.topLabel.text = this.topPullLabel;
			if (num2 > this.pullValue * 2f)
			{
				this.topLabel.text = this.topReleaseLabel;
				this._isCanLoadUp = true;
			}
		}
		else
		{
			this.topLabel.gameObject.SetActive(false);
		}
		if (num2 < -this.pullValue && this.isPullBottom)
		{
			this.bottomLabel.gameObject.SetActive(true);
			this.bottomLabel.text = this.bottomPullLabel;
			if (num2 < -this.pullValue * 2f)
			{
				this.bottomLabel.text = this.bottomReleaseLabel;
				this._isCanLoadDown = true;
			}
		}
		else
		{
			this.bottomLabel.gameObject.SetActive(false);
		}
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (this._isCanLoadUp)
		{
			//this.PullLoad(DailyInfiniteScroll.Direction.Top);
		}
		else if (this._isCanLoadDown)
		{
			//this.PullLoad(DailyInfiniteScroll.Direction.Bottom);
		}
		this._isCanLoadUp = false;
		this._isCanLoadDown = false;
	}

	public void AddTopOffset(int offset)
	{
		this.top += offset;
	}

	public void SetScrollTitleOffset(int offset)
	{
		this.titleBarHeight = offset;
	}

	public void InitData(int count, int scrollTo, List<int> gapRows, bool lazyIconLoad)
	{
		float num = (float)this.top;
		for (int i = 0; i < count; i++)
		{
			this.viewPositions.Add(new Vector2(0f, -num));
			if (gapRows.Contains(i))
			{
				num += (float)this.headerHeight;
			}
			else
			{
				num += (float)this.cellHeight;
			}
			if (i < count - 1)
			{
				num += (float)this.spacing;
			}
		}
		this._previousPosition = 0;
		this._count = count;
		float y = num + (float)this.bottom;
		this._content.sizeDelta = new Vector2(this._content.sizeDelta.x, y);
		Vector2 anchoredPosition = this._content.anchoredPosition;
		anchoredPosition.y = 0f;
		this._content.anchoredPosition = anchoredPosition;
		int num2 = scrollTo;
		num2 -= this.topPreloadRows;
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (count > this._views.Length && scrollTo + this._views.Length > count - 1)
		{
			num2 = count - this._views.Length - 1;
		}
		this.position = num2;
		this._previousPosition = this.position;
		int num3 = this.position % this._views.Length;
		for (int j = num3; j < this._views.Length; j++)
		{
			bool active = j < count;
			this._views[j].gameObject.SetActive(active);
			if (num2 < this.viewPositions.Count)
			{
				this._rects[j].anchoredPosition = this.viewPositions[num2];
			}
			if (j + 1 <= this._count)
			{
				this.FillItem(num2, this._views[j], lazyIconLoad);
				num2++;
			}
		}
		for (int k = 0; k < num3; k++)
		{
			bool active = k < count;
			this._views[k].gameObject.SetActive(active);
			this._rects[k].anchoredPosition = this.viewPositions[num2];
			if (k + 1 <= this._count)
			{
				this.FillItem(num2, this._views[k], lazyIconLoad);
				num2++;
			}
		}
		if (scrollTo >= this.topPreloadRows)
		{
			if (scrollTo == count - 1 && count > this._views.Length)
			{
				scrollTo--;
			}
			num3 = scrollTo % this._views.Length;
			this._content.anchoredPosition = new Vector2(this._content.anchoredPosition.x, -this._rects[num3].anchoredPosition.y - (float)this.titleBarHeight);
		}
	}

	public void SetLoadingState(bool isLoading)
	{
		if (isLoading)
		{
			this.loadingIndicator.SetActive(true);
		}
		else
		{
			this.loadingIndicator.SetActive(false);
		}
	}

	public DailyRowItem GetRow(int row)
	{
		for (int i = 0; i < this._views.Length; i++)
		{
			if (this._views[i].Row == row)
			{
				return this._views[i];
			}
		}
		return null;
	}

	public DailyRowItem GetRowWithId(int picId)
	{
		for (int i = 0; i < this._views.Length; i++)
		{
			if (this._views[i].ContainsItem(picId))
			{
				return this._views[i];
			}
		}
		return null;
	}

	public IEnumerator UpdPosition(Vector2 pos, RectTransform rt)
	{
		yield return new WaitForEndOfFrame();
		rt.anchoredPosition = pos;
		yield break;
	}

	private float GetTopPreoloadHeight()
	{
		float num = 0f;
		UnityEngine.Debug.Log("top preload height " + num);
		return (float)(this.topPreloadRows * this.headerHeight);
	}

	private int GetTopRowNumber()
	{
		bool flag = false;
		int num = 0;
		for (int i = Mathf.Max(0, this._previousPosition - 5); i < this._count; i++)
		{
			if (this._content.anchoredPosition.y - (float)this.top < -this.viewPositions[i].y)
			{
				num = i - 1;
				num = Mathf.Max(0, num);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
		}
		return num;
	}

	private void CreateViews()
	{
		ObjectContainer objectContainer = (!SafeLayout.IsTablet) ? this.phonePool : this.tabletPool;
		int num = Mathf.RoundToInt(((RectTransform)base.transform).rect.height / (float)this.cellHeight) + this.topPreloadRows + this.bottomPreloadRows;
		this._views = new DailyRowItem[num];
		for (int i = 0; i < num; i++)
		{
			DailyRowItem entity = objectContainer.GetEntity<DailyRowItem>(this._content);
			entity.name = i + string.Empty;
			entity.Row = -1;
			entity.transform.localScale = Vector3.one;
			entity.transform.localPosition = Vector3.zero;
			RectTransform component = entity.GetComponent<RectTransform>();
			component.pivot = new Vector2(0.5f, 1f);
			component.anchorMin = new Vector2(0f, 1f);
			component.anchorMax = new Vector2(1f, 1f);
			component.offsetMax = new Vector2(0f, 0f);
			component.offsetMin = new Vector2(0f, component.offsetMin.y);
			this._views[i] = entity;
		}
		this._rects = new RectTransform[this._views.Length];
		for (int j = 0; j < this._views.Length; j++)
		{
			this._rects[j] = this._views[j].gameObject.GetComponent<RectTransform>();
		}
	}

	private void CreateLabels()
	{
		GameObject gameObject = new GameObject("TopLabel");
		gameObject.transform.SetParent(this._scroll.viewport.transform);
		this.topLabel = gameObject.AddComponent<Text>();
		this.topLabel.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		this.topLabel.fontSize = 24;
		this.topLabel.transform.localScale = Vector3.one;
		this.topLabel.alignment = TextAnchor.MiddleCenter;
		this.topLabel.text = this.topPullLabel;
		RectTransform component = this.topLabel.GetComponent<RectTransform>();
		component.pivot = new Vector2(0.5f, 1f);
		component.anchorMin = new Vector2(0f, 1f);
		component.anchorMax = new Vector2(1f, 1f);
		component.offsetMax = new Vector2(0f, 0f);
		component.offsetMin = new Vector2(0f, -55f);
		component.anchoredPosition3D = Vector3.zero;
		gameObject.SetActive(false);
		GameObject gameObject2 = new GameObject("BottomLabel");
		gameObject2.transform.SetParent(this._scroll.viewport.transform);
		this.bottomLabel = gameObject2.AddComponent<Text>();
		this.bottomLabel.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		this.bottomLabel.fontSize = 24;
		this.bottomLabel.transform.localScale = Vector3.one;
		this.bottomLabel.alignment = TextAnchor.MiddleCenter;
		this.bottomLabel.text = this.bottomPullLabel;
		this.bottomLabel.transform.position = Vector3.zero;
		component = this.bottomLabel.GetComponent<RectTransform>();
		component.pivot = new Vector2(0.5f, 0f);
		component.anchorMin = new Vector2(0f, 0f);
		component.anchorMax = new Vector2(1f, 0f);
		component.offsetMax = new Vector2(0f, 55f);
		component.offsetMin = new Vector2(0f, 0f);
		component.anchoredPosition3D = Vector3.zero;
		gameObject2.SetActive(false);
	}

	public GameObject loadingIndicator;

	[Header("Item settings")]
	public ObjectContainer phonePool;

	public ObjectContainer tabletPool;

	public int headerHeight = 130;

	public int cellHeight = 530;

	public int topPreloadRows = 3;

	public int bottomPreloadRows = 3;

	[Header("Padding")]
	public int top = 10;

	public int bottom = 10;

	public int spacing = 2;

	public int titleBarHeight;

	[Header("Labels")]
	public string topPullLabel = "Pull to refresh";

	public string topReleaseLabel = "Release to load";

	public string bottomPullLabel = "Pull to refresh";

	public string bottomReleaseLabel = "Release to load";

	[Header("Directions")]
	public bool isPullTop = true;

	public bool isPullBottom = true;

	[Header("Pull coefficient")]
	[Range(0.01f, 0.1f)]
	public float pullValue = 0.05f;

	[HideInInspector]
	public Text topLabel;

	[HideInInspector]
	public Text bottomLabel;

	private ScrollRect _scroll;

	private RectTransform _content;

	private RectTransform[] _rects;

	private DailyRowItem[] _views;

	private bool _isCanLoadUp;

	private bool _isCanLoadDown;

	private int _previousPosition;

	private int _count;

	public int position;

	private float topPreloadHeight;

	private List<Vector2> viewPositions = new List<Vector2>();

	private float contentLastUpdPos;

	public enum Direction
	{
		Top,
		Bottom
	}

	public struct Range
	{
		public bool Include(int val)
		{
			return val >= this.From && val <= this.To;
		}

		public override string ToString()
		{
			return string.Format("[{0},{1}]", this.From, this.To);
		}

		public int From;

		public int To;
	}
}

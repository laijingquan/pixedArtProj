// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour, IDropHandler, IEventSystemHandler
{
	public InfiniteScroll.Range DisplayRange
	{
		get
		{
			return new InfiniteScroll.Range
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

	public ScrollRowItem[] Views
	{
		get
		{
			return this._views;
		}
	}

	 
	public event Action<int> RequestNewContent;

	 
	public event Action<int, ScrollRowItem, bool> FillItem;




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
		float num = this._content.anchoredPosition.y - (float)(this.spacing * this.topPreloadRows) - (float)(this.height * this.topPreloadRows);
		if (num < 0f)
		{
			return;
		}
		this.position = Mathf.FloorToInt(num / (float)(this.height + this.spacing));
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
			int num2 = this.position % this._views.Length;
			num2--;
			if (num2 < 0)
			{
				num2 = this._views.Length - 1;
			}
			int num3 = this.position + this._views.Length - 1;
			if (num3 < this._count)
			{
				this.FillItem(num3, this._views[num2], false);
				Vector2 anchoredPosition = this._rects[num2].anchoredPosition;
				anchoredPosition.y = this.GetViewBottomPosition(num3 - 1);
				this._rects[num2].anchoredPosition = anchoredPosition;
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
			int num4 = this.position % this._views.Length;
			this.FillItem(this.position, this._views[num4], false);
			Vector2 anchoredPosition2 = this._rects[num4].anchoredPosition;
			anchoredPosition2.y = this.GetViewTopPosition(this.position + 1) + (float)this.height;
			this._rects[num4].anchoredPosition = anchoredPosition2;
		}
		this._previousPosition = this.position;
	}

	private float GetViewTopPosition(int row)
	{
		for (int i = 0; i < this._rects.Length; i++)
		{
			if (this._views[i].Row == row)
			{
				return this._rects[i].anchoredPosition.y;
			}
		}
		return (float)this.top;
	}

	private float GetViewBottomPosition(int row)
	{
		for (int i = 0; i < this._rects.Length; i++)
		{
			if (this._views[i].Row == row)
			{
				return this._rects[i].anchoredPosition.y - (float)this.height;
			}
		}
		return (float)this.top;
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
			//this.PullLoad(InfiniteScroll.Direction.Top);
		}
		else if (this._isCanLoadDown)
		{
			//this.PullLoad(InfiniteScroll.Direction.Bottom);
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

	public void InitData(int count, bool lazyIconLoad)
	{
		this._previousPosition = 0;
		this._count = count;
		float y = (float)(this.height * count) * 1f + (float)this.top + (float)this.bottom + (float)((count != 0) ? ((count - 1) * this.spacing) : 0);
		this._content.sizeDelta = new Vector2(this._content.sizeDelta.x, y);
		Vector2 anchoredPosition = this._content.anchoredPosition;
		anchoredPosition.y = 0f;
		this._content.anchoredPosition = anchoredPosition;
		int num = this.top;
		for (int i = 0; i < this._views.Length; i++)
		{
			bool active = i < count;
			this._views[i].gameObject.SetActive(active);
			anchoredPosition = this._rects[i].anchoredPosition;
			anchoredPosition.y = (float)(-(float)num);
			anchoredPosition.x = 0f;
			this._rects[i].anchoredPosition = anchoredPosition;
			num += this.spacing + this.height;
			if (i + 1 <= this._count)
			{
				this.FillItem(i, this._views[i], lazyIconLoad);
			}
		}
	}

	public void InitData(int count, int scrollTo, bool lazyIconLoad)
	{
		this._previousPosition = 0;
		this._count = count;
		float y = (float)(this.height * count) * 1f + (float)this.top + (float)this.bottom + (float)((count != 0) ? ((count - 1) * this.spacing) : 0);
		this._content.sizeDelta = new Vector2(this._content.sizeDelta.x, y);
		Vector2 anchoredPosition = this._content.anchoredPosition;
		if (anchoredPosition.y >= (float)(this.top - this.titleBarHeight))
		{
			anchoredPosition.y = (float)(this.top - this.titleBarHeight + 1);
		}
		this._content.anchoredPosition = anchoredPosition;
		int num = this.top;
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
		for (int i = 0; i < num2; i++)
		{
			num += this.spacing + this.height;
		}
		int num3 = this.position % this._views.Length;
		for (int j = num3; j < this._views.Length; j++)
		{
			bool active = j < count;
			this._views[j].gameObject.SetActive(active);
			anchoredPosition = this._rects[j].anchoredPosition;
			anchoredPosition.y = (float)(-(float)num);
			anchoredPosition.x = 0f;
			this._rects[j].anchoredPosition = anchoredPosition;
			num += this.spacing + this.height;
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
			anchoredPosition = this._rects[k].anchoredPosition;
			anchoredPosition.y = (float)(-(float)num);
			anchoredPosition.x = 0f;
			this._rects[k].anchoredPosition = anchoredPosition;
			num += this.spacing + this.height;
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

	public void ApplyDataTo(int count, int newCount, InfiniteScroll.Direction direction)
	{
		if (this._count == 0)
		{
			this.InitData(count, 0, false);
			return;
		}
		Vector2 anchoredPosition = this._content.anchoredPosition;
		FMLogger.Log(string.Concat(new object[]
		{
			"append new rows ",
			newCount,
			" total ",
			count
		}));
		this._count = count;
		float y = (float)(this.height * count) * 1f + (float)this.top + (float)this.bottom + (float)((count != 0) ? ((count - 1) * this.spacing) : 0);
		this._content.sizeDelta = new Vector2(this._content.sizeDelta.x, y);
		int num = int.MaxValue;
		int num2 = int.MaxValue;
		for (int i = 0; i < this._views.Length; i++)
		{
			if (this._views[i].Row < num2)
			{
				num2 = this._views[i].Row;
				num = i;
			}
		}
		int num3 = this.position - num2;
		if (num3 > 0)
		{
			int num4 = count - newCount;
			for (int j = 0; j < num3; j++)
			{
				int num5 = num;
				num++;
				if (num == this._views.Length)
				{
					num = 0;
				}
				Vector2 anchoredPosition2 = this._rects[num5].anchoredPosition;
				anchoredPosition2.y = (float)(-(float)(this.top + num4 * this.spacing + num4 * this.height));
				this._rects[num5].anchoredPosition = anchoredPosition2;
				this.FillItem(num4, this._views[num5], true);
				num4++;
			}
		}
		base.StartCoroutine(this.UpdPosition(anchoredPosition, this._content));
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

	public ScrollRowItem GetRow(int row)
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

	public ScrollRowItem GetRowWithId(int picId)
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

	private void CreateViews()
	{
		ObjectContainer objectContainer = (!SafeLayout.IsTablet) ? this.phonePool : this.tabletPool;
		int num = Mathf.RoundToInt(((RectTransform)base.transform).rect.height / (float)this.height) + this.topPreloadRows + this.bottomPreloadRows;
		this._views = new ScrollRowItem[num];
		for (int i = 0; i < num; i++)
		{
			ScrollRowItem entity = objectContainer.GetEntity<ScrollRowItem>(this._content);
			entity.name = i + string.Empty;
			entity.Row = -1;
			entity.transform.localScale = Vector3.one;
			entity.transform.localPosition = Vector3.zero;
			RectTransform component = entity.GetComponent<RectTransform>();
			component.pivot = new Vector2(0.5f, 1f);
			component.anchorMin = new Vector2(0f, 1f);
			component.anchorMax = new Vector2(1f, 1f);
			component.offsetMax = new Vector2(0f, 0f);
			component.offsetMin = new Vector2(0f, (float)(-(float)this.height));
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

	public int height = 110;

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

	private ScrollRowItem[] _views;

	private bool _isCanLoadUp;

	private bool _isCanLoadDown;

	private int _previousPosition;

	private int _count;

	public int position;

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

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(UI_InfiniteScroll))]
public class UI_InfiniteScrollSnap : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IEventSystemHandler
{
	public bool IsDragging { get; private set; }

	public int CurrentPageClamped
	{
		get
		{
			int num = this.currentPage;
			if (num < 0)
			{
				if (num % this.totalPages == 0)
				{
					num = 0;
				}
				else
				{
					num = num % this.totalPages + this.totalPages;
				}
			}
			else if (num >= this.totalPages)
			{
				num %= this.totalPages;
			}
			return num;
		}
	}

	private Vector2 GetPositionTargetPos(int page)
	{
		Vector2 a = (float)page * new Vector2(this.itemOffset, 0f);
		return a * -1f;
	}

	private void OnDisable()
	{
		if (this.moveCoroutine != null)
		{
			base.StopCoroutine(this.moveCoroutine);
			this.scroll.content.anchoredPosition = this.targetPos;
			this.isScrolling = false;
		}
	}

	private int CalcCurrentPage()
	{
		return -1 * Mathf.RoundToInt(this.scroll.content.anchoredPosition.x / this.itemOffset);
	}

	public void InitScroll()
	{
		this.scroll = base.GetComponent<ScrollRect>();
		this.totalPages = this.scroll.content.childCount;
		if (this.OnCompleteEvent != null)
		{
			this.OnCompleteEvent.Invoke(this.CurrentPageClamped);
		}
		UI_InfiniteScroll component = base.GetComponent<UI_InfiniteScroll>();
		component.Init();
		this.itemOffset = component.ItemSize;
	}

	public void MoveRight()
	{
		if (this.isScrolling)
		{
			return;
		}
		this.currentPage++;
		this.moveCoroutine = base.StartCoroutine(this.MoveScroll(true));
	}

	public void MoveLeft()
	{
		if (this.isScrolling)
		{
			return;
		}
		this.currentPage--;
		this.moveCoroutine = base.StartCoroutine(this.MoveScroll(true));
	}

	private IEnumerator MoveScroll(bool autoScroll = false)
	{
		this.isScrolling = true;
		this.targetPos = this.GetPositionTargetPos(this.currentPage);
		RectTransform rt = this.scroll.content;
		if (autoScroll)
		{
			yield return this.MoveCoroutine(rt, this.targetPos, this.autoScrollDuration, this.autoScrollCurve);
		}
		else
		{
			yield return this.MoveCoroutine(rt, this.targetPos, this.duration, this.curve);
		}
		rt.anchoredPosition = this.targetPos;
		this.isScrolling = false;
		this.moveCoroutine = null;
		if (this.OnCompleteEvent != null)
		{
			this.OnCompleteEvent.Invoke(this.CurrentPageClamped);
		}
		yield break;
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		if (this.isScrolling)
		{
			return;
		}
		this.accumulatedSwipe += eventData.delta.x;
		this.accumulatedSwipeTime += Time.deltaTime;
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		this.currentPage = this.CalcCurrentPage();
		if (this.isScrolling)
		{
			this.accumulatedSwipe = 0f;
			this.accumulatedSwipeTime = 0f;
			return;
		}
		this.IsDragging = false;
		if (eventData.pressPosition.x - eventData.position.x > 0f)
		{
			this.currentPage++;
		}
		else
		{
			this.currentPage--;
		}
		this.moveCoroutine = base.StartCoroutine(this.MoveScroll(false));
		this.accumulatedSwipe = 0f;
		this.accumulatedSwipeTime = 0f;
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		base.StopAllCoroutines();
		this.isScrolling = false;
		this.IsDragging = true;
	}

	private IEnumerator MoveCoroutine(RectTransform rt, Vector2 to, float animDuration, AnimationCurve animCurve)
	{
		Vector2 from = rt.anchoredPosition;
		float i = 0f;
		float currentTime = 0f;
		while (i <= 1f)
		{
			currentTime += Time.deltaTime;
			i = currentTime / animDuration;
			rt.anchoredPosition = Vector2.Lerp(from, to, animCurve.Evaluate(i));
			yield return 0;
		}
		rt.anchoredPosition = Vector2.Lerp(from, to, 1f);
		yield break;
	}

	public AnimationCurve curve;

	public float duration;

	public float autoScrollDuration;

	public AnimationCurve autoScrollCurve;

	private ScrollRect scroll;

	private int currentPage;

	private int LastPage;

	private int totalPages;

	private bool isScrolling;

	[HideInInspector]
	private float minQuickSwipeAmount = 0.05f;

	private float movementTreshold = 0.002f;

	private float accumulatedSwipe;

	private float accumulatedSwipeTime;

	public OnPageChangedEvent OnCompleteEvent;

	private Vector2 targetPos;

	private Vector2 firstItemPos = new Vector2(450f, 0f);

	private float itemOffset;

	private Coroutine moveCoroutine;
}

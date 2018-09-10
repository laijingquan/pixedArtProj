// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(UI_InfiniteScroll))]
public class UI_InfiniteScrollSnapOld : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IEventSystemHandler
{
	private void OnEnable()
	{
		this.isScrolling = false;
		this.InitScroll(0);
	}

	public void InitScroll(int page = 0)
	{
		this.scroll = base.GetComponent<ScrollRect>();
		this.totalPages = this.scroll.content.childCount;
		this.step = 1f / (float)(this.totalPages - 1);
		if (this.OnCompleteEvent != null)
		{
			this.OnCompleteEvent.Invoke(this.GetRelativePage(null));
		}
		if (page > 0)
		{
			this.currentPage = page;
			this.scroll.horizontalNormalizedPosition = this.step * (float)page;
		}
		UI_InfiniteScroll component = base.GetComponent<UI_InfiniteScroll>();
		component.Init();
	}

	public void MoveRight()
	{
		if (this.isScrolling)
		{
			return;
		}
		base.StartCoroutine(this.MoveScrollRight(false));
		this.currentPage++;
	}

	public void MoveLeft()
	{
		if (this.isScrolling)
		{
			return;
		}
		base.StartCoroutine(this.MoveScrollLeft(false));
		this.currentPage--;
	}

	private IEnumerator MoveScrollRight(bool isAlignToCurrentPage = false)
	{
		this.isScrolling = true;
		float target = (!isAlignToCurrentPage) ? ((float)(this.currentPage + 1) * this.step) : ((float)this.currentPage * this.step);
		while (this.scroll.horizontalNormalizedPosition < target - this.movementTreshold)
		{
			this.scroll.horizontalNormalizedPosition = Mathf.Lerp(this.scroll.horizontalNormalizedPosition, target, this.scrollSpeed);
			yield return null;
		}
		this.isScrolling = false;
		if (this.OnCompleteEvent != null)
		{
			this.OnCompleteEvent.Invoke(this.GetRelativePage(null));
		}
		yield break;
	}

	private IEnumerator MoveScrollLeft(bool isAlignToCurrentPage = false)
	{
		this.isScrolling = true;
		float target = (!isAlignToCurrentPage) ? ((float)(this.currentPage - 1) * this.step) : ((float)this.currentPage * this.step);
		while (this.scroll.horizontalNormalizedPosition > target + this.movementTreshold)
		{
			this.scroll.horizontalNormalizedPosition = Mathf.Lerp(this.scroll.horizontalNormalizedPosition, target, this.scrollSpeed);
			yield return null;
		}
		this.isScrolling = false;
		if (this.OnCompleteEvent != null)
		{
			this.OnCompleteEvent.Invoke(this.GetRelativePage(null));
		}
		yield break;
	}

	private int GetRelativePage(int? nextPage = null)
	{
		int num = (nextPage != null) ? nextPage.Value : this.currentPage;
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

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		if (this.isScrolling)
		{
			return;
		}
		this.accumulatedSwipe += eventData.delta.x;
		this.accumulatedSwipeTime += Time.deltaTime;
		int num = Mathf.RoundToInt(this.scroll.horizontalNormalizedPosition / this.step);
		if (num != this.LastPage)
		{
			this.currentPage = num;
		}
		this.LastPage = num;
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		if (this.isScrolling)
		{
			this.accumulatedSwipe = 0f;
			this.accumulatedSwipeTime = 0f;
			return;
		}
		if (Mathf.Abs(this.accumulatedSwipe) > this.minQuickSwipeAmount * (float)Screen.width && Mathf.Abs(this.accumulatedSwipe) < 0.4f * (float)Screen.width && this.accumulatedSwipeTime < 0.15f)
		{
			if (this.accumulatedSwipe > 0f)
			{
				this.MoveLeft();
			}
			else if (this.accumulatedSwipe < 0f)
			{
				this.MoveRight();
			}
		}
		else if (this.scroll.horizontalNormalizedPosition < (float)this.currentPage * this.step && !this.isScrolling)
		{
			base.StartCoroutine(this.MoveScrollRight(true));
		}
		else if (this.scroll.horizontalNormalizedPosition > (float)this.currentPage * this.step && !this.isScrolling)
		{
			base.StartCoroutine(this.MoveScrollLeft(true));
		}
		this.accumulatedSwipe = 0f;
		this.accumulatedSwipeTime = 0f;
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		base.StopAllCoroutines();
		this.isScrolling = false;
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(0f, 0f, 400f, 100f), string.Concat(new object[]
		{
			this.scroll.horizontalNormalizedPosition,
			" ",
			this.step,
			" ",
			this.currentPage
		}));
	}

	private ScrollRect scroll;

	private int currentPage;

	private int LastPage;

	private float step;

	private float scrollSpeed = 0.2f;

	private int totalPages;

	private bool isScrolling;

	[HideInInspector]
	private float minQuickSwipeAmount = 0.05f;

	private float movementTreshold = 0.002f;

	private float accumulatedSwipe;

	private float accumulatedSwipeTime;

	public OnPageChangedEvent OnCompleteEvent;
}

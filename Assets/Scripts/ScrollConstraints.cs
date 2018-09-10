// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ScrollConstraints : MonoBehaviour
{
	public void Init()
	{
		this.outsideParent = this.scrollContent.parent;
		this.initialSiblingIndexesAsc = new int[this.constraints.Length];
		this.initialPos = new Vector2[this.constraints.Length];
		for (int i = 0; i < this.constraints.Length; i++)
		{
			this.initialSiblingIndexesAsc[i] = this.constraints[i].GetSiblingIndex();
			this.initialPos[i] = this.constraints[i].anchoredPosition;
		}
		this.inited = true;
	}

	private void Update()
	{
		if (!this.inited)
		{
			return;
		}
		if (this.scrollContent.anchoredPosition.y < 0f)
		{
			if (!this.isRestricted)
			{
				this.isRestricted = true;
				for (int i = 0; i < this.constraints.Length; i++)
				{
					this.constraints[i].SetParent(this.outsideParent);
					this.constraints[i].anchoredPosition = this.initialPos[i];
					this.constraints[i].SetSiblingIndex(i + 1);
				}
			}
		}
		else if (this.isRestricted)
		{
			this.isRestricted = false;
			for (int j = 0; j < this.constraints.Length; j++)
			{
				this.constraints[j].SetParent(this.scrollContent);
				this.constraints[j].anchoredPosition = this.initialPos[j];
				this.constraints[j].SetSiblingIndex(this.initialSiblingIndexesAsc[j]);
			}
		}
	}

	[SerializeField]
	private RectTransform[] constraints;

	private int[] initialSiblingIndexesAsc;

	private Vector2[] initialPos;

	[SerializeField]
	private RectTransform scrollContent;

	private Transform outsideParent;

	private bool isRestricted;

	private bool inited;
}

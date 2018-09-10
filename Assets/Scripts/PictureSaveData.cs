// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PictureSaveData
{
	public void SetSteps(List<SaveStep> s)
	{
		this.steps = s;
		this.progres = (int)((float)this.steps.Count / (float)this.totalSteps * 100f);
	}

	public void SetTotalStepsCount(int c)
	{
		this.totalSteps = c;
		if (this.steps != null)
		{
			this.progres = (int)((float)this.steps.Count / (float)this.totalSteps);
		}
	}

	public int ProgressRoundFive()
	{
		return Mathf.RoundToInt((float)this.progres / 5f) * 5;
	}

	public int TimeSpentRoundFive()
	{
		return Mathf.RoundToInt(this.timeSpent / 5f) * 5;
	}

	public void AddTimeSpent(float timeFromLastAction)
	{
		this.timeSpent += timeFromLastAction;
	}

	public int PackId;

	public int Id;

	public string iconMaskPath;

	public float timeSpent;

	public int progres;

	public int hintsUsed;

	public List<SaveStep> steps;

	private int totalSteps;
}

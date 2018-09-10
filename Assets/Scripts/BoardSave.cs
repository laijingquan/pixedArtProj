// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class BoardSave
{
	public int StepCount
	{
		get
		{
			return this.steps.Count;
		}
	}

	public List<SaveStep> Steps
	{
		get
		{
			return this.steps;
		}
	}

	public void AddStep(int colorId, Point p)
	{
		this.steps.Add(new SaveStep(colorId, p));
	}

	public void LoadSteps(List<SaveStep> s)
	{
		this.steps = new List<SaveStep>(s);
	}

	private List<SaveStep> steps = new List<SaveStep>();
}

// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class LevelValidator
{
	public bool Solved
	{
		get
		{
			for (int i = 0; i < this.progress.Count; i++)
			{
				if (this.progress[i] > 0)
				{
					return false;
				}
			}
			return true;
		}
	}

	public int TotalPieces { get; private set; }

	public void Init(PaletteData pd)
	{
		for (int i = 0; i < pd.entities.Length; i++)
		{
			this.TotalPieces += pd.entities[i].indexes.Length;
			this.progress.Add(pd.entities[i].indexes.Length);
		}
	}

	public bool Filled(int cId)
	{
		List<int> list;
		list = this.progress; (list )[cId] = list[cId] - 1;
		return this.progress[cId] == 0;
	}

	public bool IsFilled(int cId)
	{
		return this.progress[cId] == 0;
	}

	private List<int> progress = new List<int>();
}

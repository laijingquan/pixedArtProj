// dnSpy decompiler from Assembly-CSharp.dll
using System;

[Serializable]
public class RewardConfig
{
	public bool IsValid()
	{
		return this.dailyBonus != 0 && this.timingBonus != 0 && this.timingDelay >= 0 && this.timingInterval >= 10;
	}

	public int dailyBonus;

	public int timingBonus;

	public int timingDelay;

	public int timingBonusShowTime;

	public int timingInterval;
}

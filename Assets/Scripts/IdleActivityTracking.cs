// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleActivityTracking : MonoBehaviour
{
	private void Awake()
	{
		this.painting = true;
		this.gameboard.Solved += delegate()
		{
			this.painting = false;
		};
	}

	private void Update()
	{
		if (!this.painting)
		{
			return;
		}
		this.idleTime += (double)Time.deltaTime;
		if (Input.touchCount > 0 || Input.anyKey)
		{
			this.idleTime = 0.0;
			this.eventStep = 0;
		}
		for (int i = this.eventStep; i < this.intervals.Count; i++)
		{
			if (this.idleTime > (double)this.intervals[i])
			{
				this.SendIdleEvent(this.intervals[i]);
				this.eventStep++;
			}
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			this.idleTime = 0.0;
			this.eventStep = 0;
		}
	}

	private void SendIdleEvent(int time)
	{
		UnityEngine.Debug.Log("idle " + time);
		AnalyticsManager.PotentialInterstitialShow(time);
	}

	[SerializeField]
	private Gameboard gameboard;

	private List<int> intervals = new List<int>
	{
		10,
		15,
		20,
		30
	};

	private int eventStep;

	private double idleTime;

	private bool painting;
}

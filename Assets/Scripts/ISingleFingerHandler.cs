// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public interface ISingleFingerHandler
{
	void OnSingleFingerDown(Vector2 position);

	void OnSingleFingerUp(Vector2 position, bool ignore);

	void OnSingleFingerDrag(Vector2 position);
}

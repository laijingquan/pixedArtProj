// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public interface IPinchExtHandler
{
	void OnPinchStarted(Vector2 posOne, Vector2 posTwo);

	void OnPinch(Vector2 posOne, Vector2 posTwo);

	void OnPinchEnd();
}

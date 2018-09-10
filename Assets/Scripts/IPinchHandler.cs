// dnSpy decompiler from Assembly-CSharp.dll
using System;

public interface IPinchHandler
{
	void OnPinchStart();

	void OnPinchEnd();

	void OnPinchZoom(float gapDelta);
}

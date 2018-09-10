// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISolvePage
{
	bool IsOpened { get; }

	bool IsAnimating { get; }

	void Clean();

	void ForceFinishAnimation();

	Vector2 GetBrnPosNormilized();

	void CutShareableTexture(Action<Texture2D> callback);

	void SetData(List<SaveStep> saveSteps, IPaintFill pFill, Func<int, Color> idToColor);
}

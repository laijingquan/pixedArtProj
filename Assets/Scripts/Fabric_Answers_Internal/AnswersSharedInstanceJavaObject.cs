// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

namespace Fabric.Answers.Internal
{
	internal class AnswersSharedInstanceJavaObject
	{
		public AnswersSharedInstanceJavaObject()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.crashlytics.android.answers.Answers");
			this.javaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
		}

		public void Log(string methodName, AnswersEventInstanceJavaObject eventInstance)
		{
			this.javaObject.Call(methodName, new object[]
			{
				eventInstance.javaObject
			});
		}

		private AndroidJavaObject javaObject;
	}
}

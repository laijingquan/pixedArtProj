// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Fabric.Crashlytics.Internal
{
	internal class AndroidImpl : Impl
	{
		private AndroidJavaObject Native
		{
			get
			{
				if (this.native == null)
				{
					this.native = new AndroidJavaObject("com.crashlytics.android.Crashlytics", new object[0]);
				}
				return this.native;
			}
		}

		private AndroidJavaClass CrashWrapper
		{
			get
			{
				if (this.crashWrapper == null)
				{
					this.crashWrapper = new AndroidJavaClass("io.fabric.unity.crashlytics.android.CrashlyticsAndroidWrapper");
				}
				return this.crashWrapper;
			}
		}

		private AndroidJavaObject Instance
		{
			get
			{
				if (this.instance == null)
				{
					this.instance = this.Native.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
				}
				if (this.instance == null)
				{
					throw new AndroidImpl.JavaInteropException("Couldn't get an instance of the Crashlytics class!");
				}
				return this.instance;
			}
		}

		public override void Crash()
		{
			this.CrashWrapper.CallStatic("crash", new object[0]);
		}

		public override void Log(string message)
		{
			this.Instance.CallStatic("log", new object[]
			{
				message
			});
		}

		public override void SetKeyValue(string key, string value)
		{
			this.Instance.CallStatic("setString", new object[]
			{
				key,
				value
			});
		}

		public override void SetUserIdentifier(string identifier)
		{
			this.Instance.CallStatic("setUserIdentifier", new object[]
			{
				identifier
			});
		}

		public override void SetUserEmail(string email)
		{
			this.Instance.CallStatic("setUserEmail", new object[]
			{
				email
			});
		}

		public override void SetUserName(string name)
		{
			this.Instance.CallStatic("setUserName", new object[]
			{
				name
			});
		}

		public override void RecordCustomException(string name, string reason, StackTrace stackTrace)
		{
			this.RecordCustomException(name, reason, stackTrace.ToString());
		}

		public override void RecordCustomException(string name, string reason, string stackTraceString)
		{
			this.references.Clear();
			IntPtr clazz = AndroidJNI.FindClass("java/lang/Exception");
			IntPtr methodID = AndroidJNI.GetMethodID(clazz, "<init>", "(Ljava/lang/String;)V");
			jvalue[] array = new jvalue[1];
			array[0].l = AndroidJNI.NewStringUTF(name + " : " + reason);
			IntPtr intPtr = AndroidJNI.NewObject(clazz, methodID, array);
			this.references.Add(array[0].l);
			this.references.Add(intPtr);
			IntPtr clazz2 = AndroidJNI.FindClass("java/lang/StackTraceElement");
			IntPtr methodID2 = AndroidJNI.GetMethodID(clazz2, "<init>", "(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;I)V");
			Dictionary<string, string>[] array2 = Impl.ParseStackTraceString(stackTraceString);
			IntPtr intPtr2 = AndroidJNI.NewObjectArray(array2.Length, clazz2, IntPtr.Zero);
			this.references.Add(intPtr2);
			for (int i = 0; i < array2.Length; i++)
			{
				Dictionary<string, string> dictionary = array2[i];
				jvalue[] array3 = new jvalue[4];
				array3[0].l = AndroidJNI.NewStringUTF(dictionary["class"]);
				array3[1].l = AndroidJNI.NewStringUTF(dictionary["method"]);
				array3[2].l = AndroidJNI.NewStringUTF(dictionary["file"]);
				this.references.Add(array3[0].l);
				this.references.Add(array3[1].l);
				this.references.Add(array3[2].l);
				array3[3].i = int.Parse(dictionary["line"]);
				IntPtr intPtr3 = AndroidJNI.NewObject(clazz2, methodID2, array3);
				this.references.Add(intPtr3);
				AndroidJNI.SetObjectArrayElement(intPtr2, i, intPtr3);
			}
			IntPtr methodID3 = AndroidJNI.GetMethodID(clazz, "setStackTrace", "([Ljava/lang/StackTraceElement;)V");
			jvalue[] array4 = new jvalue[1];
			array4[0].l = intPtr2;
			AndroidJNI.CallVoidMethod(intPtr, methodID3, array4);
			IntPtr clazz3 = AndroidJNI.FindClass("com/crashlytics/android/Crashlytics");
			IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(clazz3, "logException", "(Ljava/lang/Throwable;)V");
			jvalue[] array5 = new jvalue[1];
			array5[0].l = intPtr;
			AndroidJNI.CallStaticVoidMethod(clazz3, staticMethodID, array5);
			foreach (IntPtr obj in this.references)
			{
				AndroidJNI.DeleteLocalRef(obj);
			}
		}

		private readonly List<IntPtr> references = new List<IntPtr>();

		private AndroidJavaObject native;

		private AndroidJavaClass crashWrapper;

		private AndroidJavaObject instance;

		public class JavaInteropException : Exception
		{
			public JavaInteropException(string message) : base(message)
			{
			}
		}
	}
}

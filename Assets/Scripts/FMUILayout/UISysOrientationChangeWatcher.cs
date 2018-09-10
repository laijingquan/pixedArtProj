// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Diagnostics;
using UnityEngine;

namespace FMUILayout
{
	public class UISysOrientationChangeWatcher : MonoBehaviour
	{
		 
		public static event Action<DeviceOrientation> OrientationChanged;

		public static DeviceOrientation CurrentOrientation { get; private set; }

		private void Awake()
		{
			UISysOrientationChangeWatcher.CurrentOrientation = Input.deviceOrientation;
		}

		private void Update()
		{
			if (UISysOrientationChangeWatcher.CurrentOrientation != Input.deviceOrientation)
			{
				UISysOrientationChangeWatcher.CurrentOrientation = Input.deviceOrientation;
				if (UISysOrientationChangeWatcher.OrientationChanged != null)
				{
					UISysOrientationChangeWatcher.OrientationChanged(UISysOrientationChangeWatcher.CurrentOrientation);
				}
			}
		}
	}
}

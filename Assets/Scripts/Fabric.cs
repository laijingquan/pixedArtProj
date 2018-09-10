// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Reflection;
using Fabric.Runtime.Internal;
using UnityEngine;

namespace Fabric.Runtime
{
	public class Fabric
	{
		public static void Initialize()
		{
			string text = Fabric.impl.Initialize();
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			foreach (string kitMethod in array)
			{
				Fabric.Initialize(kitMethod);
			}
		}

		internal static void Initialize(string kitMethod)
		{
			int num = kitMethod.LastIndexOf('.');
			string typeName = kitMethod.Substring(0, num);
			string name = kitMethod.Substring(num + 1);
			Type type = Type.GetType(typeName);
			if (type == null)
			{
				return;
			}
			MethodInfo method = type.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				return;
			}
			object obj = (!typeof(ScriptableObject).IsAssignableFrom(type)) ? Activator.CreateInstance(type) : ScriptableObject.CreateInstance(type);
			if (obj == null)
			{
				return;
			}
			method.Invoke(obj, new object[0]);
		}

		private static readonly Impl impl = Impl.Make();
	}
}

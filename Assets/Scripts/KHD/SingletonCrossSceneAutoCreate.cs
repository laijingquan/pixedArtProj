// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

namespace KHD
{
	public class SingletonCrossSceneAutoCreate<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance
		{
			get
			{
				if (SingletonCrossSceneAutoCreate<T>.applicationIsQuitting)
				{
					UnityEngine.Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
					return (T)((object)null);
				}
				object @lock = SingletonCrossSceneAutoCreate<T>._lock;
				T instance;
				lock (@lock)
				{
					if (SingletonCrossSceneAutoCreate<T>._instance == null)
					{
						SingletonCrossSceneAutoCreate<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
						if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
						{
							UnityEngine.Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopening the scene might fix it.");
							return SingletonCrossSceneAutoCreate<T>._instance;
						}
						if (SingletonCrossSceneAutoCreate<T>._instance == null)
						{
							GameObject gameObject = new GameObject();
							SingletonCrossSceneAutoCreate<T>._instance = gameObject.AddComponent<T>();
							gameObject.name = "(singleton) " + typeof(T).ToString();
							UnityEngine.Object.DontDestroyOnLoad(gameObject);
							UnityEngine.Debug.Log(string.Concat(new object[]
							{
								"[Singleton] An instance of ",
								typeof(T),
								" is needed in the scene, so '",
								gameObject,
								"' was created."
							}));
						}
						else
						{
							UnityEngine.Debug.Log("[Singleton] Using instance already created: " + SingletonCrossSceneAutoCreate<T>._instance.gameObject.name);
							UnityEngine.Object.DontDestroyOnLoad(SingletonCrossSceneAutoCreate<T>._instance.gameObject);
						}
					}
					instance = SingletonCrossSceneAutoCreate<T>._instance;
				}
				return instance;
			}
		}

		public static bool IsExist()
		{
			return SingletonCrossSceneAutoCreate<T>._instance != null;
		}

		protected virtual void OnDestroy()
		{
			SingletonCrossSceneAutoCreate<T>._instance = (T)((object)null);
			SingletonCrossSceneAutoCreate<T>.applicationIsQuitting = true;
		}

		private static T _instance;

		private static bool applicationIsQuitting = false;

		private static object _lock = new object();
	}
}

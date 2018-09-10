// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
	public T GetEntity<T>(Transform parent)
	{
		GameObject gameObject;
		if (this.entityArray.Count > 0)
		{
			gameObject = this.entityArray[0];
			this.entityArray.RemoveAt(0);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.entity);
		}
		gameObject.transform.SetParent(parent);
		gameObject.transform.localScale = Vector3.one;
		return gameObject.GetComponent<T>();
	}

	public GameObject GetEntity(Transform parent)
	{
		GameObject gameObject;
		if (this.entityArray.Count > 0)
		{
			gameObject = this.entityArray[0];
			this.entityArray.RemoveAt(0);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.entity);
		}
		gameObject.transform.SetParent(parent);
		gameObject.transform.localScale = Vector3.one;
		return gameObject;
	}

	public void UnUseItem(GameObject item)
	{
		this.entityArray.Add(item);
		item.transform.SetParent(base.transform);
		item.transform.localPosition = Vector3.zero;
	}

	[SerializeField]
	private GameObject entity;

	[SerializeField]
	private List<GameObject> entityArray;
}

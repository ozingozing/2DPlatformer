using System.Collections;
using System.Collections.Generic;
using Ozing.Interfaces;
using UnityEngine;

namespace Ozing.ObjectPoolSystem
{
	public abstract class ObjectPool
	{
		public abstract void Release();
		public abstract void ReturnObject(Component component);
	}

	public class ObjectPool<T> : ObjectPool where T : Component
	{
		private readonly T prefab;

		private readonly Queue<T> pool = new Queue<T>();
		private readonly List<IObjectPoolItem> allItems = new List<IObjectPoolItem>(); 

		public ObjectPool(T prefab, int startCount = 1)
		{
			this.prefab = prefab;

			for(var i = 0; i < startCount; i++)
			{
				var obj = InstantiateNewObject();

				pool.Enqueue(obj);
			}
		}

		private T InstantiateNewObject()
		{
			var obj = Object.Instantiate(prefab);
			obj.name = prefab.name;

			if(!obj.TryGetComponent<IObjectPoolItem>(out var objectPoolItem))
			{
				Debug.Log($"{obj.name} doesn't have a component that implements IObjectPoolItme!!!");
				return obj;
			}

			objectPoolItem.SetObjectPool(this, obj);
			allItems.Add(objectPoolItem);
			return obj;
		}

		public T GetObject()
		{
			if (!pool.TryDequeue(out var obj))
			{
				obj = InstantiateNewObject();
				return obj;
			}

			obj.gameObject.SetActive(true);
			return obj;
		}

		public override void ReturnObject(Component component)
		{
			Debug.Log($"poolList Length : {pool.Count}");
			if (component is not T compObj) return;

			compObj.gameObject.SetActive(false);
			pool.Enqueue(compObj);
		}

		public override void Release()
		{
			foreach (var item in pool)
			{
				allItems.Remove(item as IObjectPoolItem);
				Object.Destroy(item.gameObject);
			}

			foreach (var item in allItems)
			{
				item.Release();
			}
		}

	}
}
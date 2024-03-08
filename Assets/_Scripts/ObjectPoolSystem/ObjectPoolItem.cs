using Ozing.Interfaces;
using System.Collections;
using UnityEngine;

namespace Ozing.ObjectPoolSystem
{
	public class ObjectPoolItem : MonoBehaviour, IObjectPoolItem
	{
		private ObjectPool objectPool;
		private Component component;

		public void ReturnItem(float delay = 0f)
		{
			if(delay > 0)
			{
				StartCoroutine(ReturnItemWithDelay(delay));
				return;
			}

			ReturnItemToPool();
		}

		private void ReturnItemToPool()
		{
			if (objectPool != null)
			{
				objectPool.ReturnObject(component);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private IEnumerator ReturnItemWithDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			
			ReturnItemToPool();
		}
		public void SetObjectPool<T>(ObjectPool pool, T comp) where T : Component
		{
			objectPool = pool;

			component = GetComponent(comp.GetType());
		}

		public void Release()
		{
			objectPool = null;
		}

		private void OnDisable()
		{
			StopAllCoroutines();
		}


	}
}
using Ozing.ObjectPoolSystem;
using System.Collections;
using UnityEngine;


namespace Ozing.Interfaces
{
	public interface IObjectPoolItem 
	{
		void SetObjectPool<T>(ObjectPool pool, T comp) where T : Component;
		void Release();
	}
}
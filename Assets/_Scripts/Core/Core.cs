using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Ozing.CoreSystem
{
	public class Core : MonoBehaviour
	{
		private readonly List<CoreComponent> CoreComponents = new List<CoreComponent>();

		[field : SerializeField] public GameObject Root { get; private set; }

		private void Awake()
		{
			Root = Root ? Root : transform.parent.gameObject;
		}

		private void Start()
		{
			Debug.Log(transform.parent.gameObject.name + " : " + CoreComponents.Count);
		}


		public void LogicUpdate()
		{
			foreach (CoreComponent compoenet in CoreComponents)
			{
				compoenet.LogicUpdate();
			}
		}

		public void AddComponent(CoreComponent component)
		{
			if (!CoreComponents.Contains(component))
			{
				CoreComponents.Add(component);
			}
		}

		public T GetCoreComponent<T>() where T : CoreComponent
		{
			var comp = CoreComponents.OfType<T>().FirstOrDefault();

			if (comp != null)
				return comp;

			//Awake의 내부 함수가 끝나지 않았는데 OnEnable진입시 문제가 될 수 있기에 한 번 더 체크
			comp = GetComponentInChildren<T>();

			if (comp != null)
				return comp;


			Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");

			return null;
		}

		public T GetCoreComponent<T>(ref T value) where T : CoreComponent
		{
			value = GetCoreComponent<T>();
			return value;
		}
	}
}
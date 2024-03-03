using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
    [Serializable]
    public abstract class ComponentData
    {
        [SerializeField, HideInInspector] private string name;

        public Type ComponentDependency { get; protected set; }

        public ComponentData()
        {
            SetComponentName();
            SetComponentDependency();
		}
        public void SetComponentName() => name = GetType().Name;
        protected abstract void SetComponentDependency();
        public virtual void SetAttackDataNames() { }

        public virtual void InitializeAttackData(int numberOfAttacks) { }
	}
    [Serializable]
    public abstract class ComponentData<T> : ComponentData where T : AttackData.AttackData
    {
        //get/private set으로 설정하면 함수에 레퍼런스형식 파라미터로 못 넘김
        //그래서 정 넘기고 싶으면 get/set 쪽에서 넘길 변수에 값을 넣어주고 넘기면 됨
        [SerializeField] private T[] attackData;
        public T[] AttackData { get => attackData; private set => attackData = value; }


		public override void SetAttackDataNames()
        {
            base.SetAttackDataNames();
            for(int i = 0; i < AttackData.Length; i++)
            {
                AttackData[i].SetAttackName(i + 1);
            }
        }

		public override void InitializeAttackData(int numberOfAttacks)
		{
			base.InitializeAttackData(numberOfAttacks);

            //numberOfAttacks가 oldLen 이하면 리턴 또는 attackData를 numberOfAttacks길이만큼 줄임
            var oldLen = attackData != null ? attackData.Length : 0;
            if(oldLen == numberOfAttacks) return;
            Array.Resize(ref attackData, numberOfAttacks);

            //oldLen 보다 초과면 ^여기서 늘린 배열에 초기화한 값을 넣어줌
            if(oldLen < numberOfAttacks)
            {
                for(var i = oldLen; i < attackData.Length; i++)
                {
                    var newObj = Activator.CreateInstance(typeof(T)) as T;
                    attackData[i] = newObj;
                }
            }

            SetAttackDataNames();
		}
	}
}

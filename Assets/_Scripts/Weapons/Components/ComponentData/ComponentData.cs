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
        //get/private set���� �����ϸ� �Լ��� ���۷������� �Ķ���ͷ� �� �ѱ�
        //�׷��� �� �ѱ�� ������ get/set �ʿ��� �ѱ� ������ ���� �־��ְ� �ѱ�� ��
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

            //numberOfAttacks�� oldLen ���ϸ� ���� �Ǵ� attackData�� numberOfAttacks���̸�ŭ ����
            var oldLen = attackData != null ? attackData.Length : 0;
            if(oldLen == numberOfAttacks) return;
            Array.Resize(ref attackData, numberOfAttacks);

            //oldLen ���� �ʰ��� ^���⼭ �ø� �迭�� �ʱ�ȭ�� ���� �־���
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

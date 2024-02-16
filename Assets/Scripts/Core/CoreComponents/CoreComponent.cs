using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core core;


	protected virtual void Awake()
	{
		core = transform.parent.GetComponent<Core>();
		Debug.Log(this.gameObject.transform.parent.name);
		if(core ==  null )
		{
			Debug.LogError("There is no Core on the parent");
		}
	}
}

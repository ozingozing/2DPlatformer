using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Weapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

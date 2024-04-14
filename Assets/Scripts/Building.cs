using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
/// <summary>
/// ½¨ºB
/// </summary>
public class Building : Unit
{

    private void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (isDead) 
        {
            return;
        }
        Attack();
    }

    private void Attack() 
    {

    }

    protected override void Dead(Unit attacker)
    {
        base.Dead(attacker);

    }

}

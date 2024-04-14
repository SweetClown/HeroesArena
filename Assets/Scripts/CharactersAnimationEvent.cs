using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色動畫事件
/// </summary>
public class CharactersAnimationEvent : MonoBehaviour
{
    private Unit unit;
    void Start()
    {
        unit = GetComponentInParent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 動畫事件
    /// </summary>
    private void AttackAnimationEvent() 
    {
       unit.AttackAnimationEvent();
    }

}

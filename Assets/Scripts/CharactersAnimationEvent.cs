using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ɫ�Ӯ��¼�
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
    /// �Ӯ��¼�
    /// </summary>
    private void AttackAnimationEvent() 
    {
       unit.AttackAnimationEvent();
    }

}

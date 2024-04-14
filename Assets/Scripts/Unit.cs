using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public UnitInfo unitInfo;
    public bool isOrange;
    public bool hasTarget;//�����Ŀ�˹���,��t�������Ƅ�
    public bool isDead;
    public int currentHp;

    //�M������
    public Animator animator;
    public NavMeshAgent meshAgent;

    //����������
    public Unit defaultTarget; //Ĭ�J����Ŀ��:����
    public Unit targetUnit;//Ŀ�˹����λ
    public List<Unit> targetsList = new List<Unit>();//��ǰ�����������x���˵��б�(���ӷ�)
    public List<Unit> attackerList = new List<Unit>(); //�����҂��Ĕ����б�(���ӷ�)


    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
        GetComponentInChildren<SphereCollider>().radius = unitInfo.attackArea * 2;
    }


    /// <summary>
    /// ��λ�Ƅ�
    /// </summary>
    protected virtual void UnitMove()
    {
        //��Ŀ��
        if (hasTarget)
        {
            //Ŀ�˛]���N��,�]������
            if (targetUnit != null && !targetUnit.isDead)
            {
                //��Ŀ���Ƅ�
                meshAgent.SetDestination(targetUnit.transform.position);
                JudegeIfReachTargetPos(transform.position, targetUnit.transform.position);
            }
            else
            {
                //����Ŀ��
                Resettarget();
            }
        }
        //�]��Ŀ��
        else
        {
            //�@ȡ��ǰĬ�JĿ��λ��
            GameController.Instance.UnitGetTargetPos(this, isOrange);
            //Ĭ�JĿ���Ƿ���
            if (defaultTarget != null)
            {

                //��Ĭ�JĿ���Ƅ�
                meshAgent.SetDestination(defaultTarget.transform.position);
                JudegeIfReachTargetPos(transform.position, defaultTarget.transform.position);
            }
        }
    }

    /// <summary>
    /// �Д��Ƿ��_�����������K�M���Ƅ�
    /// </summary>
    /// <param name="currentPos"></param>
    /// <param name="target"></param>
    public void JudegeIfReachTargetPos(Vector3 currentPos, Vector3 target)
    {
        //�]�е��_��������
        if (Vector3.Distance(currentPos, target) >= unitInfo.attackArea)
        {
            //�������P�О�
            UnitBehaviour();
        }
        //���_��������
        else
        {
            meshAgent.isStopped = true;
            //�Ӯ����������Ź���Ӯ�
            animator.SetBool("isAttacking", true);
            animator.SetBool("isMoving", false);
        }
    }

    private void Resettarget()
    {

    }

    /// <summary>
    /// ��λ�����О� 
    /// </summary>
    protected virtual void UnitBehaviour()
    {
        //�Ӯ������������ƄӄӮ�
        animator.SetBool("isAttacking", false);
        animator.SetBool("isMoving", true);

    }

    /// <summary>
    /// �ܵ�����
    /// </summary>
    /// <param name="damageValue">������ֵ</param>
    /// <param name="attacker">������(�����҂�����)</param>
    private void TakeDamage(int damageValue, Unit attacker)
    {

    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="attacker">������(�����҂�����)</param>
    protected virtual void Dead(Unit attacker)
    {

    }
    /// <summary>
    /// ����Ӯ��¼�
    /// </summary>
    public virtual void AttackAnimationEvent() 
    {
        if (hasTarget)
        {
            transform.LookAt(targetUnit.transform);
        }
        else 
        {
            transform.LookAt(defaultTarget.transform);
        }
        //���˿�Ѫ
        if (targetUnit)
        {
            targetUnit.TakeDamage(unitInfo.attackValue, this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tag")) 
        {
            Unit unit = other.GetComponentInParent<Unit>();
            if (isOrange != unit.isOrange) 
            {
                targetsList.Add(unit);
                unit.AddAttackerToList(this);
            }
        }
        
    }
    /// <summary>
    /// �҂���������Ҫ�����҂�����L�z�y, ������Ҫ�ѹ����҂��Č�����ӵ��������б��� (�����������ӷ�,  �҂��Ǳ��ӷ�)
    /// </summary>
    /// <param name="unit"></param>
    public void AddAttackerToList(Unit unit) 
    {
        attackerList.Add(unit);
    }
}
/// <summary>
/// Unit Information
/// </summary>
//[Serializable]
public struct UnitInfo 
{
    public int id;
    public string unitName;
    public int cost;
    public int hp;
    public float attackArea;
    public float speed;
    public int attackValue;
    public bool canCreateAnywhere;
}


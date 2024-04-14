using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public UnitInfo unitInfo;
    public bool isOrange;
    public bool hasTarget;//如果有目標攻擊,否則朝國王移動
    public bool isDead;
    public int currentHp;

    //組件引用
    public Animator animator;
    public NavMeshAgent meshAgent;

    //其他的引用
    public Unit defaultTarget; //默認攻擊目標:國王
    public Unit targetUnit;//目標攻擊單位
    public List<Unit> targetsList = new List<Unit>();//當前攻擊範圍可選敵人的列表(主動方)
    public List<Unit> attackerList = new List<Unit>(); //攻擊我們的敵人列表(被動方)


    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
        GetComponentInChildren<SphereCollider>().radius = unitInfo.attackArea * 2;
    }


    /// <summary>
    /// 單位移動
    /// </summary>
    protected virtual void UnitMove()
    {
        //有目標
        if (hasTarget)
        {
            //目標沒有銷毀,沒有死亡
            if (targetUnit != null && !targetUnit.isDead)
            {
                //朝目標移動
                meshAgent.SetDestination(targetUnit.transform.position);
                JudegeIfReachTargetPos(transform.position, targetUnit.transform.position);
            }
            else
            {
                //重置目標
                Resettarget();
            }
        }
        //沒有目標
        else
        {
            //獲取當前默認目標位置
            GameController.Instance.UnitGetTargetPos(this, isOrange);
            //默認目標是否為空
            if (defaultTarget != null)
            {

                //朝默認目標移動
                meshAgent.SetDestination(defaultTarget.transform.position);
                JudegeIfReachTargetPos(transform.position, defaultTarget.transform.position);
            }
        }
    }

    /// <summary>
    /// 判斷是否達到攻擊範圍並進行移動
    /// </summary>
    /// <param name="currentPos"></param>
    /// <param name="target"></param>
    public void JudegeIfReachTargetPos(Vector3 currentPos, Vector3 target)
    {
        //沒有到達攻擊範圍
        if (Vector3.Distance(currentPos, target) >= unitInfo.attackArea)
        {
            //執行相關行為
            UnitBehaviour();
        }
        //到達攻擊範圍
        else
        {
            meshAgent.isStopped = true;
            //動畫控制器播放攻擊動畫
            animator.SetBool("isAttacking", true);
            animator.SetBool("isMoving", false);
        }
    }

    private void Resettarget()
    {

    }

    /// <summary>
    /// 單位執行行為 
    /// </summary>
    protected virtual void UnitBehaviour()
    {
        //動畫控制器播放移動動畫
        animator.SetBool("isAttacking", false);
        animator.SetBool("isMoving", true);

    }

    /// <summary>
    /// 受到傷害
    /// </summary>
    /// <param name="damageValue">攻擊數值</param>
    /// <param name="attacker">攻擊者(攻擊我們的人)</param>
    private void TakeDamage(int damageValue, Unit attacker)
    {

    }

    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="attacker">攻擊者(攻擊我們的人)</param>
    protected virtual void Dead(Unit attacker)
    {

    }
    /// <summary>
    /// 攻擊動畫事件
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
        //敵人扣血
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
    /// 我們被其他想要攻擊我們的隊長檢測, 所以需要把攻擊我們的對象添加到攻擊者列表中 (攻擊者是主動方,  我們是被動方)
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


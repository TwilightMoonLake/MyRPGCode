using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Playercontral : MonoBehaviour
{

    private NavMeshAgent agent;
    public Animator anim;
   // public Texture2D point,attack,doorway,target,arrow;
    private GameObject attackTarget;
    private float lastattackTime;
    private CharacterStats characterStats;
    private bool isDead;
    private float stopDistance;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        stopDistance = agent.stoppingDistance;
      
    }
    void OnEnable()
    {
        Mousemove.Instance.OnMouseClicked += MoveToTarget;
        Mousemove.Instance.OnEnemyClicked += EventAttack;
        GameManager.Instance.RigisterPlayer(characterStats);
    }
    //此处的修改是为了解决转换场景中点击失效的问题
    //OnEnable的使用需要注意：需要让场景中的物体处于关闭状态
    void Start()
    {
        //Mousemove.Instance.OnMouseClicked += MoveToTarget;
        //Mousemove.Instance.OnEnemyClicked += EventAttack;

        
        GameManager.Instance.RigisterPlayer(characterStats);
        SaveManager.Instance.LoadPlayerData();
    }
    void OnDisable()
    {
        Mousemove.Instance.OnMouseClicked -= MoveToTarget;
        Mousemove.Instance.OnEnemyClicked -= EventAttack;

    }

    private void Update()
    {
    
        isDead = characterStats.currentHealth == 0;
        if (isDead)
        {
            GameManager.Instance.NotifyObservers();//Player死亡时进行广播
        }
        SwtichAnimation();
        lastattackTime -= Time.deltaTime;
    }
    public void SwtichAnimation()
    {
        anim.SetFloat("Speed",agent.velocity.sqrMagnitude);
        anim.SetBool("Death",isDead);
    }
    public void MoveToTarget(Vector3 target)
    {
        agent.stoppingDistance = stopDistance;
        if (isDead) return;
        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = target;
       
    }
    private void EventAttack(GameObject target)
    {
        if (isDead) return;
        if (target != null)
        {
            attackTarget = target;
            characterStats.iscritical = UnityEngine.Random.value < characterStats.attackData.critiaclChance;
            //一如既往的判断暴击率，是否产生暴击
            StartCoroutine(MoveToAttackTarget());
        }
        
    }
    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(attackTarget.transform.position,transform.position)>characterStats.attackData.attackRange)//当目标距离大于攻击范围时
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;
        if (lastattackTime < 0 )
        {
            anim.SetBool("Critical",characterStats.iscritical);
            anim.SetTrigger("Attack");
            lastattackTime = characterStats.attackData.coolDown;//重置CD时间
        }
    }
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Rock>()&& attackTarget.GetComponent<Rock>().rockStat == Rock.RockStat.HitNothing)//判断是不是石头
            {
                attackTarget.GetComponent<Rock>().rockStat = Rock.RockStat.HitEnemy;//转换为HitEnemy的状态
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20,ForceMode.Impulse);
            }
        }
        //攻击时获得目标的状态，如果鼠标点击史莱姆，那么attackTarget就等于史莱姆
        else
        { var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamge(characterStats, targetStats);
        }
    }


}

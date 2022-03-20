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
    //�˴����޸���Ϊ�˽��ת�������е��ʧЧ������
    //OnEnable��ʹ����Ҫע�⣺��Ҫ�ó����е����崦�ڹر�״̬
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
            GameManager.Instance.NotifyObservers();//Player����ʱ���й㲥
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
            //һ��������жϱ����ʣ��Ƿ��������
            StartCoroutine(MoveToAttackTarget());
        }
        
    }
    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(attackTarget.transform.position,transform.position)>characterStats.attackData.attackRange)//��Ŀ�������ڹ�����Χʱ
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;
        if (lastattackTime < 0 )
        {
            anim.SetBool("Critical",characterStats.iscritical);
            anim.SetTrigger("Attack");
            lastattackTime = characterStats.attackData.coolDown;//����CDʱ��
        }
    }
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Rock>()&& attackTarget.GetComponent<Rock>().rockStat == Rock.RockStat.HitNothing)//�ж��ǲ���ʯͷ
            {
                attackTarget.GetComponent<Rock>().rockStat = Rock.RockStat.HitEnemy;//ת��ΪHitEnemy��״̬
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20,ForceMode.Impulse);
            }
        }
        //����ʱ���Ŀ���״̬����������ʷ��ķ����ôattackTarget�͵���ʷ��ķ
        else
        { var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamge(characterStats, targetStats);
        }
    }


}

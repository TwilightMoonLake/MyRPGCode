using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyStates { GUARD,PATROL,CHASE,DEAD};//����ö��
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]//�������
public class enemycontral : MonoBehaviour,IEndGameObserver
{
    private EnemyStates enemystates;
    // Start is called before the first frame update
    private NavMeshAgent agent;
    private Animator anim;
    [Header("Basic settings")]
    public float sightRadius; //���ӷ�Χ
    public bool isGuard;
    private float speed;
    protected GameObject attackTarget;
    public float lookAtTime;  //����ͣ����ʱ��
    private float remainLookAtTime;
    [Header("Patrol State")]
    public float patrolRange;
    private float lastAttackTime;
    private Quaternion guardRotation;
    private Vector3 wayPoint; //·�ϵ�һ����
    private Vector3 guardPos;
    protected CharacterStats characterStats;
    private Collider coll;
    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool PlayerDead;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        speed = agent.speed;
        guardPos = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookAtTime; 
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();
    }
    void Start()
    {
        if (isGuard)
        {
            enemystates = EnemyStates.GUARD;
        }
        else
        {
            enemystates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        GameManager.Instance.AddOberser(this);
    }
    //private void OnEnable()
    //{
    //    GameManager.Instance.AddOberser(this);
    //    Debug.Log("�����ʵ��");

    //}
    private void OnDisable()
    {
        if (!GameManager.IsInitilized) return;
        GameManager.Instance.RemoveOberser(this);
    }
    //void OnEnable() //����ʱ
    //{
    //    GameManager.Instance.AddOberser(this);
    //}
    //void OnDisable() //����ʱ
    //{
    //    GameManager.Instance.RemoveOberser(this);
    //}
    void Update()
    {
        if (characterStats.currentHealth == 0)
        {
            isDead = true;
        }
        if (!PlayerDead)
        {
            SwtichStates();
            SwtichAnimation();
            lastAttackTime -= Time.deltaTime;
        }
        else
        {
            Debug.Log("�ر��˶���");
        }
    }
    void SwtichAnimation()
    {
        anim.SetBool("Walk",isWalk);
        anim.SetBool("Chase",isChase);
        anim.SetBool("Follow",isFollow);
        anim.SetBool("Critical", characterStats.iscritical);
        anim.SetBool("Death",isDead);

    }
    void SwtichStates()
    {
        if (isDead)
        {
            enemystates = EnemyStates.DEAD;//��������״̬
        }
        else if (FoundPlayer())
        {
            enemystates = EnemyStates.CHASE;
            Debug.Log("�ҵ���Player");
        }
        //����Player����Ҫ�л���׷��״̬
        switch (enemystates)
        {
            case EnemyStates.GUARD:  //����״̬
                isWalk = true;
                agent.isStopped = false;
                agent.destination = guardPos;//destinationĿ�ĵ�
                if (Vector3.SqrMagnitude(guardPos - transform.position)<=agent.stoppingDistance)//���������ռ����
                {
                    isWalk = false;
                    transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.01f);
                }
                break;
            case EnemyStates.PATROL:  //Ѳ��״̬
                isChase = false;
                agent.speed = speed * 0.5f;
                //�ж��Ƿ��ߵ���Ѳ�ߵ�
                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)    //�ж����¶�λѲ�ߵ��ʱ��
                    { remainLookAtTime -= Time.deltaTime; }
                    else
                    { GetNewWayPoint(); }//ִ��Ѳ��
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }                
                break;
            case EnemyStates.CHASE: //׷��״̬
                isWalk = false;
                isChase = true;
                speed = agent.speed;  //���ø�״̬�µ��ƶ��ٶ�
                if (!FoundPlayer())//���û�з������
                {
                    isFollow = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                    {
                        enemystates = EnemyStates.GUARD;
                    }
                    else
                    {
                        enemystates = EnemyStates.PATROL;
                    }
                }
                else//������Ŀ��
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                    //����׷��������׷��Ŀ��λ��
                }
                if (TargetInAttackRange()||TargetInSkillRange())//��������˹����ж�
                {
                    isFollow = false;//�ر�׷������
                    agent.isStopped = true;
                    if (lastAttackTime < 0 ) //���й�����ʱ��
                    {    
                        lastAttackTime = characterStats.attackData.coolDown;  //�˴���������CD
                        characterStats.iscritical = Random.value < characterStats.attackData.critiaclChance;
                        //�Ƿ��������
                        //���й���
                      Attack();
                    }
                }
                break;
            case EnemyStates.DEAD:
                coll.enabled = false;
                agent.radius = 0;
                //  agent.enabled = false;
              
                Destroy(gameObject,2f);
                break;
        }
    }
    void Attack()
    {
        transform.LookAt(attackTarget.transform);//������ת��Ŀ��
        if (TargetInAttackRange())
        {
            //��ս��������
            anim.SetTrigger("Attack");
        }
        if (TargetInSkillRange())
        {
            Debug.Log("test�������˱���");
            //���ܹ�������
            anim.SetTrigger("Skill");
        }
    }
    bool FoundPlayer()   //Ѱ�����
    {
        var colliders = Physics.OverlapSphere(transform.position,sightRadius);
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
    void GetNewWayPoint()//ʵ������ƶ�
    {
        remainLookAtTime = lookAtTime;  
        float randomX = Random.Range(-patrolRange,patrolRange);
        float randomZ = Random.Range(-patrolRange,patrolRange);
        //�������X��Z����ֵ
        Vector3 randomPoint = new Vector3(guardPos.x+randomX,transform.position.y,guardPos.z+randomZ);
        // wayPoint = randomPoint;//���»������
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }
    private void OnDrawGizmosSelected()  //������ʾ���ƶ���Χ
    {
        Gizmos.color = Color.blue; //ȷ����ʾ�ߵ���ɫ
        Gizmos.DrawWireSphere(transform.position,sightRadius); //ȷ�����η�Χ
    }
    bool TargetInAttackRange()  //��ս������Χ
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        }
        else
        {
            return false;          
        }
    }
    bool TargetInSkillRange()  //���ܴ�����Χ
    {
        if (attackTarget != null)
        {
            //�ж�������Ŀ��ľ����Ƿ�С�ڹ�������
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        }
        else
        {
            return false;
        }
    }
    void Hit()
    {
        if (attackTarget != null&&transform.IsFacingTarget(attackTarget.transform))
        {
 
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamge(characterStats,targetStats);
        }
    }

    public void EndNotify() //ʵ�ֽӿ�
    {
        //��ʤ
        //ֹͣ�ƶ�
        //ֹͣAgent
        //ֻ��Ҫ�����ܺͷ���д�����棬�Ϳ���ʵ�ֶ��ĺ͹㲥�Ĺ���
        Debug.Log("ʵ���˽ӿ�");
        anim.SetBool("Win",true);
        PlayerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
    /*
     * TODO��
     * �������ԭ��û��ִ�б���������ִ���ж�����
     * �޸�bug
     */
} 

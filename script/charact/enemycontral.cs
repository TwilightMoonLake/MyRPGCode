using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyStates { GUARD,PATROL,CHASE,DEAD};//设置枚举
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]//需求组件
public class enemycontral : MonoBehaviour,IEndGameObserver
{
    private EnemyStates enemystates;
    // Start is called before the first frame update
    private NavMeshAgent agent;
    private Animator anim;
    [Header("Basic settings")]
    public float sightRadius; //可视范围
    public bool isGuard;
    private float speed;
    protected GameObject attackTarget;
    public float lookAtTime;  //怪物停留的时间
    private float remainLookAtTime;
    [Header("Patrol State")]
    public float patrolRange;
    private float lastAttackTime;
    private Quaternion guardRotation;
    private Vector3 wayPoint; //路上的一个点
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
    //    Debug.Log("完成了实例");

    //}
    private void OnDisable()
    {
        if (!GameManager.IsInitilized) return;
        GameManager.Instance.RemoveOberser(this);
    }
    //void OnEnable() //启用时
    //{
    //    GameManager.Instance.AddOberser(this);
    //}
    //void OnDisable() //禁用时
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
            Debug.Log("关闭了动画");
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
            enemystates = EnemyStates.DEAD;//进入死亡状态
        }
        else if (FoundPlayer())
        {
            enemystates = EnemyStates.CHASE;
            Debug.Log("找到了Player");
        }
        //发现Player，需要切换到追击状态
        switch (enemystates)
        {
            case EnemyStates.GUARD:  //警卫状态
                isWalk = true;
                agent.isStopped = false;
                agent.destination = guardPos;//destination目的地
                if (Vector3.SqrMagnitude(guardPos - transform.position)<=agent.stoppingDistance)//计算两个空间矩阵
                {
                    isWalk = false;
                    transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.01f);
                }
                break;
            case EnemyStates.PATROL:  //巡逻状态
                isChase = false;
                agent.speed = speed * 0.5f;
                //判断是否走到了巡逻点
                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)    //判断重新定位巡逻点的时间
                    { remainLookAtTime -= Time.deltaTime; }
                    else
                    { GetNewWayPoint(); }//执行巡逻
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }                
                break;
            case EnemyStates.CHASE: //追击状态
                isWalk = false;
                isChase = true;
                speed = agent.speed;  //设置该状态下的移动速度
                if (!FoundPlayer())//如果没有发现玩家
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
                else//发现了目标
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                    //开启追击动画，追击目标位置
                }
                if (TargetInAttackRange()||TargetInSkillRange())//如果进入了攻击判定
                {
                    isFollow = false;//关闭追击动画
                    agent.isStopped = true;
                    if (lastAttackTime < 0 ) //进行攻击计时器
                    {    
                        lastAttackTime = characterStats.attackData.coolDown;  //此处用来重置CD
                        characterStats.iscritical = Random.value < characterStats.attackData.critiaclChance;
                        //是否产生暴击
                        //进行攻击
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
        transform.LookAt(attackTarget.transform);//将方向转向目标
        if (TargetInAttackRange())
        {
            //近战攻击动画
            anim.SetTrigger("Attack");
        }
        if (TargetInSkillRange())
        {
            Debug.Log("test：进行了暴击");
            //技能攻击动画
            anim.SetTrigger("Skill");
        }
    }
    bool FoundPlayer()   //寻找玩家
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
    void GetNewWayPoint()//实现随机移动
    {
        remainLookAtTime = lookAtTime;  
        float randomX = Random.Range(-patrolRange,patrolRange);
        float randomZ = Random.Range(-patrolRange,patrolRange);
        //随机生成X和Z的数值
        Vector3 randomPoint = new Vector3(guardPos.x+randomX,transform.position.y,guardPos.z+randomZ);
        // wayPoint = randomPoint;//重新获得坐标
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }
    private void OnDrawGizmosSelected()  //用来显示可移动范围
    {
        Gizmos.color = Color.blue; //确认显示线的颜色
        Gizmos.DrawWireSphere(transform.position,sightRadius); //确认球形范围
    }
    bool TargetInAttackRange()  //近战触发范围
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
    bool TargetInSkillRange()  //技能触发范围
    {
        if (attackTarget != null)
        {
            //判断自身与目标的距离是否小于攻击距离
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

    public void EndNotify() //实现接口
    {
        //获胜
        //停止移动
        //停止Agent
        //只需要将功能和方法写到里面，就可以实现订阅和广播的功能
        Debug.Log("实现了接口");
        anim.SetBool("Win",true);
        PlayerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
    /*
     * TODO：
     * 初步检查原因：没有执行暴击动作的执行判定代码
     * 修复bug
     */
} 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : enemycontral
{
    [Header("Skill")]
    public float kickForce = 35; //击飞力

    public GameObject rockPrefab;
    public Transform handPos;


    public void KickOff()
    {
        if (attackTarget!=null&&transform.IsFacingTarget(attackTarget.transform))//判定目标位置
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            Vector3 direction = (targetStats.transform.position - transform.position).normalized;//方法一
                                                                                                 //direction.Normalize();
                                                                                                 //计算目标的坐标，转换为空间矢量值
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;//给出一个反方向的速度
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            targetStats.TakeDamge(characterStats,targetStats);
        }
    }
    public void ThrowRock()
    {
        if (attackTarget!=null)
        {
            var rock = Instantiate(rockPrefab,handPos.position,Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }


}

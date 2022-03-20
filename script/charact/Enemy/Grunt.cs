using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Grunt :enemycontral
{
    [Header("Skill")]
    public float kickForce = 300; //击飞力

    public void KickOff()
    {
        if (attackTarget)
        {
            transform.LookAt(attackTarget.transform); //盯着目标

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;//给出一个反方向的速度
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
           
        }
    }

}

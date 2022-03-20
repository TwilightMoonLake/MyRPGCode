using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Grunt :enemycontral
{
    [Header("Skill")]
    public float kickForce = 300; //������

    public void KickOff()
    {
        if (attackTarget)
        {
            transform.LookAt(attackTarget.transform); //����Ŀ��

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;//����һ����������ٶ�
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
           
        }
    }

}

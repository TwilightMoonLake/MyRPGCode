using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : enemycontral
{
    [Header("Skill")]
    public float kickForce = 35; //������

    public GameObject rockPrefab;
    public Transform handPos;


    public void KickOff()
    {
        if (attackTarget!=null&&transform.IsFacingTarget(attackTarget.transform))//�ж�Ŀ��λ��
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            Vector3 direction = (targetStats.transform.position - transform.position).normalized;//����һ
                                                                                                 //direction.Normalize();
                                                                                                 //����Ŀ������꣬ת��Ϊ�ռ�ʸ��ֵ
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;//����һ����������ٶ�
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

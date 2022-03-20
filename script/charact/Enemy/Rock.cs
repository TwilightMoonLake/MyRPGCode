using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStat { HitPlayer,HitEnemy,HitNothing}
    //设置枚举：令Rock可以攻击player和enemy
    private Rigidbody rb;

    public RockStat rockStat;
    [Header("Basic Setting")]
    public float force;
    public GameObject target;
    private Vector3 direction;
    public int damge;
    public GameObject breakEffect;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        rockStat = RockStat.HitPlayer;
        FlyToTarget();
    }
    public void FlyToTarget()    
    {
        if (target == null)
        {
            target = FindObjectOfType<Playercontral>().gameObject;
        }
        direction = (target.transform.position - transform.position+Vector3.up).normalized;
        rb.AddForce(direction*force,ForceMode.Impulse);
    }
    private void Update()
    {
        //Destroy(gameObject, 3f);
    }
    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1f)
        {
            rockStat = RockStat.HitNothing;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        switch (rockStat)
        {
            case RockStat.HitPlayer:
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<NavMeshAgent>().isStopped = true;//传统停止攻击
                    collision.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;//经典击飞
                    collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy") ;//经典眩晕状态                                  
                    collision.gameObject.GetComponent<CharacterStats>().TakeDamge(damge,collision.gameObject.GetComponent<CharacterStats>());//通过重载，来进行引用计算
                    rockStat = RockStat.HitNothing;
                }
                break;
            case RockStat.HitEnemy:
                if (collision.gameObject.GetComponent<Golem>())
                {
                    var otherStat = collision.gameObject.GetComponent<CharacterStats>();
                    otherStat.TakeDamge(damge,otherStat);
                    Instantiate(breakEffect,transform.position,Quaternion.identity);
                    Destroy(gameObject);
                }

                break;

            case RockStat.HitNothing:

                break;
        }
    }
}

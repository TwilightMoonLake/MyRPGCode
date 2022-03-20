using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;//短程攻击

    public float skillRange;//远程攻击

    public float coolDown;//CD冷却时间

    public int minDamge;//最小伤害

    public int maxDamge;//最大伤害

    public float critiaclMultplier;//暴击加成倍率

    public float critiaclChance;//暴击率
}

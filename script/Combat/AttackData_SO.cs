using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;//�̳̹���

    public float skillRange;//Զ�̹���

    public float coolDown;//CD��ȴʱ��

    public int minDamge;//��С�˺�

    public int maxDamge;//����˺�

    public float critiaclMultplier;//�����ӳɱ���

    public float critiaclChance;//������
}

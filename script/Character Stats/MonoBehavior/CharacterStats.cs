using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int,int>updateHealthBarOnAttack;
    public CharacterDate_SO templateData;
    public CharacterDate_SO characterData;
    public AttackData_SO attackData;
    public bool iscritical;
    #region Read from Data_SO
    [HideInInspector]
    #region Read from Data_SO
    private void Awake()
    {
        if (templateData != null)tr
        {
            characterData = Instantiate(templateData);
        }
    }
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.maxHealth;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.maxHealth = value;
        }

    }
    public int currentHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentHealth;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.currentHealth = value;
        }

    }
    public int baseDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.baseDefence;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.baseDefence = value;
        }
    }
    public int currentDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentDefence;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.currentDefence = value;
        }
    }
    #endregion

    #region Character Combat
    //传入两个参数，攻击力和防御力
    public void TakeDamge(CharacterStats attacker,CharacterStats defener)//伤害判定
    {
        int damge = Mathf.Max(attacker.CurrentDmage() - defener.currentDefence,0);
        //伤害计算
        currentHealth = Mathf.Max(currentHealth-damge,0);//生命值计算
        if (attacker.iscritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        updateHealthBarOnAttack?.Invoke(currentHealth,MaxHealth);
        if (currentHealth <= 0)
        {
            attacker.characterData.UpdateExp(characterData.killPoint);//将自身point加给攻击者
        }

    }
    public void TakeDamge(int damge, CharacterStats defener) //重载
    {
        int currentDamge = Mathf.Max(damge - defener.currentDefence,0);
        currentHealth = Mathf.Max(currentHealth - damge,0);
        updateHealthBarOnAttack?.Invoke(currentHealth, MaxHealth);
    }
    private int CurrentDmage()
    {
        float coreDamge = UnityEngine.Random.Range(attackData.minDamge,attackData.maxDamge);
        //产生随机数
        if (iscritical)//如果暴击，进行暴击倍率计算
        {
            coreDamge *= attackData.critiaclMultplier;
            Debug.Log("暴击:"+coreDamge);

        }
        return (int)coreDamge;
    }
    #endregion
}
#endregion
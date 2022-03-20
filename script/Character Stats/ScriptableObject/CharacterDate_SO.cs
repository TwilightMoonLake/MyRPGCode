using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterDate_SO : ScriptableObject
{


    //bug:进入游戏后，maxHeal成为2
    [Header("Stats Info")]
    public int maxHealth;

    public int tureMaxHealth;

    public int currentHealth;

    public int baseDefence;

    public int currentDefence;

    [Header("Kill")]
    public int killPoint;

    [Header("Level")]
    public int currentLevel;

    public int maxLevel;

    public int baseExp;

    public int currentExp;

    public float levelBuff;
    
         

    public float LevelMultiplier
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }//幅度太大，课调整
    }

    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentExp = 0;
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        baseExp = (int)(baseExp * LevelMultiplier);
        maxHealth = (int)(maxHealth * (LevelMultiplier + 1));
        currentHealth = maxHealth;
        Debug.Log("LEVEL UP:" + currentLevel + "Max Health" + maxHealth);
    }
}

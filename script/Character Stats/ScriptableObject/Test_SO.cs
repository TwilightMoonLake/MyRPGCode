using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="test", menuName = "Character Stats/Test")]
public class Test_SO : ScriptableObject
{
    [Header("Test")]
    public int testOne;
    public int testTwo;
}
/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public Character_SO characterData;
    #region Read from Data_SO

    public int MaxHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.maxHealth;
            }
            elsem
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
}

 */
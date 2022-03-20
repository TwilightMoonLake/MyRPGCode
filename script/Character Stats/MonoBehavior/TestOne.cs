using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOne : MonoBehaviour
{
    public Test_SO testSO;
    public int one
    {
        get 
        {
            if (testSO != null)
            {
                return testSO.testOne;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            testSO.testOne = value;
        }
    }
    public int two
    {
        get
        {
            if (testSO!= null)
            {
                return testSO.testTwo;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            testSO.testTwo = value;
        }
    }
    /*
     * [CreateAssetMenu(fileName = "New Data",menuName = "Character Stats/Data")]

public class Character_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;
}
     */

}

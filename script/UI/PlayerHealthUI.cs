using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;

    Image healthSlider;

    Image expSlider;

    void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();    
    }
    void Update()
    {
        levelText.text = "Level" + GameManager.Instance.playerStat.characterData.currentLevel;
        UpdateHealth();
        UpdateaExp();
    }
    void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playerStat.currentHealth / GameManager.Instance.playerStat.MaxHealth;
        healthSlider.fillAmount = sliderPercent; 
    }
    void UpdateaExp()
    {
        float sliderPercent = (float)GameManager.Instance.playerStat.characterData.currentExp / GameManager.Instance.playerStat.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
}

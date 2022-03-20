using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;

    public Transform barPoint;

    public bool alwaysVisible;

    public float visibleTime; //可视化时间

    public float timeLeft;//血条的显示时间
    Image healthSlider;

    Transform UIbar;

    Transform cam;

    CharacterStats currentStats;

    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();

        currentStats.updateHealthBarOnAttack += UpdateHealthBar;
        
    }
    void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;//生成并拿到坐标
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();//获取子物体的图片
                UIbar.gameObject.SetActive(alwaysVisible);
            }
        }
    
    }
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(UIbar.gameObject);
        }
        UIbar.gameObject.SetActive(true);
        timeLeft = visibleTime;

        float sliderPercent =(float)currentHealth / maxHealth;//生命值百分比
        healthSlider.fillAmount = sliderPercent;
    }
    void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = barPoint.position; //更新血条位置
            UIbar.forward =  -cam.forward;
            if (timeLeft <= 0 && !alwaysVisible)
            {
                UIbar.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
                
            }
        }
    }
}

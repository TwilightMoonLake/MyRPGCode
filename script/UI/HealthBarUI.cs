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

    public float visibleTime; //���ӻ�ʱ��

    public float timeLeft;//Ѫ������ʾʱ��
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
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;//���ɲ��õ�����
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();//��ȡ�������ͼƬ
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

        float sliderPercent =(float)currentHealth / maxHealth;//����ֵ�ٷֱ�
        healthSlider.fillAmount = sliderPercent;
    }
    void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = barPoint.position; //����Ѫ��λ��
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(CharacterCombat))]
public class StatsUI : MonoBehaviour
{
    public GameObject uiHealthPrefab;

    float visibleTime = 2;
    public float currentHealthUI;

    float lastMadeVisibleTime;
    Transform healthBarUI;

    Image healthSlider;
    Image manaSlider;
    Text textStats;

    Image healthFollowSlider;
    Transform cam;

    PlayerController playerController;

    public float healingSpeed = 1f;
    public float damageSpeed = 10f;

    bool isIncrease = false;

    bool isIncreaseMaxHeal = false;

    TextMeshProUGUI levelText;

    

    private bool m_isEnable;
    public bool isEnable { get { return m_isEnable; } set { m_isEnable = value; if (healthBarUI != null) { LogUtils.Log("Hey Mother"); healthBarUI.gameObject.SetActive(value); }  } }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;

        currentHealthUI = GetComponent<CharacterCombat>().maxHealth;

        healthBarUI = transform.CompareTag("Player") ? GameManager.Instance.uIController.PlayerHealUI.transform : GameManager.Instance.uIController.BossHealUI.transform;

        levelText = healthBarUI.GetChild(healthBarUI.childCount - 1).GetComponent<TextMeshProUGUI>();

        healthSlider = healthBarUI.GetChild(2).GetComponent<Image>();
        healthFollowSlider = healthBarUI.GetChild(1).GetComponent<Image>();

        healthFollowSlider.fillAmount = 1f;
        healthSlider.fillAmount = 1f;

        if (healthSlider.GetComponent<HealthBar>() != null)
        {
            healthSlider.GetComponent<HealthBar>().MaxHealthPoints = GetComponent<CharacterCombat>().maxHealth;
        }

        //levelText.text = GetComponent<CharacterCombat>().currentLevel.GetValue().ToString();
        GetComponent<CharacterCombat>().OnHealthChanged += OnHealthChanged;
        //GetComponent<CharacterCombat>().OnManaChangedEvent += OnManaChanged;
        //GetComponent<CharacterCombat>().OnLevelUp += OnLevelUp;
    }

    void OnHealthChanged(int maxHealth, int currentHealth, int change)
    {
        if (healthBarUI != null)
        {
            lastMadeVisibleTime = Time.time;

            HealthBar hbScripts = healthSlider.GetComponent<HealthBar>();

            if (hbScripts != null)
            {
                hbScripts.MaxHealthPoints = maxHealth;
            }

            float healthPercent = (float)currentHealth / maxHealth;

            if (currentHealth == currentHealthUI)
            {
                healthSlider.fillAmount = healthPercent;
                isIncreaseMaxHeal = true;
                isIncrease = false;
            }
            else if (currentHealth > currentHealthUI)
            {
                isIncreaseMaxHeal = false;
                isIncrease = true;
                healthFollowSlider.fillAmount = healthPercent;
            }
            else
            {
                isIncreaseMaxHeal = false;
                isIncrease = false;
                healthSlider.fillAmount = healthPercent;
            }

            if(transform.tag == "Friendly")
            {
                if (isIncrease)
                {
                    healthFollowSlider.color = new Color32(247, 246, 129, 255);
                } else
                {
                    healthFollowSlider.color = new Color32(244, 8, 8, 255);
                }
            }

            currentHealthUI = currentHealth;

            if (currentHealth <= 0)
            {
                isEnable = false;
            }
        }
    }

    private void Update()
    {
        if(healthBarUI != null)
        {
            if (isIncreaseMaxHeal)
            {
                healthFollowSlider.fillAmount = healthSlider.fillAmount;
            }
            else
            {
                if (!isIncrease && healthFollowSlider.fillAmount > healthSlider.fillAmount)
                {
                    healthFollowSlider.fillAmount -= 0.25f * Time.deltaTime;
                }
                else if (isIncrease && healthFollowSlider.fillAmount > healthSlider.fillAmount)
                {
                    healthSlider.fillAmount += 0.25f * Time.deltaTime;
                }
            }

            if(currentHealthUI > 0 && m_isEnable)
            {
                healthBarUI.gameObject.SetActive(true);
            } else
            {
                healthBarUI.gameObject.SetActive(false);
            }

            //if(PlayerManager.instance.player != null)
            //{
            //    float distance = Vector3.Distance(playerController.transform.position, transform.position);
            //    if (distance < playerController.lookRadius)
            //    {
            //        healthBarUI.gameObject.SetActive(true);
            //    }
            //    else
            //    {
            //        healthBarUI.gameObject.SetActive(false);
            //    }
            //}
                
        }

    }

    internal void ResetFillCount()
    {
        if(healthBarUI != null)
        {
            healthSlider.fillAmount = 1;
            healthFollowSlider.fillAmount = 1;
        }
    }

    void LateUpdate()
    {
        //if (healthBarUI != null)
        //{
        //    healthBarUI.position = healthBarTarget.position;
        //    healthBarUI.forward = -cam.forward;
        //}
    }
}

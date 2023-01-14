using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class HealthUI : MonoBehaviour
{
    private List<GameObject> healthHeartsList = new List<GameObject>();
    [SerializeField] Transform hearthTransform;

    [SerializeField] GameObject sliderBarHealth;

    [SerializeField] Image healthBarImage;
    [SerializeField] TMP_Text healthAmountText;

    private void OnEnable()
    {
        GameManager.Instance.GetPlayer().healthEvent.OnHealthChanged += HealthEvent_OnHeealthChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.GetPlayer().healthEvent.OnHealthChanged -= HealthEvent_OnHeealthChanged;
    }

    private void Start()
    {
        RefreshHealthUI();
    }

    private void HealthEvent_OnHeealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        SetHealthBar(healthEventArgs);
        SetHealthBarImage(healthEventArgs);
    }

    private void CleraHealthBar()
    {
        foreach (GameObject heartIcon in healthHeartsList)
        {
            Destroy(heartIcon);
        }

        healthHeartsList.Clear();
    }

    private void SetHealthBar(HealthEventArgs healthEventArgs)
    {
        CleraHealthBar();

        //int healthHearts = Mathf.CeilToInt(healthEventArgs.healthPercent * 100f / 20f);
        int healthHearts = Mathf.CeilToInt(healthEventArgs.healthAmount / 20f);

        for (int i = 0; i < healthHearts; i++)
        {
            GameObject hearth = Instantiate(GameResources.Instance.hearthPrefab, hearthTransform);
            hearth.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.uiHearthSpacing * i, 0f);
            healthHeartsList.Add(hearth);
        }
    }

    private void SetHealthBarImage(HealthEventArgs healthEventArgs)
    {
        healthAmountText.text = $"{Mathf.Max(0,healthEventArgs.healthAmount)} / {GameManager.Instance.GetPlayer().GetPlayerMaxHealth()}";
        healthBarImage.rectTransform.localScale = new Vector3(Mathf.Max(0,healthEventArgs.healthPercent), 1f, 1f);
    }

    private void SetHealthDisplay(int myChoice)
    {
        if (myChoice == 0)
        {
            hearthTransform.gameObject.SetActive(true);
            sliderBarHealth.SetActive(false);
        }
        else if (myChoice == 1)
        {
            hearthTransform.gameObject.SetActive(false);
            sliderBarHealth.SetActive(true);
        }
    }

    public void RefreshHealthUI()
    {
        int healthOption = PlayerPrefs.GetInt("HealthDisplay", 0);
        SetHealthDisplay(healthOption);
    }
}

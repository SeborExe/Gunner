using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;

    public void EnableHealthBar()
    {
        gameObject.SetActive(true);
    }

    public void DisableHealthBar()
    {
        gameObject.SetActive(false);
    }

    public void SetHealthBarValue(float healthPercentage)
    {
        healthBar.transform.localScale = new Vector3(healthPercentage, 1f, 1f);
    }

    public void SetHealthBarValue(int healthAmount)
    {
        healthBar.transform.localScale = new Vector3(healthAmount, 1f, 1f);
    }
}

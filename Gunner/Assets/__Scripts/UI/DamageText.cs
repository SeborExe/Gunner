using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour, IText
{
    TMP_Text text;

    private void Awake()
    {   
        text = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.sortingLayerName = "Front";
    }

    public void SetUp(float damage, bool isPlayer = false)
    {
        text.text = damage.ToString("F1");
        if (isPlayer)
        {
            text.color = Color.blue;
        }
    }

    public void DestroyDamageText()
    {
        Destroy(gameObject);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}

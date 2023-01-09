using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldGuideRecord : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text description;

    public void SetUp(string itemID, string description, Sprite sprite)
    {
        spriteRenderer.sprite = sprite;

        if (PlayerPrefs.HasKey(itemID))
        {
            this.description.text = description;
        }
        else
        {
            this.description.text = "?";
            spriteRenderer.color = Color.black;
        }
    }
}

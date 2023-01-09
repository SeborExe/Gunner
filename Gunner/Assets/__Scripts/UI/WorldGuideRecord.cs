using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldGuideRecord : MonoBehaviour
{
    [SerializeField] TMP_Text name;
    [SerializeField] Image image;
    [SerializeField] TMP_Text description;

    public void SetUp(string itemID, string description, Sprite sprite, string name)
    {
        image.sprite = sprite;
        this.name.text = name;

        if (PlayerPrefs.HasKey(itemID))
        {
            this.description.text = description;
        }
        else
        {
            this.name.text = "?";
            this.description.text = "?";
            image.color = Color.black;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemText : MonoBehaviour
{
    [SerializeField] TMP_Text itemText;

    private void OnEnable()
    {
        Canvas canvas = this.GetComponent<Canvas>();
        canvas.sortingLayerName = "Instances";
    }

    public void DestroyText()
    {
        Destroy(gameObject);
    }

    public void SetText(string itemText)
    {
        this.itemText.text = itemText;
    }
}

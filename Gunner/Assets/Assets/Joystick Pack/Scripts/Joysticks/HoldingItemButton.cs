using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldingItemButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool holdingItemButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        holdingItemButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        holdingItemButtonPressed = false;
    }
}

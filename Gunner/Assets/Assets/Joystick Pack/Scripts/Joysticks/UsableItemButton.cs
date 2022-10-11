using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UsableItemButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool usableButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        usableButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        usableButtonPressed = false;
    }
}

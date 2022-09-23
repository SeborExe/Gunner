using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool actionButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        actionButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        actionButtonPressed = false;
    }
}

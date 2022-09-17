using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool rollButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        rollButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rollButtonPressed = false;
    }
}

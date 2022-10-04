using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReloadButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool reloadButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        reloadButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        reloadButtonPressed = false;
    }
}

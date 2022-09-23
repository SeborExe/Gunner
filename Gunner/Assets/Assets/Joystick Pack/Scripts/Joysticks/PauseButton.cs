using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool pauseButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        pauseButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pauseButtonPressed = false;
    }
}

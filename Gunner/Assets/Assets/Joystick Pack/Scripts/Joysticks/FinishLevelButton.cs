using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FinishLevelButton : MonoBehaviour, IPointerDownHandler
{
    public bool finishLevelButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        finishLevelButtonPressed = true;
    }

    private void OnDisable()
    {
        finishLevelButtonPressed = false;
    }
}

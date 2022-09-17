using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponChangeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool weaponChangeButtonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        weaponChangeButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        weaponChangeButtonPressed = false;
    }
}

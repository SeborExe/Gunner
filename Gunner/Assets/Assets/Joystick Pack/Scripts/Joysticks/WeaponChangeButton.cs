using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponChangeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool weaponChangeButtonPressed;

    private float timer;

    private void Update()
    {
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0) timer = 0;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (timer != 0) return;

        weaponChangeButtonPressed = true;
        timer = 0.1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        weaponChangeButtonPressed = false;
    }
}

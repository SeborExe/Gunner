using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool minimapButtonButtonPressed;
    [SerializeField] GameObject closeMinimapButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        if ((GameManager.Instance.gameState == GameState.engagingEnemies) || (GameManager.Instance.gameState == GameState.engagingBoss)) return;

        minimapButtonButtonPressed = true;
        closeMinimapButton.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        minimapButtonButtonPressed = false;
    }

    public void SetMapActiveFalse()
    {
        minimapButtonButtonPressed = false;
        closeMinimapButton.SetActive(false);
    }
}

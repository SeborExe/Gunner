using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlternativeControls", menuName = "Scriptable Objects/Items/Effects/Alternative Controls")]
public class EffectAlternativeControls : ItemEffect
{
    public override void ActiveEffect()
    {
        GameManager.Instance.joystick.GetComponent<RectTransform>().transform.localPosition = new Vector3(220f, -15f, 0f);
        GameManager.Instance.rotationJoystick.GetComponent<RectTransform>().transform.localPosition = new Vector3(-240f, -35f, 0f);
        GameManager.Instance.actionButton.GetComponent<RectTransform>().transform.localPosition = new Vector3(-260f, -20f, 0f);
        GameManager.Instance.rollButton.GetComponent<RectTransform>().transform.localPosition = new Vector3(-210f, -20f, 0f);
        GameManager.Instance.mapExitButton.GetComponent<RectTransform>().transform.localPosition = new Vector3(30f, 25f, 0f);
    }
}

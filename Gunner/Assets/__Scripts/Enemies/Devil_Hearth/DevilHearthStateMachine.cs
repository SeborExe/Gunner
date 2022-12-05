using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilHearthStateMachine : MonoBehaviour
{
    DevilHearthStats devilHearth;

    private void Awake()
    {
        devilHearth = GetComponent<DevilHearthStats>();
    }
}

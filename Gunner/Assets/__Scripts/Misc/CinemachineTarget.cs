using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;

    [SerializeField] private Transform cursorTarget;

    private void Awake()
    {
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Start()
    {
        SetCinemachineTargetGroup();
    }

    private void Update()
    {
        cursorTarget.position = HelperUtilities.GetMouseWorldPosition();
    }

    private void SetCinemachineTargetGroup()
    {
        CinemachineTargetGroup.Target cinemachineTargetGroup_player = new CinemachineTargetGroup.Target
        {
            weight = 1f,
            radius = 2.5f,
            target =
            GameManager.Instance.GetPlayer().transform
        };

        /*
        CinemachineTargetGroup.Target cinemachineTargetGroup_cursor = new CinemachineTargetGroup.Target 
        {
            weight = 1f, 
            radius = 1f, 
            target = cursorTarget 
        };
        */

        CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[] 
        { 
            cinemachineTargetGroup_player,
            //cinemachineTargetGroup_cursor
        };

        cinemachineTargetGroup.m_Targets = cinemachineTargetArray;
    }
}

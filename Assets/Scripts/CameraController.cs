using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;
    private CinemachineFramingTransposer composer;
    private Transform hero;

    private void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        composer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Start()
    {
        hero = vCam.Follow;
    }

    private void Update()
    {
        if (hero.position.x < -9f)
        {
            composer.m_DeadZoneWidth = 2f;
        }
        else
        {
            composer.m_DeadZoneWidth = 0f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCameraManager : MonoBehaviour
{
    private CinemachineFreeLook mainCameraHelper;
    private float X;
    private float Y;

    void Awake()
    {
        mainCameraHelper = GetComponent<CinemachineFreeLook>();
        X = mainCameraHelper.m_XAxis.m_MaxSpeed;
        Y = mainCameraHelper.m_YAxis.m_MaxSpeed;
    }

    public void blockMovement()
    {
        mainCameraHelper.m_XAxis.m_MaxSpeed = .1f;
        mainCameraHelper.m_YAxis.m_MaxSpeed = .1f;
    }

    public void unblockMovement()
    {
        mainCameraHelper.m_XAxis.m_MaxSpeed = X;
        mainCameraHelper.m_YAxis.m_MaxSpeed = Y;
    }
}

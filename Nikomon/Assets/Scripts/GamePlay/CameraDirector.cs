using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    public CinemachineTargetGroup TargetGroup;
    public CinemachineVirtualCamera TargetCinemachine;

    public void SetTargetCinemachine(GameObject obj1, GameObject obj2)
    {
        CinemachineTargetGroup.Target t1 = new CinemachineTargetGroup.Target()
        {
            target = obj1.transform,
            weight = 1,
            radius = 1
        };

        CinemachineTargetGroup.Target t2 = new CinemachineTargetGroup.Target()
        {
            target = obj2.transform,
            weight = 1,
            radius = 1
        };
        TargetGroup.m_Targets = new[] {t1, t2};
        TargetCinemachine.Priority = 50;
    }
    
    public void ResetTargetCinemachine()
    {
        foreach (var target in TargetGroup.m_Targets)
        {
            TargetGroup.RemoveMember(target.target);
        }
        TargetCinemachine.Priority = 5;
    }
}
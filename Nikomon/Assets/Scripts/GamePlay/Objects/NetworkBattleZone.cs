using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class NetworkBattleZone : MonoBehaviour
{
    public CinemachineVirtualCamera camera;
    private void OnTriggerEnter(Collider other)
    {
        camera.Priority = 13;
        UnityEngine.Debug.Log("Start network battle!");
        GlobalManager.Instance.StartNetworkBattle();
    }

    private void OnTriggerExit(Collider other)
    {
        camera.Priority = 8;
        UnityEngine.Debug.Log("Stop network battle!");
        GlobalManager.Instance.StopPairNetworkBattle();
    }
}

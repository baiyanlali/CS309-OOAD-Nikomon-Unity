using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = PokemonCore.Debug;

public class NetworkBattleZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Start network battle!");
        GlobalManager.Instance.StartNetworkBattle();
    }
}

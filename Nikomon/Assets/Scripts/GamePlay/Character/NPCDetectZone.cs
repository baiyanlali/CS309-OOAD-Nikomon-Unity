using System;
using UnityEngine;

namespace GamePlay.Character
{
    public class NPCDetectZone: MonoBehaviour
    {
        public Transform AimTarget;
        private void OnTriggerStay(Collider other)
        {
            if (AimTarget == null) return;
            if (other.GetComponent<PlayerController>())
            {
                AimTarget.position = other.transform.position.ChangeY(AimTarget.position.y);
            }
        }
    }
}
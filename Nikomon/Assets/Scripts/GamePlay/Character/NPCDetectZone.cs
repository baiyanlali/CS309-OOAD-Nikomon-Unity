using System;
using GamePlay.UI;
using GamePlay.UI.UIFramework;
using UnityEngine;

namespace GamePlay.Character
{
    public class NPCDetectZone: MonoBehaviour
    {
        public Transform AimTarget;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>() == null) return;

            void ONInteractive()
            {
                GetComponentInParent<NPC>().OnInteractive(other.gameObject);
            }

            UIManager.Instance.Show<InteractPanel>("Talk",gameObject.transform, (Action) ONInteractive);
        }

        private void OnTriggerStay(Collider other)
        {
            if (AimTarget == null) return;
            if (other.GetComponent<PlayerController>())
            {
                AimTarget.position = other.transform.position.ChangeY(AimTarget.position.y);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerController>() == null) return;
            UIManager.Instance.Hide<InteractPanel>();
        }
    }
}
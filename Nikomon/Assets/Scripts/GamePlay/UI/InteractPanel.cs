using System;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace GamePlay.UI
{
    public class InteractPanel : BaseUI
    {
    
        public override UILayer Layer  => UILayer.MainUI;

        public Text InfoText;
        public Transform FollowTarget;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0 for the text information, 1 for the position to hint(vector 3), 2 for the recall action when press current key</param>
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            InfoText.text = (string)args[0];
            if (args.Length >= 2)
            {
                
                FollowTarget = (Transform)args[1];
                // var position = Camera.main.WorldToScreenPoint((Vector3)args[1]);
                // transform.position = position;
                if (args.Length >= 3)
                {
                    OnTrigger = (Action)args[2];
                }
            }
        }

        private Action OnTrigger;

        public void Update()
        {
            Debug.Assert(Camera.main != null, "Camera.main != null");
            var position = Camera.main.WorldToScreenPoint(FollowTarget.position);
            transform.position = position;
            if (NicomonInputSystem.Instance.accept==true && OnTrigger != null)
            {
                OnTrigger();
                OnTrigger = null;
            }
        }

        public override void OnExit()
        {
            OnTrigger = null;
            base.OnExit();
        }
    }
}

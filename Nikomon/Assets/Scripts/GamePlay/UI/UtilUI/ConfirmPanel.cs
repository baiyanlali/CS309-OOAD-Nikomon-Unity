using System;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlay.UI.UtilUI
{
    public class ConfirmPanel:BaseUI,IUIAnimator
    {
        public Text Text;
        public Button Submit;
        public Button Cancel;
        
        public override UILayer Layer => UILayer.NormalUI;
        public override bool IsOnly => false;
        // public override bool IsBlockPlayerControl => true;
        public float TweenTime=0.2f;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0 for ConfirmInformation string, 1 for confirm or cancel action
        /// if more, 2 for yes string, 3 for no string
        /// </param>
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            string confirmInformation = args[0] as string;
            Action<bool> confirmAction=null;
            if(args.Length>=2)
                confirmAction = args[1] as Action<bool>;

            string yes = "ConfirmPanel.yes";
            string no = "ConfirmPanel.no";
            if (args.Length >= 4)
            {
                yes = args[2] as string;
                no = args[3] as string;
            }

            Text.text = confirmInformation;
            
            Submit.gameObject.GetComponentInChildren<Text>().text = Messages.Messages.Get(yes);
            Cancel.gameObject.GetComponentInChildren<Text>().text = Messages.Messages.Get(no);

            Submit.onClick.RemoveAllListeners();
            Submit.onClick.AddListener(() =>
            {
                UIManager.Instance.Hide(this);
                confirmAction?.Invoke(true);
            });
            Cancel.onClick.RemoveAllListeners();
            Cancel.onClick.AddListener(() =>
            {
                UIManager.Instance.Hide(this);
                confirmAction?.Invoke(false);
            });

            
        }

        public void OnEnterAnimator()
        {
            RectTransform rect=transform as RectTransform;
            if (rect == null) return;
            rect.localScale=Vector3.zero;
            LeanTween.scale(rect, Vector3.one, TweenTime);
        }

        public void OnExitAnimator()
        {
            RectTransform rect=transform as RectTransform;
            LeanTween.scale(rect, Vector3.zero, TweenTime);
        }
    }
}
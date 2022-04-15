using System;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.UtilUI
{
    public class InformPanel : BaseUI, IUIAnimator
    {
        public Button OKBtn;
        public Text InformText;
        public float TweenTime=0.2f;

        /// <summary>
        /// 0 for information, 1 for callback, 2 for ok string
        /// </summary>
        /// <param name="args"></param>
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            string confirmInformation = args[0] as string;

            string ok = "OK";
            Action OK = (Action) args[1];
            if (args.Length >= 3)
            {
                ok = args[2] as string;
            }

            InformText.text = confirmInformation;

            OKBtn.gameObject.GetComponentInChildren<Text>().text = Messages.Messages.Get(ok);

            OKBtn.onClick.RemoveAllListeners();
            OKBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.Hide(this);
                OK?.Invoke();
            });
        }

        public void OnEnterAnimator()
        {
            RectTransform rect = transform as RectTransform;
            if (rect == null) return;
            rect.localScale = Vector3.zero;
            LeanTween.scale(rect, Vector3.one, TweenTime).setEase(LeanTweenType.easeInSine);
        }

        public void OnExitAnimator()
        {
            RectTransform rect = transform as RectTransform;
            LeanTween.scale(rect, Vector3.zero, TweenTime).setEase(LeanTweenType.easeInSine);
        }
    }
}
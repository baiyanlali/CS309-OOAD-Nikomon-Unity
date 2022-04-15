using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace GamePlay.UI.UtilUI
{
    public class DialogPanel : BaseUI
    {
        public enum FadeType
        {
            Button,
            Automatic,
            Dialogue
        }

        public Text NameText;

        public Text DialogText;

        // private Queue<LocalizedLine> reports;
        private LocalizedLine currentReport;
        public GameObject ShowNext;
        public RectTransform DialogueAttachment;
        public float TextSpeed = 0.01f;
        public float PauseTime = 1f;

        public override UILayer Layer => UILayer.NormalUI;

        public FadeType fadeType = FadeType.Button;

        Action OnContinue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0 for the report string, 1 for fadetype, 2 for on continue action</param>
        public override void Init(params object[] args)
        {
            // print("On Init");
            base.Init(args);
            if (args != null && args.Length >= 1)
            {
                currentReport = (LocalizedLine) args[0];
                if (args.Length >= 2)
                    fadeType = (FadeType) args[1];
                else
                {
                    fadeType = FadeType.Button;
                }

                if (args.Length >= 3)
                    OnContinue = args[2] as Action;
                else
                {
                    OnContinue = null;
                }
            }

            // reports ??= new Queue<LocalizedLine>();
            if (DialogText == null)
            {
                DialogText = gameObject.GetComponentInChildren<Text>();
            }

            if (ShowNext == null)
            {
                ShowNext = transform.Find("ShowNext").gameObject;
            }

            // if (currentReport == null || string.IsNullOrEmpty(currentReport))
            //     currentReport = "";
        }

        /// <summary>
        /// 0 for message
        /// </summary>
        /// <param name="args"></param>
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);

            if (args == null)
            {
                return;
            }
            

            // DialogText.text = currentReport;
            if (args.Length >= 1)
            {
                // print("To report");
                OnReport(currentReport);
                ExitBtn.onClick.RemoveAllListeners();

                ExitBtn.onClick.AddListener(() =>
                {
                    StartDialogImmediate(currentReport);
                });
            }
#if UNITY_ANDROID || UNITY_IPHONE
            UIManager.Instance.Hide<VirtualControllerPanel>();
            
            
#endif
        }

        public override void OnRefresh(params object[] args)
        {
            // print("On Refresh");
            base.OnRefresh(args);
            gameObject.SetActive(true);
        }

        private void OnFinishDrawing()
        {
            if (fadeType == FadeType.Automatic)
                StartCoroutine(PauseAndHide());
            else
            {
                ExitBtn.onClick.RemoveAllListeners();

                ExitBtn.onClick.AddListener(() =>
                {
                    // if(fadeType!=FadeType.Dialogue)
                    UIManager.Instance.Hide(this);
                    OnContinue?.Invoke();
                });
            }

            // gameObject.SetActive(false);
        }

        IEnumerator PauseAndHide()
        {
            yield return new WaitForSeconds(PauseTime);
            UIManager.Instance.Hide(this);
        }

        private void OnReport(LocalizedLine report)
        {
            StartCoroutine(DrawDialog(report));
        }

        private void StartDialogImmediate(LocalizedLine line)
        {
            StopAllCoroutines();
            DialogText.text = line.TextWithoutCharacterName.Text;

            OnFinishDrawing();
        }

        IEnumerator DrawDialog(LocalizedLine line)
        {
            if (string.IsNullOrEmpty(line.CharacterName))
            {
                NameText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                NameText.transform.parent.gameObject.SetActive(true);
                NameText.text = line.CharacterName;
            }

            string str = line.TextWithoutCharacterName.Text;

            // print($"Start Coroutine {str}");
            StringBuilder sb = new StringBuilder();
            int index = 0;
            while (index < str.Length)
            {
                sb.Append(str[index]);
                DialogText.text = sb.ToString();
                // print(sb.ToString());
                yield return new WaitForSeconds(TextSpeed);
                index++;
            }

            // print("Finish drawDialog");
            OnFinishDrawing();
            yield return null;
        }
    }
}
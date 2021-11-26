using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.UtilUI
{
    public class DialogPanel:BaseUI
    {
        public enum FadeType
        {
            Button,
            Automatic
        }
        public Text DialogText;
        private Queue<string> reports;
        private string currentReport;
        public GameObject ShowNext;
        private bool isDrawing = false;
        public Action<string> OnDialogFinished;
        public float TextSpeed=0.01f;
        public float PauseTime = 1f;

        public override UILayer Layer  => UILayer.Top;

        public FadeType fadeType=FadeType.Button;

        public override void Init(params object[] args)
        {
            // print("On Init");
            base.Init(args);
            if(args!=null&&args.Length>=1)
            {
                
                currentReport = (string) args[0];
                if (args.Length >= 2)
                    fadeType = (FadeType) args[1];
            }
            
            reports ??= new Queue<string>();
            if (DialogText == null)
            {
                DialogText = gameObject.GetComponentInChildren<Text>();
            }

            if (ShowNext == null)
            {
                ShowNext = transform.Find("ShowNext").gameObject;
            }

            if (currentReport == null || string.IsNullOrEmpty(currentReport))
                currentReport = "";
            isDrawing = false;
        }

        /// <summary>
        /// 0 for message
        /// </summary>
        /// <param name="args"></param>
        public override void OnEnter(params object[] args)
        {
            print("On Enter");
            base.OnEnter(args);
            // print(currentReport);

            // if (fadeType == FadeType.Button)
            // {
            //     
            // }

            // DialogText.text = currentReport;
            if(args!=null&&args.Length>=1)
            {
                print("To report");
                OnReport(currentReport);
            }
        }

        public override void OnRefresh(params object[] args)
        {
            print("On Refresh");
            base.OnRefresh(args);
            gameObject.SetActive(true);
        }

        private void OnFinishDrawing()
        {
            if (reports.Count == 0)
            {
                OnDialogFinished?.Invoke("");
                if(fadeType==FadeType.Automatic)
                    StartCoroutine(PauseAndHide());
                else
                {
                    ExitBtn.onClick.RemoveAllListeners();
                    ExitBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.Hide(this);
                    });
                    
                }
                // gameObject.SetActive(false);
                return;
            }

            if (fadeType == FadeType.Automatic)
            {
                currentReport = reports.Dequeue();
                StartCoroutine(DrawDialog(currentReport));
            }
            else
            {
                ExitBtn.onClick.RemoveAllListeners();
                ExitBtn.onClick.AddListener(() =>
                {
                    currentReport = reports.Dequeue();
                    StartCoroutine(DrawDialog(currentReport));
                });
            }
            
        }

        IEnumerator PauseAndHide()
        {
            yield return new WaitForSeconds(PauseTime);
            UIManager.Instance.Hide(this);
        }
        
        private void OnReport(string report)
        {
            reports.Enqueue(report);

            if (reports.Count == 1)
            {
                StartCoroutine(DrawDialog(reports.Dequeue()));
            }
        }

        private void StartDialogImmediate(string str)
        {
            StopAllCoroutines();
            DialogText.text = str;
            isDrawing = false;
            
            OnFinishDrawing();
        }
        
        IEnumerator DrawDialog(string str)
        {
            // print($"Start Coroutine {str}");
            isDrawing = true;
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

            isDrawing = false;
            // print("Finish drawDialog");
            OnFinishDrawing();
            yield return null;
        }
    }
}
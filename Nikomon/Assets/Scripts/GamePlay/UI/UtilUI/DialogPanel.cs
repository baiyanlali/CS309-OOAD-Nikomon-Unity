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
        public Text DialogText;
        private static DialogHandler sInstance;
        private Queue<string> reports;
        private string currentReport;
        public GameObject ShowNext;
        private bool isDrawing = false;
        public Action<string> OnDialogFinished;
        public float TextSpeed=20;

        public override float DisplayTime { get; } = 0.5f;

        public override void Init(params object[] args)
        {
            base.Init(args);
            currentReport = (string) args[0];
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
            base.OnEnter(args);
            print(currentReport);
            // ExitBtn.onClick.RemoveAllListeners();
            // ExitBtn.onClick.AddListener(() =>
            // {
            //     if (isDrawing)
            //     {
            //         StartDialogImmediate(currentReport);
            //     }
            // });
            DialogText.text = currentReport;
            // OnReport(currentReport);
        }

        private void OnFinishDrawing()
        {
            if (reports.Count == 0)
            {
                OnDialogFinished?.Invoke("");
                UIManager.Instance.Hide(this);
                // gameObject.SetActive(false);
                return;
            }
            print("Finish drawing");
            currentReport = reports.Dequeue();
            StartCoroutine(DrawDialog(currentReport));
        }
        
        private void OnReport(string report)
        {
            reports.Enqueue(report);

            if (reports.Count == 1)
            {
                StartCoroutine(DrawDialog(report));
            }
        }

        private void StartDialogImmediate(string str)
        {
            StopAllCoroutines();
            DialogText.text = str;
            isDrawing = false;
            
            OnFinishDrawing();
        }
        
        private IEnumerator DrawDialog(string str)
        {
            isDrawing = true;
            StringBuilder sb = new StringBuilder();
            int index = 0;
            while (index < str.Length)
            {
                sb.Append(str[index]);
                DialogText.text = sb.ToString();
                yield return new WaitForSeconds(1/TextSpeed);
                index++;
            }

            isDrawing = false;
            OnFinishDrawing();
        }
    }
}
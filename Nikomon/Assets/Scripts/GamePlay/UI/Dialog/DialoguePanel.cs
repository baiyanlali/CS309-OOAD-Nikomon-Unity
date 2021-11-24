using System;
using System.Collections;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.Dialog
{
    public class DialoguePanel:BaseUI
    {
        private string[] dialogList;
        public DialogEngine de;
        public string debugBoxString;

        /*
        private Image dialogBox;
        private Text dialogBoxText;
        private CanvasGroup dialogCanvasGroup;*/

        public override UILayer Layer => UILayer.NormalUI;
        public override bool IsOnly => false;
        public override bool IsBlockPlayerControl => true;
        public float TweenTime=0.2f;

        public override void Init(params object[] args)
        {
            Debug.Log("ini");
            base.Init(args);
            //de.transform.GetComponent<DialogEngine>();
            //de = GET(de,nameof(DialogEngine),GET_TYPE.Component);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0 for ConfirmInformation string, 1 for confirm or cancel action
        /// if more, 2 for yes string, 3 for no string
        /// </param>
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            Debug.Log("Enter");
            string dialogContent = args[0] as string;
            Action<bool> confirmAction=null;
            
            if(args.Length>=2)//call choiceNode
                confirmAction = args[1] as Action<bool>;
            string yes = "ConfirmPanel.yes";
            string no = "ConfirmPanel.no";
            if (args.Length >= 4)
            {
                yes = args[2] as string;
                no = args[3] as string;
            }
            de.DrawDialogBox();
            de.StartCoroutine("DrawText", dialogContent);
            de.StartCoroutine("floatingTriagle");

        }

        public override void OnExit()
        {
            Debug.Log("exit!");
            de.StartCoroutine("fadeEffect", 0);
            base.OnExit();
        }
    }
}
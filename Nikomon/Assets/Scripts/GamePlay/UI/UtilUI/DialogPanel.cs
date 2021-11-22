using System;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.UtilUI
{
    public class DialogPanel:BaseUI
    {
        public Text DialogText;
        public NicomonInputSystem nicoInput;
        public GameObject ShowNext;
        private static DialogHandler sInstance;
        private Queue<string> reports;
        private string currentReport;
        private bool isDrawing = false;
        public Action<string> OnDialogFinished;
    }
}
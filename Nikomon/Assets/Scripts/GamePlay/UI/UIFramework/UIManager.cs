using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.UI.UIFramework
{
    public enum UILayer
    {
        MainUI,
        NormalUI,//仅有这个有stack层数
        PopupUI,
        Top
    }
    public class UIManager:MonoBehaviour
    {
        private static UIManager _instance;

        public static UIManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<UIManager>();
                if (_instance != null) return _instance;
                CreateUIManager();
                DontDestroyOnLoad(_instance);
                return _instance;
            }
        }

        private static void CreateUIManager()
        {
            GameObject UIManager = new GameObject();
            UIManager.name = "UIManager";
            _instance = UIManager.AddComponent<UIManager>();
        }


        private Stack<BaseUI> _normalStack = new Stack<BaseUI>();
        private BaseUI _mainUI;


        public void PushUI(BaseUI ui)
        {
            switch (ui.Layer)
            {
                case UILayer.MainUI:
                    break;
                case UILayer.NormalUI:
                    break;
                case UILayer.PopupUI:
                    break;
            }
        }
        
        
        
    }
}
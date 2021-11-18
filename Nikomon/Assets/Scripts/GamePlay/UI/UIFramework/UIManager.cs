using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.UI.UIFramework
{
    public enum UILayer
    {
        MainUI,
        NormalUI,//仅有这个有stack层数
        PopupUI,
        Top//用于保存状态（转圈圈啥的
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

        protected Dictionary<Type, BaseUI> _uiDics = new Dictionary<Type, BaseUI>();

        private Stack<BaseUI> _normalStack = new Stack<BaseUI>();
        private Stack<BaseUI> _popStack = new Stack<BaseUI>();
        //主界面上可能会有许多个UI元素
        private List<BaseUI> _mainUI=new List<BaseUI>();

        public Transform MainUIParent;
        public Transform NormalUIParent;
        public Transform PopUIParent;


        public Transform GetUIParent(UILayer layer)
        {
            switch (layer)
            {
                case UILayer.MainUI:
                    return MainUIParent;
                    break;
                case UILayer.NormalUI:
                    return NormalUIParent;
                case UILayer.PopupUI:
                    return PopUIParent;
                
            }

            return null;
        }
        
        public void PushUI(BaseUI ui)
        {
            switch (ui.Layer)
            {
                case UILayer.MainUI:
                    _mainUI.Add(ui);
                    break;
                case UILayer.NormalUI:
                    _normalStack.Peek().OnPause();
                    _normalStack.Push(ui);
                    break;
                case UILayer.PopupUI:
                    break;
            }
        }

        public void Show<T>(params object[] args)where T:BaseUI
        {
            BaseUI curUI;
            if (_uiDics.ContainsKey(typeof(T)))
            {
                curUI = _uiDics[typeof(T)];

            }
            else
            {
                curUI = GameResources.SpawnUIPrefab(nameof(T));
            }
            if(curUI.IsOnly)
                PopAllUI(curUI.Layer);
            PushUI(curUI);
            
            curUI.transform.SetParent(GetUIParent(curUI.Layer));
            
            curUI.Init();
            
            curUI.OnEnter(args);

        }

        public void PopAllUI(UILayer layer)
        {
            if (layer == UILayer.NormalUI)
            {
                PopUIUntilNone(_normalStack);
            }
            else if (layer == UILayer.PopupUI)
            {
                PopUIUntilNone(_popStack);
            }
            else if (layer == UILayer.MainUI)
            {
                foreach (var mainUI in _mainUI)
                {
                    mainUI.OnExit();
                }
                _mainUI.Clear();
            }
        }

        /// <summary>
        /// 弹出ui，直到until弹出为止
        /// </summary>
        /// <param name="until"></param>
        /// <param name="stack"></param>
        void PopUIUntil(BaseUI until,Stack<BaseUI> stack)
        {
            if (!stack.Contains(until)) return;
            BaseUI node = stack.Pop();
            while (node)
            {
                if (node == until)
                {
                    node.OnExit();
                    return;
                }

                if (stack.Count <= 0) return;
                node.OnExit();
                node = stack.Pop();
            }
            
            if(stack.Count>0)
                stack.Peek().OnResume();
        }

        void PopUIUntilNone(Stack<BaseUI> stack)
        {
            while (stack.Count>0)
            {
                BaseUI node = stack.Pop();
                node.OnExit();
            }
        }
        
        
        
        
    }
}
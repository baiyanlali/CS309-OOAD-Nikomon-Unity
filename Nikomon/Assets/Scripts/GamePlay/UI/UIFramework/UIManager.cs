using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GamePlay.Utilities;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

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
            // GameObject UIManager = new GameObject();
            // UIManager.name = "UIManager";
            // _instance = UIManager.AddComponent<UIManager>();
            GameObject UIManager = GameResources.SpawnPrefab(typeof(UIManager));
            UIManager = Instantiate(UIManager);
            UIManager.name = nameof(UIManager);
            _instance = UIManager.GetComponent<UIManager>();
            _instance.Init();
        }

        protected Dictionary<Type, BaseUI> _uiDics = new Dictionary<Type, BaseUI>();

        private Stack<BaseUI> _normalStack = new Stack<BaseUI>();
        private Stack<BaseUI> _popStack = new Stack<BaseUI>();
        //主界面上可能会有许多个UI元素
        private List<BaseUI> _mainUI=new List<BaseUI>();
        private List<BaseUI> _topUI = new List<BaseUI>();

        public Transform MainUIParent;
        public Transform NormalUIParent;
        public Transform PopUIParent;
        public Transform TopParent;


        public Transform GetUIParent(UILayer layer)
        {
            switch (layer)
            {
                case UILayer.MainUI:
                    return MainUIParent;
                case UILayer.NormalUI:
                    return NormalUIParent;
                case UILayer.PopupUI:
                    return PopUIParent;
                case UILayer.Top:
                    return TopParent;
            }

            return null;
        }

        public void Init()
        {
            SceneManager.sceneLoaded += (s, m) =>
            {
                if (m == LoadSceneMode.Single)
                {
                    PopAllUI(UILayer.Top);
                    PopAllUI(UILayer.MainUI);
                    PopAllUI(UILayer.NormalUI);
                    PopAllUI(UILayer.PopupUI);
                }
            };

        }
        
        
        
        public void PushUI(BaseUI ui)
        {
            switch (ui.Layer)
            {
                case UILayer.MainUI:
                    _mainUI.Add(ui);
                    break;
                case UILayer.NormalUI:
                    if(_normalStack.Count!=0)
                        _normalStack.Peek()?.OnPause();
                    _normalStack.Push(ui);
                    break;
                case UILayer.PopupUI:
                    if(_popStack.Count!=0)
                        _popStack.Peek()?.OnPause();
                    _popStack.Push(ui);
                    break;
                case UILayer.Top:
                    _topUI.Add(ui);
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
                GameObject tmp = GameResources.SpawnPrefab(typeof(T));
                // if(tmp.GetComponent<RectTransform>())
                // curUI = Instantiate(tmp,tmp.transform.position,tmp.transform.rotation).GetComponent<BaseUI>();
                
                curUI = Instantiate(tmp,GetUIParent(tmp.GetComponent<BaseUI>().Layer)).GetComponent<BaseUI>();
                curUI.name = curUI.name.Replace("(Clone)", "");
                tmp = null;
                _uiDics.AddOrReplace(typeof(T),curUI);
            }
            if(curUI.IsOnly)
                PopAllUI(curUI.Layer);
            PushUI(curUI);
            
            curUI.transform.SetParent(GetUIParent(curUI.Layer),false);
            
            curUI.transform.SetAsLastSibling();
            // curUI.Init();
            
            curUI.OnEnter(args);

        }

        public void Hide<T>() where T : BaseUI
        {
            Type type = typeof(T);
            if (_uiDics.ContainsKey(type))
            {
                Hide(_uiDics[type]);
            }
        }

        public void Hide<T>(T obj)where T:BaseUI
        {
            var tmp = obj as BaseUI;
            _uiDics.AddOrReplace(tmp.GetType(),tmp);
            if (_normalStack.Contains(tmp))
            {
                PopUIUntil(tmp,_normalStack);
            }else if (_popStack.Contains(tmp))
            {
                PopUIUntil(tmp, _popStack);
            }
            else if(_mainUI.Contains(tmp))
            {
                _mainUI.Remove(tmp);
                tmp.OnExit();
            }else if (_topUI.Contains(tmp))
            {
                _topUI.Remove(tmp);
                tmp.OnExit();
            }
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
            }else if (layer == UILayer.Top)
            {
                foreach (var topUI in _topUI)
                {
                    topUI.OnExit();
                }
                _topUI.Clear();
            }
        }

        public void Refresh<T>(params object[] args)where T:BaseUI
        {
            Type type = typeof(T);
            if (!_uiDics.ContainsKey(type))
            {
                
                GameObject tmp = GameResources.SpawnPrefab(typeof(T));
                // if(tmp.GetComponent<RectTransform>())
                // curUI = Instantiate(tmp,tmp.transform.position,tmp.transform.rotation).GetComponent<BaseUI>();
                
                var curUI = Instantiate(tmp,GetUIParent(tmp.GetComponent<BaseUI>().Layer)).GetComponent<BaseUI>();
                curUI.name = curUI.name.Replace("(Clone)", "");
                tmp = null;
                _uiDics.AddOrReplace(typeof(T),curUI);
                curUI.gameObject.SetActive(false);
            }
            
            _uiDics[type].OnRefresh(args);
        }

        public T GetUI<T>() where T : BaseUI
        {
            Type type = typeof(T);
            if (_uiDics.ContainsKey(type))
            {
                return _uiDics[type].GetComponent<T>();
            }

            return null;
            // throw new Exception($"no such ui {type} found");
        }
        
        /// <summary>
        /// 弹出ui，直到until弹出为止
        /// </summary>
        /// <param name="until"></param>
        /// <param name="stack"></param>
        public void PopUIUntil(BaseUI until,Stack<BaseUI> stack)
        {
            if (!stack.Contains(until)) return;
            BaseUI node = stack.Pop();
            while (node)
            {
                if (node == until)
                {
                    node.OnExit();
                    if(stack.Count>0)
                        stack.Peek().OnResume();
                    return;
                }

                if (stack.Count <= 0) return;
                node.OnExit();
                node = stack.Pop();
            }

            if (stack.Count > 0)
            {
                stack.Peek().OnResume();
                stack.Peek().transform.SetAsLastSibling();
            }

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
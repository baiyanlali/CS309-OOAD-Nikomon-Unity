using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlay.UI.UIFramework
{
    public interface IUIAnimator
    {
        void OnEnterAnimator();
        void OnExitAnimator();
    }
    public abstract class BaseUI : MonoBehaviour
    {
        public enum GET_TYPE
        {
            GameObject,
            Component
        }
        public virtual UILayer Layer { get; set; }
        public virtual bool IsOnly { get; }
        public virtual bool IsBlockPlayerControl { get; set; }
        public virtual float DisplayTime { get; } = -1;//-1表示一直显示

        private bool CanPlayerControlBefore;

        private List<Selectable> allUIAsset = new List<Selectable>();


        /// <summary>
        /// 用于手柄，当窗口打开时默认选中哪一种元素
        /// </summary>
        public GameObject FirstSelectable;
        public Button ExitBtn;

        public UnityAction CancelAction;
        public virtual void Init(params object[] args)
        {
            if(ExitBtn!=null)
                ExitBtn.onClick.AddListener(CancelAction);
        }

        public virtual void SetInteractable(bool interactable)
        {
            for (int i = 0; i < allUIAsset.Count; i++)
            {
                allUIAsset[i].interactable = interactable;
            }
        }

        protected T GET<T>(BaseUI ui, T obj,string name,GET_TYPE type)where T:UnityEngine.Object
        {
            return ui.GET(obj,name,type);
            // return obj!=null ? obj : ui.transform.Find(name).GetComponent<T>();
        }
        //TODO:写得太烂了！重写！
        protected T GET<T>(T obj,string name,GET_TYPE type)where T:UnityEngine.Object
        {
            if (type == GET_TYPE.Component)
            {
                T result = obj!=null ? obj : transform.Find(name).GetComponent<T>();
                if (result is Selectable)
                {
                    allUIAsset.Add(result as Selectable);
                }

                return result;
            }
            else if(type==GET_TYPE.GameObject)
            {
                T result= obj != null ? obj : transform.Find(name).gameObject as T;
                return result;
            }

            return null;
        }
        
        
        
        public virtual void OnEnter(params object[] args)
        {
            this.gameObject.SetActive(true);
            this.Init(args);
            if(this is IUIAnimator)(this as IUIAnimator).OnEnterAnimator();
            CanPlayerControlBefore = GlobalManager.Instance.CanPlayerControlled;
            if (IsBlockPlayerControl) GlobalManager.Instance.CanPlayerControlled = false;

            SetInteractable(true);

            if (FirstSelectable != null)
            {
                EventSystem.current.SetSelectedGameObject(FirstSelectable.gameObject);
            }
        }

        public virtual void OnExit()
        {
            if(this is IUIAnimator)(this as IUIAnimator).OnExitAnimator();
            else gameObject.SetActive(false);

            GlobalManager.Instance.CanPlayerControlled = CanPlayerControlBefore;
        }

        private GameObject currentSelectObj;

        /// <summary>
        /// 这个是用于当别的窗口叠在本窗口之上时，本窗口内暂停
        /// </summary>
        public virtual void OnPause()
        {
            currentSelectObj = EventSystem.current.currentSelectedGameObject;
            SetInteractable(false);
        }

        /// <summary>
        /// 当别的窗口被pop掉时，本窗口继续
        /// </summary>
        public virtual void OnResume()
        {
            SetInteractable(true);
            if(currentSelectObj!=null)
                EventSystem.current.SetSelectedGameObject(currentSelectObj);
        }

        public void BindListener(Button button, Action callback)
        {
            if (button == null) return;
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=> callback());
            
        }
        
        
    }
    
    
    
}
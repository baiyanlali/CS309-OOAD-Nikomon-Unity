using System;
using UnityEngine;
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
        public virtual UILayer Layer { get; set; }
        public virtual bool IsOnly { get; } = false;
        public virtual bool IsBlockPlayerControl { get; set; } = true;
        public virtual float DisplayTime { get; } = -1;//-1表示一直显示

        public bool CanPlayerControlBefore;

        /// <summary>
        /// 用于手柄，当窗口打开时默认选中哪一种元素
        /// </summary>
        public Selectable FirstSelectable;
        public Button ExitBtn;
        public virtual void Init(params object[] args)
        {
            
        }
        
        
        public void OnEnter(params object[] args)
        {
            this.gameObject.SetActive(true);
            this.Init(args);
            if(this is IUIAnimator)(this as IUIAnimator).OnEnterAnimator();
            CanPlayerControlBefore = GlobalManager.CanPlayerControlled;
            if (IsBlockPlayerControl) GlobalManager.CanPlayerControlled = false;
            
            EventSystem.current.SetSelectedGameObject(FirstSelectable.gameObject);
        }

        public void OnExit()
        {
            if(this is IUIAnimator)(this as IUIAnimator).OnExitAnimator();
            else gameObject.SetActive(false);

            GlobalManager.CanPlayerControlled = CanPlayerControlBefore;
        }

        /// <summary>
        /// 这个是用于当别的窗口叠在本窗口之上时，本窗口内暂停
        /// </summary>
        public void OnPause()
        {
            
        }

        /// <summary>
        /// 当别的窗口被pop掉时，本窗口继续
        /// </summary>
        public void OnResume()
        {
        }

        public void BindListener(Button button, Action callback)
        {
            if (button == null) return;
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=> callback());
            
        }
        
        
    }
    
    
    
}
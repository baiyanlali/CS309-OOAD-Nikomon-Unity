using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlay.UIFramework
{
    public abstract class ViewBase : MonoBehaviour
    {
        public virtual void OnEnter(ContextBase context)
        {
        }
        
        public virtual void OnExit(ContextBase context)
        {
            
        }
        
        public virtual void OnPause(ContextBase context)
        {
            
        }
        
        public virtual void OnResume(ContextBase context)
        {
            
        }
    }
}
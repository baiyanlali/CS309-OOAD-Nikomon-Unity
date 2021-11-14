using System;
using System.Collections.Generic;

namespace GamePlay.UIFramework
{
    public class ContextManager
    {
        private Stack<ContextBase> _contextStack = new Stack<ContextBase>();

        public void Push(ContextBase contextBase,Action OnLoad=null)
        {
            if (_contextStack.Count != 0)
            {
                var lastOne = _contextStack.Peek();
                lastOne.ViewBase.OnPause(lastOne);
            }
            
            _contextStack.Push(contextBase);
            contextBase.ViewBase.OnEnter(contextBase);
            
            OnLoad?.Invoke();
            
        }

        public void Pop(Action OnUnload=null)
        {
            if (_contextStack.Count == 0) return;
            var curContext = _contextStack.Pop();
            
            curContext.ViewBase.OnExit(curContext);

            if (_contextStack.Count == 0) return;

            curContext = _contextStack.Peek();
            curContext.ViewBase.OnResume(curContext);
            
            OnUnload?.Invoke();

        }

        public ContextBase PeekOrNull()
        {
            if (_contextStack.Count == 0) return null;
            return _contextStack.Peek();
        }
        
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class EventPool
    {
        private static Queue<Action> _events = new Queue<Action>();

        public static void Schedule(Action e)
        {
            _events.Enqueue(e);
        }

        public static void Tick()
        {
            while (_events.Count!=0)
            {
                var action=_events.Dequeue();
                action.Invoke();
            }
        }
    }
}
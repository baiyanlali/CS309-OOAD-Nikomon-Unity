using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public static class Util
    {
        public static void Disable(this List<TargetChooserUI> uis)
        {
            foreach (var ui in uis)
            {
                ui.Disable();
            }
        }

        public static void Enable(this List<TargetChooserUI> uis)
        {
            foreach (var ui in uis)
            {
                ui.Enable();
            }
        }

        public static void OnSelected(this List<TargetChooserUI> uis, Action action)
        {
            foreach (var ui in uis)
            {
                ui.OnSelected=action;
            }
        }
        
        public static void On(this List<TargetChooserUI> uis)
        {
            foreach (var ui in uis)
            {
                ui.On();
            }
        }
        
        public static void Off(this List<TargetChooserUI> uis)
        {
            foreach (var ui in uis)
            {
                ui.Off();
            }
        }

        public static T RandomPickOne<T>(this T[] ts)
        {
            return ts[Random.Range(0,ts.Length)];
        }
        
    }
}
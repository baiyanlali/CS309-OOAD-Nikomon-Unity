using System;
using System.Collections.Generic;
using UnityEngine;
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


        public static Vector3 ChangeX(this Vector3 vec, float x)
        {
            return new Vector3(x, vec.y, vec.z);
        }
        
        public static Vector3 ChangeY(this Vector3 vec, float y)
        {
            return new Vector3(vec.x, y, vec.z);
        }
        
        public static Vector3 ChangeZ(this Vector3 vec, float z)
        {
            return new Vector3(vec.x, vec.y, z);
        }
        
    }
}
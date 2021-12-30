using System;
using UnityEngine;


namespace GamePlay.Utilities
{
    public abstract class MonoSingleton<T>:MonoBehaviour where T:MonoSingleton<T>
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                instance = instance ?? (FindObjectOfType<T>());
                instance = instance ?? new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                return instance;
            }
        }

        private void OnApplicationQuit()
        {
            instance = null;
        }
    }
}
using UnityEngine;

namespace GamePlay.Utilities
{
    public static class GameObjectExtension
    {
        public static void GET<T>(this MonoBehaviour getter, ref T component, string path) where T : Component
        {
            component ??= getter.transform.Find(path).GetComponent<T>();
        }

        public static void GET(this MonoBehaviour getter, ref GameObject obj, string path)
        {
            if (obj == null)
            {
                obj = getter.transform.Find(path).gameObject;
            }
            // obj ??= getter.transform.Find(path).gameObject;
        }

        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            T result = obj.GetComponent<T>();
            if (result != null) return result;
            result = obj.AddComponent<T>();
            return result;
        }
    }
}
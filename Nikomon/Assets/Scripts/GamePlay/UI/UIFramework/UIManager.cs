using System.Collections.Generic;
using GamePlay.Utilities;
using UnityEngine;

namespace GamePlay.UIFramework
{
    public static class UIManager
    {
        public static Dictionary<UIType, GameObject> _UIDict = new Dictionary<UIType, GameObject>();

        private static Canvas _canvas;

        public static GameObject GetUI(UIType type)
        {
            if (_UIDict.ContainsKey(type) == false || _UIDict[type] == null)
            {
                GameObject tmp = Resources.Load<GameObject>(type.Path);
                GameObject obj = GameObject.Instantiate(tmp);
                obj.transform.SetParent(_canvas.transform, false);
                obj.name = type.Name;
                _UIDict.AddOrReplace(type,obj);
                return obj;
            }

            return _UIDict[type];
        }
        
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Utilities
{
    public static class DictionaryExtension
    {
        public static Dictionary<TKey,TVal> AddOrReplace<TKey,TVal>(this Dictionary<TKey,TVal> dic,TKey key,TVal val)
        {
            dic[key] = val;
            return dic;
        }
    }
}
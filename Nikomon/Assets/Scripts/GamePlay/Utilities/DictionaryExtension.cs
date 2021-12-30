using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Utilities
{
    public static class DictionaryExtension
    {
        public static void AddOrReplace<TKey,TVal>(this Dictionary<TKey,TVal> dic,TKey key,TVal val)
        {
            dic[key] = val;
        }

        public static void AddAndUseIfHas(this Dictionary<string, Action> dic, string key,Action val)
        {
            if (dic.ContainsKey(key))
            {
                Debug.Log("Anim:"+key+" has been invoked by a add and if has");
                dic[key]?.Invoke();
            }
            dic[key] = val;
        }



        public static string GetAnimateName(this Animator animator, AnimatorStateInfo info)
        {
            var arr = animator.GetCurrentAnimatorClipInfo(0);

            foreach (var aci in arr)
            {
                if (info.shortNameHash.Equals(Animator.StringToHash(aci.clip.name)))
                {
                    return aci.clip.name;
                }
            }

            return "";
        }
    }
}
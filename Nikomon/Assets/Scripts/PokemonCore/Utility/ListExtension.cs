using System;
using System.Collections.Generic;
using System.Linq;
using PokemonCore.Combat.Interface;

namespace PokemonCore.Utility
{
    public static class ListExtension
    {
        public static string ConverToString<T>(this List<T> i)
        {
            return String.Join(", ", i);
        }

        public static string ConvertToString<T>(this HashSet<T> i)
        {
            return String.Join(", ", i);
        }
        public static string ConvertToString<T,K>(this Dictionary<T,K> i)
        {
            return String.Join(",", i);
        }
        
        

        /// <summary>
        /// 两个参数的List的count必须相同
        /// </summary>
        /// <param name="cons"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool Satisfy(this List<Condition> cons,List<IPropertyModify> property)
        {
            if (cons == null) return true;
            if (cons.Count == 0) return true;
            for (int i = 0; i < cons.Count; i++)
            {
                if (cons[i].Satisfied(property[i]) == false) return false;
            }

            return true;
        }


    }

    public static class TypesExtension
    {
        public static TypeRelationship CompareTypes(this Types t1,Types t2)
        {
            return Types.CompareTypes(t1, t2);
        }
        public static TypeRelationship CompareTypes(this Types t1,int? t2)
        {
            return Types.CompareTypes(t1, t2);
        }
        
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
        return source ?? Enumerable.Empty<T>();
        }
        
        
        
    }
    
    
}
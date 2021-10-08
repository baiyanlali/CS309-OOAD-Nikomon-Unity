using System;
using System.Collections.Generic;
using System.Linq;
using PokemonCore.Combat;
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

        public static bool TrainerAllFaint(this List<Trainer> trainers)
        {
            int num = 0;
            foreach (var trainer in trainers)
            {
                num += trainer.ablePokemonCount;
            }

            return num == 0;
        }

        public static T RandomPickOne<T>(this List<T> list)
        {
            return list[Game.Random.Next(0, list.Count)];
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
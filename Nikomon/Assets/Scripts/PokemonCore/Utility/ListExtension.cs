using System;
using System.Collections.Generic;

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
        
        
        
        
    }
}
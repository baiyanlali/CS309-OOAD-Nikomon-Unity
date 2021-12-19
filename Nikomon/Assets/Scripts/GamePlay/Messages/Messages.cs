using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using PokemonCore.Utility;

namespace GamePlay.Messages
{
    public static class Messages
    {
        private static Dictionary<string, string> Translator=new Dictionary<string, string>();

        private static string _currentCulture;
        
        public static string Current_culture
        {
            get
            {
                if (string.IsNullOrEmpty(_currentCulture))
                {
                    SetUp("");
                }

                return _currentCulture;
            }
            private set
            {
                _currentCulture = value;
                Thread.CurrentThread.CurrentCulture = new CultureInfo(value);
            }
        }

        public static void SetUp(string lang="")
        {
            if (lang.Equals(""))
                Current_culture = Thread.CurrentThread.CurrentCulture.ToString();
            else
                Current_culture = lang;
            
            // Translator = SaveLoad.Load<Dictionary<string, string>>("language_" + Current_culture);
        }
        
        public static string Get(string str)
        {
            if (Translator.TryGetValue(str, out string var))
                return var;
            else
                return str;
        }
        
    }
}
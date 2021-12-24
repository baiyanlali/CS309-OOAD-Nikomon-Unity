using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using PokemonCore.Utility;

namespace GamePlay.Messages
{
    public static class Messages
    {
        public static readonly Dictionary<string, string> Languages = new Dictionary<string, string>()
        {
            ["English"]="en",
            ["简体中文"]="zh-CN",
            ["日本語"]="ja"
        };
        
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
                // Thread.CurrentThread.CurrentCulture = new CultureInfo(value);
            }
        }

        public static Action<string> OnLanguageChanged;
        public static void SetUp(string lang="")
        {
            if (lang.Equals(""))
                Current_culture = Thread.CurrentThread.CurrentCulture.ToString();
            else
                Current_culture = lang;
            
            Translator = GameResources.LoadLocalization(Current_culture); //lang_id);
            OnLanguageChanged?.Invoke(_currentCulture);
            // Translator = SaveLoad.Load<Dictionary<string, string>>("language_" + Current_culture);
        }
        
        public static void SetUpByLanguageName(string lang="")
        {
            bool hasValue = Languages.TryGetValue(lang, out string result);
            if(hasValue)
                SetUp(result);
            else
            {
                SetUp("");
            }
        }
        
        public static string Get(string str)
        {
            if (Translator.TryGetValue(str, out string var))
                return var;
            else
                return str;
        }

        public static string GetLanguageName()
        {
            foreach (var language in Languages)
            {
                if (language.Value == _currentCulture)
                {
                    return language.Key;
                }
            }

            return null;
        }


    }
}
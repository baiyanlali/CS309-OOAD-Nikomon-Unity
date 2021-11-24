using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace GamePlay.Messages
{
    public static class Languages
    {
        public static Dictionary<string, Language> Lang = new Dictionary<string, Language>()
        {
            ["English"] = new Language("English","en", "en-EN"),
            ["Chinese"] = new Language("Chinese", "zh", "zh-CN"),
            ["Japanese"] = new Language("Japanese", "ja", "ja-JP")
        };
        
    }
    
    
    // Languages(String name, String code, Status status, String[] reviewers, String[] translators)
    public struct Language
    {
        public string name;
        public string code;
        public String culture;

        public Language(string country_name, string country_code, String current_culture)
        {
            name = country_name;
            code = country_code;
            culture = current_culture;
        }
    }
    
    
}
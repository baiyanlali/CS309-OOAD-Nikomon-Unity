using System.Collections.Generic;
using PokemonCore.Utility;

namespace GamePlay.Core
{
    public static class Translator
    {
        public static Dictionary<string, string> Translate;
        public static int LanguageID;

        public static void ChangeLanguage(int lang_id)
        {
            Translate = SaveLoad.Load<Dictionary<string,string>>("");
        }

        public static string TranslateStr(string str)
        {
            return null;
        }
    }
}
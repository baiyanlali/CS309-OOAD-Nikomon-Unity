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
            Translate = GameResources.LoadLocalization("ch-CN"); //lang_id);
        }

        public static string TranslateStr(string str)
        {
            bool hasValue = Translate.TryGetValue(str,out string result);
            return hasValue? result:str;
        }
    }
}
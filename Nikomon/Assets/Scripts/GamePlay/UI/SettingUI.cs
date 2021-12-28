using System.Linq;
using GamePlay.UI.UIFramework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
    public class SettingUI:BaseUI
    {
        public Text Language,Ratio;
        public Button LanguageLeftArrow, LanguageRightArrow;
        public Button RatioLeftArrow, RatioRightArrow;
        public GameObject objLanguageLeftArrow,objLanguageRightArrow;
        public GameObject objRatioLeftArrow,objRatioRightArrow;
        private string[] ratios = {"high","low"};
        private string[] languages;
        private string ratio;
        private int LanguageIndex;
        public override bool IsOnly { get; } = true;
        
        
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            // print(111111);
            ratio = ratios[0];
            Ratio.text = ratio;
            languages = Messages.Messages.Languages.Keys.ToArray();
            Language.text = Messages.Messages.GetLanguageName();
            LanguageIndex = 0;
            
            objLanguageLeftArrow.GetComponent<TriggerSelect>().onSelect = () =>
            {
                Vector3 v = new Vector3();
                LeanTween.scale(objLanguageLeftArrow, Vector3.one * 1.15f, 0.2f);
                // v.x = 1.15f;
                // v.y = 1.15f;
                // v.z = 1.15f;
                // objLanguageLeftArrow.transform.localScale = v;
            };
            objLanguageRightArrow.GetComponent<TriggerSelect>().onSelect = () =>
            {
                Vector3 v = new Vector3();
                v.x = 1.15f;
                v.y = 1.15f;
                v.z = 1.15f;
                objLanguageRightArrow.transform.localScale = v;
            };
            objLanguageLeftArrow.GetComponent<TriggerSelect>().onDeSelect = () =>
            {
                Vector3 v = new Vector3();
                v.x = 1f;
                v.y = 1f;
                v.z = 1f;
                objLanguageLeftArrow.transform.localScale = v;
            };
            objLanguageRightArrow.GetComponent<TriggerSelect>().onDeSelect = () =>
            {
                Vector3 v = new Vector3();
                v.x = 1f;
                v.y = 1f;
                v.z = 1f;
                objLanguageRightArrow.transform.localScale = v;
            };
            
            
            objRatioLeftArrow.GetComponent<TriggerSelect>().onSelect = () =>
            {
                Vector3 v = new Vector3();
                v.x = 1.15f;
                v.y = 1.15f;
                v.z = 1.15f;
                objRatioLeftArrow.transform.localScale = v;
            };
            objRatioRightArrow.GetComponent<TriggerSelect>().onSelect = () =>
            {
                Vector3 v = new Vector3();
                v.x = 1.15f;
                v.y = 1.15f;
                v.z = 1.15f;
                objRatioRightArrow.transform.localScale = v;
            };
            objRatioLeftArrow.GetComponent<TriggerSelect>().onDeSelect = () =>
            {
                Vector3 v = new Vector3();
                v.x = 1f;
                v.y = 1f;
                v.z = 1f;
                objRatioLeftArrow.transform.localScale = v;
            };
            objRatioRightArrow.GetComponent<TriggerSelect>().onDeSelect = () =>
            {
                Vector3 v = new Vector3();
                v.x = 1f;
                v.y = 1f;
                v.z = 1f;
                objRatioRightArrow.transform.localScale = v;
            };
            
            RatioLeftArrow.onClick.RemoveAllListeners();
            RatioLeftArrow.onClick.AddListener(() =>
            {
                if (ratio == ratios[1])
                {
                    ratio = ratios[0];
                    Ratio.text = ratio;
                }
                else
                {
                    ratio = ratios[1];
                    Ratio.text = ratio;
                }
            });
            
            RatioRightArrow.onClick.RemoveAllListeners();
            RatioRightArrow.onClick.AddListener(() =>
            {
                if (ratio == ratios[1])
                {
                    ratio = ratios[0];
                    Ratio.text = ratio;
                }
                else
                {
                    ratio = ratios[1];
                    Ratio.text = ratio;
                }
                
            });
            
            
            //---------------------------------------------------------------------
            LanguageLeftArrow.onClick.RemoveAllListeners();
            LanguageLeftArrow.onClick.AddListener(() =>
            {
                LanguageIndex = (LanguageIndex + languages.Length - 1) % languages.Length;
                Language.text = languages[LanguageIndex];
                Messages.Messages.SetUpByLanguageName(languages[LanguageIndex]);
            });
            
            LanguageRightArrow.onClick.RemoveAllListeners();
            LanguageRightArrow.onClick.AddListener(() =>
            {
                LanguageIndex = (LanguageIndex + 1) % languages.Length;
                Language.text = languages[LanguageIndex];
                Messages.Messages.SetUpByLanguageName(languages[LanguageIndex]);
            });
            
        }
    }
}
using System.Linq;
using GamePlay.UI.UIFramework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
    public class SettingUI:BaseUI
    {
        public Text Language,Ratio,Voice;
        public Button LanguageLeftArrow, LanguageRightArrow;
        public Button RatioLeftArrow, RatioRightArrow;
        public Button VoiceLeftArrow, VoiceRightArrow;
        public GameObject objLanguageLeftArrow,objLanguageRightArrow;
        public GameObject objRatioLeftArrow,objRatioRightArrow;
        public GameObject objVoiceLeftArrow,objVoiceRightArrow;
        private string[] ratios = {"high","low"};
        private int[] voices = {1,2,3,4,5,6,7,8,9,10};
        private string[] languages;
        private string ratio;
        private int LanguageIndex;
        private int voice = -1; 
        private int VoiceIndex;
        public override bool IsOnly { get; } = true;
        
        
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            if(ratio == null)
                ratio = ratios[0];
            if (voice == -1)
            {
                //print(1111111111);
                voice = voices[4];
            }

            Ratio.text = ratio;
            Voice.text = voice.ToString();
            languages = Messages.Messages.Languages.Keys.ToArray();
            Language.text = Messages.Messages.GetLanguageName();
            LanguageIndex = 0;
            VoiceIndex = 4;
            for (int i = 0; i < languages.Length; i++)
            {
                if(Messages.Messages.GetLanguageName() == languages[i])
                    LanguageIndex = i;
            }
            print(voice);
            for (int i = 0; i < voices.Length; i++)
            {
                if (voice == voices[i])
                {
                    //print(i);
                    VoiceIndex = i;
                }
            }
            
            
            objLanguageLeftArrow.GetComponent<TriggerSelect>().onSelect = () =>
            {
                LeanTween.scale(objLanguageLeftArrow, Vector3.one * 1.15f, 0.2f);
            };
            objLanguageRightArrow.GetComponent<TriggerSelect>().onSelect = () =>
            {
                LeanTween.scale(objLanguageRightArrow, Vector3.one * 1.15f, 0.2f);
            };
            objLanguageLeftArrow.GetComponent<TriggerSelect>().onDeSelect = () =>
            {
                LeanTween.scale(objLanguageLeftArrow, Vector3.one, 0.2f);
            };
            objLanguageRightArrow.GetComponent<TriggerSelect>().onDeSelect = () =>
            {
                LeanTween.scale(objLanguageRightArrow, Vector3.one, 0.2f);
            };
            
            objVoiceLeftArrow.GetComponent<TriggerSelect>().onSelect = () =>
            {
                LeanTween.scale(objVoiceLeftArrow, Vector3.one * 1.15f, 0.2f);
            };
            objVoiceRightArrow.GetComponent<TriggerSelect>().onSelect = () =>
            {
                LeanTween.scale(objVoiceRightArrow, Vector3.one * 1.15f, 0.2f);
            };
            objVoiceLeftArrow.GetComponent<TriggerSelect>().onDeSelect = () =>
            {
                LeanTween.scale(objVoiceLeftArrow, Vector3.one, 0.2f);
            };
            objVoiceRightArrow.GetComponent<TriggerSelect>().onDeSelect = () =>
            {
                LeanTween.scale(objVoiceRightArrow, Vector3.one, 0.2f);
            };
            
            objRatioLeftArrow.GetComponent<TriggerSelect>().onSelect = () =>
            {
                LeanTween.scale(objRatioLeftArrow, Vector3.one * 1.15f, 0.2f);
            };
            objRatioRightArrow.GetComponent<TriggerSelect>().onSelect = () =>
            {
                LeanTween.scale(objRatioRightArrow, Vector3.one * 1.15f, 0.2f);
            };
            objRatioLeftArrow.GetComponent<TriggerSelect>().onDeSelect = () =>
            {
                LeanTween.scale(objRatioLeftArrow, Vector3.one , 0.2f);
            };
            objRatioRightArrow.GetComponent<TriggerSelect>().onDeSelect = () =>
            {
                LeanTween.scale(objRatioRightArrow, Vector3.one , 0.2f);
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
            //-----------------------------------------------------------------------------
            
            VoiceLeftArrow.onClick.RemoveAllListeners();
            VoiceLeftArrow.onClick.AddListener(() =>
            {
                VoiceIndex = (VoiceIndex + voices.Length - 1) % voices.Length;
                Voice.text = voices[VoiceIndex].ToString();
                voice = voices[VoiceIndex];
                //print(voice);
            });
            
            VoiceRightArrow.onClick.RemoveAllListeners();
            VoiceRightArrow.onClick.AddListener(() =>
            {
                VoiceIndex = (VoiceIndex + 1) % voices.Length;
                Voice.text = voices[VoiceIndex].ToString();
                voice = voices[VoiceIndex];
                //print(voice);
            });
            
            
        }
    }
}
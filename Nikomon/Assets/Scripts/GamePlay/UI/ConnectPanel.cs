using System.Linq;
using GamePlay.UI.UIFramework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
    public class ConnectPanel:BaseUI
    {
        public Text LevelText;
        public Button LeftArrow, RightArrow;
        private int[] levels = {1,2,3};
        private int level = -1; 
        private int levelIndex;
        public GameObject obj;
        private string code;
        public Button _Start;
        public override bool IsOnly { get; } = true;
        
        
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            if (level == -1)
            {
                level = levels[0];
                levelIndex = 0;
            }

            LevelText.text = level.ToString();
            
            for (int i = 0; i < levels.Length; i++)
            {
                if (level == levels[i])
                {
                    levelIndex = i;
                }
            }
            
            
            
            
            //---------------------------------------------------------------------
            LeftArrow.onClick.RemoveAllListeners();
            LeftArrow.onClick.AddListener(() =>
            {
                levelIndex = (levelIndex + levels.Length - 1) % levels.Length;
                LevelText.text = levels[levelIndex].ToString();
            });
            
            RightArrow.onClick.RemoveAllListeners();
            RightArrow.onClick.AddListener(() =>
            {
                levelIndex = (levelIndex + 1) % levels.Length;
                LevelText.text = levels[levelIndex].ToString();
            });
            
         
            _Start.onClick.RemoveAllListeners();
            _Start.onClick.AddListener(() =>
            {
                InputField inputField = obj.GetComponent<InputField>();
                code = inputField.text;
                GlobalManager.Instance.StartNetworkBattle(2,levels[levelIndex],code);
            });
 


           
            
        }
    }
}
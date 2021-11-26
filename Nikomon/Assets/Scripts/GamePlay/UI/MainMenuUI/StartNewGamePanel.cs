using UnityEngine.UI;

namespace GamePlay.UI.MainMenuUI
{
    public class StartNewGamePanel:TabContent
    {
        private Button StartBtn;
        private InputField NameText;
        public override void Init(params object[] args)
        {
            base.Init(args);
            StartBtn = GET(StartBtn, "Start");
            NameText = NameText!=null?NameText:transform.Find("NameText").GetComponent<InputField>();
        
            print("Init!");
            StartBtn.onClick.RemoveAllListeners();
            StartBtn.onClick.AddListener(() =>
            {
                StartMenuManager.StartGameWithNew(NameText.text);
            });
        }

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);
            StartBtn = GET(StartBtn, "Start");
            NameText = NameText!=null?NameText:transform.Find("NameText").GetComponent<InputField>();
        
            // print("Init!");
            StartBtn.onClick.RemoveAllListeners();
            StartBtn.onClick.AddListener(() =>
            {
                StartMenuManager.StartGameWithNew(NameText.text);
            });
        }
    }
}
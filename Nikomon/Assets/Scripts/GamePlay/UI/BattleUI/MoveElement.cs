using PokemonCore.Attack;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.BattleUI
{
    public class MoveElement : MonoBehaviour
    {
        public Text Name;
        public Image TypeIcon;
        public Text PP;
        public Button _button;
        public Move _move;

        public void Init(Move move)
        {
            _move = move;
            Name.text = Messages.Messages.Get(move._baseData.innerName);
            TypeIcon.sprite = GameResources.TypeIcons[move._baseData.Type];
            TypeIcon.color = GameResources.TypeColors[move._baseData.Type];
            PP.text = move.PP + "/" + move.TotalPP;
            
            
            // _button.onClick.RemoveAllListeners();
            // _button.onClick.AddListener(() =>
            // {
            //     // UIManager.Instance.Show<DialogueChooserPanel>(new List<string>
            //     // {
            //     //     "Buy","Cancel"
            //     // }, new Vector2(0, 1),action,obj.transform as RectTransform);
            //     
            // });
        }
        
    }
}
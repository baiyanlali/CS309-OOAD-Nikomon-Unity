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

        public void Init(Move move)
        {
            Name.text = Messages.Messages.Get(move._baseData.innerName);
            TypeIcon.sprite = GameResources.TypeIcons[move._baseData.Type];
            TypeIcon.color = GameResources.TypeColors[move._baseData.Type];
            PP.text = move.PP + "/" + move.TotalPP;
        }
        
    }
}
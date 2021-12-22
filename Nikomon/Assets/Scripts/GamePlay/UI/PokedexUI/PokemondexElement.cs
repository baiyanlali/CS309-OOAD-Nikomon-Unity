using PokemonCore.Attack;
using PokemonCore.Monster.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.BattleUI
{
    public class PokemondexElement : MonoBehaviour
    {
        public Text Name;
        public Image PokemonIcon;
        public Text ID;
        public int number = -1;
        public string name;
        public int id = -1;

        public void Init(PokemonData pokemon)
        {
            //TODO:这个Name是不是重新命名的呀？这里的Pokemon是宝可梦的类吗？
            Name.text = pokemon.innerName;
            (Name as TextTranslated).AutoTranslatedOnAwake = false;
            //print(pokemon.innerName);
            PokemonIcon.sprite = GameResources.PokemonIcons[pokemon.ID];
            ID.text = "NO." + pokemon.ID;
            id = pokemon.ID;
            name = pokemon.innerName;
        }
        
    }
}

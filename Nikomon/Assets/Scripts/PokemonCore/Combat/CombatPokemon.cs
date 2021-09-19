using System.Collections.Generic;
using PokemonCore.Attack;
using PokemonCore.Combat.Interface;

namespace PokemonCore.Combat
{
    public class CombatPokemon
    {
        public int CombatID { get; set; }
        private Pokemon pokemon { get; set; }
        public int HP { get; private set; }
        public int TotalHP { get; private set; }
        public int ATK { get; private set; }
        public int DEF { get; private set; }
        public int SPA { get; private set; }
        public int SPD { get; private set; }
        public int SPE { get; private set; }

        public string Name { get; private set; }

        public CombatMove[] moves { get; private set; }
        public int Exp { get; }

        public int Type1 { get; private set; }
        public int Type2 { get; private set; }
        public int Type3 { get; private set; }

        public int AbilityID { get; }

        public int ItemID { get; }
        
        /// <summary>
        /// Used only in battle
        /// </summary>
        public int Accuracy { get; private set; }
        /// <summary>
        /// Used only in battle
        /// </summary>
        public int Evasion { get; private set; }
        
        public List<IEffect> Effects { get; set; }

        public CombatPokemon(Pokemon pokemon)
        {
            this.pokemon = pokemon;
            this.HP = pokemon.HP;
            this.TotalHP = pokemon.TotalHp;
            this.ATK = pokemon.ATK;
            this.DEF = pokemon.DEF;
            this.SPA = pokemon.SPA;
            this.SPE = pokemon.SPE;
            this.SPD = pokemon.SPD;

            Accuracy = 100;
            Evasion = 0;

            CombatID = this.pokemon.GetHashCode()+Name.GetHashCode();

        }

        private void InitCombatMove()
        {
            moves = new CombatMove[pokemon.moves.Length];
            
        }
    }
}
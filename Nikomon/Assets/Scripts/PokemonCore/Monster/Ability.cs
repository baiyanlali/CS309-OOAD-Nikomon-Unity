using System.Collections.Generic;
using PokemonCore.Combat;
using PokemonCore.Combat.Interface;

namespace PokemonCore.Monster
{
    public class Ability:IEffect
    {
        public int ID { get; private set; }
        public CombatMove executeEffect(CombatMove cm)
        {
            throw new System.NotImplementedException();
        }
    }
}
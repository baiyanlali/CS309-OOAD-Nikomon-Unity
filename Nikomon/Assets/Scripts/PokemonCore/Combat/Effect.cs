using System.Collections.Generic;
using PokemonCore.Combat.Interface;

namespace PokemonCore.Combat
{
    public class Effect:IEffect
    {
        public int EffectID { get; set; }
        public string innerName { get; set; }
        public List<EffectElement> EffectElements { get; set; }
        public EffectLastType EffectLastType { get; set; }
        public int EffectChance { get; set; }

        public Effect(int effectID,string innerName="effect",EffectLastType lastType=EffectLastType.ROUND,int effectChance=100)
        {
            this.EffectID = effectID;
            this.innerName = innerName;
            this.EffectLastType = lastType;
            this.EffectChance = effectChance;
            EffectElements = new List<EffectElement>();
        }
        
        public void OnEffectBegin()
        {
            throw new System.NotImplementedException();
        }

        public Instruction OnChoosing(Battle battle, CombatPokemon pokemon)
        {
            throw new System.NotImplementedException();
        }

        public CombatMove OnMoving(Battle battle, CombatMove move, CombatPokemon attacker)
        {
            throw new System.NotImplementedException();
        }

        public Damage OnHit(Damage damage)
        {
            throw new System.NotImplementedException();
        }

        public Damage BeHurt(Damage damage)
        {
            throw new System.NotImplementedException();
        }

        public void OnDamaged(Battle battle, CombatPokemon attacker, CombatPokemon defender)
        {
            throw new System.NotImplementedException();
        }

        public bool OnSwitchPokemon(CombatPokemon poke)
        {
            throw new System.NotImplementedException();
        }

        public bool OnEffectEnd()
        {
            throw new System.NotImplementedException();
        }
    }
}
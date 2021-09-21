using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PokemonCore.Combat.Interface;
using PokemonCore.Utility;

namespace PokemonCore.Combat
{
    public class Effect:IEffect,IPropertyModify
    {
        public int EffectID { get; set; }
        public string innerName { get; set; }
        public EffectLastType EffectLastType { get; set; }
        public int RoundNum { get; set; }
        public int EffectChance { get; set; }

        public List<EffectElement> EffectElements { get; set; }

        public Effect(int effectID,string innerName="effect",EffectLastType lastType=EffectLastType.ROUND,int effectChance=100)
        {
            this.EffectID = effectID;
            this.innerName = innerName;
            this.EffectLastType = lastType;
            this.EffectChance = effectChance;
            EffectElements = new List<EffectElement>();
            TriggerConditions = new List<Condition>();
        }

        public void BeSwitched()
        {
            EffectElement[] ees = (from ee in EffectElements where ee.WhenToAct == EffectActTime.BeSwitched select ee)
                .ToArray();
            
        }
        /// <summary>
        /// 被打倒
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void BeFainted()
        {
            EffectElement[] ees = (from ee in EffectElements where ee.WhenToAct == EffectActTime.BeFainted select ee)
                .ToArray();
        }

        public void OnEffectBegin()
        {

        }

        public List<Condition> TriggerConditions { get; set; }

        public Instruction OnChoosing(Battle battle, CombatPokemon pokemon)
        {
            EffectElement[] ees = (from ee in EffectElements where ee.WhenToAct == EffectActTime.Choosing select ee)
                .ToArray();
            
            return null;
        }

        public CombatMove OnMoving(Battle battle, CombatMove move, CombatPokemon attacker)
        {
            EffectElement[] ees = (from ee in EffectElements where ee.WhenToAct == EffectActTime.Moving select ee)
                .ToArray();

            return null;
        }

        public Damage OnHit(Damage damage)
        {
            EffectElement[] ees = (from ee in EffectElements where ee.WhenToAct == EffectActTime.Hit select ee)
                .ToArray();

            return null;
        }

        public Damage BeHurt(Damage damage)
        {
            EffectElement[] ees = (from ee in EffectElements where ee.WhenToAct == EffectActTime.BeHurt select ee)
                .ToArray();

            return null;
        }

        public void OnDamaged(Battle battle, CombatPokemon attacker, CombatPokemon defender)
        {
            EffectElement[] ees = (from ee in EffectElements where ee.WhenToAct == EffectActTime.Damaged select ee)
                .ToArray();
        }

        public bool OnSwitchPokemon(CombatPokemon poke)
        {
            EffectElement[] ees = (from ee in EffectElements where ee.WhenToAct == EffectActTime.BeSwitched select ee)
                .ToArray();

            return true;
        }

        public bool OnEffectEnd()
        {
            throw new System.NotImplementedException();
        }
        
        public object this[string propertyName]
        {
            get
            {
                Type t = this.GetType();
                PropertyInfo pi = t.GetProperty(propertyName);
                return pi.GetValue(this, null);
            }
            set
            {
                Type t = this.GetType();
                PropertyInfo pi = t.GetProperty(propertyName);
                pi.SetValue(this, value, null);
            }
        }
    }
}
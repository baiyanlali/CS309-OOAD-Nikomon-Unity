using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using PokemonCore.Attack;
using PokemonCore.Combat.Interface;
using PokemonCore.Utility;

namespace PokemonCore.Combat
{
    public class CombatPokemon:IPropertyModify
    {
        [JsonIgnore]
        public Battle battle;
        public int TrainerID { get; set; }

        
        //TODO:改变一种方法
        public int CombatID
        {
            get => TrainerID * 100 + pokemon._base.ID;
        }

        public Pokemon pokemon { get; private set; }
        public int HP { get; private set; }
        public int TotalHP { get; private set; }
        public int ATK { get; private set; }
        public int DEF { get; private set; }
        public int SPA { get; private set; }
        public int SPD { get; private set; }
        public int SPE { get; private set; }
        
        public int Level
        {
            get => pokemon.Level;
        }

        /// <summary>
        /// Used only in battle
        /// </summary>
        public int Accuracy { get; private set; }

        /// <summary>
        /// Used only in battle
        /// </summary>
        public int Evasion { get; private set; }

        #region State Change

        private int m_HPStateChange;

        public int HPStateChange
        {
            get { return m_HPStateChange; }
            set { m_HPStateChange = Math.Max(Math.Min(6, value), -6); }
        }

        private int m_TotalHPStateChange;

        public int TotalHPStateChange
        {
            get { return m_TotalHPStateChange; }
            set { m_TotalHPStateChange = Math.Max(Math.Min(6, value), -6); }
        }

        private int m_ATKStateChange;

        public int ATKStateChange
        {
            get { return m_ATKStateChange; }
            set { m_ATKStateChange = Math.Max(Math.Min(6, value), -6); }
        }

        private int m_DEFStateChange;

        public int DEFStateChange
        {
            get { return m_DEFStateChange; }
            set { m_DEFStateChange = Math.Max(Math.Min(6, value), -6); }
        }

        private int m_SPAStateChange;

        public int SPAStateChange
        {
            get { return m_SPAStateChange; }
            set { m_SPAStateChange = Math.Max(Math.Min(6, value), -6); }
        }

        private int m_SPDStateChange;

        public int SPDStateChange
        {
            get { return m_SPDStateChange; }
            set { m_SPDStateChange = Math.Max(Math.Min(6, value), -6); }
        }

        private int m_SPEStateChange;

        public int SPEStateChange
        {
            get { return m_SPEStateChange; }
            set { m_SPEStateChange = Math.Max(Math.Min(6, value), -6); }
        }

        private int m_AccuracyStateChange;

        public int AccuracyStateChange
        {
            get { return m_AccuracyStateChange; }
            set { m_AccuracyStateChange = Math.Max(Math.Min(6, value), -6); }
        }

        private int m_EvasionStateChange;

        public int EvasionStateChange
        {
            get { return m_EvasionStateChange; }
            set { m_EvasionStateChange = Math.Max(Math.Min(6, value), -6); }
        }

        #endregion

        public string Name
        {
            get => pokemon == null ? "" : pokemon.Name;
        }

        // public CombatMove[] moves { get; private set; }

        public int Exp { get; }

        public int? Type1 { get; private set; }

        public int? Type2 { get; private set; }

        public int? Type3 { get; private set; }

        public int AbilityID { get; }

        public int ItemID { get; }

        public List<IEffect> Effects { get; set; }

        public Move lastMove;

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

        public CombatPokemon(Pokemon pokemon, Battle battle)
        {
            this.battle = battle;
            this.pokemon = pokemon;
            this.HP = pokemon.HP;
            this.TotalHP = pokemon.TotalHp;
            this.ATK = pokemon.ATK;
            this.DEF = pokemon.DEF;
            this.SPA = pokemon.SPA;
            this.SPE = pokemon.SPE;
            this.SPD = pokemon.SPD;

            this.Type1 = pokemon.Type1;
            this.Type2 = pokemon.Type2;
            this.Type3 = null;

            Accuracy = 100;
            Evasion = 0;

            //TODO:Add ability

            TrainerID = pokemon.TrainerID;

            lastMove = null;

            battle.OnThisTurnEnd += () => this.pokemon.HP = this.HP;
        }


        public Instruction OnChoosing()
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                Instruction i = e.OnChoosing(battle, this);
                if (i != null) return i;
            }

            return null;
        }


        public bool OnSwitch()
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.OnSwitchPokemon(this) == false) return false;
            }

            return true;
        }

        public Damage OnHit(Damage damage)
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                damage = e.OnHit(damage);
            }

            return damage;
        }

        public void BeHurt(Damage damage)
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                damage = e.BeHurt(damage);
            }


            this.HP -= damage.finalDamage;
            if (damage.combatMove.TargetEffects != null)
                Effects.AddRange(damage.combatMove.TargetEffects);
        }

        public override string ToString()
        {
            return $"Pokemon: {pokemon.Name} Lv.{pokemon.Level} HP:{HP}/{TotalHP}";
        }
    }
}
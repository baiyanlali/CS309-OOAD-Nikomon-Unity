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
    public class CombatPokemon
    {
        [JsonIgnore]
        public Battle battle=>Game.battle;
        public int TrainerID { get; set; }

        public int CombatID
        {
            get => TrainerID * 100 + pokemon._base.ID +randomID ;
        }

        public Pokemon pokemon { get; private set; }
        public int HP
        {
            get { return pokemon.HP;} private set { pokemon.HP = value; }
        }
        public int TotalHP { get; private set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int SPA { get; set; }
        public int SPD { get; set; }
        public int SPE { get; set; }
        
        public int Level
        {
            get => pokemon.Level;
        }

        /// <summary>
        /// Used only in battle
        /// </summary>
        public int Accuracy { get; set; }

        /// <summary>
        /// Used only in battle
        /// </summary>
        public int Evasion { get; set; }

        #region State Change
        
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

        public int? Type2 { get; set; }

        public int? Type3 { get; set; }

        public int AbilityID { get; set; }

        public int ItemID { get; }

        public List<Effect> Effects { get; set; }

        public Move lastMove;
        
        public int randomID { get; private set; }

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

            this.Type1 = pokemon.Type1;
            this.Type2 = pokemon.Type2;
            this.Type3 = null;

            Accuracy = 100;
            Evasion = 0;

            Effects = new List<Effect>();
            Effects.Add(Game.LuaEnv.Global.Get<Effect>("effect0"));

            //TODO:Add ability

            TrainerID = pokemon.TrainerID;

            lastMove = null;

            randomID = Game.Random.Next(100);
        }


        public Instruction OnChoosing()
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.OnChoosing == null) continue;
                Instruction i = e.OnChoosing(this);
                if (i != null) return i;
            }

            return null;
        }

        public CombatMove OnMoving(CombatMove cmove)
        {
            // UnityEngine.Debug.Log(cmove);
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.OnMoving == null) continue;
                
                e.OnMoving(cmove);
            }
            // UnityEngine.Debug.Log(cmove);
            return cmove;
        }


        public bool OnSwitch()
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.OnSwitchPokemon == null) return true;
                if (e.OnSwitchPokemon(this) == false) return false;
            }

            return true;
        }

        public Damage OnHit(Damage damage)
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.OnHit == null) continue;
                e.OnHit(this,damage);
            }

            return damage;
        }

        public void BeHurt(Damage damage)
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.BeHurt == null) continue;
                e.BeHurt(this,damage);
            }


            this.HP -= damage.finalDamage;
            if (HP <= 0)
            {
                HP = 0;
                OnFainting();
                return;
            }
            if (damage.combatMove.TargetEffects != null)
                Effects.AddRange(damage.combatMove.TargetEffects);
        }

        public void OnFainting()
        {
            foreach (var effect in Effects.OrEmptyIfNull())
            {
                if (effect.BeFainted == null) continue;
                effect.BeFainted(this);
            }
            battle.PokemonFainting(this);
        }

        public override string ToString()
        {
            return $"Pokemon: {pokemon.Name} Lv.{pokemon.Level} HP:{HP}/{TotalHP}";
        }
    }
}
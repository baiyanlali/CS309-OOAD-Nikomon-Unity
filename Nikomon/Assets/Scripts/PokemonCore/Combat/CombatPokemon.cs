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
    public enum PokemonStatus
    {
        NoStatus,
        Burn,
        Freeze,
        Paralysis,
        Sleep,
        Poison,
    }
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
            get { return pokemon.HP;} 
            set { pokemon.HP = value; }
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
        
        public PokemonStatus PokemonStatus
        {
            get
            {
                bool hasStatus = PokemonStatus.TryParse(pokemon.StatusID,out PokemonStatus result);
                if (!hasStatus) return PokemonStatus.NoStatus;
                return result;
            }
            set
            {
                pokemon.StatusID = value.ToString();
            } 
        }
        
        public List<Effect> Effects {get; private set; }

        public void AddEffect(Effect e)
        {
            Effects.Add(e);
            e.OnEffectBegin?.Invoke(this);
        }

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

            if (PokemonStatus != PokemonStatus.NoStatus)
            {
                Effects.Add(Game.LuaEnv.Global.Get<Effect>(statusDictionary[PokemonStatus]));
            }
        }

        private static  readonly Dictionary<PokemonStatus, string> statusDictionary = new Dictionary<PokemonStatus, string>()
        {
            [PokemonStatus.Burn]="effect6",
            [PokemonStatus.Freeze]="effect4",
            [PokemonStatus.Poison]="effect3",
            [PokemonStatus.Sleep]="effect2",
            [PokemonStatus.Paralysis]="effect5",
            
        };


        public Instruction OnChoosing()
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.OnChoosing == null) continue;
                Instruction i = e.OnChoosing(this);
                if (i != null) return i;
            }
            // Effects.EffectUpdate(this);
            return null;
        }

        public CombatMove OnMoving(CombatMove cmove)
        {
            // UnityEngine.Debug.Log(cmove);
            // foreach (var e in cmove.Effects.OrEmptyIfNull())
            // {
            //     if (e.OnEffectBegin == null) continue;
            //     e.OnEffectBegin(this);
            // }
            
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.OnMoving == null) continue;
                e.OnMoving(cmove);
            }
            // UnityEngine.Debug.Log(cmove);
            // Effects.EffectUpdate(this);
            return cmove;
        }

        public void Moved()
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.OnMoved == null) continue;
                e.OnMoved(this);
            }
        }


        public bool OnSwitch()
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.OnSwitchPokemon == null) return true;
                if (e.OnSwitchPokemon(this) == false) return false;
            }
            // Effects.EffectUpdate(this);
            return true;
        }

        public Damage OnHit(Damage damage)
        {
            foreach (var e in Effects.OrEmptyIfNull())
            {
                if (e.OnHit == null) continue;
                e.OnHit(this,damage);
            }
            // Effects.EffectUpdate(this);
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
            // if (damage.combatMove.Effects != null)
            //     Effects.AddRange(damage.combatMove.Effects);
        }

        public void OnFainting()
        {
            foreach (var effect in Effects.OrEmptyIfNull())
            {
                if (effect.BeFainted == null) continue;
                effect.BeFainted(this);
            }
            // Effects.EffectUpdate(this);
            battle.PokemonFainting(this);
        }

        public override string ToString()
        {
            return $"Pokemon: {pokemon.Name} Lv.{pokemon.Level} HP:{HP}/{TotalHP}";
        }
    }
}
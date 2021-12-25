

using System;
using System.Collections.Generic;
using PokemonCore.Utility;
using XLua;

namespace PokemonCore.Combat.Interface
{
    public enum EffectLastType
    {
        ALWAYS,// last the effect forever
        ROUND,//last for a few rounds
        UNTIL_SWITCH_POKEMON//until switch to another pokemon
    }

    public enum EffectTargetType
    {
        NULL,
        PokemonSponsor,
        PokemonTarget,
        Move,
        Trainer,
        BattleField,
        Effect,
        Damage
    }

    public enum EffectResultType
    {
        ConstantValue,//固定数字
        RandomValue,//随机一个数字
        RatioValue//按照百分比来
    }

    public enum EffectActTime
    {
        Choosing,
        Moving,
        Damaging,
        Damaged,
        Moved,
        Hit,
        BeHurt,
        EnterBattle,
        BeSwitched,
        BeFainted
    }

    public enum EffectType
    {
        Damage,
        Heal,
        SkipOneTurn,
        UseLastTurnMove,
        StatusChange
    }

    [LuaCallCSharp]
    public class Effect
    {
        public int ID;
        public string InnerName;
        public int Round;
        public EffectLastType EffectLastType;
        public int EffectChance;

        public bool isUsed;
        
        public Action<Effect,CombatPokemon> BeSwitched;
        public Action<Effect,CombatPokemon> BeFainted;
        public Action<Effect,CombatPokemon> OnEffectBegin;
        public Action<Effect,CombatPokemon> OnEffectEnd;
        public Action<Effect,CombatPokemon> OnMoved;

        public Action<Effect, CombatItem> OnUseItem;

        public Func<Effect,CombatPokemon,Instruction> OnChoosing;
        /// <summary>
        /// param: combat move, move sponsor, the result of combat move
        /// </summary>
        public Action<Effect,CombatMove> OnMoving;
        
        public Action<Effect,CombatPokemon,Damage> OnHit;
        public Action<Effect,CombatPokemon,Damage> BeHurt;
        /// <summary>
        ///sponsor, target
        /// </summary>
        public Action<Effect,CombatPokemon,CombatPokemon> OnDamaged;
        public Func<Effect,CombatPokemon,bool> OnSwitchPokemon;
        
        
    }
    
}
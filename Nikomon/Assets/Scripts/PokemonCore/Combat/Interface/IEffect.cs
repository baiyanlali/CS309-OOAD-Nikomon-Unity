

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


    public class Effect
    {
        public int ID;
        public string InnerName;
        public int Round;
        public EffectLastType EffectLastType;
        public int EffectChance;
        
        public Action<CombatPokemon> BeSwitched;
        public Action<CombatPokemon> BeFainted;
        public Action<CombatPokemon> OnEffectBegin;

        public Func<CombatPokemon,Instruction> OnChoosing;
        /// <summary>
        /// param: combat move, move sponsor, the result of combat move
        /// </summary>
        public Func<CombatMove,CombatMove> OnMoving;
        
        public Func<CombatPokemon,Damage,Damage> OnHit;
        public Func<CombatPokemon,Damage,Damage> BeHurt;
        /// <summary>
        ///sponsor, target
        /// </summary>
        public Action<CombatPokemon,CombatPokemon> OnDamaged;
        public Func<CombatPokemon,bool> OnSwitchPokemon;
        public Func<CombatPokemon,bool> OnEffectEnd;
        
    }
    
    [CSharpCallLua]
    public interface IEffect
    {
        
        public int round { get; set; }
        public string innerName { get; set; }


        public EffectLastType EffectLastType { get; set; }
        public int RoundNum { get; set; }

        public int EffectChance { get; set; }

        public void BeSwitched();

        public void BeFainted();
        
        public void OnEffectBegin();


        /// <summary>
        /// 在选取指令的时候使用
        /// </summary>
        /// <param name="battle"></param>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public Instruction OnChoosing(Battle battle, CombatPokemon pokemon);
        /// <summary>
        /// 在招式发挥效用之前使用
        /// </summary>
        /// <param name="battle"></param>
        /// <param name="move"></param>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public CombatMove OnMoving(Battle battle, CombatMove move, CombatPokemon attacker);
        /// <summary>
        /// 在招式即将攻击到对方的时候使用
        /// </summary>
        /// <param name="battle"></param>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public Damage OnHit(Damage damage);
        /// <summary>
        /// 再招式即将攻击到对方的时候使用
        /// </summary>
        /// <returns></returns>
        public Damage BeHurt(Damage damage);
        /// <summary>
        /// 在招式已经造成伤害后使用
        /// </summary>
        /// <param name="battle"></param>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public void OnDamaged(Battle battle, CombatPokemon attacker,CombatPokemon defender);

        public bool OnSwitchPokemon(CombatPokemon poke);

        public bool OnEffectEnd();



    }
}


using System.Collections.Generic;

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
        PokemonSponsor,
        PokemonTarget,
        Move,
        Trainer,
        BattleField
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
        BeHurt
    }
    
    public struct EffectElement
    {
        public EffectTargetType TargetType;
        public string Property;
        public EffectActTime WhenToAct;
        public EffectResultType ResultType;
    }
    
    public interface IEffect
    {
        public string innerName { get; set; }
        
        public List<EffectElement> EffectElements { get; set; }

        public EffectLastType EffectLastType { get; set; }
        
        public int EffectChance { get; set; }

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
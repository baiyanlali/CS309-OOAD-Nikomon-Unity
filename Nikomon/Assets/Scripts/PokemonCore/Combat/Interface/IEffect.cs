

using System.Collections.Generic;
using PokemonCore.Utility;
using UnityEngine.InputSystem.Controls;

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
        BeHurt
    }

    public enum EffectType
    {
        Damage,
        Heal,
        SkipOneTurn,
        UseLastTurnMove,
        StatusChange
    }

    public static class TypeTranslator
    {
        public static System.Type Translate(EffectTargetType targetType)
        {
            System.Type type=null;
            switch (targetType)
            {
                case EffectTargetType.PokemonSponsor:
                case EffectTargetType.PokemonTarget:
                    type = typeof(CombatPokemon);
                    break;
                case EffectTargetType.Effect:
                    type = typeof(Effect);
                    break;
                case EffectTargetType.Move:
                    type = typeof(CombatPokemon);
                    break;
                case EffectTargetType.Trainer:
                    type = typeof(Trainer);
                    break;
                case EffectTargetType.BattleField:
                    type = typeof(Battle);
                    break;
                case EffectTargetType.Damage:
                    type = typeof(Damage);
                    break;
                default:
                    type = null;
                    break;
            }

            return type;
        }
    }
    
    public class EffectElement
    {
        public string innerName;

        #region Define Type

        private EffectTargetType m_TargetType;

        private EffectTargetType m_DependType;

        public EffectTargetType TargetType { get=>m_TargetType;
            set => ChangeTargetType(value);
        }

        public EffectTargetType DependType
        {
            get => m_DependType;
            set => ChangeDependType(value);
        }

        private System.Type targetType;
        private System.Type dependType;

        #endregion


        /// <summary>
        /// 对于某个宝可梦的Property做出修改
        /// </summary>
        public string Property;

        /// <summary>
        /// 做出的数值伤害所依赖的Property，不一定用得到
        /// </summary>
        public string ValueDependProperty;

        public EffectActTime WhenToAct;

        public EffectResultType ResultType;

        public EffectType EffectType;

        public int EffectElementChance;
        /// <summary>
        /// 用来表示造成数值的变化大小，当有Depend依赖时，Result Type只能为Ratio，表示对于依赖数值的倍率（倍率/100）
        /// </summary>
        public int value;

        public EffectElement(string innerName="Give an explanation",string property="",string valueDependProperty="",EffectTargetType targetType=EffectTargetType.Move,EffectTargetType dependType=EffectTargetType.Damage)
        {
            this.innerName = innerName;
            this.Property = property;
            this.ValueDependProperty = valueDependProperty;
            this.TargetType = targetType;
            this.DependType = dependType;
            this.ResultType = EffectResultType.ConstantValue;
            this.WhenToAct = EffectActTime.Choosing;
            EffectElementChance = 100;
            EffectType = EffectType.Damage;
            value = 0;
        }

        public void ChangeTargetType(EffectTargetType type)
        {
            m_TargetType = type;
            targetType = TypeTranslator.Translate(type);
        }

        public void ChangeDependType(EffectTargetType type)
        {
            m_DependType = type;
            dependType = TypeTranslator.Translate(type);
        }
        
        
        
    }
    
    public interface IEffect
    {
        public string innerName { get; set; }
        
        public List<EffectElement> EffectElements { get; set; }

        public EffectLastType EffectLastType { get; set; }
        
        public int EffectChance { get; set; }

        public void OnEffectBegin();

        public List<Condition> TriggerConditions { get; set; }

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
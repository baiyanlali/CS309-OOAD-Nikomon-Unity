using System;
using System.Reflection;
using PokemonCore.Combat;
using PokemonCore.Combat.Interface;

namespace PokemonCore.Utility
{
    public enum ConditionMode
    {
        If,
        IfNot,
        Less,
        LessEqual,
        Equal,
        NotEqual,
        GreaterEqual,
        Greater
    }

    public class Condition
    {
        /// <summary>
        /// 利用反射通过属性名来找到对应属性
        /// </summary>
        public string property;

        /// <summary>
        /// 使条件成立的阈值,如果是IF和IFNOT，0代表false，1代表true
        /// </summary>
        public float treshhold;

        /// <summary>
        /// 条件模式
        /// </summary>
        public ConditionMode mode;

        public EffectTargetType TargetType
        {
            get => m_TargetType;
            set { ChangeType(value); }
        }

        private EffectTargetType m_TargetType;

        public EffectResultType effectResultType;


        public Type type;


        public Condition(EffectTargetType targetType=EffectTargetType.PokemonSponsor,ConditionMode mode=ConditionMode.Equal, string property="HP", float treshhold=1f,EffectResultType effectResultType=EffectResultType.ConstantValue)
        {
            this.property = property;
            this.treshhold = treshhold;
            this.mode = mode;
            ChangeType(m_TargetType);
            this.effectResultType = effectResultType;
        }

        public void ChangeType(EffectTargetType targetType)
        {
            m_TargetType = targetType;
            type = TypeTranslator.Translate(targetType);
        }

        public bool Satisfied(float num)
        {
            switch (mode)
            {
                case ConditionMode.If: return num == treshhold;
                case ConditionMode.IfNot: return !(num == treshhold);
                case ConditionMode.Less: return num < treshhold;
                case ConditionMode.LessEqual: return num <= treshhold;
                case ConditionMode.Equal: return  num == treshhold;
                case ConditionMode.NotEqual: return num != treshhold;
                case ConditionMode.GreaterEqual: return num >= treshhold;
                case ConditionMode.Greater: return num > treshhold;
            }

            return false;
        }

        public bool Satisfied(object obj)
        {
            PropertyInfo p = type.GetProperty(property);
            return Satisfied(p.GetValue(obj));
        }
    }
}
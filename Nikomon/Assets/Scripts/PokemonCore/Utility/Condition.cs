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
        public string property
        {
            get => m_property;
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                m_property = value;
                PropertyType = type.GetProperty(m_property).DeclaringType;
            }
        }

        private string m_property;

        /// <summary>
        /// 使条件成立的阈值,可能有很多种->string,int,float,bool
        /// </summary>
        public float treshholdFloat;
        public int treshholdInt;
        public string treshholdString;
        public bool treshholdBool;

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

        /// <summary>
        /// 这里的type指的是target的type，不是target的property的type
        /// </summary>
        public Type type;

        public Type PropertyType;


        public Condition(EffectTargetType targetType=EffectTargetType.PokemonSponsor,ConditionMode mode=ConditionMode.Equal, string property="HP",EffectResultType effectResultType=EffectResultType.ConstantValue)
        {
            this.property = property;
            
            this.mode = mode;
            TargetType = targetType;
            this.effectResultType = effectResultType;
            treshholdBool = false;
            treshholdFloat = 0;
            treshholdInt = 0;
            treshholdString = "";
        }

        public void ChangeType(EffectTargetType targetType)
        {
            m_TargetType = targetType;
            type = TypeTranslator.Translate(targetType);
        }
        
        
        public bool Satisfied(bool num)
        {
            switch (mode)
            {
                case ConditionMode.If: return treshholdBool == num;
                case ConditionMode.IfNot: return treshholdBool != num;
            }
            return false;
        }
        
        public bool Satisfied(string num)
        {
            switch (mode)
            {
                case ConditionMode.Equal: return treshholdString.Equals(num);
                case ConditionMode.NotEqual: return treshholdString.Equals(num);
            }
            return false;
        }

        public bool Satisfied(int num)
        {
            switch (mode)
            {
                case ConditionMode.Less: if(effectResultType==EffectResultType.ConstantValue) return num < treshholdInt;
                    break;
                case ConditionMode.LessEqual: return num <= treshholdInt;
                case ConditionMode.Equal: return  num == treshholdInt;
                case ConditionMode.NotEqual: return num != treshholdInt;
                case ConditionMode.GreaterEqual: return num >= treshholdInt;
                case ConditionMode.Greater: return num > treshholdInt;
            }
            return false;
        }
        
        public bool Satisfied(float num)
        {
            switch (mode)
            {
                case ConditionMode.Less: return num < treshholdFloat;
                case ConditionMode.LessEqual: return num <= treshholdFloat;
                case ConditionMode.Equal: return  num == treshholdFloat;
                case ConditionMode.NotEqual: return num != treshholdFloat;
                case ConditionMode.GreaterEqual: return num >= treshholdFloat;
                case ConditionMode.Greater: return num > treshholdFloat;
            }

            return false;
        }

        //TODO:没有支持到ResultType
        public bool Satisfied(IPropertyModify obj)
        {
            if(PropertyType==typeof(int))
                return Satisfied((int)obj[property]);
            else if (PropertyType == typeof(byte))
                return Satisfied((byte)obj[property]);
            else if (PropertyType== typeof(float))
                return Satisfied((float)obj[property]);
            else if(PropertyType==typeof(bool))
                return Satisfied((bool)obj[property]);
            else if (PropertyType == typeof(string))
                return Satisfied((string)obj[property]);

            throw new Exception("Other type have not been build");
        }
    }
}
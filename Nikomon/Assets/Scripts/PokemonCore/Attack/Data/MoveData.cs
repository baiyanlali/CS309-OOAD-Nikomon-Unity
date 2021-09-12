
using System;

namespace PokemonCore.Attack.Data
{
    //记录宝可梦的招式信息，暂不考虑加入华丽大赛内容
    [Serializable]
    public struct MoveData
    {
        public int MoveID { get; private set; }
        public int? Accuracy { get; private set; }
        public int? Power { get; private set; }
        public byte PP { get; private set; }
        public int Priority { get; private set; }
        public Flag Flags { get; private set; }
        public Targets Target { get; private set; }
        public Types Type { get; private set; }
        public int EffectID { get; private set; }
        public int? EffectChance { get; private set; }

        public string Name => this.ToString(TextScripts.Name);
        public string Description => this.ToString(TextScripts.Description);

        public MoveData(
            int moveID=0,
            int? accuracy=null,
            int? power=null,
            byte pp=0,
            int priority=0,
            Targets target = Targets.USER,
            Types? type=null,
            int effectID=0,
            int? effectChance=null
            )
        {
            this.MoveID = moveID;
            this.Accuracy = accuracy;
            this.Power = power;
            this.PP = pp;
            this.Priority = priority;
            this.Target = target;
            this.Type = type;
            this.EffectID = effectID;
            this.EffectChance = effectChance;
        }
        
        //TODO:FIX TO STRING FUNCTION
        public string ToString(TextScripts ts)
        {
            return "";
        }
    }
}
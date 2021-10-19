using System;
using System.Collections.Generic;

namespace PokemonCore.Attack.Data
{
    public enum Category
    {
        Status = 1,
        Physical = 2,
        Special = 3
    }

    //记录宝可梦的招式信息，暂不考虑加入华丽大赛内容
    [Serializable]
    public class MoveData
    {
        public int MoveID { get;  set; }
        public int? Accuracy { get;  set; }
        public int? Power { get;  set; }
        public byte PP { get;  set; }
        public int Priority { get;  set; }
        
        public int CriticalLevel { get;  set; }
        public Targets Target { get;  set; }
        public int Type { get;  set; }
        public int EffectID { get;  set; }
        public int? EffectChance { get;  set; }
        
        public string innerName { get;  set; }

        public Category Category { get; set; }

        public MoveData(
            int moveID = 0,
            string innerName="",
            int? accuracy = 100,
            int? power = 0,
            byte pp = 0,
            int priority = 0,
            Targets target = Targets.SELECTED_OPPONENT_POKEMON,
            int type = 0,
            int effectID = 0,
            int? effectChance = 0,
            int criticalLevel=0
        )
        {
            this.MoveID = moveID;
            this.innerName = innerName;
            this.Accuracy = accuracy;
            this.Power = power;
            this.PP = pp;
            this.Priority = priority;
            this.Target = target;
            this.Type = type;
            this.EffectID = effectID;
            this.EffectChance = effectChance;
            this.CriticalLevel = criticalLevel;
        }

        public override string ToString()
        {
            return $"Move Name: {innerName}\n" +
                   $"Type: {Game.TypesMap[Type].Name}\n" +
                   $"Category: {Category}\n" +
                   $"Power: {Power}\n" +
                   $"Accuracy: {Accuracy}";
        }
        
    }
}
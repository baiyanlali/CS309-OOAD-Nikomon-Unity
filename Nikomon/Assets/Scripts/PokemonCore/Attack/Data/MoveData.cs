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

        [Serializable]
        public class EffectInfo
        {
            public int EffectID;
            public int EffectChance;
            public Targets TargetType;
        }

        public int MoveID;
        public int Accuracy;
        public int Power;
        public byte PP ;
        public int Priority ;
        
        public int CriticalLevel ;
        public Targets Target ;
        public int Type ;
        public string description;
        
        // public int EffectID ;
        // public int? EffectChance ;

        public EffectInfo[] EffectInfos;
        
        public string innerName ;

        public Category Category;

        public bool MustHit;//必定命中

        public MoveData(
            int moveID = 0,
            string innerName="",
            int accuracy = 100,
            int power = 0,
            byte pp = 0,
            int priority = 0,
            Targets target = Targets.SELECTED_OPPONENT_POKEMON,
            int type = 0,
            // int effectID = 0,
            // int? effectChance = 0,
            EffectInfo[] effectInfos=null,
            int criticalLevel=0,
            bool mustHit=false,
            string description = ""
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
            // this.EffectID = effectID;
            // this.EffectChance = effectChance;
            this.EffectInfos = effectInfos;
            this.CriticalLevel = criticalLevel;
            this.MustHit = mustHit;
            this.description = description;
        }
        
        // public MoveData(
        //     int moveID = 0,
        //     string innerName="",
        //     int? accuracy = 100,
        //     int? power = 0,
        //     byte pp = 0,
        //     int priority = 0,
        //     Targets target = Targets.SELECTED_OPPONENT_POKEMON,
        //     int type = 0,
        //     int effectID = 0,
        //     int? effectChance = 0,
        //     // EffectInfo[] effectInfos=null,
        //     int criticalLevel=0
        //     // bool mustHit=false
        // )
        // {
        //     this.MoveID = moveID;
        //     this.innerName = innerName;
        //     this.Accuracy = accuracy;
        //     this.Power = power;
        //     this.PP = pp;
        //     this.Priority = priority;
        //     this.Target = target;
        //     this.Type = type;
        //     this.EffectInfos = new[]
        //     {
        //         new EffectInfo()
        //         {
        //             EffectChance = effectChance.HasValue ? effectChance.Value : -1,
        //             EffectID = effectID,
        //             TargetType = Targets.USER
        //         }
        //     };
        //     // this.EffectID = effectID;
        //     // this.EffectChance = effectChance;
        //     // this.EffectInfos = effectInfos;
        //     this.CriticalLevel = criticalLevel;
        //     this.MustHit = false;
        // }

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
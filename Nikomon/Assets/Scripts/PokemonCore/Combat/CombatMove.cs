using System.Collections.Generic;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat.Interface;

namespace PokemonCore.Combat
{
    /// <summary>
    /// 战斗使用的Move方法,为什么要把属性复制一遍->便于在战斗中修改
    /// </summary>
    public class CombatMove
    {
        public Battle battle;
        public Move move { get; private set; }
        public List<IEffect> TargetEffects { get; private set; }
        public List<IEffect> AttackerEffects { get; private set; }
        public List<IEffect> FieldEffects { get; private set; }
        public int? power { get; private set; }
        public Types types { get; private set; }
        public int TotalPP { get; private set; }
        public byte pp { get; private set; }
        public int? Accuracy { get; private set; }
        public int Priority { get; private set; }
        public CombatPokemon Sponsor { get; private set; }
        public List<CombatPokemon> Targets { get; private set; }

        

        public CombatMove(Move move,Battle battle,CombatPokemon sponsor,List<CombatPokemon> targets)
        {
            this.move = move;
            this.battle = battle;
            this.Sponsor = sponsor;
            sponsor.lastMove = move;
            this.Targets = targets;
            this.types = Game.TypesMap[move._baseData.Type];
            this.TotalPP = move.TotalPP;
            this.pp = move.PP;
            this.Accuracy = move._baseData.Accuracy;
            this.Priority = move._baseData.Priority;
            this.power = move._baseData.Power;
            battle.OnThisTurnEnd += () =>
            {
                move.PP = pp;
            };
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat.Interface;
using PokemonCore.Inventory;
using PokemonCore.Utility;

namespace PokemonCore.Combat
{

    public class CombatAction
    {
        public int Priority { get; set; }
        public CombatPokemon Sponsor { get; set; }
        public List<CombatPokemon> Targets { get; set; }
    }

    public class CombatItem:CombatAction
    {
        public Item item;
        public CombatItem(CombatPokemon sponsor,List<CombatPokemon> targets,Item item)
        {
            this.item = item;
            this.Sponsor = sponsor;
            this.Targets = targets;
        }
        public override string ToString()
        {
            return
                $"{item.name}:\n" +
                $"sponsor:{Sponsor.Name}\n" +
                $"target:{(from t in Targets select t.Name).ToList().ConverToString()}\n";
        }
        
    }
    
    /// <summary>
    /// 战斗使用的Move方法,为什么要把属性复制一遍->便于在战斗中修改
    /// </summary>
    public class CombatMove:CombatAction
    {
        public Battle battle;
        public Move move { get; private set; }
        // public List<Effect> Effects { get; private set; }

        public int? power { get; set; }
        public Types types { get; set; }
        public int TotalPP { get; set; }
        public byte pp { get; set; }
        public int? Accuracy { get; set; }

        public Category Category { get; set; }


        public CombatMove(Move move, Battle battle, CombatPokemon sponsor, List<CombatPokemon> targets)
        {
            this.move = move;
            this.battle = battle;
            this.Sponsor = sponsor;
            sponsor.lastMove = move;
            this.Targets = targets;
            this.types = Game.TypesMap[move._baseData.Type];
            this.TotalPP = move.TotalPP;
            this.pp = (byte) (move.PP - 1);
            this.Accuracy = move._baseData.Accuracy;
            this.Priority = move._baseData.Priority;
            this.power = move._baseData.Power;
            battle.OnThisTurnEnd += () => { move.PP = pp; };
            this.Category = move._baseData.Category;
            
        }

        public override string ToString()
        {
            return
                $"{move._baseData.innerName}:\n" +
                $"sponsor:{Sponsor.Name}\n" +
                $"target:{(from t in Targets select t.Name).ToList().ConverToString()}\n" +
                $"power:{power}\n";
        }
    }
}
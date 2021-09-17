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
        public Move move { get; private set; }
        public List<IEffect> effects { get; private set; }
        public int power { get; private set; }
        public Types types { get; private set; }
        public int TotalPP { get; private set; }
        public int pp { get; private set; }
        public int Accuracy { get; private set; }
        public int Priority { get; private set; }
        public Targets Targets { get; private set; }

        public int? Damage { get; private set; }
        

        public CombatMove(Move move)
        {
            this.move = move;
            
        }
        
    }
}
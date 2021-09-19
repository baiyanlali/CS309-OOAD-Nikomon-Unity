using System;
using PokemonCore.Utility;

namespace PokemonCore.Combat
{
    public class Damage
    {
        public CombatMove combatMove { get; private set; }
        private int damage { get; set; }
        public int finalDamage
        {
            get =>(int)Math.Floor(damage * damageMultiplyingPower);
        }
        
        float damageMultiplyingPower = 1;
        
        public CombatPokemon target;
        public CombatPokemon sponsor;
        public Damage(CombatMove combatMove,CombatPokemon sponsor,CombatPokemon target)
        {
            this.combatMove = combatMove;
            damage = 0;
            //damage的计算公式还没写出来，先随便代替一下
            if(combatMove.power!=null)
                damage = combatMove.power.Value;
            this.target = target;
            this.sponsor = sponsor;
            
            
            //TODO:我个人感觉不应该把属性相克的判断写在这里，到时候再说
            
            if (combatMove.types.CompareTypes(target.Type1)==TypeRelationship.NotEffective ||
                combatMove.types.CompareTypes(target.Type2)==TypeRelationship.NotEffective ||
                combatMove.types.CompareTypes(target.Type3)==TypeRelationship.NotEffective
            )
            {
                damageMultiplyingPower = 0;
            }
            else
            {
                if (combatMove.types.CompareTypes(target.Type1) == TypeRelationship.SuperEffective)
                {
                    damageMultiplyingPower *= 2;
                }else if (combatMove.types.CompareTypes(target.Type1) == TypeRelationship.NotVeryEffective)
                {
                    damageMultiplyingPower *= 0.5f;
                }
                
                if (combatMove.types.CompareTypes(target.Type2) == TypeRelationship.SuperEffective)
                {
                    damageMultiplyingPower *= 2;
                }else if (combatMove.types.CompareTypes(target.Type2) == TypeRelationship.NotVeryEffective)
                {
                    damageMultiplyingPower *= 0.5f;
                }
                
                if (combatMove.types.CompareTypes(target.Type3) == TypeRelationship.SuperEffective)
                {
                    damageMultiplyingPower *= 2;
                }else if (combatMove.types.CompareTypes(target.Type3) == TypeRelationship.NotVeryEffective)
                {
                    damageMultiplyingPower *= 0.5f;
                }
                
                
            }
        }

    }
}
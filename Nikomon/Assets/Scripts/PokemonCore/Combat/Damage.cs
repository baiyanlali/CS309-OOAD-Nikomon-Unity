using System;
using System.Reflection;
using PokemonCore.Combat.Interface;
using PokemonCore.Utility;
using UnityEngine;

namespace PokemonCore.Combat
{
    public class Damage
    {
        public CombatMove combatMove { get; private set; }

        private int damage { get; set; }

        //用来计算Critical Level的等级
        public int criticalHitLevel { get; set; }

        public int finalDamage
        {
            get => (int) Math.Floor(damage * typeRate * effectRate *criticalHitRate);
        }

        public float typeRate { get; private set; }
        public float effectRate { get; set; }
        public float criticalHitRate { get; set; }

        public bool isCritical;

        public CombatPokemon target;
        public CombatPokemon sponsor;

        public Damage(CombatMove combatMove, CombatPokemon sponsor, CombatPokemon target)
        {
            this.combatMove = combatMove;
            damage = (int) Math.Floor(Calculator.Damage(sponsor, target, combatMove));
            this.target = target;
            this.sponsor = sponsor;
            this.criticalHitLevel = combatMove.move._baseData.CriticalLevel;
            typeRate = 1;
            criticalHitRate = 1;
            effectRate = 1.5f;

            isCritical = Calculator.isCritical(this);

            if (combatMove.types.CompareTypes(target.Type1) == TypeRelationship.NotEffective ||
                combatMove.types.CompareTypes(target.Type2) == TypeRelationship.NotEffective ||
                combatMove.types.CompareTypes(target.Type3) == TypeRelationship.NotEffective
            )
            {
                typeRate = 0;
            }
            else
            {
                if (combatMove.types.CompareTypes(target.Type1) == TypeRelationship.SuperEffective)
                {
                    typeRate *= 2;
                }
                else if (combatMove.types.CompareTypes(target.Type1) == TypeRelationship.NotVeryEffective)
                {
                    typeRate *= 0.5f;
                }

                if (combatMove.types.CompareTypes(target.Type2) == TypeRelationship.SuperEffective)
                {
                    typeRate *= 2;
                }
                else if (combatMove.types.CompareTypes(target.Type2) == TypeRelationship.NotVeryEffective)
                {
                    typeRate *= 0.5f;
                }

                if (combatMove.types.CompareTypes(target.Type3) == TypeRelationship.SuperEffective)
                {
                    typeRate *= 2;
                }
                else if (combatMove.types.CompareTypes(target.Type3) == TypeRelationship.NotVeryEffective)
                {
                    typeRate *= 0.5f;
                }
            }
        }
    }
}
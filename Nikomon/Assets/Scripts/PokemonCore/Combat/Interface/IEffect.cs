

namespace PokemonCore.Combat.Interface
{
    public enum EffectLastType
    {
        ALWAYS,// last the effect forever
        ROUND,//last for a few rounds
        UNTIL_SWITCH_POKEMON//until switch to another pokemon
    }
    public interface IEffect
    {
        /// <summary>
        /// 在选取指令的时候使用
        /// </summary>
        /// <param name="battle"></param>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public Instruction OnChoosing(Battle battle, CombatPokemon pokemon);
        /// <summary>
        /// 在招式发挥效用之前使用
        /// </summary>
        /// <param name="battle"></param>
        /// <param name="move"></param>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public CombatMove OnMoving(Battle battle, CombatMove move, CombatPokemon attacker);
        /// <summary>
        /// 在招式即将攻击到对方的时候使用
        /// </summary>
        /// <param name="battle"></param>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public Damage OnHit(Battle battle, CombatPokemon attacker);
        /// <summary>
        /// 再招式即将攻击到对方的时候使用
        /// </summary>
        /// <param name="battle"></param>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public Damage BeHurt(Battle battle, CombatPokemon attacker,CombatPokemon defender);
        /// <summary>
        /// 在招式已经造成伤害后使用
        /// </summary>
        /// <param name="battle"></param>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public void OnDamaged(Battle battle, CombatPokemon attacker,CombatPokemon defender);


    }
}
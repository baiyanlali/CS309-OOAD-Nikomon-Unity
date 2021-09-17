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
        public CombatMove executeEffect(CombatMove cm);
    }
}
namespace PokemonCore.Combat
{
    public class Damage
    {
        public CombatMove combatMove { get; private set; }
        public int damage { get; private set; }
        public CombatPokemon target;

    }
}
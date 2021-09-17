namespace PokemonCore.Attack.Data
{
    //Should be able to handle multiple pokemon battle
    public enum Targets
    {
        USER,
        SPECIFIC_MOVE,
        SELECTED_OPPONENT_POKEMON,
        SELECTED_POKEMON,
        SELECTED_POKEMON_USER_FIRST,
        ALL_OTHER_POKEMON,
        ALLY,
        USER_OR_ALLY,
        USER_AND_ALLIES,
        RANDOM_OPPONENT,
        ALL_OPPONENTS,
        ENTIRE_FIELD,
        ALL_POKEMON
    }
}
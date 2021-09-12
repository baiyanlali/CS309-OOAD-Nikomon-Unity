using PokemonCore.Attack;
using PokemonCore.Monster;

public class Pokemon : IPokemon
{
    public int TotalHp { get; }
    public int HP { get; }
    public int ATK { get; }
    public int DEF { get; }
    public int SPA { get; }
    public int SPD { get; }
    public int SPE { get; }
    public int[] IV { get; }
    public byte[] EV { get; }
    public int Happiness { get; }
    public Move[] moves { get; }
    public string TrainerID { get; }
    public string Name { get; }
    public int ballUsed { get; }
    public int Exp { get; }
    public int StatusID { get; }
    public int SpeciesID { get; }
    public int Item { get; }
    public int ItemInitial { get; }
    public ObtainMethod ObtainMode { get; }
    public bool IsNicknamed { get; }
    public int Type1 { get; }
    public int Type2 { get; }
}

using System;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;

public class Pokemon : IPokemon, IEquatable<Pokemon>, IEqualityComparer<Pokemon>
{
    public int TotalHp => _base.BaseStatsHP == 1
        ? 1 + this.EV[0] / 4
        : (2 * _base.BaseStatsHP + this.IV[0] + this.EV[0] / 4) * this.Level / 100 + this.Level + 10;

    protected PokemonData _base => Game.PokemonsData[0];
    public int HP { get; }
    private int hp { get; set; }
    public int NatureID { get; private set; }

    public int ATK =>
        (int) Math.Floor(
            (double) ((2 * this._base.BaseStatsATK + this.IV[1] + (int) this.EV[1] / 4) * this.Level / 100 + 5) *
            (double) Game.NatureData[NatureID].ATK);

    public int DEF =>
        (int) Math.Floor(
            (double) ((2 * this._base.BaseStatsDEF + this.IV[2] + (int) this.EV[2] / 4) * this.Level / 100 + 5) *
            (double) Game.NatureData[NatureID].DEF);
    public int SPA =>
        (int) Math.Floor(
            (double) ((2 * this._base.BaseStatsSPA + this.IV[3] + (int) this.EV[3] / 4) * this.Level / 100 + 5) *
            (double) Game.NatureData[NatureID].SPA);
    public int SPD =>
        (int) Math.Floor(
            (double) ((2 * this._base.BaseStatsSPD + this.IV[4] + (int) this.EV[4] / 4) * this.Level / 100 + 5) *
            (double) Game.NatureData[NatureID].SPD);
    public int SPE =>
        (int) Math.Floor(
            (double) ((2 * this._base.BaseStatsSPE + this.IV[5] + (int) this.EV[5] / 4) * this.Level / 100 + 5) *
            (double) Game.NatureData[NatureID].SPE);
    public int[] IV { get; private set; }
    public byte[] EV { get; private set; }
    public int Happiness { get; private set; }
    public Move[] moves { get; private set; }
    public int TrainerID { get; }
    public string Name { get; }
    public int ballUsed { get; set; }
    public int Exp { get; }
    public int StatusID { get; }
    public int SpeciesID { get; }
    public int Item { get; }
    public int ItemInitial { get; }
    public ObtainMethod ObtainMode { get; private set; }
    public bool IsNicknamed { get; }
    public int Type1 { get; }
    public int Type2 { get; }
    public string ObtainMap { get; }
    public int ObtainLevel { get; }
    public int AbilityID { get; }

    public Experience Experience { get; private set; }

    
    
    private int _Exp
    {
        set
        {
            if (value < 0)
            {
                //TODO: Add debug info
            }
            else
            {
                // this.Experience = new Experience(, value);
            }
        }
    }
    public int Level
    {
        get => Experience.GetLevelFromExperience(this.GrothRate, this.Experience.Total);
    }

    public int GrothRate => this._base.GrowthRate;

    public bool Equals(Pokemon other)
    {
        throw new NotImplementedException();
    }

    public bool Equals(Pokemon x, Pokemon y)
    {
        throw new NotImplementedException();
    }

    public int GetHashCode(Pokemon obj)
    {
        throw new NotImplementedException();
    }
}
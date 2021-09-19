using System;
using System.Collections.Generic;
using System.Linq;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Combat;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;

public class Pokemon : IPokemon, IEquatable<Pokemon>, IEqualityComparer<Pokemon>
{
    public int TotalHp => _base.BaseStatsHP == 1
        ? 1 + this.EV[0] / 4
        : (2 * _base.BaseStatsHP + this.IV[0] + this.EV[0] / 4) * this.Level / 100 + this.Level + 10;

    public PokemonData _base { get; private set; }
    public int HP { get; set; }
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
    public int FriendShip { get; private set; }
    public Move[] moves { get; private set; }
    public int TrainerID { get; set; }
    public string Name { get; set; }
    public int ballUsed { get; set; }
    public int StatusID { get; }
    public int SpeciesID => _base.Species;
    public int Item { get; set; }
    public int ItemInitial { get; set; }
    public ObtainMethod ObtainMode { get; private set; }
    public bool IsNicknamed { get; set; }
    public int? Type1 { get; }
    public int? Type2 { get; }
    public string ObtainMap { get; set; }
    public int ObtainLevel { get; set; }
    public int AbilityID { get; set; }

    public Experience Exp { get; private set; }
    
    public int Level
    {
        get => Exp.level;
    }

    public int GrowthRate => this._base.GrowthRate;

    public Pokemon(PokemonData pd,string nickName="",Trainer trainer=null,int initLevel=1,int ballUsed=0,string obtainMap="NoWhere",int statusID=0)
    {
        _base = pd;
        
        Exp = new Experience(_base.GrowthRate,Game.ExperienceTable[_base.GrowthRate][initLevel-1]);
        
        Type1 = pd.type1;
        Type2 = pd.type2;
        IV = new int[6];
        EV = new byte[6];
        HP = TotalHp;

        this.TrainerID = trainer.id;

        FriendShip = _base.BaseFriendship;

        if (!String.IsNullOrEmpty(nickName))
        {
            
            this.Name = nickName;
            this.IsNicknamed = false;
        }
        else
            this.Name = _base.innerName;
        this.ballUsed = ballUsed;
        this.ObtainMap = obtainMap;


        //初始化技能
        moves = new Move[Game.MaxMovesPerPokemon];
        var moveOrdered= _base.LevelMoves.OrderBy(pair => -pair.Key).Where(pair => pair.Key<Level);
        int currentMove = 0;
        foreach (var v in moveOrdered)
        {
            if (currentMove >= moves.Length) break;
            if (v.Key > Level)
            {
                continue;
            }
            else
            {
                foreach (var moveID in v.Value)
                {
                    if (currentMove >= moves.Length) break;
                    moves[currentMove] = new Move(Game.MovesData[moveID]);
                    currentMove++;
                }
            }
        }

        this.StatusID = statusID;

        //TODO:Change it to a normal value
        this.AbilityID = _base.Ability1;
    }

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
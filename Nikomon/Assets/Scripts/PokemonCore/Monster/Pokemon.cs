using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;

[JsonObject(MemberSerialization.OptOut)]
public class Pokemon : IPokemon, IEquatable<Pokemon>, IEqualityComparer<Pokemon>
{
    
    
    [JsonIgnore]
    public PokemonData _base { get; private set; }
    
    public int ID
    {
        get => _base.ID;
        set
        {
            _base = Game.PokemonsData[value];
        }
    }

    [JsonIgnore]
    public int TotalHp => _base.BaseStatsHP == 1
        ? 1 + this.EV[0] / 4
        : (2 * _base.BaseStatsHP + this.IV[0] + this.EV[0] / 4) * this.Level / 100 + this.Level + 10;
    public int HP { get; set; }
    public int NatureID { get; private set; }
    [JsonIgnore]
    public int ATK =>
        (int) Math.Floor(
            (double) ((2 * this._base.BaseStatsATK + this.IV[1] + (int) this.EV[1] / 4) * this.Level / 100 + 5) *
            (double) Game.NatureData[NatureID].ATK);
    [JsonIgnore]
    public int DEF =>
        (int) Math.Floor(
            (double) ((2 * this._base.BaseStatsDEF + this.IV[2] + (int) this.EV[2] / 4) * this.Level / 100 + 5) *
            (double) Game.NatureData[NatureID].DEF);
    [JsonIgnore]
    public int SPA =>
        (int) Math.Floor(
            (double) ((2 * this._base.BaseStatsSPA + this.IV[3] + (int) this.EV[3] / 4) * this.Level / 100 + 5) *
            (double) Game.NatureData[NatureID].SPA);
    [JsonIgnore]
    public int SPD =>
        (int) Math.Floor(
            (double) ((2 * this._base.BaseStatsSPD + this.IV[4] + (int) this.EV[4] / 4) * this.Level / 100 + 5) *
            (double) Game.NatureData[NatureID].SPD);
    [JsonIgnore]
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
    [JsonIgnore]
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
    
    public bool isMale { get; private set; }
    
    [JsonIgnore]
    public int Level
    {
        get => Exp.level;
    }

    [JsonIgnore]
    public int GrowthRate => this._base.GrowthRate;
    [JsonConstructor]
    public Pokemon(
        int id,
        int HP,
        int NatureID,
        int[] IV,
        byte[] EV,
        int FriendShip,
        Move[] moves,
        int TrainerID,
        string name,
        int ballUsed,
        int statusID,
        int Item,
        int itemInitial,
        ObtainMethod obtainMode,
        bool isNicknamed,
        int? Type1,
        int? Type2,
        string obtainMap,
        int obtainLevel,
        int abilityID,
        Experience exp,
        bool isMale
    )
    {
        this.ID=id;
        this.HP = HP;
        this.NatureID = NatureID;
        this.IV = IV;
        this.EV = EV;
        this.FriendShip = FriendShip;
        this.moves = moves;
        this.TrainerID = TrainerID;
        this.Name = name;
        this.ballUsed = ballUsed;
        this.StatusID = statusID;
        this.Item = Item;
        this.ItemInitial = itemInitial;
        this.ObtainMode = obtainMode;
        this.IsNicknamed = isNicknamed;

        this.Type1 = Type1;
        this.Type2 = Type2;
        this.ObtainMap = obtainMap;
        this.ObtainLevel = obtainLevel;
        this.AbilityID = abilityID;

        Exp = exp;

        this.isMale = isMale;
    }

    public Pokemon(int id, int initLevel, int trainerID)
    {
        this.ID = id;
        
        Exp = new Experience(_base.GrowthRate,Game.ExperienceTable[_base.GrowthRate][initLevel-1],true);
        
        Type1 = _base.type1;
        Type2 = _base.type2;
        IV = new int[6];
        EV = new byte[6];
        HP = TotalHp;
        
        FriendShip = _base.BaseFriendship;
        this.Name = _base.innerName;
        
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
        
        this.AbilityID = _base.Ability1;
        
        this.TrainerID = trainerID;
    }


    public Pokemon(int id,int initLevel)
    {
        this.ID = id;
        
        Exp = new Experience(_base.GrowthRate,Game.ExperienceTable[_base.GrowthRate][initLevel-1],true);
        
        Type1 = _base.type1;
        Type2 = _base.type2;
        IV = new int[6];
        EV = new byte[6];
        HP = TotalHp;
        
        FriendShip = _base.BaseFriendship;
        this.Name = _base.innerName;
        
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
        
        this.AbilityID = _base.Ability1;
    }
    
    
    public Pokemon(PokemonData pd,string nickName="",Trainer trainer=null,int initLevel=1,int ballUsed=0,string obtainMap="NoWhere",int statusID=0)
    {
        _base = pd;
        
        Exp = new Experience(_base.GrowthRate,Game.ExperienceTable[_base.GrowthRate][initLevel-1],true);
        
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


    public void AddMove(int id)
    {
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i] != null) continue;
            moves[i] = new Move(Game.MovesData[id]);
            break;
        }
    }
    
    public void AddMove(MoveData moveData)
    {
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i] != null) continue;
            moves[i] = new Move(moveData);
            break;
        }
    }

    public void ReplaceMove(int index, MoveData moveData)
    {
        moves[index] = new Move(moveData);
    }

    public void CheckEvolution()
    {
        
    }
    
    public bool Equals(Pokemon other)
    {
        return
            this.ID == other.ID &&
            this.HP == other.HP &&
            this.NatureID == other.NatureID &&
            this.IV == other.IV &&
            this.EV == other.EV &&
            this.FriendShip == other.FriendShip &&
            this.moves == other.moves &&
            this.TrainerID == other.TrainerID &&
            this.Name == other.Name &&
            this.ballUsed == other.ballUsed &&
            this.StatusID == other.StatusID &&
            this.Item == other.Item &&
            this.ItemInitial == other.ItemInitial &&
            this.ObtainMode == other.ObtainMode &&
            this.IsNicknamed == other.IsNicknamed &&
            this.Type1 == other.Type1 &&
            this.Type2 == other.Type2 &&
            this.ObtainMap == other.ObtainMap &&
            this.ObtainLevel == other.ObtainLevel &&
            this.AbilityID == other.AbilityID &&
            Exp == other.Exp &&
            this.isMale == other.isMale;
    }

    public bool Equals(Pokemon x, Pokemon y)
    {
        return x?.Equals(y) ?? false;
    }

    public int GetHashCode(Pokemon obj)
    {
       return base.GetHashCode();
    }
}

using PokemonCore.Attack;

namespace PokemonCore.Monster
{
    public enum ObtainMethod
    {
        SNAGGED=-1,
        MET=0,
        EGG=1,
        TRADED=2,
        FATEFUL_ENCOUNTER=4,//命中注定的相遇！
    }

    public enum PokemonActions
    {
        Battle,
        CallTo,
        Party,
        DayCare,
    }
    //不搞什么宝可梦病毒了，暂时也不搞能力、勋章什么的
    public interface IPokemon
    {
        #region BasicStatus
        int TotalHp { get; }
        int HP { get; }//血量
        int ATK { get; }//攻击
        int DEF { get; }//防御
        int SPA { get; }//特攻
        int SPD { get; }//速度
        int SPE { get; }//特防
        int[] IV { get; }//个体值
        byte[] EV { get; }//努力值
        #endregion
        int FriendShip { get; }
        Move[] moves { get; }
        int TrainerID { get; }
        string Name { get; }
        int ballUsed { get; }
        string StatusID { get; }
        int SpeciesID { get; }
        int Item { get; }
        int ItemInitial { get; }
        ObtainMethod ObtainMode { get; }
        bool IsNicknamed { get; }
        int? Type1 { get; }
        int? Type2 { get; }
        

        string ObtainMap { get; }
        int ObtainLevel { get; }
        
        int AbilityID { get; }
    }
}
using System;
using PokemonCore.Attack.Data;

namespace PokemonCore.Attack
{
    [Serializable]
    //This class used to describe 技能
    public class Move
    {
        public MoveData _baseData { get;private set; }
        private byte pp { get; set; }

        public byte PP
        {
            get => this.pp;
            set => this.pp = value < (byte) 0 ? (byte) 0 : value > TotalPP ? this.TotalPP : value;
        }

        public byte TotalPP
        {
            get { return (byte) (_baseData.PP + PPups); }
        }

        public int PPups { get; set; }

        public Move(MoveData data)
        {
            this._baseData = data;
            PPups = 0;
        }
        
    }
}
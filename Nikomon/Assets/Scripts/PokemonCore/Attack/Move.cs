using System;
using Newtonsoft.Json;
using PokemonCore.Attack.Data;

namespace PokemonCore.Attack
{
    [Serializable]
    //This class used to describe 技能
    public class Move
    {
        [JsonIgnore]
        public MoveData _baseData { get;private set; }

        public int moveID;
        [JsonProperty]
        private byte pp { get; set; }

        [JsonIgnore]
        public byte PP
        {
            get => this.pp;
            set => this.pp = value < (byte) 0 ? (byte) 0 : value > (byte)(_baseData.PP+PPups) ? (byte)(_baseData.PP+PPups) : value;
        }

        [JsonIgnore]
        public byte TotalPP
        {
            get { return (byte) (_baseData.PP + PPups); }
        }

        public int PPups { get; set; }

        public Move(MoveData data)
        {
            this._baseData = data;
            this.moveID = data.MoveID;
            PPups = 0;
            pp = TotalPP;
        }

        [JsonConstructor]
        public Move(int moveID, byte pp, byte PPups)
        {
            this.moveID = moveID;
            this._baseData = Game.MovesData[moveID];
            this.pp = pp;
            this.PPups = PPups;
        }
        
    }
}
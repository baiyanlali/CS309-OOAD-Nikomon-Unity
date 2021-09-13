using System;
using System.Collections.Generic;

namespace PokemonCore.Monster.Data
{
    [Serializable]
    public struct PokemonData:IEquatable<PokemonData>,IEqualityComparer<PokemonData>
    {
        private int? type1;

        private int? type2;
        //TODO: Add Abilities

        public int ID { get; private set; }
        public int EvoChainID { get; private set; }
        public int? EvolveFrom { get; private set; }
        public int Order { get; private set; }
        public int?[] Type => new int?[2] {type1, type2};

        public int CatchRate { get; private set; }
        public float Height { get; private set; }
        public float Weight { get; private set; }

        public int BaseFriendship { get; private set; }

        public int BaseExpYield { get; private set; }

        public int BaseStatsHP { get; private set; }

        public int BaseStatsATK { get; private set; }

        public int BaseStatsDEF { get; private set; }

        public int BaseStatsSPA { get; private set; }

        public int BaseStatsSPD { get; private set; }

        public int BaseStatsSPE { get; private set; }

        public int evYieldHP { get; private set; }

        public int evYieldATK { get; private set; }

        public int evYieldDEF { get; private set; }

        public int evYieldSPA { get; private set; }

        public int evYieldSPD { get; private set; }

        public int evYieldSPE { get; private set; }
        
        public int GrowthRate { get; private set; }

        public int[] EVYield => new int[6]
        {
            evYieldHP, evYieldATK, evYieldDEF, evYieldSPA,
            evYieldSPD, evYieldSPE
        };

        public PokemonData(
            int ID = -1,
            int? type1 = null,
            int? type2 = null,
            int catchRate = 50,
            float height = 0f,
            float weight = 0f,
            int baseFriendship = 0,
            int baseExpYield = 0,
            int baseStatsHP = 0,
            int baseStatsATK = 0,
            int baseStatsDEF = 0,
            int baseStatsSPA = 0,
            int baseStatsSPD = 0,
            int baseStatsSPE = 0,
            int evYieldHP = 0,
            int evYieldATK = 0,
            int evYieldDEF = 0,
            int evYieldSPA = 0,
            int evYieldSPD = 0,
            int evYieldSPE = 0,
            int evoChainID = 0,
            int? EvolveFrom = null,
            int order = -1,
            int growthRate=0
        )
        {
            this.ID = ID;
            this.type1 = type1;
            this.type2 = type2;
            this.EvoChainID = evoChainID;
            this.EvolveFrom = EvolveFrom;
            this.Order = order;
            this.CatchRate = catchRate;
            this.Height = height;
            this.Weight = weight;
            BaseFriendship = baseFriendship;
            BaseExpYield = baseExpYield;
            BaseStatsHP = baseStatsHP;
            BaseStatsATK = baseStatsATK;
            BaseStatsDEF = baseStatsDEF;
            BaseStatsSPA = baseStatsSPA;
            BaseStatsSPD = baseStatsSPD;
            BaseStatsSPE = baseStatsSPE;

            this.evYieldHP = evYieldHP;
            this.evYieldATK = evYieldATK;
            this.evYieldDEF = evYieldDEF;
            this.evYieldSPA = evYieldSPA;
            this.evYieldSPD = evYieldSPD;
            this.evYieldSPE = evYieldSPE;
            this.GrowthRate = growthRate;
        }

        public bool Equals(PokemonData other)
        {
            return this.ID == other.ID;
        }

        public bool Equals(PokemonData x, PokemonData y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(PokemonData obj)
        {
            return this.ID;
        }
    }
}
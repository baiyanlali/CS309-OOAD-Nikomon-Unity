using System;
using System.Collections.Generic;

namespace PokemonCore.Monster.Data
{

    public enum Gender
    {
        Male,
        Female,
        Gerderless
    }

    public enum EvolutionMethod
    {
        
    }
    [Serializable]
    public class PokemonData:IEquatable<PokemonData>,IEqualityComparer<PokemonData>
    {
        public string innerName { get; set; }
        
        public int? type1;

        public int? type2;
         
         public Gender Gender { get; set; }
        public int Ability1 { get;  set; }
        public int Ability2 { get;  set; }
        public int AbilityHidden { get;  set; }
        public int ID { get;  set; }
        public int EvoChainID { get;  set; }
        public int?[] Type => new int?[2] {type1, type2};

        public int CatchRate { get;  set; }
        public float Height { get;  set; }
        public float Weight { get;  set; }
        
        public int BaseFriendship { get;  set; }
        
        public Dictionary<int,List<int>> LevelMoves { get; set; }
        /// <summary>
        /// the percent of male in this kind of pokemon
        /// </summary>
        public int MaleRatio { get; set; }
        

        #region Stats

        public int BaseExpYield { get;  set; }

        public int BaseStatsHP { get;  set; }

        public int BaseStatsATK { get;  set; }

        public int BaseStatsDEF { get;  set; }

        public int BaseStatsSPA { get;  set; }

        public int BaseStatsSPD { get;  set; }

        public int BaseStatsSPE { get;  set; }

        public int evYieldHP { get;  set; }

        public int evYieldATK { get;  set; }

        public int evYieldDEF { get;  set; }

        public int evYieldSPA { get;  set; }

        public int evYieldSPD { get;  set; }

        public int evYieldSPE { get;  set; }
        #endregion

        
        public int GrowthRate { get;  set; }
        
        public int Species { get; set; }
        
        public EvolutionMethod EvolutionMethod { get; set; }

        public int[] EVYield => new int[6]
        {
            evYieldHP, evYieldATK, evYieldDEF, evYieldSPA,
            evYieldSPD, evYieldSPE
        };

        public PokemonData(
            int ID = -1,
            int? type1 = -1,
            int? type2 = -1,
            int catchRate = 50,
            int ability1=0,
            int ability2=0,
            int abilityHidden=0,
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
            int growthRate=0
        )
        {
            this.ID = ID;
            this.type1 = type1;
            this.type2 = type2;
            this.EvoChainID = evoChainID;
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

            this.Ability1 = ability1;
            this.Ability2 = ability2;
            this.AbilityHidden = abilityHidden;

            Gender = Gender.Gerderless;

            LevelMoves = new Dictionary<int, List<int>>();
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
        
        private string Type1Name
        {
            get => type1.HasValue ? Game.TypesMap[type1.Value].Name : "";
        }
        private string Type2Name
        {
            get => type2.HasValue ? Game.TypesMap[type2.Value].Name : "";
        }

        public override string ToString()
        {
            return $"Pokemon ID: {ID}\n" +
                   $"Pokemon inner Name: {innerName}\n" +
                   $"Poekmon Type {Type1Name},{Type2Name}";
        }
        
        
        
    }
}
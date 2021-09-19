
using System;
using PokemonCore.Saving;

namespace PokemonCore.Monster.Data
{
    public class Experience
    {
        public int Total { get; private set; }
        public int Current => this.Total - Experience.GetExperience(levelingRate, level);

        public byte level => Experience.GetLevelFromExperience(levelingRate, Total);

        public int NextLevelExp => Experience.GetExperience(levelingRate, level + 1);

        public int PointsNeeded => NextLevelExp - Total;
        
        public int levelingRate;

        public void AddExperience(int experienceGain)
        {
            this.Total += experienceGain;
            int experience = Experience.GetExperience(levelingRate, 100);
            if (Total <= experience)
                return;
            this.Total = experience;

        }

        public int ExperienceNeeded(int toLv) => Math.Max(0, Experience.GetExperience(levelingRate, toLv) - Total);
        private static int GetExperience(int levelingRate, int currentLevel)
        {
            int exp = 0;
            if (currentLevel <= 100)
                exp = Game.ExperienceTable[levelingRate][currentLevel - 1];
            else exp = Game.ExperienceTable[levelingRate][99];
            return exp;
        }

        public static int GetMaxExperience(int levelRate) => GetExperience(levelRate, 100);

        public static byte GetLevelFromExperience(int levelingRate, int exp)
        {
            if (exp <= 0) return 1;
            if(exp> (GetMaxExperience(levelingRate)))return 100;
            for (int i = 1; i <= 100; i++)
            {
                if (exp < GetExperience(levelingRate, i))
                    return (byte) (i - 1);
            }

            return 0;
        }

        public Experience(int rate,int initialValue=0)
        {
            this.levelingRate = rate;
            Total = initialValue;
        }
        

    }
}
using System;

namespace PokemonCore.Monster
{
    public enum BasicState
    {
        ATK,
        DEF,
        SPA,
        SPD,
        SPE,
    }

    [Serializable]
    public struct Nature
    {
        public int id { get; private set; }
        public float ATK { get; private set; }
        public float DEF { get; private set; }
        public float SPA { get; private set; }
        public float SPD { get; private set; }
        public float SPE { get; private set; }

        public Nature(int id, float[] changes)
        {
            this.id = id;
            ATK = 1.0f + changes[0];
            DEF = 1.0f + changes[1];
            SPA = 1.0f + changes[2];
            SPD = 1.0f + changes[3];
            SPE = 1.0f + changes[4];
        }
    }
}
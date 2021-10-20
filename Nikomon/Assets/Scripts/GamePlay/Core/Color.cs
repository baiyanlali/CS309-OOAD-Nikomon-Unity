using Newtonsoft.Json;
using UnityEngine;

namespace GamePlay.Core
{
    public struct PokeColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public PokeColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color toColor()
        {
            return new Color(r, g, b, a);
        }

        public static PokeColor toPokeColor(Color color)
        {
            return new PokeColor(color.r, color.g, color.b, color.a);
        }
        
        
    }
}
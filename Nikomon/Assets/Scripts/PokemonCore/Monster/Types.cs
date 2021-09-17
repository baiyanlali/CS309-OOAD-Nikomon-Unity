using System;
using System.Collections.Generic;

namespace PokemonCore
{
    public enum TypeRelationship
    {
        Effective,
        SuperEffective,
        NotVeryEffective,
        NotEffective
    }
    [Serializable]
    public class Types
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public HashSet<int> SEType { get; private set; }//super effective type
        public HashSet<int> NVEType { get; private set; }//not very effective type
        public HashSet<int> NEType { get; private set; }//not effective type
        
        public Types(int id, string name="")
        {
            this.ID = id;
            this.Name = name;
            SEType = new HashSet<int>();
            NVEType = new HashSet<int>();
            NEType = new HashSet<int>();
        }

        public string ToString(TextScripts ts)
        {
            return base.ToString();
        }
    }
}
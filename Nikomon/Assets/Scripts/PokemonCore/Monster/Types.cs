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
        public string Description { get; private set; }
        public HashSet<Types> SEType { get; private set; }//super effective type
        public HashSet<Types> NVEType { get; private set; }//not very effective type
        public HashSet<Types> NEType { get; private set; }//not effective type
        
        public Types(int id, string name, string description)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
        }

        public string ToString(TextScripts ts)
        {
            return base.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using PokemonCore.Utility;

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

        public override string ToString()
        {
            List<string> se_names = new List<string>();
            foreach (var se in SEType)
            {
                se_names.Add(Game.TypesMap[se].ToString(TextScripts.Name));
            }

            List<string> nve_names = new List<string>();
            foreach (var nev in NVEType)
            {
                nve_names.Add(Game.TypesMap[nev].ToString(TextScripts.Name));
            }
            List<string> ne_names = new List<string>();
            foreach (var ne in NVEType)
            {
                ne_names.Add(Game.TypesMap[ne].ToString(TextScripts.Name));
            }

            
            
            return $"\nTypes ID:{ID}\nName:{Name}\n" +
                   $"Super Effect Type ID: {se_names.ConverToString()}\n" +
                   $"Not Very Effective Type ID: {nve_names.ConverToString()}\n" +
                   $"No Effect Type ID: {ne_names.ConverToString()}";
        }

        public string ToString(TextScripts ts)
        {
            switch (ts)
            {
                case TextScripts.Name:
                    return Name;
                case TextScripts.Description:
                    return this.ToString();
            }

            return "";
        }

        
    }
}
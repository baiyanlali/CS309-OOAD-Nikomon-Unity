using System;
using UnityEngine;

public abstract class PartyElement:MonoBehaviour
    {
        public virtual void Init(Pokemon pokemon, int index, string[] dialogChoose, Action<int,int> actions)
    {
        
    }

    public virtual void UpdateData(Pokemon pokemon)
    {
        
    }
}

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PC",menuName = "Inventory/New PC")]
public class PCInventory : ScriptableObject
{
    public List<PCItem> itemList = new List<PCItem>();
    public List<Pokemon> pokemonList = new List<Pokemon>();


}

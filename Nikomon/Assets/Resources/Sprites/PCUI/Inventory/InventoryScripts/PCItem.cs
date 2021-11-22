using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlay;

[CreateAssetMenu(fileName = "New PCItem", menuName = "Inventory/New PCItem")]
public class PCItem : ScriptableObject
    //PCitem就是相当于一个宝可梦！
{
    public string itemName;//精灵的名字
    public Sprite itemImage;//精灵的图片
    public int itemNumber;//应该没什么用,记录这是第几个???
    public string itemInform;//信息介绍
    public Pokemon pokemon;
    public void Init(Pokemon poke)
    {
        itemName = poke.IsNicknamed ? poke.Name : poke.Name;
        itemImage = GameResources.PokemonIcons[poke.ID];
        itemNumber = poke.ID;
        pokemon = poke;


    }

}

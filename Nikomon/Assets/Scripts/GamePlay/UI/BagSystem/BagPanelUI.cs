using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.Messages;
using GamePlay.UI.UIFramework;
using PokemonCore.Inventory;
using PokemonCore.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BagPanelUI : BaseUI
{

    public Text TableName;
    public TabSystem BagTable;

    private Dictionary<TabElement, GameObject> elements;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">0 for TrainerBag</param>
    public override void Init(params object[] args)
    {
        base.Init(args);
        TableName = GET(TableName, "TableName/Text");
        BagTable = GET(BagTable, nameof(BagTable));

        var bagContentElement = GameResources.SpawnPrefab(typeof(BagContentElementUI));
        //这里没办法用Spawn Prefab，因为它的类型不独特，就这样子生成吧
        // GameObject table = transform.Find("Tables/Table").gameObject;
        // GameObject bagContents = transform.Find("TableContent/BagContents").gameObject;
        GameObject table = GameResources.SpawnPrefab("Table");
        GameObject bagContents = GameResources.SpawnPrefab("BagContents");
        string[] strs = Enum.GetNames(typeof(Item.Tag));
        elements = new Dictionary<TabElement, GameObject>();
        TrainerBag bag=args[0] as TrainerBag;


        GameObject lastTable=null;//用于动态绑定navigation
        for (int i = 0; i < strs.Length; i++)
        {
            GameObject tab = Instantiate(table);
            if (lastTable == null)
            {
                FirstSelectable = tab;
                tab.GetComponent<Selectable>().navigation = new Navigation()
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnDown = null,
                    selectOnLeft = null,
                    selectOnRight = null,
                    selectOnUp = null,
                    wrapAround = false
                };
            }
            else
            {
                tab.GetComponent<Selectable>().navigation = new Navigation()
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnLeft = lastTable.GetComponent<Selectable>()
                };

                Navigation navi=lastTable.GetComponent<Selectable>().navigation;
                navi.selectOnRight = tab.GetComponent<Selectable>();
                lastTable.GetComponent<Selectable>().navigation = navi;
            }

            lastTable = tab;
            tab.name = strs[i];
            tab.transform.Find("Image").GetComponent<Image>().sprite = GameResources.BagIcons[(Item.Tag) Enum.Parse(typeof(Item.Tag),strs[i])];
            // tab.GetComponent<Image>().color=Color.black;
            GameObject contents = Instantiate(bagContents);
            contents.name = strs[i];
            TabElement ele = tab.GetComponent<TabElement>();
            elements.Add(ele,contents);
            var items = bag[(Item.Tag) Enum.Parse(typeof(Item.Tag), strs[i])];
            var tabContents = contents.GetComponent<TabContent>();

            GameObject lastItem = null;
            foreach (var item in items.OrEmptyIfNull())
            {
                if (item == null) break;
                var o = Instantiate(bagContentElement,contents.transform);
                if (lastItem == null)
                {
                    tabContents.FirstSelectable = o;
                    o.GetComponent<Selectable>().navigation = new Navigation()
                    {
                        mode = Navigation.Mode.Explicit,
                        selectOnDown = null,
                        selectOnLeft = null,
                        selectOnRight = null,
                        selectOnUp = null,
                        wrapAround = false
                    };
                }
                else
                {
                    o.GetComponent<Selectable>().navigation = new Navigation()
                    {
                        mode = Navigation.Mode.Explicit,
                        selectOnUp = lastItem.GetComponent<Selectable>()
                    };

                    Navigation navi=lastItem.GetComponent<Selectable>().navigation;
                    navi.selectOnDown = o.GetComponent<Selectable>();
                    lastItem.GetComponent<Selectable>().navigation = navi;
                }
                o.GetComponent<BagContentElementUI>().Init(item,bag[item]);
                
                lastItem = o;

            }

        }
        
        
        BagTable.Init(elements);
        BagTable.OnChoosed += (o) =>
        {
            TableName.text = Messages.Get(elements[o].name);
        };
        
    }

}

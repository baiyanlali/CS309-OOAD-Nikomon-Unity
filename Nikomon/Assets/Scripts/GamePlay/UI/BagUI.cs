using System;
using System.Collections.Generic;
using PokemonCore.Inventory;
using PokemonCore.Utility;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TabSystem))]
public class BagUI : MonoBehaviour
{
    private readonly string path = @"Prefabs/UI/BagSystem/";
    private Dictionary<TabElement, GameObject> elements;
    private Text TableName;
    private TabSystem _tabSystem;

    public static BagUI Instance
    {
        get
        {
            if (sInstance != null) return sInstance;
            sInstance = FindObjectOfType<BagUI>();
            if (sInstance != null) return sInstance;
            // CreateBagUI();
            return sInstance;
        }
    }

    private static BagUI sInstance;

    private static void CreateBagUI()
    {
        var obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/BagSystem/BagTable"), GameObject.Find("Canvas").transform);
        RectTransform rect = (RectTransform) obj.transform;
        // rect.pivot = new Vector2(1, 0.5f);
        sInstance = obj.GetComponent<BagUI>();
    }
    
    private void Awake()
    {
        sInstance = this;
    }


    private void Start()
    {
        sInstance = this;
    }

    public void Init(TrainerBag bag)
    {
        TableName = TableName ? TableName : transform.Find("TableName/Text").GetComponent<Text>();
        // print("Bag Init");
        _tabSystem = GetComponent<TabSystem>();
        string[] strs = Enum.GetNames(typeof(Item.Tag));
        var table = Resources.Load<GameObject>(path + "Table");
        var bagContents = Resources.Load<GameObject>(path + "BagContents");
        elements=new Dictionary<TabElement, GameObject>();
        for (int i = 0; i < strs.Length; i++)
        {
            
            GameObject tab = Instantiate(table);
            tab.name = strs[i];
            tab.GetComponent<Image>().sprite = GlobalManager.Instance.BagIcons[(Item.Tag) Enum.Parse(typeof(Item.Tag),strs[i])];
            tab.GetComponent<Image>().color=Color.black;
            GameObject contents = Instantiate(bagContents);
            contents.name = strs[i];
            TabElement ele = tab.GetComponent<TabElement>();
            elements.Add(ele,contents);
            var items = bag[(Item.Tag) Enum.Parse(typeof(Item.Tag), strs[i])];
            foreach (var item in items.OrEmptyIfNull())
            {
                if (item == null) break;
                var o = Instantiate(Resources.Load<GameObject>(path + "BagContentElement"),contents.transform);
                o.GetComponent<BagContentElementUI>().Init(item,bag[item]);
            }
        }
        
        _tabSystem.Init(elements);
        _tabSystem.OnChoosed += (o) =>
        {
            TableName.text = elements[o].name;
        };
        
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
}
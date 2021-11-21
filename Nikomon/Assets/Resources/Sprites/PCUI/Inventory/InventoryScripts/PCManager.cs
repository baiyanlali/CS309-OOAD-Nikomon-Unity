using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamePlay;

public class PCManager : MonoBehaviour
{

    static PCManager instance;

    public PCInventory myPC;
    public GameObject imageGrid;//PC里面的格子。
    public GameObject information;
    public Text Name,HP, ATK, DEF, SPA, SPD, SPE;
    //public PCImage imagePrefab;
    //public Text information;//介绍宝可梦的信息。(先不要)

    public List<GameObject> slots = new List<GameObject>();
    public GameObject emptySlot;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    private void OnEnable()
    {
        Refresh();
    }


    public static void Refresh()
    {
        //for(int i = 0; i < instance.myPC.pokemonList.Count; i++)
        //先清空一下instance的slots,不知道作用大不大？
        instance.slots.Clear();
        Debug.Log(instance.imageGrid.transform.childCount);
        for (int i = 0; i < instance.myPC.itemList.Count; i++)
        {
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.imageGrid.transform);
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.myPC.itemList[i],i);
        }
    }

    public static void refreshMenu()
    {
        for (int i = 0; i < instance.myPC.itemList.Count; i++)
        {
            instance.slots[i].GetComponent<Slot>().menu.SetActive(false);
            instance.slots[i].GetComponent<Slot>().judge = 0;

        }
    }
    public static void refreshInformation(int number)
    {
        if (instance.slots[number].GetComponent<Slot>().pcItem.pokemon == null)
        {
            instance.Name.text = instance.slots[number].GetComponent<Slot>().number.ToString();
            instance.HP.text = "bbb";
            instance.ATK.text = "ccc";
            instance.DEF.text = "ddd";
            instance.SPA.text = "eee";
            instance.SPD.text = "fff";
            instance.SPE.text = "ggg";
        }
        else
        {
            instance.Name.text = instance.slots[number].GetComponent<Slot>().pcItem.pokemon.Name.ToString();
            instance.HP.text = instance.slots[number].GetComponent<Slot>().pcItem.pokemon.HP.ToString();
            instance.ATK.text = instance.slots[number].GetComponent<Slot>().pcItem.pokemon.ATK.ToString();
            instance.DEF.text = instance.slots[number].GetComponent<Slot>().pcItem.pokemon.DEF.ToString();
            instance.SPA.text = instance.slots[number].GetComponent<Slot>().pcItem.pokemon.SPA.ToString();
            instance.SPD.text = instance.slots[number].GetComponent<Slot>().pcItem.pokemon.SPD.ToString();
            instance.SPE.text = instance.slots[number].GetComponent<Slot>().pcItem.pokemon.SPE.ToString();
        }
    }
    public static void openInform()
    {
        instance.information.SetActive(true);
    }

    public static void closeInform()
    {
        instance.information.SetActive(false);
    }

    //public static void CreateNewPCImage(PCItem item)//或者直接传一个pokemon？？（目前是准备pokemon和PCitem交互，然后PCitem和这个交互）
    //    //TODO:应该是不需要item的直接pokemon交互就行！！！！这个加到捕捉里面！！！
    //{
    //    PCImage newItem = Instantiate(instance.imagePrefab, instance.imageGrid.transform.position, Quaternion.identity);
    //    newItem.gameObject.transform.SetParent(instance.imageGrid.transform);
    //    newItem.pcItem = item;
    //    newItem.image.sprite = item.itemImage;
    //    newItem.number.text = item.itemNumber.ToString();

    //}

    ////在每一次捕捉里面判断，如果背包满了就加进仓库里面，如果没满就弄进背包里面！！！
    //public void CreateNewPCImage(Pokemon pokemon)
    //{

    //    PCImage newItem = Instantiate(instance.imagePrefab, instance.imageGrid.transform.position, Quaternion.identity);
    //    newItem.gameObject.transform.SetParent(instance.imageGrid.transform);
    //    newItem.pokemon = pokemon;
    //    newItem.image.sprite = GameResources.PokemonIcons[pokemon.ID];
    //    newItem.number.text = pokemon.ID.ToString();
    //    myPC.pokemonList.Add(pokemon);

    //}
    /*
             //TODO: change to translator
        itemName = pokemon.IsNicknamed ? pokemon.Name : pokemon.Name;
        itemImage = GameResources.PokemonIcons[pokemon.ID];
        itemNumber = pokemon.ID;
     */
}


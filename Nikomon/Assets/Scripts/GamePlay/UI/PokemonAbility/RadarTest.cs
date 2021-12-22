using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UtilUI;
using UnityEngine;

/// <summary>
/// 测试雷达图
/// </summary>
public class RadarTest : MonoBehaviour
{
    public UIPolygon uiPolygon;
    List<float> datas = new List<float>();
    public Pokemon pokemon;

    private void Awake()
    {
        // pokemon = AbilityPanel.Pokemon;
        // float hp = pokemon.HP * 1f / 255;
        // float atk = pokemon.ATK * 1f / 255;
        // float def = pokemon.DEF * 1f / 255;
        // float spa = pokemon.SPA * 1f / 255;
        // float spd = pokemon.SPD * 1f / 255;
        // float spe = pokemon.SPE * 1f / 255;
        // datas.Clear();
        // // HP
        // datas.Add(hp);//左下
        // // ATK
        // datas.Add(atk);//最下面
        // //DEF
        // datas.Add(def);//右下
        // //SPA
        // datas.Add(spa);//右上
        // // SPD
        // datas.Add(spd);//上
        // //SPE
        // datas.Add(spe);//左上
        // uiPolygon.DrawPolygon(datas);
        // datas.Clear();
        // // HP
        // datas.Add(0.5f);//左下
        // // ATK
        // datas.Add(0.5f);//最下面
        // //DEF
        // datas.Add(1f);//右下
        // //SPA
        // datas.Add(0.5f);//右上
        // // SPD
        // datas.Add(0.5f);//上
        // //SPE
        // datas.Add(0.5f);//左上
        //
        // uiPolygon.DrawPolygon(datas);
    }

    public void Init(Pokemon poke)//上限255

    {
        float hp = poke.HP / 255f;
        float atk = poke.ATK / 255f;
        float def = poke.DEF / 255f;
        float spa = poke.SPA / 255f;
        float spd = poke.SPD / 255f;
        float spe = poke.SPE / 255f;
        print(hp);
        datas.Clear();
        // HP
        datas.Add(hp);//左下
        // ATK
        datas.Add(atk);//最下面
        //DEF
        datas.Add(def);//右下
        //SPA
        datas.Add(spa);//右上
        // SPD
        datas.Add(spd);//上
        //SPE
        datas.Add(spe);//左上
        uiPolygon.DrawPolygon(datas);
        
    }

    private void Update()
    {
        
        // 重新更新数据
        // if (Input.GetMouseButtonDown(0))
        // {
            // uiPolygon.DrawPolygon(datas);
            // print(111111);
        // }
    }

}

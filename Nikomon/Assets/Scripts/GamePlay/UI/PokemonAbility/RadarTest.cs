using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 测试雷达图
/// </summary>
public class RadarTest : MonoBehaviour
{
    public UIPolygon uiPolygon;
    List<float> datas = new List<float>();

    void Start()
    {
        // HP
        datas.Add(0.5f);//左下
        // ATK
        datas.Add(0.5f);//最下面
        //DEF
        datas.Add(1f);//右下
        //SPA
        datas.Add(0.5f);//右上
        // SPD
        datas.Add(0.5f);//上
        //SPE
        datas.Add(0.5f);//左上
        uiPolygon.DrawPolygon(datas);
    }

    public void Init(Pokemon poke)//上限255

    {

        //poke.Exp.ExperienceNeeded;//还需要的经验值！！！！
        // HP
        datas.Add(0.5f);//左下
        // ATK
        datas.Add(0.5f);//最下面
        //DEF
        datas.Add(1f);//右下
        //SPA
        datas.Add(0.5f);//右上
        // SPD
        datas.Add(0.5f);//上
        //SPE
        datas.Add(0.5f);//左上
        uiPolygon.DrawPolygon(datas);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(datas.Count);
            for(int i=0,cnt = datas.Count;i<cnt;++i)
            {
                //datas[i] = Random.Range(0f, 1f);
            }
            // 重新更新数据
            uiPolygon.DrawPolygon(datas);
        }
    }

}

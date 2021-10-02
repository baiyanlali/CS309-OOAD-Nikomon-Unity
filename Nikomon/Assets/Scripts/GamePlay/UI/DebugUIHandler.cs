using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIHandler : MonoBehaviour
{
    ///<summary>单例模式
    /// 
    /// </summary>

    public static DebugUIHandler Instance
    {
        get => s_instance;
    }

    private static DebugUIHandler s_instance;

    public Text DebugInfoText { get; private set; }
    public List<string> DebugInfo;
    public float removeTime = 5f;
    private void Awake()
    {
        if (s_instance == null) s_instance = this;
        else Debug.LogError("More than one debugUIHandler descovered");
        DebugInfoText = GetComponentInChildren<Text>();
        DebugInfo = new List<string>();
    }
    
    /// <summary>
    /// 插入debug信息
    /// </summary>
    /// <param name="info"></param>
    public void InsertInfo(string info,string s="",LogType logType=LogType.Log)
    {
        DebugInfo.Add(info);
        refreshDebugText();
        StartCoroutine(deleteInfoByTime(removeTime,info));
    }
/// <summary>
/// 按照时间删除debug信息
/// </summary>
/// <param name="time"></param>
/// <param name="info"></param>
/// <returns></returns>
    IEnumerator deleteInfoByTime(float time,string info)
    {
        yield return new WaitForSeconds(time);
        DebugInfo.Remove(info);
        refreshDebugText();
    }

    private void refreshDebugText()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var str in DebugInfo)
        {
            sb.Append(str);
            sb.Append("\n");
        }
        if(DebugInfoText!=null)
            DebugInfoText.text = sb.ToString();
    }

    

}

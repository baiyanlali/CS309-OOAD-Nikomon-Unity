using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class dialogNode
{
    public int nodeId;
    public int style;//0:default style  1:style 1  2:style 3

    public bool invokeChoice;
    public List<int> nodeIdList;
    public string content;

    public dialogNode(string nodeId , string style ,string nodeidList, string invokeChoice,string content)
    {
        this.nodeId = int.Parse(nodeId);
        this.style = int.Parse(style);
        this.invokeChoice = bool.Parse(invokeChoice);
        this.content = content;

        nodeIdList = new List<int>();
        if (nodeidList != "")
        {
            string[] nl = nodeidList.Split('$');
            for (int i = 0; i < nl.Length; i++)
            {
                nodeIdList.Add(int.Parse(nl[i]));
            }
        }
    }



}

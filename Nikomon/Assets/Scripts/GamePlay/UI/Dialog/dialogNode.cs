using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class dialogNode
{
    public int nodeId;
    public int style;//0:default style  1:style 1  2:style 3

    public int selectType;//0:no select  1:
    List<int> nodeIdList;

    private dialogNode(int nodeId ,int style ,List<int> nodeidList,int selectType)
    {
        this.nodeId = nodeId;
        this.style = style;
        this.nodeIdList = nodeidList ;
        this.selectType = selectType;
    }

    public dialogNode readDialogNode() //read from dialogContent.txt
    {
        int nodeId = 0;
        int style = 1;
        List<int> nodeIdList = new List<int>();
        int selectType = 1;



        dialogNode dn = new dialogNode(nodeId,style,nodeIdList,selectType);

        return dn;
    }

}

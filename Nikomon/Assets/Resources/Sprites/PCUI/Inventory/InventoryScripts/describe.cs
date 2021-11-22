using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class describe : MonoBehaviour
{
      
      private GameObject buttonObj;
    private GameObject buttonObj1;
    private GameObject buttonObj2;
    private GameObject buttonObj3;
    private void Start()
{
         buttonObj = GameObject.Find("Button");
         buttonObj.GetComponent<Button>().onClick.AddListener(M);
        buttonObj1 = GameObject.Find("Button1");
        buttonObj1.GetComponent<Button>().onClick.AddListener(F);
        buttonObj2 = GameObject.Find("Button2");
        buttonObj2.GetComponent<Button>().onClick.AddListener(N);
        buttonObj3 = GameObject.Find("Button3");
        buttonObj3.GetComponent<Button>().onClick.AddListener(L);
        //buttonObj.GetComponent<Button>().onClick.AddListener(F);
        //buttonObj.GetComponent<Button>().onClick.AddListener(N);
        //buttonObj.GetComponent<Button>().onClick.AddListener(L);
    }
     void M()
     {
             print("执行了M方法!");
         }
     public void F()
     {
             print("执行了F方法!");
         }
    public void N()
    {
        print("执行了N方法!");
    }
    public void L()
    {
        print("执行了L方法!");
    }

    /*
     * buttonObj.GetComponent<Button>().onClick.AddListener(M);可以换成
buttonObj.GetComponent<Button>().onClick.AddListener
 (
      delegate () { M(); }
 );
     */
}

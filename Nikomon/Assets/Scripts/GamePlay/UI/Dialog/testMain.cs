using System.Collections;
using UnityEngine;

public class testMain : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogEngine de;
    private DialogManager dm;
    private int condition;
    void Start()
    {
        dm = DialogManager.dialogManagerIn;
        condition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //dm.onDialogText("八百标兵奔北坡，炮兵并排北边跑！");
        //dm.offDialog();
        StartCoroutine("on");

        

    }
    IEnumerator off()
    {
        
        yield return new WaitForSeconds(2);
        if (condition == 1)
        {
            Debug.Log("off");
            dm.offDialog();
            condition = 2;
            
        }

    }
    IEnumerator on()
    {
        yield return new WaitForSeconds(2);
        if (condition == 0)
        {
            Debug.Log("on");
            dm.onDialogText("八百标兵奔北坡，炮兵并排北边跑！");
            condition = 1;
            StartCoroutine("off");
        }

    }


}

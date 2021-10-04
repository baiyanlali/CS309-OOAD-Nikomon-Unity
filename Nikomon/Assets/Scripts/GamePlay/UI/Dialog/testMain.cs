using System.Collections;
using UnityEngine;

public class testMain : MonoBehaviour
{
    // Start is called before the first frame update
    public dialogEngine de;
    private dialogManager dm;
    void Start()
    {
        //de = transform.Find("dialogSystem").GetComponent<dialogEngine>();
        dm = dialogManager.getDialogManager();
    }

    // Update is called once per frame
    void Update()
    {
            dm.onDialogText("八百标兵奔北坡，炮兵并排北边跑！");
            dm.offDialog();


    }



}

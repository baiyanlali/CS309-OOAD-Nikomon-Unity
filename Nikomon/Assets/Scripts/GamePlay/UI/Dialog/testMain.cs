using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class testMain : MonoBehaviour
{
    // Start is called before the first frame update
    public dialogEngine de;
    private dialogManager dm;
    private int condition;
    void Start()
    {
        

        //de = transform.Find("dialogSystem").GetComponent<dialogEngine>();
        dm = dialogManager.getDialogManager();
        condition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //dm.onDialogText("八百标兵奔北坡，炮兵并排北边跑！");
        //dm.offDialog();
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            //执行顺序 isPressed = false -> 按下：wasPressedThisFrame = true -> 中途：isPressed = true -> 松开：wasReleasedThisFrame = true -> isPressed = false
            if (keyboard.sKey.wasPressedThisFrame)
                //dm.onDialogText("至诚无息，薄厚有会员");
                dm.onDialogId(0);
            if (keyboard.aKey.wasPressedThisFrame)
                dm.onDialogId();
            if (keyboard.lKey.wasPressedThisFrame)
                dm.offDialog();
        }

    }



}

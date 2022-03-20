using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.isMobilePlatform)
        {
            UIManager.Instance.Show<VirtualControllerPanel>();
            Button btn;
            // btn.navigation
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

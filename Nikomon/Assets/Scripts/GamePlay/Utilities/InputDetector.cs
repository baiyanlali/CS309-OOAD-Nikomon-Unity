using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputDetector : MonoBehaviour
{
#if UNITY_ANDROID || UNITY_IPHONE
    void Start()
    {
        UIManager.Instance.Show<VirtualControllerPanel>();
    }
#endif
}
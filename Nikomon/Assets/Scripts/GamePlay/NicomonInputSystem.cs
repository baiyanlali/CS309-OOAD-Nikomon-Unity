using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;

public class NicomonInputSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static NicomonInputSystem Instance;

    private NicomonInput nicomonInput;

    public NicomonInput NicomonInput => nicomonInput;

    public Vector2 move { get; private set; }

    public Vector2 look { get; private set; }

    public bool accept { get; private set; }

    public bool back { get; private set; }
    public bool menu { get; private set; }
    public bool bag { get; private set; }

    public Vector2 ui_navigate { get; private set; }
    public Vector2 ui_point { get; private set; }
    public bool ui_click { get; private set; }
    public bool ui_submit { get; private set; }
    public bool ui_cancel { get; private set; }
    public Vector2 ui_scroll_wheel { get; private set; }

    void Awake()
    {
        nicomonInput = new NicomonInput();
        nicomonInput.Enable();


        nicomonInput.Player.Move.performed += (context) => move = context.ReadValue<Vector2>();
        nicomonInput.Player.Move.canceled += (context) => move = Vector2.zero;

        nicomonInput.Player.Look.performed += (context) => look = context.ReadValue<Vector2>();
        nicomonInput.Player.Look.canceled += (context) => look = Vector2.zero;

        nicomonInput.Player.Accept.performed += (context) => accept = context.ReadValue<float>() >= 0.5f;
        nicomonInput.Player.Accept.canceled += (context) => accept = false;

        nicomonInput.Player.Back.performed += (context) => back = context.ReadValue<float>() >= 0.5f;
        nicomonInput.Player.Back.canceled += (context) => back = false;

        nicomonInput.Player.Menu.performed += (context) => menu = context.ReadValue<float>() >= 0.5f;
        nicomonInput.Player.Menu.canceled += (context) => menu = false;

        nicomonInput.Player.Bag.performed += (context) => bag = context.ReadValue<float>() >= 0.5f;
        nicomonInput.Player.Bag.canceled += (context) => bag = false;


        nicomonInput.UI.Submit.performed += (context) => ui_submit = context.ReadValue<float>() >= 0.5f;
        nicomonInput.UI.Submit.canceled += (context) => ui_submit = false;

        nicomonInput.UI.Navigate.performed += (context) => ui_navigate = context.ReadValue<Vector2>();
        nicomonInput.UI.Navigate.canceled += (context) => ui_navigate = Vector2.zero;

        nicomonInput.UI.Point.performed += (context) => ui_point = context.ReadValue<Vector2>();
        nicomonInput.UI.Point.canceled += (context) => ui_point = Vector2.zero;

        nicomonInput.UI.ScrollWheel.performed += (context) => ui_scroll_wheel = context.ReadValue<Vector2>();
        nicomonInput.UI.ScrollWheel.canceled += (context) => ui_scroll_wheel = Vector2.zero;

        nicomonInput.UI.Cancel.performed += (context) => ui_cancel = context.ReadValue<float>() >= 0.5f;
        nicomonInput.UI.Cancel.canceled += (context) => ui_cancel = false;

        nicomonInput.UI.Click.performed += (context) => ui_click = context.ReadValue<float>() >= 0.5f;
        nicomonInput.UI.Click.canceled += (context) => ui_click = false;


        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }


    private void OnEnable()
    {
        nicomonInput?.Enable();
    }

    private void OnDisable()
    {
        nicomonInput?.Disable();
    }
}
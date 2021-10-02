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
    public Vector2 move { get; private set; }
    
    public Vector2 look { get; private set; }
    
    public bool accept { get; private set; }
    
    public bool back { get; private set; }
    
    void Awake()
    {
        nicomonInput = new NicomonInput();
        nicomonInput.Enable();

        nicomonInput.Player.Move.performed += (context) => move = context.ReadValue<Vector2>();
        nicomonInput.Player.Move.canceled += (context) => move = Vector2.zero;
        
        nicomonInput.Player.Look.performed += (context) => look = context.ReadValue<Vector2>();
        nicomonInput.Player.Look.canceled += (context) => look = Vector2.zero;
        
        nicomonInput.Player.Accept.performed += (context) => accept = context.ReadValue<float>()>=0.5f;
        nicomonInput.Player.Accept.canceled += (context) => accept = false;
        
        nicomonInput.Player.Back.performed += (context) => back = context.ReadValue<float>()>=0.5f;
        nicomonInput.Player.Back.canceled += (context) => back = false;
        if(Instance==null)
            Instance = this;
        else Destroy(this);

    }

    // Update is called once per frame
    void Update()
    {
        
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

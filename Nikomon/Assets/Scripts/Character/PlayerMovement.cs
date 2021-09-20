using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public NicomonInputSystem nicoInput;
    private Animator animator;
    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        this.move = nicoInput.move;
        Movement();
        // if (isWalking)
        // {
        //     
        //     // DebugUIHandler.Instance.InsertInfo(move.ToString());
        //     // Debug.Log("Move vector: "+move.ToString());
        //     if (move.magnitude < 0.03f) return;
        //     move.Normalize();
        //     float cameraForward =
        //         Camera.main.transform.rotation.eulerAngles.y;
        //     
        //     float theta = 0;
        //     if(move.y>0)
        //         theta = Mathf.Atan(move.x / move.y);
        //     else if (move.y < 0)
        //         theta = Mathf.Atan(move.x / move.y) + Mathf.PI;
        //     else
        //     {
        //         theta = move.x > 0 ? Mathf.PI / 2 : Mathf.PI * 3 / 2;
        //     }
        //     theta = Mathf.Rad2Deg * theta;
        //     transform.rotation = Quaternion.Euler(0, cameraForward + theta, 0);
        //     animator?.SetBool("IsWalking",true);
        // }
        // else
        // {
        //     animator?.SetBool("IsWalking",false);
        // }

    }

    public void Movement()
    {
        
        if (move != Vector2.zero)
        {
            isWalking = true;
            float cameraForward =
                Camera.main.transform.rotation.eulerAngles.y;
            float theta = 0;
            if(move.y>0)
                theta = Mathf.Atan(move.x / move.y);
            else if (move.y < 0)
                theta = Mathf.Atan(move.x / move.y) + Mathf.PI;
            else
            {
                theta = move.x > 0 ? Mathf.PI / 2 : Mathf.PI * 3 / 2;
            }
            theta = Mathf.Rad2Deg * theta;
            transform.rotation = Quaternion.Euler(0, cameraForward + theta, 0);
        }
        else
        {
            isWalking = false;
        }
        animator?.SetBool("IsWalking",isWalking);

    }

    private bool isWalking;
    private Vector2 move;
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            // isWalking = true;
            move = context.ReadValue<Vector2>();
            
        }
        else
        {
            // isWalking = false;
            move=Vector2.zero;
            
        }
    }
}
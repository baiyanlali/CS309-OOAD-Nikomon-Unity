using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Keyboard.current!=null)
        // {
        //     if(Keyboard.current.wKey.wasPressedThisFrame)
        //         animator?.SetBool("IsWalking",true);
        //     else
        //         animator?.SetBool("IsWalking",false);
        // }
        // else
        // {
        //     animator?.SetBool("IsWalking",false);
        // }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            float cameraForward =
                Camera.main.transform.rotation.eulerAngles.y;
            Vector2 move = context.ReadValue<Vector2>();
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
            animator?.SetBool("IsWalking", true);
            
        }
        else
        {
            animator?.SetBool("IsWalking", false);
        }
    }
}
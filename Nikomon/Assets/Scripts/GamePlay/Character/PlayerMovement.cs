using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;

public class PlayerMovement : MonoBehaviour
{
    public NicomonInputSystem nicoInput;
    public float InteractDepth = 5f;
    private Animator animator;

    private GameObject VirtualController;

    void Start()
    {
        animator = GetComponent<Animator>();
        VirtualController=GameObject.Find("VirtualController");
        if (nicoInput == null)
            nicoInput = FindObjectOfType<NicomonInputSystem>();
    }

    void Update()
    {

        if (GlobalManager.isBattling)
        {
            if(GlobalManager.Instance.Config.UseVirtualControl) VirtualController?.SetActive(false);
            isWalking = false;
            animator?.SetBool("IsWalking",false);
            move=Vector2.zero;
            return;
        }
        if (nicoInput == null)
            nicoInput = FindObjectOfType<NicomonInputSystem>();
        else
        {
            if(GlobalManager.Instance.Config.UseVirtualControl) VirtualController?.SetActive(true);
            this.move = nicoInput.move;
            Movement();
            if (nicoInput.menu)
            {
                UIManager.Instance.Show<MainMenuUI>();
                // GlobalManager.Instance.SaveSaveData();
            }
            if (nicoInput.accept)
            {
                CheckInteractable();
            }
        }
    }
    
    
    


    void CheckInteractable()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, InteractDepth))
        {
            IInteractive interactive = raycastHit.transform.GetComponent<IInteractive>();
            interactive?.OnInteractive();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (GlobalManager.isBattling) return;
        IInteractive interactive = other.gameObject.GetComponent<IInteractive>();
        interactive?.OnInteractive(this.gameObject);
    }

    public void Movement()
    {
        if (move != Vector2.zero)
        {
            isWalking = true;
            Debug.Assert(Camera.main != null, "Camera.main != null");
            float cameraForward =
                Camera.main.transform.rotation.eulerAngles.y;
            float theta = 0;
            if (move.y > 0)
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

        animator?.SetBool("IsWalking", isWalking);
    }

    private bool isWalking;
    private Vector2 move;


    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Character;
using GamePlay.UI.UIFramework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : Player
{
    public NicomonInputSystem nicoInput;
    private Animator _animator;
    private CharacterController _characterController;

    private bool _isGround = true;
    private float _verticalSpeed = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        if (nicoInput == null)
            nicoInput = FindObjectOfType<NicomonInputSystem>();
        if (nicoInput == null)
        {
            nicoInput = gameObject.AddComponent<NicomonInputSystem>();
        }
    }

    private void Update()
    {
        if (nicoInput.menu)
        {
            UIManager.Instance.Show<MainMenuUI>();
            // GlobalManager.Instance.SaveSaveData();
        }
    }

    private void FixedUpdate()
    {
        CalculateVerticalMovement();
        Movement();
    }

    private void CalculateVerticalMovement()
    {
        if (_isGround)
        {
            _verticalSpeed = -20f * 0.3f;
        }
        else
        {
            if (_verticalSpeed > 0) _verticalSpeed = 0;
            _verticalSpeed -= 20f * Time.deltaTime;
        }
    }

    public void Teleport(Vector3 position)
    {
        this._characterController.enabled = false;
        transform.position = position;
        this._characterController.enabled = true;
    }

    private void Movement()
    {
        Vector2 move = nicoInput.move;
        if (move != Vector2.zero)
        {
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

            if (math.abs(transform.rotation.eulerAngles.y - (cameraForward + theta)) > 1f)
            {
                LeanTween.value(this.gameObject, (Vector3 o) => { transform.rotation = Quaternion.Euler(o); },
                    transform.rotation.eulerAngles,
                    Quaternion.Euler(0, cameraForward + theta, 0).eulerAngles,
                    0.1f
                );
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, cameraForward + theta, 0);
            }

            // 
            // rigid.angularVelocity = Vector3.zero;

            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 movement = Vector3.zero;
        if (_isGround)
        {
            // RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);
            if (Physics.Raycast(ray, out var hit, 1f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                movement = Vector3.ProjectOnPlane(_animator.deltaPosition, hit.normal);
            }
            else
            {
                //hardly occur
                movement = _animator.deltaPosition;
            }
        }
        else
        {
            //避免出现下很大的台阶时判断为跳跃的情况
            Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);
            if (Physics.Raycast(ray, out var hit, 0.5f + _characterController.stepOffset, Physics.AllLayers,
                QueryTriggerInteraction.Ignore))
            {
                _isGround = true;
                movement = Vector3.ProjectOnPlane(_animator.deltaPosition, hit.normal);
            }
        }

        movement += _verticalSpeed * Vector3.up * Time.deltaTime;

        _characterController.Move(movement);


        _animator.SetBool("IsGround", _isGround);

        _isGround = _characterController.isGrounded;
    }


    private void OnDrawGizmos()
    {
        if (_characterController == null)
        {
            _characterController = GetComponent<CharacterController>();
        }

        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f,
            transform.position + Vector3.up * 0.5f - (Vector3.up * (0.5f + _characterController.stepOffset)));
    }


    private void OnCollisionEnter(Collision other)
    {
        if (GlobalManager.isBattling) return;
        IInteractive interactive = other.gameObject.GetComponent<IInteractive>();
        interactive?.OnInteractive(this.gameObject);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (GlobalManager.isBattling) return;
        IInteractive interactive = hit.gameObject.GetComponent<IInteractive>();
        interactive?.OnInteractive(this.gameObject);
    }
}
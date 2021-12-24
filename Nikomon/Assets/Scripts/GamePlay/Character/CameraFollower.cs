using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private new Cinemachine.CinemachineVirtualCamera camera;
    public float HorizontalRotateSpeed = 3;
    public float VerticalRotateSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        camera = this.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NicomonInputSystem.Instance.look != Vector2.zero)
        {
            
            // Vector2 look = NicomonInputSystem.Instance.look;
            // var transform1 = camera.transform;
            // Vector3 offset = transform1.right * look.x * HorizontalRotateSpeed+
            //                  transform1.up * look.y * VerticalRotateSpeed;
            // camera.ForceCameraPosition(transform.position+offset,quaternion.identity);
        }
    }
}
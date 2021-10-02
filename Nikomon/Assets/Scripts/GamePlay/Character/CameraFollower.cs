using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera camera;
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
            Vector2 look = NicomonInputSystem.Instance.look;
            camera.gameObject.transform.localPosition += new Vector3(-look.x *HorizontalRotateSpeed, -look.y*VerticalRotateSpeed)*Time.deltaTime*10;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSystem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerMovement>()?.CheckWater(true);
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rigidbody = other.gameObject.GetComponent<Rigidbody>();
        rigidbody?.AddForce(Vector3.up * Mathf.Abs(other.transform.position.y - transform.position.y) * rigidbody.mass * 10,
            ForceMode.Force);
    }
    

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<PlayerMovement>()?.CheckWater(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyantObject : MonoBehaviour
{
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;
    public float waterHeight = 0f;

    private Rigidbody m_Rigidbody;

    private bool underWater;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float difference = transform.position.y - waterHeight;

        if (difference < 0)
        {
            m_Rigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), transform.position,
                ForceMode.Force);
            if (!underWater)
            {
                underWater = true;
                SwitchState(underWater);
            }
        }
        else
        {
            underWater = false;
            SwitchState(underWater);
        }
    }

    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            m_Rigidbody.drag = underWaterDrag;
            m_Rigidbody.angularDrag = underWaterDrag;
        }
        else
        {
            m_Rigidbody.drag = airDrag;
            m_Rigidbody.angularDrag = airAngularDrag;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public bool enabled;
    public Transform Target;
    public Vector3 Offset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (Target != null)
            {
                gameObject.transform.position = Target.position + Offset;
            }
        }
    }
}

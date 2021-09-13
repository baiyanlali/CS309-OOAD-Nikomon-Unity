using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public bool mEnabled;
    public Transform Target;
    public Vector3 Offset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mEnabled)
        {
            if (Target != null)
            {
                gameObject.transform.position = Target.position + Offset;
            }
        }
    }
}

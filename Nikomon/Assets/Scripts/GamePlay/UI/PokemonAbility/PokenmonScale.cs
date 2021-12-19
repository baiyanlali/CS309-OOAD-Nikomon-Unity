using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokenmonScale : MonoBehaviour
{
    private void Awake()
    {
        // GameObject gameObject= this.GetComponentInChildren<CapsuleCollider>()
        foreach (Transform child in transform)
        {
            try
            {
                float scale = child.GetComponent<CapsuleCollider>().radius / 35;
                float result = 0.1f / scale;
                string temp = name + " " + scale.ToString();
                // Debug.Log(temp);
                Vector3 v = new Vector3();
                v.x = result;
                v.y = result;
                v.z = result;
                child.localScale = v;
                // child.GetComponent<CapsuleCollider>()
            }
            catch
            {
                float scale = child.GetComponent<SphereCollider>().radius / 35;
                float result = 0.1f / scale;
                string temp = name + " " + scale.ToString();
                // Debug.Log(temp);
                Vector3 v = new Vector3();
                v.x = result;
                v.y = result;
                v.z = result;
                child.localScale = v;
            }
        }

    }
}

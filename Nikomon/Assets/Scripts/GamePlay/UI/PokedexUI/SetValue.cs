using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetValue : MonoBehaviour
{
    private void Start()
    {
        print(this.transform.GetComponent<Scrollbar>().value);
        this.transform.GetComponent<Scrollbar>().value = 0.9999f;
        print(this.transform.GetComponent<Scrollbar>().value);
        
    }
}

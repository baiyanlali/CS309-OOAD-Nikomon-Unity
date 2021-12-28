using System.Collections;
using System.Collections.Generic;
using GamePlay.Messages;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AutoTranslateOnStart : MonoBehaviour
{
    private string originalString = null;
    void Start()
    {
        // Text text = GetComponent<Text>();
        //
        // if (originalString == null)
        // {
        //     originalString = text.text;
        // }
        // if(Application.isPlaying)
        //     text.text = Messages.Get(originalString);
    }
    
    void OnEnable()
    {
        // GetComponent<Text>().
        Text text = GetComponent<Text>();

        if (originalString == null)
        {
            originalString = text.text;
        }
        if(Application.isPlaying)
            text.text = Messages.Get(originalString);
    }
    
}

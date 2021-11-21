using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public Text text;
    public static int nextSceneID;
    public static string nextSceneName;
    private void Start()
    {
        StartCoroutine(LoadLeaver(nextSceneID));
        
    }
    
    IEnumerator LoadLeaver(int index)
    {
        yield return new WaitForSeconds(100);
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);                                                                                               
        //operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            text.text = String.Format("{0:N1} ", operation.progress * 111.111111) + "%";
            
            yield return null;
        }
    }
}

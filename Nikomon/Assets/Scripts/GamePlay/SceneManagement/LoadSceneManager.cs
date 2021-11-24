using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public Text text;
    public static int nextSceneID = -1;
    public static string nextSceneName = "";
    public SpriteRenderer sr;
    public Image image;
        
        
    private void Start()
    {
        print(nextSceneID);
        if (nextSceneID != -1)
        {
            StartCoroutine(LoadLeaver(nextSceneID));
        }
        if (nextSceneName != "")
        {
            StartCoroutine(LoadLeaver(nextSceneName));
        }
    }
    void Update () {
        image.sprite = sr.sprite;
    }
    IEnumerator LoadLeaver(int index)
    {
        //yield return new WaitForSeconds(100);
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);                                                                                               
        //operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            text.text = String.Format("{0:N1} ", operation.progress * 111.111111) + "%";
            
            yield return null;
        }
    }
    IEnumerator LoadLeaver(string name)
    {
        //yield return new WaitForSeconds(100);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);                                                                                               
        //operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            text.text = String.Format("{0:N1} ", operation.progress * 111.111111) + "%";
            
            yield return null;
        }
    }
}

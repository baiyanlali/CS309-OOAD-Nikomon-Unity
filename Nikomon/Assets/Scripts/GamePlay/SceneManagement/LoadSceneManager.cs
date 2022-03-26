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
    public static Action onComplete;
        
        
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        // print(nextSceneID);
        if (nextSceneID != -1)
        {
            StartCoroutine(LoadLeaver(nextSceneID));
        }
        if (nextSceneName != "")
        {
            StartCoroutine(LoadLeaver(nextSceneName));
        }
    }
    // void Update () {
    //     image.sprite = sr.sprite;
    // }
    
    
    IEnumerator LoadLeaver(int index)
    {
        yield return new WaitForSeconds(0.7f);
        text.text = String.Format("{0:N1} ", 0.5 * 111.111111) + "%";
        yield return new WaitForSeconds(0.7f);
        text.text = String.Format("{0:N1} ", 0.7 * 111.111111) + "%";
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);                                                                                               
        //operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            text.text = String.Format("{0:N1} ", operation.progress * 111.111111) + "%";
            // text.text = String.Format("{0:C1} ", operation.progress * 111.111111) + "%";
            
            yield return null;
        }

        yield return null;
        onComplete?.Invoke();
        onComplete = null;
        DestroyImmediate(this.gameObject);
    }
    IEnumerator LoadLeaver(string name)
    {
        yield return new WaitForSeconds(0.7f);
        text.text = String.Format("{0:N1} ", 0.5 * 111.111111) + "%";
        yield return new WaitForSeconds(0.7f);
        text.text = String.Format("{0:N1} ", 0.7 * 111.111111) + "%";
        //yield return new WaitForSeconds(100);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);                                                                                               
        //operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            text.text = String.Format("{0:N1} ", operation.progress * 111.111111) + "%";
            // text.text = String.Format("{0:C1} ", operation.progress * 111.111111) + "%";
            
            yield return null;
        }
        
        yield return null;
        onComplete?.Invoke();
        onComplete = null;
        
        DestroyImmediate(this.gameObject);
    }
}

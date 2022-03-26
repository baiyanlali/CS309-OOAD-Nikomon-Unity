using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransmitor
{
    public static void LoadSceneID(int index, Action onComplete = null)
    {
        SceneManager.LoadScene("LoadScene");
        LoadSceneManager.nextSceneID = index;
        LoadSceneManager.onComplete = onComplete;
    }
    public static void LoadSceneName(string name, Action onComplete = null)
    {
        SceneManager.LoadScene("LoadScene");
        LoadSceneManager.nextSceneName = name;
        LoadSceneManager.onComplete = onComplete;

    }
    
}

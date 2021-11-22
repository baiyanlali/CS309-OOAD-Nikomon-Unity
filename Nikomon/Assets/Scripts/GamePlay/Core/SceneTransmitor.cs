using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransmitor
{
    public static void LoadSceneID(int index)
    {
        SceneManager.LoadScene("LoadScene");
        LoadSceneManager.nextSceneID = index;
    }
    public static void LoadSceneName(string name)
    {
        SceneManager.LoadScene("LoadScene");
        LoadSceneManager.nextSceneName = name;
    }
}

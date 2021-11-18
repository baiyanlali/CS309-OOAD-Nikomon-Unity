using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CreateNewTrainer(bool isMale)
    {
        InputField inputField = GameObject.Find("NameText").GetComponent<InputField>();
        GlobalManager.Instance.game.CreateNewSaveFile(inputField.text, isMale);

        // StartGame();
    }
}

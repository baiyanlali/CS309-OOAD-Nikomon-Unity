
//Attach this script to a GameObject.
//Create a Text GameObject (Create>UI>Text) and attach it to the My Text field in the Inspector of your GameObject
//Press the space bar in Play Mode to see the Text change.

using UnityEngine;
using UnityEngine.UI;

public class Texts : MonoBehaviour
{
    public Text m_MyText;

    void Start()
    {
        Pokemon p = new Pokemon(1, 30);

        m_MyText.text = "This is my text";//读取宝可梦信息
    }

}

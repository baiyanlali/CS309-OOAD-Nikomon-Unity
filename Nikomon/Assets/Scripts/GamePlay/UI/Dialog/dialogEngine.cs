using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogEngine : MonoBehaviour
{
    public string debugBoxString;

    private Image dialogBox;
    private Text dialogBoxText;
    // private Text dialogBoxTextShadow;
    private CanvasGroup dialogCanvasGroup;

    private Image choiceBox;
    private Text choiceBoxText;
    // private Text choiceBoxTextShadow;
    private Image choiceBoxSelect;

    public AudioClip selectClip;

    private float charPerSec = 60f;
    public float scrollSpeed = 0.1f;

    public int chosenIndex;

    public int defaultChoiceWidth = 86;
    public int defaultChoiceY = 0;
    public int dialogLines = 2;
    public int fontSize = 20;

    public float fadeSpeed = 1f;
    private Transform trangleTrn;

    private Transform dialogBoxTrn;
    private void Awake()
    {
        dialogBoxTrn = transform.Find("DialogPanel");
        dialogBoxText = dialogBoxTrn.transform.Find("DialogText").GetComponent<Text>();
        dialogCanvasGroup = transform.GetComponent<CanvasGroup>();
        trangleTrn = transform.GetChild(1).GetChild(1).transform;
        
        //test area
        PlayerPrefs.SetInt("dialogStyle",1);
    }

    public IEnumerator DrawText(string text)
    {
        yield return StartCoroutine(DrawText(text, 1f / charPerSec, false));
    }

    public IEnumerator DrawText(string text, float secPerChar)
    {
        yield return StartCoroutine(DrawText(text, secPerChar, false));
    }

    public IEnumerator DrawTextSilent(string text)
    {
        yield return StartCoroutine(DrawText(text, 1f / charPerSec, true));
    }

    public IEnumerator DrawTextInstant(string text)
    {
        yield return StartCoroutine(DrawText(text, 0, false));
    }

    public IEnumerator DrawText(string text, float secPerChar, bool audioEffect)
    {
        string[] words = text.Split(new char[] { ' ' });
        Debug.Log(text);
        for (int i = 0; i < words.Length; i++)
        {
            Debug.Log(i+" "+words[i]);
        }
        if (!audioEffect)
        {
            //AudioManager.Play(selectClip);
        }
        for (int i = 0; i < words.Length; i++)
        {
            if (secPerChar > 0)
            {
                yield return StartCoroutine(DrawWord(words[i], secPerChar));
            }
            else
            {
                StartCoroutine(DrawWord(words[i], secPerChar));
            }
        }
    } //split by ' '

    private IEnumerator DrawWord(string word, float secPerChar)
    {
        yield return StartCoroutine(DrawWord(word, false, false, false, secPerChar));
    }

    private IEnumerator DrawWord(string word, bool large, bool bold, bool italic, float secPerChar)
    {
        char[] chars = word.ToCharArray();
        float startTime = Time.time;
        
        if (chars.Length > 0)
        {
            //ensure no blank words get processed
            if (chars[0] == '\\') //deal with effect 
            {
                switch (chars[1])
                {
                    case ('p'): //Player
                        if (secPerChar > 0)
                        {
                            yield return StartCoroutine(DrawWord("TB", large, bold, italic, secPerChar));
                        }
                        else
                        {
                            StartCoroutine(DrawWord("TB", large, bold, italic, secPerChar));//SaveData.currentSave.Player.Name
                        }
                        break;
                    case ('t')://type of Nikomon

                        break;
                    case ('l'): //Large
                        large = true;
                        break;
                    case ('b'): //Bold
                        bold = true;
                        break;
                    case ('i'): //Italic
                        italic = true;
                        break;
                    case ('n'): //New Line
                        dialogBoxText.text += "\n";
                        break;
                }
                if (chars.Length > 2) //there is effect on current word
                {
                    string subStr = word.Substring(word.Length - 2);

                    yield return StartCoroutine(DrawWord(subStr, large, bold, italic, secPerChar));
                }
            }
            else // deal with pure word
            {
                string currentText = dialogBoxText.text;

                for (int i = 0; i <= chars.Length; i++)
                {
                    string added = "";

                    //apply open tags
                    added += (large) ? "<size=26>" : "";
                    added += (bold) ? "<b>" : "";
                    added += (italic) ? "<i>" : "";

                    //apply displayed text
                    for (int i2 = 0; i2 < i; i2++)
                    {
                        added += chars[i2].ToString();
                    }

                    //apply hidden text
                    added += "<color=#0000>";
                    for (int i2 = i; i2 < chars.Length; i2++)
                    {
                        added += chars[i2].ToString();
                    }
                    added += "</color>";

                    //apply close tags
                    added += (italic) ? "</i>" : "";
                    added += (bold) ? "</b>" : "";
                    added += (large) ? "</size>" : "";

                    dialogBoxText.text = currentText + added;
                    // dialogBoxTextShadow.text = dialogBoxText.text;

                    while (Time.time < startTime + (secPerChar * (i + 1)))
                    {
                        yield return null;
                    }
                }

                //add a space after every word -> no space is needed any more
                //dialogBoxText.text += " ";
                //dialogBoxTextShadow.text = dialogBoxText.text;

            }
        }
        else
        {
            dialogBoxText.text += " <color=#0000></color>";

            while (Time.time < startTime + (secPerChar)) //realize the function of delay display
            {
                yield return null;
            }
        }
    }


    public void DrawDialogBox()
    {
        StartCoroutine(DrawDialogBox(dialogLines,PlayerPrefs.GetInt("dialogStyle")));
    }
    public void DrawDialogBox(int lines)
    {
        StartCoroutine(DrawDialogBox(lines, PlayerPrefs.GetInt("dialogStyle")));
    }

    private IEnumerator DrawDialogBox(int lines,int style)
    {
        Debug.Log("DrawDialogbox"+ PlayerPrefs.GetInt("dialogStyle"));
        dialogBoxTrn.gameObject.SetActive(true);
        dialogBoxText.text = "";
        dialogBoxText.fontSize = fontSize;

        yield return StartCoroutine(fadeEffect(1));

    }
    
    public IEnumerator fadeEffect(int alpha)
    {
        CanvasGroup cg = dialogCanvasGroup;
        while (cg.alpha != alpha)
        {
            cg.alpha = Mathf.Lerp(cg.alpha, alpha, fadeSpeed * Time.deltaTime);
            yield return null;
            // Debug.Log(cg.alpha);
            if (Mathf.Abs(alpha - cg.alpha) <= 0.01)
            {
                cg.alpha = alpha;
            }
        }
        
    }

    public IEnumerator floatingTriagle()
    {
        float diff = 0.1f;
        while (true)
        {
            trangleTrn.position = new Vector3(trangleTrn.position.x, trangleTrn.position.y - diff,
                trangleTrn.position.z);
            if (trangleTrn.position.y > 65) diff = -0.1f;
            if (trangleTrn.position.y < 45) diff = 0.1f;

        }
    }

    public IEnumerator DrawChoiceBox()
    {
        yield return
            StartCoroutine(DrawChoiceBox(new string[] { "Yes", "No" }, null, -1, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices)
    {
        yield return StartCoroutine(DrawChoiceBox(choices, null, -1, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(int startIndex)
    {
        yield return
            StartCoroutine(DrawChoiceBox(new string[] { "Yes", "No" }, null, startIndex, defaultChoiceY,
                defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, int startIndex)
    {
        yield return StartCoroutine(DrawChoiceBox(choices, null, startIndex, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, string[] flavourText)
    {
        yield return StartCoroutine(DrawChoiceBox(choices, flavourText, -1, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, string[] flavourText, int startIndex)
    {
        yield return StartCoroutine(DrawChoiceBox(choices, flavourText, startIndex, defaultChoiceY, defaultChoiceWidth))
            ;
    }

    public IEnumerator DrawChoiceBox(string[] choices, int yPosition, int width)
    {
        yield return
            StartCoroutine(DrawChoiceBox(new string[] { "Yes", "No" }, null, -1, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, int startIndex, int yPosition, int width)
    {
        yield return
            StartCoroutine(DrawChoiceBox(new string[] { "Yes", "No" }, null, startIndex, defaultChoiceY,defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, string[] flavourText, int startIndex, int yPosition, int width)
    {
        if (startIndex < 0)
        {
            startIndex = choices.Length - 1;
        }

        choiceBox.gameObject.SetActive(true);
        choiceBox.sprite = Resources.Load<Sprite>("Dialog/choice" + PlayerPrefs.GetInt("dialogStyle"));
        Debug.Log(choiceBox.name);
        Debug.Log(choiceBox.sprite.name);

        choiceBox.rectTransform.localPosition = new Vector3(171 - width - 1, yPosition - 96, 0);
        choiceBox.rectTransform.sizeDelta = new Vector2(width, 16f + (fontSize * choices.Length));
        choiceBoxSelect.rectTransform.localPosition = new Vector3(8, 9f + (fontSize * startIndex), 0);
        choiceBoxText.rectTransform.sizeDelta = new Vector2(width - 30, choiceBox.rectTransform.sizeDelta.y);
        // choiceBoxTextShadow.rectTransform.sizeDelta = new Vector2(choiceBoxText.rectTransform.sizeDelta.x,
            // choiceBoxText.rectTransform.sizeDelta.y);

        choiceBoxText.text = "";
        for (int i = 0; i < choices.Length; i++)
        {
            choiceBoxText.text += choices[i];
            if (i != choices.Length - 1)
            {
                choiceBoxText.text += "\n";
            }
        }
        // choiceBoxTextShadow.text = choiceBoxText.text;

        bool selected = false;
        UpdateChosenIndex(startIndex, choices.Length, flavourText);
        while (!selected)
        {
            if (Input.GetButtonDown("Select"))
            {
                selected = true;
            }
            else if (Input.GetButtonDown("Back"))
            {
                chosenIndex = 1;
                //a little hack to bypass the newIndex != chosenIndex in the below method, ensuring a true return
                if (UpdateChosenIndex(0, choices.Length, flavourText))
                {
                    yield return new WaitForSeconds(0.2f);
                }
                selected = true;
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (UpdateChosenIndex(chosenIndex + 1, choices.Length, flavourText))
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (UpdateChosenIndex(chosenIndex - 1, choices.Length, flavourText))
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
            yield return null;
        }
    }

    private bool UpdateChosenIndex(int newIndex, int choicesLength, string[] flavourText)
    {
        //Check for an invalid new index
        if (newIndex < 0 || newIndex >= choicesLength)
        {
            return false;
        }
        //Even if new index is the same as old, set the graphics in case of needing to override modified graphics.
        choiceBoxSelect.rectTransform.localPosition = new Vector3(8, 9f + (fontSize * newIndex), 0);
        if (flavourText != null)
        {
            DrawDialogBox();
            StartCoroutine(DrawText(flavourText[flavourText.Length - 1 - newIndex], 0));
        }
        //If chosen index is the same as before, do not play a sound effect, then return false
        if (chosenIndex == newIndex)
        {
            return false;
        }
        chosenIndex = newIndex;
       
        return true;
    }


    public void UndrawDialogBox()
    {
        dialogBox.gameObject.SetActive(false);
    }

    public IEnumerator UndrawSignBox()
    {
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1f / 0.2f) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }

            dialogBox.rectTransform.localPosition = new Vector2(dialogBox.rectTransform.localPosition.x,
                -dialogBox.rectTransform.sizeDelta.y * increment);
            yield return null;
        }
        dialogBox.gameObject.SetActive(false);
    }

    public void UndrawChoiceBox()
    {
        choiceBox.gameObject.SetActive(false);
    }
}



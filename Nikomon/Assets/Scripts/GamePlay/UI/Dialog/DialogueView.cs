using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using UnityEngine;
using Yarn.Unity;

public class DialogueView : DialogueViewBase
{
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        UIManager.Instance.Show<DialogPanel>(dialogueLine.Text.Text,DialogPanel.FadeType.Button,onDialogueLineFinished);
        
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        var texts = dialogueOptions
                .Select(d => d.Line.TextWithoutCharacterName.Text);
        UIManager.Instance.Show<DialogueChooserPanel>(texts,new Vector2(0,0),onOptionSelected);
    }
}

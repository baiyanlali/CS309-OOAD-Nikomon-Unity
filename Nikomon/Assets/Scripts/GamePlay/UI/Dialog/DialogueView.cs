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
        void FinishedDebug()
        {
            print("Ready for next line!");
            onDialogueLineFinished?.Invoke();
            ReadyForNextLine();
        }
        UIManager.Instance.Show<DialogPanel>(dialogueLine.Text.Text,
            DialogPanel.FadeType.Dialogue,(Action)FinishedDebug);
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        var texts = dialogueOptions
                .Select(d => d.Line.TextWithoutCharacterName.Text);
        // RectTransform rect = .transform as RectTransform;
        
        UIManager.Instance.Show<DialogueChooserPanel>(texts.ToList(),new Vector2(1,0),onOptionSelected,UIManager.Instance.GetUI<DialogPanel>().DialogueAttachment);
    }

    public override void DismissLine(Action onDismissalComplete)
    {
        UIManager.Instance.Hide<DialogPanel>();
        base.DismissLine(onDismissalComplete);
    }
}

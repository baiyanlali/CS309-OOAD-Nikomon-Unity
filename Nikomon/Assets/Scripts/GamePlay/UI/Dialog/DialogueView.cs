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
    public override void DialogueStarted()
    {
        GlobalManager.Instance.CanPlayerControlled = false;
    }

    // public string preText;
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        // bool canPlayerControlled = GlobalManager.Instance.CanPlayerControlled;
        // GlobalManager.Instance.CanPlayerControlled = false;
        void FinishedDebug()
        {
            // print("Ready for next line!");
            onDialogueLineFinished?.Invoke();
            // GlobalManager.Instance.CanPlayerControlled = canPlayerControlled;
            // ReadyForNextLine();
        }

        // preText = dialogueLine.Text.Text;
        UIManager.Instance.Show<DialogPanel>(dialogueLine,
            DialogPanel.FadeType.Dialogue,(Action)FinishedDebug);
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        print("Now run options!");
        // bool canPlayerControlled = GlobalManager.Instance.CanPlayerControlled;
        // GlobalManager.Instance.CanPlayerControlled = false;
        
        var texts = dialogueOptions
                .Select(d => d.Line.TextWithoutCharacterName.Text);
        // RectTransform rect = .transform as RectTransform;
        UIManager.Instance.Show<DialogPanel>();
        // UIManager.Instance.Show<DialogueChooserPanel>(texts.ToList(),new Vector2(1,0),onOptionSelected,UIManager.Instance.GetUI<DialogPanel>().DialogueAttachment);
        UIManager.Instance.Show<DialogueChooserPanel>(texts.ToList(),new Vector2(1,0),(Action<int>)OnOptionSelect,UIManager.Instance.GetUI<DialogPanel>().DialogueAttachment);

        void OnOptionSelect(int option)
        {
            // GlobalManager.Instance.CanPlayerControlled = canPlayerControlled;
            onOptionSelected?.Invoke(option);
        }
    }

    public override void DismissLine(Action onDismissalComplete)
    {
        UIManager.Instance.Hide<DialogPanel>();
        base.DismissLine(onDismissalComplete);
    }

    public override void DialogueComplete()
    {
        UIManager.Instance.Hide<DialogPanel>();
        UIManager.Instance.Hide<DialogueChooserPanel>();
        GlobalManager.Instance.CanPlayerControlled = true;
    }
}

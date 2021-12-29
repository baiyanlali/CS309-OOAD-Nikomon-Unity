using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialoguePanel : BaseUI
{
   public Text dialogueText;
   
   public override void OnEnter(params object[] args)
   {
      dialogueText.text = args[0] as string;
   }
}

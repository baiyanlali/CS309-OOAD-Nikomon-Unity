using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialoguePanel : BaseUI,IUIAnimator
{
   public Text dialogueText;
   
   public override void OnEnter(params object[] args)
   {
      base.OnEnter(args);
      dialogueText.text = args[0] as string;
      gameObject.SetActive(true);
   }

   public void OnEnterAnimator()
   {
      gameObject.transform.localScale = Vector3.zero;
      LeanTween.scale(gameObject,Vector3.one,0.3f).setOnComplete(() =>
      {

      });
   }

   public void OnExitAnimator()
   {
      gameObject.transform.localScale = Vector3.one;
      LeanTween.scale(gameObject,Vector3.zero,0.3f).setOnComplete(() =>
      {

      });
   }
}

using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialoguePanel : BaseUI,IUIAnimator
{
   public GameObject animObject;
   public Text dialogueText;
   [NonSerialized] public List<string> dialogTextList = new List<string>();
   public override UILayer Layer { get; set; } =  UILayer.PopupUI;
   public override void OnEnter(params object[] args)
   {
      base.OnEnter(args);
      
      if (dialogTextList.Count == 0)
      {
         foreach (var arg in args)
         {
            dialogTextList.Add((string)arg);
         }
         StartCoroutine(displayDialog());
      }
      else
      {
         foreach (var arg in args)
         {
            dialogTextList.Add((string)arg);
         }
      }
      
      gameObject.SetActive(true);
   }
   IEnumerator displayDialog()
   {
      while (true)
      {
         if (dialogTextList.Count == 0)
         {
            break;
         }
         Debug.Log("display"+dialogTextList[0]);
         dialogueText.text = dialogTextList[0];
         yield return new WaitForSeconds(2);
         dialogTextList.RemoveAt(0);
      }
      UIManager.Instance.Hide(this);
   }

   public void OnEnterAnimator()
   {
      // animObject.transform.localScale=Vector3.zero;
      gameObject.transform.localScale = Vector3.zero;
      LeanTween.scale(gameObject,Vector3.one,0.2f).setOnComplete(() =>
      {
      
      });
      
      // LeanTween.scale(animObject,Vector3.one,0.2f).setEase(LeanTweenType.punch);
   }

   public void OnExitAnimator()
   {
      gameObject.transform.localScale = Vector3.one;
      LeanTween.scale(gameObject,Vector3.zero,0.2f).setOnComplete(() =>
      {
         gameObject.SetActive(false);
      });
      // LeanTween.scale(animObject,Vector3.zero,0.2f).setOnComplete(() =>
      // {
      //    gameObject.SetActive(false);
      // });
   }
}

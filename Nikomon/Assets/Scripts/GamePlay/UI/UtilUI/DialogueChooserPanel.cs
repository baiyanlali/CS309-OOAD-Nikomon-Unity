using System;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using GamePlay.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.UtilUI
{
    public class DialogueChooserPanel : BaseUI
    {
        private Transform Content;

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">0 for string[] chooser, 1 for Vec2 wantedPivot, 2 for Action OnChoose, 3 for RectTransform</param>
        public override void Init(params object[] args)
        {
            base.Init(args);


            string[] chooser = args[0] as string[];
            Vector2 wantedPivot = (Vector2) args[1];
            Action<int> OnChoose = null;
            if(args.Length>=3)
                OnChoose = args[2] as Action<int>;
            RectTransform rectTransform = null;
            if(args.Length>=4)
                rectTransform = args[3] as RectTransform;
            Content = GET(Content, nameof(Content));

            List<Selectable> selectables = new List<Selectable>();
            if (rectTransform != null)
            {
                GetComponent<RectTransform>().position = rectTransform.position;
            }


            if (chooser.Length > Content.childCount)
            {
                GameObject obj = GameResources.SpawnPrefab("ChooseElement");

                for (int i = Content.childCount; i < chooser.Length; i++)
                {
                    var btn = Instantiate(obj, Content);
                }
            }
            else if (chooser.Length < Content.childCount)
            {
                for (int i = chooser.Length; i < Content.childCount; i++)
                {
                    Content.GetChild(i).gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < Mathf.Min(chooser.Length, Content.childCount); i++)
            {
                Transform obj = Content.GetChild(i);

                if (FirstSelectable == null)
                {
                    FirstSelectable = obj.gameObject;
                }

                obj.gameObject.SetActive(true);
                obj.GetComponentInChildren<Text>().text = chooser[i];
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
                selectables.Add(obj.GetComponent<Selectable>());
                int c = i;
                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    UIManager.Instance.Hide(this);
                    OnChoose?.Invoke(c);
                    // gameObject.SetActive(false);
                });
            }
            selectables.AutomateNavigation(DirectionType.Vertical);

        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            
            int width = Screen.width;
            int height = Screen.height;
            
            RectTransform rectTrans = GetComponent<RectTransform>();

            rectTrans.pivot = (Vector2)args[1];

            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTrans);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTrans);

            Rect rect = rectTrans.rect;

            Vector2 vec = rectTrans.position;

            var pivot = rectTrans.pivot;
            float left = vec.x - rect.width * pivot.x;
            float right = vec.x + rect.width * (1 - pivot.x);
            float down = vec.y - rect.height * pivot.y;
            float up = vec.y + rect.height * (1 - pivot.y);

            Vector2 finalPos = vec;
            if (left <= 0)
            {
                left = 0;
                finalPos.x = left + rect.width * pivot.x;
            }
            else if (right > width)
            {
                right = width;
                finalPos.x = right - rect.width * (1 - pivot.x);
            }
            
            if (down <= 0)
            {
                down = 0;
                finalPos.y = down + rect.height * pivot.y;
            }
            else if (up > height)
            {
                up = height;
                finalPos.y = up - rect.height * (1 - pivot.y);
            }

            rectTrans.anchoredPosition = finalPos;
            
        }
    }
}
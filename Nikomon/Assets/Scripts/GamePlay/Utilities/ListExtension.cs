using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Utilities
{
    public enum DirectionType
    {
        Horizontal,
        Vertical
    }
    public static class ListExtension
    {
        public static void AutomateNavigation(this List<Selectable> selectables, DirectionType type)
        {
            Selectable lastOne = null;
            if(type==DirectionType.Horizontal)
            {
                for (int i = 0; i < selectables.Count; i++)
                {
                    Selectable curOne = selectables[i];
                    if (lastOne == null)
                    {
                        curOne.GetComponent<Selectable>().navigation = new Navigation()
                        {
                            mode = Navigation.Mode.Explicit,
                            selectOnDown = null,
                            selectOnLeft = null,
                            selectOnRight = null,
                            selectOnUp = null,
                            wrapAround = false
                        };
                    }
                    else
                    {
                        curOne.GetComponent<Selectable>().navigation = new Navigation()
                        {
                            mode = Navigation.Mode.Explicit,
                            selectOnLeft = lastOne.GetComponent<Selectable>()
                        };

                        Navigation navi = lastOne.GetComponent<Selectable>().navigation;
                        navi.selectOnRight = curOne.GetComponent<Selectable>();
                        lastOne.GetComponent<Selectable>().navigation = navi;
                    }

                    lastOne = curOne;
                }
            }
            else if (type == DirectionType.Vertical)
            {
                for (int i = 0; i < selectables.Count; i++)
                {
                    Selectable curOne = selectables[i];
                    if (lastOne == null)
                    {
                        curOne.GetComponent<Selectable>().navigation = new Navigation()
                        {
                            mode = Navigation.Mode.Explicit,
                            selectOnDown = null,
                            selectOnLeft = null,
                            selectOnRight = null,
                            selectOnUp = null,
                            wrapAround = false
                        };
                    }
                    else
                    {
                        curOne.GetComponent<Selectable>().navigation = new Navigation()
                        {
                            mode = Navigation.Mode.Explicit,
                            selectOnUp = lastOne.GetComponent<Selectable>()
                        };

                        Navigation navi = lastOne.GetComponent<Selectable>().navigation;
                        navi.selectOnDown = curOne.GetComponent<Selectable>();
                        lastOne.GetComponent<Selectable>().navigation = navi;
                    }

                    lastOne = curOne;
                }

            }
        }
    }
}
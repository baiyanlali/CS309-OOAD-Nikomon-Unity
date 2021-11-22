using System;
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
        public static void AutomateNavigation<T>(this List<T> selectables, DirectionType type) where T : Selectable
        {
            Selectable lastOne = null;
            if (type == DirectionType.Horizontal)
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

        public static void LinkNavigation<T,T2>(this List<T> selectables, List<T2> selectables2, DirectionType type)
            where T : Selectable
            where T2: Selectable
        {
            for (int i = 0; i < Math.Min(selectables.Count, selectables2.Count); i++)
            {
                Selectable s1 = selectables[i];
                Selectable s2 = selectables2[i];

                if (s1.navigation.mode != Navigation.Mode.Explicit)
                {
                    s1.navigation = new Navigation()
                    {
                        mode = Navigation.Mode.Explicit
                    };
                }

                if (s2.navigation.mode != Navigation.Mode.Explicit)
                {
                    s2.navigation = new Navigation()
                    {
                        mode = Navigation.Mode.Explicit
                    };
                }

                Navigation navi1 = s1.navigation;
                if (type == DirectionType.Horizontal)
                    navi1.selectOnDown = s2;
                else navi1.selectOnRight = s2;
                s1.navigation = navi1;

                Navigation navi2 = s2.navigation;
                if (type == DirectionType.Horizontal)
                    navi2.selectOnUp = s1;
                else navi1.selectOnLeft = s1;
                s2.navigation = navi2;
            }
        }
    }
}
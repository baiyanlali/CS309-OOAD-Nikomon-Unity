using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class AutoSelect : MonoBehaviour,IPointerEnterHandler
{
    private Selectable _selectable;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_selectable == null)
            _selectable = GetComponent<Selectable>();
        
       _selectable.Select();
    }
}

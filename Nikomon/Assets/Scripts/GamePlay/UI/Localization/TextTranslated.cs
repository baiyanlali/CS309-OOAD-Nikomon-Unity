using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
using GamePlay.Messages;
using UnityEngine;
using UnityEngine.UI;

public class TextTranslated : Text
{
    public bool AutoTranslatedOnAwake = true;
    protected override void Awake()
    {
        base.Awake();
        if (!AutoTranslatedOnAwake) return;
        if(Application.isPlaying)
            text = Messages.Get(text);
    }
    
    public override string text
    {
        get
        {
            return m_Text;
        }
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                if (String.IsNullOrEmpty(m_Text))
                    return;
                m_Text = "";
                SetVerticesDirty();
            }
            else if (m_Text != value)
            {
                string post_value = value;
                if(Application.isPlaying)
                    post_value = Messages.Get(post_value);
                m_Text = post_value;
                SetVerticesDirty();
                SetLayoutDirty();
            }
        }
    }
}

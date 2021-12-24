using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
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
            text = Translator.TranslateStr(text);
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
                    post_value = Translator.TranslateStr(post_value);
                m_Text = post_value;
                SetVerticesDirty();
                SetLayoutDirty();
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionBTN
{
    private Option m_option;
    private int m_index;
    private InStoreScreen m_screen;
    VisualElement m_root;

    private readonly string optionButton = "optionButton";

    private Button m_button;

    public OptionBTN(int index, InStoreScreen screen, VisualElement root, Option option)
    {
        m_option = option;
        m_index = index;
        m_screen = screen;
        m_root = root;

        SetVisualElements();
        RegisterButtonCallbacks();
    }

    private void SetVisualElements()
    {
        m_button = m_root.Q<Button>(optionButton);

        m_button.text = m_option.GetOptionScript;
    }

    private void RegisterButtonCallbacks()
    {
        m_button.RegisterCallback<ClickEvent>(OnButtonSelected);
    }

    private void OnButtonSelected(ClickEvent evt)
    {
        m_screen.SelectOption(m_index);
    }
}

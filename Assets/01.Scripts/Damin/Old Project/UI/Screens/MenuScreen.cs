using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class MenuScreen : MonoBehaviour
{
    [Tooltip("this panel name")]
    [SerializeField] protected string m_ScreenName;

    [SerializeField] protected MainUiManager m_MainUI;
    [SerializeField] protected UIDocument m_Document;

    protected VisualElement m_Screen;
    protected VisualElement m_Root;

    public event Action OnScreenStarted;
    public event Action OnScreenEnd;

    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(m_ScreenName))
            m_ScreenName = this.GetType().Name;
    }

    protected virtual void Awake()
    {
        if (m_MainUI == null)
            m_MainUI = GetComponent<MainUiManager>();

        if (m_Document == null)
            m_Document = GetComponent<UIDocument>();

        if (m_Document == null && m_MainUI != null)
            m_Document = m_MainUI.MainMenuDocument;

        if (m_Document == null)
        {
            Debug.LogWarning($"MenuScreen  {m_ScreenName} : missing UIDocument. Check Script Execution Order.");
            return;
        }
        else
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }
    }

    protected virtual void SetVisualElements()
    {
        if (m_Document != null)
            m_Root = m_Document.rootVisualElement;

        m_Screen = GetVisualElement(m_ScreenName);

        if (m_Screen == null)
            Debug.LogError($"{m_ScreenName} is missing");
    }

    protected virtual void RegisterButtonCallbacks()
    {

    }

    public bool IsVisible()
    {
        if (m_Screen == null)
            return false;

        return (m_Screen.style.display == DisplayStyle.Flex);
    }

    public static void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
            return;

        visualElement.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public VisualElement GetVisualElement(string elementName)
    {
        if (string.IsNullOrEmpty(elementName) || m_Root == null)
            return null;

        return m_Root.Q(elementName);
    }

#region ShowScreen

    /// <summary>
    /// show the screen without motion.
    /// </summary>
    public virtual void ShowScreen()
    {
        if (IsVisible())
        {
            Debug.LogWarning($"{m_ScreenName} is already visible!");
            return;
        }

        OnShowScreen();
    }

    /// <summary>
    /// show the screen with motion. ***When the motion ends, the event must be executed unconditionally.***
    /// </summary>
    /// <param name="OnEventEnd"></param>
    public virtual void ShowScreenRoutine(Action OnShowEventEnd)
    {
        if (!IsVisible())
        {
            Debug.LogWarning($"{m_ScreenName} is already visible!");
            return;
        }
        OnShowEventEnd += OnShowScreen;
    }

    /// <summary>
    /// Runs when the screen is activated.
    /// </summary>
    public virtual void OnShowScreen()
    {
        ShowVisualElement(m_Screen, true);
        OnScreenStarted?.Invoke();
    }

    #endregion


#region HideScreen

    /// <summary>
    /// Hides the screen with motion. ***When the motion ends, the event must be executed unconditionally.***
    /// </summary>
    /// <param name="OnEventEnd"></param>
    public virtual void HideScreenRoutine(Action OnHideEventEnd)
    {
        if (!IsVisible())
        {
            Debug.LogWarning($"{m_ScreenName} is already invisible!");
            return;
        }
        OnHideEventEnd += OnHideScreen;
    }

    /// <summary>
    /// Hide the Screen without motion.
    /// </summary>
    public virtual void HideScreen()
    {
        if (!IsVisible())
        {
            Debug.LogWarning($"{m_ScreenName} is already invisible!");
            return;
        }

        OnHideScreen();
    }

    /// <summary>
    /// Runs when the screen is deactivated.
    /// </summary>
    public virtual void OnHideScreen()
    {
        OnScreenEnd?.Invoke();
        ShowVisualElement(m_Screen, false);
    }

#endregion

}


using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

enum State { Answer = -1, SelectOption = 1 }

public class InStoreScreen : MenuScreen
{
    private State m_curState = State.Answer;

    [SerializeField] private VisualTreeAsset _optionBTN;

    [SerializeField] private float m_typeSpeed = 0.2f;
    private bool m_isTyping = false;

    private List<OptionBTN> m_optionBTNs = new List<OptionBTN>();

    #region UI Names

    //VisualElement Names
    private readonly string n_topVisual = "Top-Visual";
    private readonly string n_textBoxVisual = "TextBox-Visual";
    private readonly string n_customerVisual = "Customer-Visual";
    private readonly string n_optionVisual = "OptionBTNContainer-Visual";

    //Label Names
    private string n_customerLabel = "Customer-Label";

    //Class Name
    private readonly string n_optionBTNCSS = "optionBTN";

    #endregion

    #region UI

    //VisualElements
    private VisualElement m_topVisual;
    private VisualElement m_textBoxVisual;
    private VisualElement m_customerVisual;
    private VisualElement m_optionContainerVisual;

    //Label Names
    private Label m_customerLabel;

    #endregion


    private void Start()
    {
        m_optionContainerVisual.Clear();
        m_optionBTNs.Clear();
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_topVisual = m_Root.Q<VisualElement>(n_topVisual);
        m_textBoxVisual = m_Root.Q<VisualElement>(n_textBoxVisual);
        m_customerVisual = m_Root.Q<VisualElement>(n_customerVisual);

        m_customerLabel = m_Root.Q<Label>(n_customerLabel);

        m_optionContainerVisual = m_Root.Q<VisualElement>(n_optionVisual);
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        m_textBoxVisual.RegisterCallback<ClickEvent>(evt => ShowNextScript());
    }

    private void ShowNextScript()
    {
        if (m_curState == State.SelectOption || m_isTyping)
            return;

        string newScript = OrderManager.Instance.GetScript();

        if (string.IsNullOrEmpty(newScript))
        {
            ShowOptions();
        }
        else if (!m_isTyping)
        {
            StartCoroutine(SeSetTypeEffectt(newScript));
        }
        else
        {
            //CancleTypeEffect(newScript);
        }
    }

    private void ShowOptions()
    {
        m_curState = State.SelectOption;
        m_optionContainerVisual.Clear();
        m_optionBTNs.Clear();

        Option[] options = OrderManager.Instance.m_curOption.GetOption();
        for(int i = 0; i < options.Length; i++)
        {
            TemplateContainer template = _optionBTN.Instantiate();
            template.AddToClassList(n_optionBTNCSS);
            m_optionContainerVisual.Add(template);

            OptionBTN optionButton = new OptionBTN(i, this, template, options[i]);
            m_optionBTNs.Add(optionButton);
        }
    }

    public void SelectOption(int index)
    {
        m_curState = State.Answer;
        OrderManager.Instance.SelectOption(index, out AnswerType optionType);

        m_optionContainerVisual.Clear();
        m_optionBTNs.Clear();

        if (optionType == AnswerType.End)
        {
            Debug.Log("대화종료");
            //씬 넘어가기 
            return;
        }
        ShowNextScript();
    }

    private void CancleTypeEffect(string _tartgetTXT)
    {
        m_isTyping = false;
        m_customerLabel.text = _tartgetTXT;
        StopAllCoroutines();
    }

    IEnumerator SeSetTypeEffectt(string _tartgetTXT)
    {
        m_isTyping = true;
        int index = 0;
        m_customerLabel.text = "";

        while (m_customerLabel.text != _tartgetTXT)
        {
            m_customerLabel.text += _tartgetTXT[index];
            index++;
            yield return new WaitForSeconds(m_typeSpeed);
        }

        m_isTyping = false;
    }
}

using Define;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AnswerType { End, Run }

[Serializable]
public class Option
{
    [TextArea(1, 5)]
    [Tooltip("선택지")]
    [SerializeField] private string optionScript;

    [TextArea]
    [Tooltip("선택지에 대한 답,첫 대화일시 사용 안함")]
    [SerializeField] private string answerScript;

    [Tooltip("End시 대화 종료")]
    [SerializeField] private AnswerType answerType; 
    [SerializeField] private Option[] options;

    public Option(string _optionScript = "오케이", string _answerScript = default, AnswerType _answerType = AnswerType.End, Option[] _options = default)
    {
        options = _options;
        optionScript = _optionScript;
        answerScript = _answerScript;
        answerType = _answerType;
    }

    public string GetOptionScript => optionScript;

    public AnswerType GetAnswerType() => answerType;

    public AnswerType GetAnswer(out string _answerScript)
    {
        if(answerType == AnswerType.Run)
        {
            //return answerType;
        }
        
        _answerScript = answerScript;
        //_answerScript = string.Empty;
        return answerType;
    }

    public Option[] GetOption()
    {
        if (options.Length <= 0)
        {
            options = new Option[1];
            options[0] = new Option();
        }
        return options;
    }

    public Option SelectOption(int index)
    {
        if (options.Length <= index)
        {
            Debug.LogError($"The maximum range for options is {index}. (You choose {index})");
            return null;
        }
        return options[index];
    }
}

//[Serializable]
//public struct Order
//{
//    [TextArea]
//    public string[] script;

//    [EnumFlags]
//    public Ingredient ingredient;

//    [Space]

//    [TextArea(1, 5)]
//    public string CorrectAnswer;

//    [TextArea(1, 5)]
//    public string WrongAnswer;
//}

[CreateAssetMenu(fileName = "New CustomerInfo", menuName = "SO/Customer/CustomerInfo")]
public class CustomerInfoSO : ScriptableObject
{
    [SerializeField] private Sprite CustomerSprite;
    [SerializeField] private string CustomerName;
    public string _customerName => CustomerName;

    [SerializeField] private Option[] m_scripts = new Option[1];
    public Option m_curOption { get; private set; }
    
    /// <summary>
    /// null일시 랜덤
    /// </summary>
    /// <param name="index"></param>
    public Option SelectFirstScript(int index = default)
    {
        m_curOption = m_scripts[Random.Range(0, m_scripts.Length)];
        return m_curOption;
    }
    
    public Option GetCurrentOption()
    {
        if(m_curOption == null)
        {
            m_curOption = m_scripts[0];
        }

        return m_curOption;
    }

    public Option SelectOption(int optionIndex)
    {
        if (m_curOption == null)
        {
            m_curOption = m_scripts[0];
        }
        else
        {
            m_curOption = m_curOption.SelectOption(optionIndex);
        }

        return m_curOption;
    }
#region Old Script
    //    [SerializeField] private Order[] Orders;
    //    public Order[] _orders => Orders;

    //    private Order m_curOrder;
    //    private int m_curOrderIndex = 0;

    //    private void OnEnable()
    //    {
    //        m_curOrderIndex = 0;
    //    }

    //#region GetOrder

    //    public Order GetRandomOrder()
    //    {
    //        m_curOrder = Orders[Random.Range(0, Orders.Length)];
    //        m_curOrderIndex = 0;
    //        return m_curOrder;
    //    }

    //    public Order GetCurrentOrder => m_curOrder;

    //    public string GetNextScript => m_curOrder.script[m_curOrderIndex++];

    //    public string GetEntireScript => String.Join("\n", m_curOrder.script);

    //    public string GetCorrectAnswer => m_curOrder.CorrectAnswer;

    //    public string GetWrongAnswer() => m_curOrder.WrongAnswer;

#endregion
}

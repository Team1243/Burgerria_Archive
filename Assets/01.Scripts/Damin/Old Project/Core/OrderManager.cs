using System;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [SerializeField] private CustomerListSO m_customers;
    private CustomerInfoSO m_curCustomer;
    public Option m_curOption { get; private set; }
    private string[] m_curScripts;
    private int m_scriptIndex = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        GetNextCustomer();
        SetScript();
    }

    public string GetScript()
    {
        if (m_scriptIndex >= m_curScripts.Length)
            return null;
        return m_curScripts[m_scriptIndex++];
    }

    public void GetNextCustomer() 
    {
        m_curCustomer = m_customers.GetRandomCustomer(m_curCustomer);
    }

    public void SelectCustomer(int index)
    {
        m_curCustomer = m_customers.GetCustomer(index);
    }


    /// <summary>
    /// 처음 대사 고를때
    /// </summary>
    /// <param name="index"></param>
    public void SetScript()
    {
        if(m_curOption == null)
        {
            m_curOption = m_curCustomer.SelectFirstScript();
        }

        string newScripts;
        var answerType = m_curOption.GetAnswer(out newScripts);
        Debug.Log(newScripts);
        m_curScripts = newScripts.Split("\n");

    }

    public void SelectOption(int index, out AnswerType answerType)
    {
        m_scriptIndex = 0;
        m_curOption = m_curOption.SelectOption(index);
        Array.Resize(ref m_curScripts, 0);

        answerType = m_curOption.GetAnswerType();

        
        SetScript();
    }
}

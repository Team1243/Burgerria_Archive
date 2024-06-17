using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainUiManager : MonoBehaviour
{
    [SerializeField] protected UIDocument m_MainMenuDocument;
    public UIDocument MainMenuDocument => m_MainMenuDocument;

    //[SerializeField] private SerializableDict<string, MenuScreen> Screens;

    private void Awake()
    {
        if (m_MainMenuDocument == null)
            m_MainMenuDocument = GetComponent<UIDocument>();

        // if(gameScreen == null)
        //     gameScreen = GetScreenCode("GameScreen") as GameScreen;
        //Dictionary<string, MenuScreen> screens = new Dictionary<string, MenuScreen>();
        //foreach(KeyValuePair<string, MenuScreen> item in screens)
        //{
        //    if (!string.IsNullOrEmpty(item.Key))
        //        continue;

        //    //item.Key = item.Value.GetType().Name.ToString();
        //}
    }


    private VisualElement GetVisualElement(string name)
    {
        if (string.IsNullOrEmpty(name) || m_MainMenuDocument == null)
            return null;

        return GetScreenCode(name).GetComponent<UIDocument>().rootVisualElement;
    }

    private MenuScreen GetScreenCode(string name)
    {
        if (string.IsNullOrEmpty(name) || m_MainMenuDocument == null)
            return null;

        return transform.Find(name).GetComponent<MenuScreen>();
    }
}

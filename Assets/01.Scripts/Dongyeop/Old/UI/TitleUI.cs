using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _root;

    private Label _gameNameLabel;
    private Label _touchToPlayLabel;

    private bool _isPlay = false;

    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;

        _gameNameLabel = _root.Q<Label>("game-name");
        _touchToPlayLabel = _root.Q<Label>("touch-to-play");
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        _gameNameLabel.AddToClassList("show");
        yield return new WaitForSeconds(1.8f);
        _isPlay = true;

        while (true)
        {
            _touchToPlayLabel.AddToClassList("show");
            yield return new WaitForSeconds(.4f);
            _touchToPlayLabel.RemoveFromClassList("show");
            yield return new WaitForSeconds(.4f);
        }
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && _isPlay)
        {
            SceneTransition.Instance.SceneTransitions(2);
            _isPlay = false;
        }
        #else
        if (Input.touchCount >= 1 && _isPlay)
        {
            SceneTransition.Instance.SceneTransitions(2);
            _isPlay = false;
        }
        #endif
    }
}

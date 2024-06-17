using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameCycleUI : MonoBehaviour
{
    public static GameCycleUI Instance;
    
    private UIDocument _uiDocument;
    private VisualElement _root;

    [Header("VisualElement")] 
    private Button _gamePanel;
    private Label _currentScore;
    private Label _bestScore;
    private Button _homeButton;
    private Button _settingButton;
    private Button _quitButton;
    private Label _timerLabel;

    [Header("Other")] 
    public bool IsProceeding = false;
    private PauseMenuUI _pauseMenuUI;
    
    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple GameCycleUI is running");
        Instance = this;
    }

    private void Start()
    {
        _pauseMenuUI = FindObjectOfType<PauseMenuUI>();
        
        StartCoroutine(GameStartCo(null, () => InGameManager.Instance.RePlay()));
    }

    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        
        _gamePanel = _root.Q<Button>("game-pnael");
        _gamePanel.RegisterCallback<ClickEvent>(evt => StartCoroutine(GameStartCo(() => AdManager.Instance.ShowFrontAds(), () =>
        {
            InGameManager.Instance.RePlay();
            SelectManager.Instance.ClearSelectList();
        })));
        _currentScore = _gamePanel.Q<Label>("current-score");
        _bestScore = _gamePanel.Q<Label>("best-score");
        _homeButton = _gamePanel.Q<Button>("home-button");
        _homeButton.RegisterCallback<ClickEvent>(evt =>
        {
            if (IsProceeding)
                return;
            IsProceeding = true;
            SceneTransition.Instance.SceneTransitions(1);
        });
        _settingButton = _gamePanel.Q<Button>("setting-button");
        _settingButton.RegisterCallback<ClickEvent>(evt =>
        {
            if (IsProceeding)
                return;
            IsProceeding = true;
            _pauseMenuUI.Pause();
        });
        _quitButton = _gamePanel.Q<Button>("quit-button");
        _quitButton.RegisterCallback<ClickEvent>(evt =>
        {
            if (IsProceeding)
                return;
            IsProceeding = true;
            SceneTransition.Instance.SceneTransitions(-1);
        });

        _timerLabel = _root.Q<Label>("count-label");
        
        _currentScore.text = $"{GameManager.Instance.CurrentScore.ToString()}";
        _bestScore.text = $"Best: {GameManager.Instance.MaxScore.ToString()}";
    }
    
    public void GameUI(bool value)
    {
        if (value)
        {
            _gamePanel.AddToClassList("show");
            _currentScore.text = $"{GameManager.Instance.CurrentScore.ToString()}";
            _bestScore.text = $"Best: {GameManager.Instance.MaxScore.ToString()}";
        }
        else
            _gamePanel.RemoveFromClassList("show");
    }

    private IEnumerator GameStartCo(Action startAction, Action endAction)
    {
        if (!IsProceeding)
        {
            startAction?.Invoke();
            GameUI(false);
            SpawnManager.Instance.DeSpawnAll();
            IsProceeding = true;

            _timerLabel.text = 3.ToString();
            for (int i = 2; i >= 0; --i)
            {
                _timerLabel.AddToClassList("big");
                yield return new WaitForSeconds(.9f);
                _timerLabel.text = i.ToString();
                _timerLabel.RemoveFromClassList("big");
                yield return new WaitForSeconds(.1f);
            }
        
            IsProceeding = false;
            endAction?.Invoke();
        }
    }
}

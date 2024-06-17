using System;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    [HideInInspector] public GameCycleType CycleType = GameCycleType.START;
    
    private float _maxTime = 30;
    private float _currentTime;
    public float CurrentTime
    {
        get => _currentTime;
        set
        {
            _currentTime = value;
            if (_currentTime < 5)
                FeedBackManager.Instance.PlayFeedBack("ScreenRed");
            else 
                FeedBackManager.Instance.FinishFeedback("ScreenRed");
            OrderSheetManager.Instance.SetTimerSlider(_currentTime, _maxTime);

            if (_currentTime <= 0 && CycleType == GameCycleType.PLAY)
                GameOver();
        }
    }
    
    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple InGameManager is running");
        Instance = this;
    }

    private void Update()
    {
        if (CycleType == GameCycleType.PLAY)
            InGameTime();
    }

    private void InGameTime()
    {
        CurrentTime -= Time.deltaTime;
    }

    public void BurgerMatch()
    {
        OrderSheetManager.Instance.TimeLabelSet(1.5f);
    }

    public void RePlay()
    {
        GameManager.Instance.CurrentScore = 0;
        
        OrderSheetManager.Instance.ObjectReset();
        CycleType = GameCycleType.PLAY;

        _currentTime = _maxTime;
        //CurrentTime = 10;
        //_maxTime = 10;
        //_current = 60;
    }

    public void GameOver()
    {
        Debug.Log("Gameover");
        Taptic.Failure();
        CycleType = GameCycleType.GAMEOVER;
        GameCycleUI.Instance.GameUI(true);
        GameManager.Instance.GameOver();
        CycleType = GameCycleType.GAMEOVER;
        FeedBackManager.Instance.FinishFeedback("ScreenRed");
    }
}

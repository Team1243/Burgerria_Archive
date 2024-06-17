using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public int CurrentScore = 0;
    [HideInInspector] public int MaxScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager is running");
            Destroy(gameObject);
        }
        Instance = this;
        
        FrameLimit();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);
        MaxScore = PlayerPrefs.GetInt("MaxScore");

        SceneManager.sceneLoaded += LoadedSceneEvent;
        SceneManager.sceneLoaded += AdManager.Instance.ShowBannerAds;

        AdManager.Instance.ShowBannerAds();

        SceneManager.LoadScene(1);
    }
    
    private void LoadedSceneEvent(Scene scene, LoadSceneMode mode)
    {
        CurrentScore = 0;
    }

    public void GameOver()
    {
        if (CurrentScore >= MaxScore)
        {
            MaxScore = CurrentScore;
            PlayerPrefs.SetInt("MaxScore", MaxScore);
        }
        SelectManager.Instance.State = SelectState.End;
    }
    
    private void FrameLimit()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = 120;
#endif
    }
}

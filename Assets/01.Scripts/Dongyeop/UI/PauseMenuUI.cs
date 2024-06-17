using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class PauseMenuUI : MonoBehaviour
{
    private UIDocument _uiDocument;

    [SerializeField] private AudioMixer _audioMixer;

    [Header("Element")]
    private VisualElement _rootElement;
    private VisualElement _pausePanel;
    private Toggle _bgmToggle;
    private Toggle _sfxToggle;
    private Button _titleButton;
    private Button _quitButton;
    private Button _cancelButton;

    [Header("Other")] 
    private bool _panelShow = false;

    private void Awake() 
    {
        _uiDocument = GetComponent<UIDocument>();

        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetInt("SFX", 1);
        if (!PlayerPrefs.HasKey("BGM"))
            PlayerPrefs.SetInt("BGM", 1);
    }

    private void OnEnable() 
    {
        _rootElement = _uiDocument.rootVisualElement;
        _pausePanel = _rootElement.Q<VisualElement>("pause-panel");
        _sfxToggle = _pausePanel.Q<Toggle>("sfx-toggle");
        _bgmToggle = _pausePanel.Q<Toggle>("bgm-toggle");
        _titleButton = _pausePanel.Q<Button>("title-button");
        _quitButton = _pausePanel.Q<Button>("quit-button");
        _cancelButton = _pausePanel.Q<Button>("cancel-button");

        _bgmToggle.value = PlayerPrefs.GetInt("BGM") == 1;
        _sfxToggle.value = PlayerPrefs.GetInt("SFX") == 1;

        _bgmToggle.RegisterValueChangedCallback(evt => AudioToggle("BGM", evt.newValue));
        _sfxToggle.RegisterValueChangedCallback(evt => AudioToggle("SFX", evt.newValue));
        _titleButton.RegisterCallback<ClickEvent>(evt =>
        {
            Time.timeScale = 1;
            SceneTransition.Instance.SceneTransitions(1);
            _pausePanel.RemoveFromClassList("show");
        });
        _quitButton.RegisterCallback<ClickEvent>(evt =>
        {
            Time.timeScale = 1;
            SceneTransition.Instance.SceneTransitions(-1);
            _pausePanel.RemoveFromClassList("show");
        });
        _cancelButton.RegisterCallback<ClickEvent>(evt => Pause());
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Pause()
    {
        if (_panelShow)
        {
            Time.timeScale = 1;
            _pausePanel.RemoveFromClassList("show");
            GameCycleUI.Instance.IsProceeding = false;
        }
        else
        {
            _pausePanel.AddToClassList("show");
            Time.timeScale = 0;
        }

        _panelShow = !_panelShow;
    }

    private void AudioToggle(string str, bool value)
    {
        _audioMixer.SetFloat(str, value ? 0 : -80);
        PlayerPrefs.SetInt(str, value ? 1 : 0);
    }

    private void AudioToggle(string str, int value)
    {
        _audioMixer.SetFloat(str, value == 1? 0 : -80);
    }
}

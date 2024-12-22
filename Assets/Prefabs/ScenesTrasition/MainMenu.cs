using System;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SceneTransitioner))]
public class MainMenu : MonoBehaviour
{
    private const string MainGameSceneName = "MainScene";

    [SerializeField] private Button _gameButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private Image _settingsPanel;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _leaderboardButton;

    private SceneTransitioner _sceneTransitioner;

    public event Action LeaderboardButtonClicked;
    
    private void Awake()
    {
        _sceneTransitioner = GetComponent<SceneTransitioner>();
    }

    private void Start()
    {
        _gameButton.onClick.AddListener(StarGameScene);
        _settingButton.onClick.AddListener(ProcessSettingsButtonClick);
        _leaderboardButton.onClick.AddListener(ProcessLeaderBoardButtonClick);
        _loadingScreen.gameObject.SetActive(false);
        _settingsPanel.gameObject.SetActive(false);
    }

    private void StarGameScene()
    {
        _loadingScreen.gameObject.SetActive(true);
        _sceneTransitioner.SwitchScene(MainGameSceneName);
        SetYandexSDKReady();
    }
    
    private void ProcessSettingsButtonClick()
    {
        _settingButton.enabled = false;
        _gameButton.enabled = false;
        _settingsPanel.gameObject.SetActive(true);
        _backButton.onClick.AddListener(ProcessOnBackButtonCLick);
    }
    
    private void ProcessLeaderBoardButtonClick()
    {
        LeaderboardButtonClicked?.Invoke();
    }

    private void ProcessOnBackButtonCLick()
    {
        _settingButton.enabled = true;
        _gameButton.enabled = true;
        _backButton.onClick.RemoveListener(ProcessOnBackButtonCLick);
        _settingsPanel.gameObject.SetActive(false);
    }

    private void SetYandexSDKReady()
    {
        YandexGamesSdk.GameReady();
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

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
    [SerializeField] private GameObject _leaderboard;

    private SceneTransitioner _sceneTransitioner;
    
    private void Awake()
    {
        _sceneTransitioner = GetComponent<SceneTransitioner>();
    }

    private void OnEnable()
    {
        _gameButton.onClick.AddListener(StarGameScene);
        _settingButton.onClick.AddListener(ProcessSettingsButtonClick);
        _leaderboardButton.onClick.AddListener(ProcessLeaderBoardButtonClick);
        YandexGame.GameplayStart();
    }

    private void OnDisable()
    {
        _gameButton.onClick.RemoveListener(StarGameScene);
        _settingButton.onClick.RemoveListener(ProcessSettingsButtonClick);
        _leaderboardButton.onClick.RemoveListener(ProcessLeaderBoardButtonClick);
    }

    private void Start()
    {
        _loadingScreen.gameObject.SetActive(false);
        _settingsPanel.gameObject.SetActive(false);
        ProcessLeaderBoardButtonClick();
    }

    private void StarGameScene()
    {
        _loadingScreen.gameObject.SetActive(true);
        _sceneTransitioner.SwitchScene(MainGameSceneName);
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
        ToggleLeaderboard(!_leaderboard.activeSelf);
    }

    private void ProcessOnBackButtonCLick()
    {
        _settingButton.enabled = true;
        _gameButton.enabled = true;
        _backButton.onClick.RemoveListener(ProcessOnBackButtonCLick);
        _settingsPanel.gameObject.SetActive(false);
    }
    
    private void ToggleLeaderboard(bool status)
    {
        _leaderboard.SetActive(status);
    }
}

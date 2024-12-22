using System;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;

public class YandexLeaderboard : MonoBehaviour
{
    private const string LeaderboardName = "Leaderboard";
    private const string AnonymousName = "Anonymous";

    [SerializeField] private LeaderboardView _leaderboardView;
    [SerializeField] private MainGameSettingsMenu _settingsMenu;
    [SerializeField] private MainMenu _mainMenuSceneMenu;

    private readonly List<LeaderboardPlayer> _leaderboardPlayers = new();

    private void OnEnable()
    {
        if (_settingsMenu != null)
            _settingsMenu.LeaderboardButtonClicked += Fill;

        if (_mainMenuSceneMenu != null)
            _mainMenuSceneMenu.LeaderboardButtonClicked += Fill;
    }

    private void OnDisable()
    {
        if (_settingsMenu != null)
            _settingsMenu.LeaderboardButtonClicked -= Fill;

        if (_mainMenuSceneMenu != null)
            _mainMenuSceneMenu.LeaderboardButtonClicked -= Fill;
    }

    public void SetPlayerScore(float score)
    {
#if !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized == false)
            return;

        Leaderboard.GetPlayerEntry(LeaderboardName, (result) =>
        {
            if (result == null || result.score < score)
                Leaderboard.SetScore(LeaderboardName, score);
        });
#endif
    }

    public void Fill()
    {
        if (PlayerAccount.IsAuthorized == false)
            return;

        _leaderboardView.gameObject.SetActive(true);
        _leaderboardPlayers.Clear();

        Leaderboard.GetEntries(LeaderboardName, (result) =>
        {
            foreach (var entry in result.entries)
            {
                int rank = entry.rank;
                int score = entry.score;
                string name = entry.player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = AnonymousName;

                _leaderboardPlayers.Add(new LeaderboardPlayer(rank, name, score));
            }
        });

        _leaderboardView.ConstructLeaderBoard(_leaderboardPlayers);
    }
}
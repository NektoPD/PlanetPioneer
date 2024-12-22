using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour
{
    private const int _maxLeaderboardElementsCount = 11;
    
    [SerializeField] private Transform _container;
    [SerializeField] private LeaderboardElement _leaderboardElementPrefab;
    [SerializeField] private int _verticalSpacing;
    [SerializeField] private Button _closeButton;

    private List<LeaderboardElement> _spawnedElements;

    private void Start()
    {
        _closeButton.onClick.AddListener(ProcessCloseButtonClicked);
        gameObject.SetActive(false);
    }

    public void ConstructLeaderBoard(List<LeaderboardPlayer> leaderboardPlayers)
    {
        ClearLeaderboard();

        for (int i = 0; i < Mathf.Min(leaderboardPlayers.Count, _maxLeaderboardElementsCount); i++)
        {
            LeaderboardPlayer player = leaderboardPlayers[i];
            LeaderboardElement leaderboardElementInstance = Instantiate(_leaderboardElementPrefab, _container);
            
            Vector3 newPosition = leaderboardElementInstance.transform.localPosition;
            newPosition.y -= i + _verticalSpacing;
            leaderboardElementInstance.transform.localPosition = newPosition;

            leaderboardElementInstance.Initialize(player.Name, player.Rank, player.Score);
            _spawnedElements.Add(leaderboardElementInstance);
        }
    }

    private void ClearLeaderboard()
    {
        foreach (var element in _spawnedElements)
        {
            Destroy(element);
        }

        _spawnedElements = new List<LeaderboardElement>();
    }

    private void ProcessCloseButtonClicked()
    {
        ClearLeaderboard();
        gameObject.SetActive(false);
    }
}

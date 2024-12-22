using UnityEngine;

public class GameEnder : MonoBehaviour
{
    [SerializeField] private RocketBuilder _rocketBuilder;
    [SerializeField] private SoundPlayer _endGameSound;

    private void OnEnable()
    {
        _rocketBuilder.RocketReady += EndGame;
    }

    private void EndGame()
    {
        _endGameSound.PlaySound();
        Time.timeScale = 0;
        //ShowWinScreen or Leaderboard 
    }
}

using System;
using UnityEngine;
using Zenject;

public class VideoAd : MonoBehaviour
{
    private const string AdPopupMessageText = "The adverts are about to start playing";
    
    private UIPopUpWindowShower _windowShower;
    
    public event Action RewardedAdWatched;
    
    public void ShowRewarded() => Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardCallback, OnCloseRewardCallback);
    public void ShowInterstitial() => Agava.YandexGames.InterstitialAd.Show(OnOpenCallback, OnCloseInterstitialCallback);

    [Inject]
    private void Construct(UIServicesProvider uiServicesProvider)
    {
        _windowShower = uiServicesProvider.PopUpWindow;
    }
    
    private void OnOpenCallback()
    {
        ShowAdPopupMessage(() =>
        {
            Time.timeScale = 0;
            AudioListener.volume = 0f;
        });
    }

    private void OnRewardCallback()
    {
        RewardedAdWatched?.Invoke();
    }

    private void OnCloseRewardCallback()
    {
        Time.timeScale = 1;
        AudioListener.volume = 1f;
    }

    private void OnCloseInterstitialCallback(bool result)
    {
        Time.timeScale = 1;
        AudioListener.volume = 1f;
    }

    private void ShowAdPopupMessage(Action onMessageShown)
    {
        _windowShower.AddMessageToQueue(AdPopupMessageText, onMessageShown);
    }
}

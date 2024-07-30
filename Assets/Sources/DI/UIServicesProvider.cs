using UnityEngine;
using Zenject;

public class UIServicesProvider : MonoInstaller
{
    [SerializeField] private UIPopUpWindowShower _popUpWindow;
    [SerializeField] private UISliderShower _UISlider;
    
    public UIPopUpWindowShower PopUpWindow => _popUpWindow;
    public UISliderShower UISlider => _UISlider;
}
